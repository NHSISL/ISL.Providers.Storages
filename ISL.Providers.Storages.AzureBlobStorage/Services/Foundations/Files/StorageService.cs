// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using Azure;
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
                        PolicyExpiresOn = dateTimeOffset.AddYears(this.blobStorageBroker.TokenLifetimeYears),
                        Permissions = permissions
                    }
                };

                signedIdentifiers.Add(blobSignedIdentifier);
            }

            await containerClient.SetAccessPolicyAsync(permissions: signedIdentifiers);
        });

        public async ValueTask<List<string>> RetrieveAllAccessPoliciesFromContainerAsync(string container)
        {
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
        }
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

        virtual internal string ConvertPolicyNameToPermissions(string policyName)
        {
            if (policyName == "reader")
            {
                return "rl";
            }
            else if (policyName == "writer")
            {
                return "w";
            }
            else
            {
                return "";
            }
        }
    }
}
