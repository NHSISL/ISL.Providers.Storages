// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
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
            await this.blobStorageBroker.CreateFileAsync(input, fileName, container);
        });

        public ValueTask RetrieveFileAsync(Stream output, string fileName, string container) =>
        TryCatch(async () =>
        {
            ValidateStorageArgumentsOnRetrieve(output, fileName, container);
            await this.blobStorageBroker.RetrieveFileAsync(output, fileName, container);
        });

        public ValueTask DeleteFileAsync(string fileName, string container) =>
        TryCatch(async () =>
        {
            ValidateStorageArgumentsOnDelete(fileName, container);
            await this.blobStorageBroker.DeleteFileAsync(fileName, container);
        });

        public ValueTask<List<string>> ListFilesInContainerAsync(string container) =>
        TryCatch(async () =>
        {
            ValidateContainerName(container);

            return await this.blobStorageBroker.ListContainerAsync(container);
        });



        public ValueTask<string> GetDownloadLinkAsync(string fileName, string container, DateTimeOffset expiresOn) =>
        TryCatch(async () =>
        {
            ValidateStorageArgumentsOnGetDownloadLink(fileName, container, expiresOn);

            return await this.blobStorageBroker.GetDownloadLinkAsync(fileName, container, expiresOn);
        });

        public ValueTask CreateContainerAsync(string container) =>
        TryCatch(async () =>
        {
            ValidateContainerName(container);
            await this.blobStorageBroker.CreateContainerAsync(container);
        });

        public ValueTask CreateDirectoryAsync(string container, string directory) =>
        TryCatch(async () =>
        {
            ValidateStorageArgumentsOnCreateDirectory(container, directory);
            await this.blobStorageBroker.CreateDirectoryAsync(container, directory);
        });

        public ValueTask CreateAndAssignAccessPoliciesToContainerAsync(string container, List<string> policyNames) =>
        TryCatch(async () =>
        {
            ValidateStorageArgumentsOnCreateAccessPolicy(container, policyNames);
            DateTimeOffset dateTimeOffset = await this.dateTimeBroker.GetCurrentDateTimeOffsetAsync();
            string timestamp = dateTimeOffset.ToString("yyyyMMddHHmmss");

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

        public ValueTask<string> CreateDirectorySasTokenAsync(
             string container, string directoryPath, string accessPolicyIdentifier, DateTimeOffset expiresOn) =>
        TryCatch(async () =>
        {
            ValidateStorageArgumentsOnCreateDirectorySasToken(
                container, directoryPath, accessPolicyIdentifier, expiresOn);

            DateTimeOffset dateTimeOffset = await this.dateTimeBroker.GetCurrentDateTimeOffsetAsync();

            var sasToken = await this.blobStorageBroker.GetSasTokenAsync(
                container, directoryPath, accessPolicyIdentifier, expiresOn);

            return sasToken;
        });

        public ValueTask<List<string>> RetrieveAllAccessPoliciesFromContainerAsync(string container) =>
        TryCatch(async () =>
        {
            ValidateStorageArgumentsOnRetrieveAllAccessPolicies(container);

            BlobContainerClient containerClient =
                this.blobStorageBroker.BlobServiceClient
                    .GetBlobContainerClient(container);

            BlobContainerAccessPolicy containerAccessPolicy = await containerClient.GetAccessPolicyAsync();
            List<string> signedIdentifiers = new List<string>();

            foreach (var signedIdentifier in containerAccessPolicy.SignedIdentifiers)
            {
                signedIdentifiers.Add(signedIdentifier.Id);
            }

            return signedIdentifiers;
        });

        public ValueTask RemoveAccessPoliciesFromContainerAsync(string container) =>
        TryCatch(async () =>
        {
            ValidateStorageArgumentsOnRemoveAccessPolicies(container);
            List<BlobSignedIdentifier> emptySignedIdentifiers = new List<BlobSignedIdentifier>();

            BlobContainerClient containerClient =
                this.blobStorageBroker.BlobServiceClient
                    .GetBlobContainerClient(container);

            await containerClient.SetAccessPolicyAsync(permissions: emptySignedIdentifiers);
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
