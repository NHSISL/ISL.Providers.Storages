﻿// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using Azure;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Sas;
using ISL.Providers.Storages.AzureBlobStorage.Brokers.DateTimes;
using ISL.Providers.Storages.AzureBlobStorage.Brokers.Storages.Blobs;
using ISL.Providers.Storages.AzureBlobStorage.Services.Foundations.Files;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace ISL.Providers.Storages.AzureBlobStorage.Services.Foundations.Storages
{
    internal partial class StorageService : IStorageService
    {
        private readonly IBlobStorageBroker blobStorageBroker;
        private readonly IDateTimeBroker dateTimeBroker;

        internal StorageService(IBlobStorageBroker blobStorageBroker, IDateTimeBroker dateTimeBroker)
        {
            this.blobStorageBroker = blobStorageBroker;
            this.dateTimeBroker = dateTimeBroker;
        }

        public ValueTask CreateFileAsync(Stream input, string fileName, string container) =>
        TryCatch(async () =>
        {
            ValidateStorageArgumentsOnCreate(input, fileName, container);

            BlobClient blobClient =
                this.blobStorageBroker.BlobServiceClient
                    .GetBlobContainerClient(container)
                    .GetBlobClient(fileName);

            await blobClient.UploadAsync(input);
        });

        public ValueTask RetrieveFileAsync(Stream output, string fileName, string container) =>
        TryCatch(async () =>
        {
            ValidateStorageArgumentsOnRetrieve(output, fileName, container);

            BlobClient blobClient =
                this.blobStorageBroker.BlobServiceClient
                    .GetBlobContainerClient(container)
                    .GetBlobClient(fileName);

            await blobClient.DownloadToAsync(output);
        });

        public ValueTask DeleteFileAsync(string fileName, string container) =>
        TryCatch(async () =>
        {
            ValidateStorageArgumentsOnDelete(fileName, container);

            BlobClient blobClient =
                this.blobStorageBroker.BlobServiceClient
                    .GetBlobContainerClient(container)
                    .GetBlobClient(fileName);

            await blobClient.DeleteAsync(DeleteSnapshotsOption.None);
        });

        public ValueTask<List<string>> ListFilesInContainerAsync(string container) =>
        TryCatch(async () =>
        {
            ValidateContainerName(container);
            List<string> fileNames = new List<string>();

            BlobContainerClient containerClient =
                this.blobStorageBroker.BlobServiceClient
                    .GetBlobContainerClient(container);

            AsyncPageable<BlobItem> blobItems = containerClient.GetBlobsAsync();

            await foreach (BlobItem blobItem in blobItems)
            {
                fileNames.Add(blobItem.Name);
            }

            return fileNames;
        });

        public ValueTask<string> GetDownloadLinkAsync(string fileName, string container, DateTimeOffset expiresOn) =>
        TryCatch(async () =>
        {
            ValidateStorageArgumentsOnGetDownloadLink(fileName, container, expiresOn);

            BlobClient blobClient =
                    this.blobStorageBroker.BlobServiceClient
                        .GetBlobContainerClient(container)
                        .GetBlobClient(fileName);

            var sasBuilder = this.blobStorageBroker.GetBlobSasBuilder(fileName, container, expiresOn);
            var blobUriBuilder = this.blobStorageBroker.GetBlobUriBuilder(blobClient.Uri);

            return blobUriBuilder.ToUri().ToString();
        });

        public ValueTask CreateContainerAsync(string container) =>
        TryCatch(async () =>
        {
            ValidateContainerName(container);
            await this.blobStorageBroker.BlobServiceClient.CreateBlobContainerAsync(container);
        });

        public ValueTask SetContainerACLAsync(string container, string accessType, string permissions) =>
            throw new NotImplementedException();

        public ValueTask CreateAndAssignAccessPoliciesToContainerAsync(string container, List<string> policyNames) =>
        TryCatch(async () =>
        {
            ValidateStorageArgumentsOnCreateAccessPolicy(container, policyNames);
            DateTimeOffset dateTimeOffset = await this.dateTimeBroker.GetCurrentDateTimeOffsetAsync();
            string timestamp = dateTimeOffset.ToString("yyyyMMddHHmms");

            BlobContainerClient containerClient =
                    this.blobStorageBroker.BlobServiceClient
                        .GetBlobContainerClient(container);

            List<BlobSignedIdentifier> signedIdentifiers = new List<BlobSignedIdentifier>();

            foreach (string policyName in policyNames)
            {
                string permissions = ConvertPolicyNameToPermissions(policyName);

                var blobSignedIdentifier = new BlobSignedIdentifier
                {
                    Id = $"{policyName}_{timestamp}",
                    AccessPolicy = new BlobAccessPolicy
                    {
                        PolicyStartsOn = dateTimeOffset,
                        PolicyExpiresOn = dateTimeOffset.AddDays(this.blobStorageBroker.TokenLifetimeDays),
                        Permissions = permissions
                    }
                };

                signedIdentifiers.Add(blobSignedIdentifier);
            }

            await containerClient.SetAccessPolicyAsync(permissions: signedIdentifiers);
        });

        public ValueTask<string> CreateDirectorySasToken(
             string container, string directoryPath, string accessPolicyIdentifier, DateTimeOffset expiresOn) =>
        TryCatch(async () =>
        {
            ValidateStorageArgumentsOnCreateDirectorySasToken(
                container, directoryPath, accessPolicyIdentifier, expiresOn);

            DateTimeOffset dateTimeOffset = await this.dateTimeBroker.GetCurrentDateTimeOffsetAsync();

            var sasBuilder = new DataLakeSasBuilder()
            {
                Identifier = accessPolicyIdentifier,
                Resource = "d",
                Path = directoryPath,
                IsDirectory = true,
                FileSystemName = container,
                StartsOn = dateTimeOffset,
                ExpiresOn = expiresOn
            };

            //var sasBuilder = this.blobStorageBroker.GetDataLakeSasBuilder(
            //  container, directoryPath, accessPolicyIdentifier, expiresOn);

            var sasToken = sasBuilder.ToSasQueryParameters(
                this.blobStorageBroker.StorageSharedKeyCredential);

            var uri = this.blobStorageBroker.GetDataLakeUriBuilder(
                this.blobStorageBroker.DataLakeServiceClient.Uri, container, directoryPath, sasToken);

            return uri.ToString();
        });


        virtual internal string ConvertPolicyNameToPermissions(string policyName) => policyName switch
        {
            "read" => "rl",
            "write" => "w",
            "delete" => "d",
            "fullaccess" => "rlwd",
            _ => ""
        };
    }
}
