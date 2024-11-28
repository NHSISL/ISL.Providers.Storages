// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using Azure;
using Azure.Core.Pipeline;
using Azure.Identity;
using Azure.Storage;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Files.DataLake;
using Azure.Storage.Sas;
using ISL.Providers.Storages.AzureBlobStorage.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace ISL.Providers.Storages.AzureBlobStorage.Brokers.Storages.Blobs
{
    internal class BlobStorageBroker : IBlobStorageBroker
    {
        private BlobServiceClient BlobServiceClient { get; set; }
        private DataLakeServiceClient DataLakeServiceClient { get; set; }
        private int TokenLifetimeDays { get; set; }
        private StorageSharedKeyCredential StorageSharedKeyCredential { get; set; }

        public BlobStorageBroker(AzureBlobStoreConfigurations azureBlobStoreConfigurations)
        {
            var blobServiceClientOptions = new BlobClientOptions()
            {
                Transport = new HttpClientTransport(new HttpClient { Timeout = new TimeSpan(1, 0, 0) }),
                Retry = { NetworkTimeout = new TimeSpan(1, 0, 0) },
                EnableTenantDiscovery = true
            };

            var dataLakeClientOptions = new DataLakeClientOptions()
            {
                Transport = new HttpClientTransport(new HttpClient { Timeout = new TimeSpan(1, 0, 0) }),
                Retry = { NetworkTimeout = new TimeSpan(1, 0, 0) },
                EnableTenantDiscovery = true
            };

            this.StorageSharedKeyCredential = new StorageSharedKeyCredential(
                accountName: azureBlobStoreConfigurations.StorageAccountName,
                accountKey: azureBlobStoreConfigurations.StorageAccountAccessKey);

            this.BlobServiceClient = new BlobServiceClient(
                serviceUri: new Uri(azureBlobStoreConfigurations.ServiceUri),
                credential: new DefaultAzureCredential(),
                options: blobServiceClientOptions);

            this.DataLakeServiceClient = new DataLakeServiceClient(
                serviceUri: new Uri(azureBlobStoreConfigurations.ServiceUri),
                credential: this.StorageSharedKeyCredential,
                dataLakeClientOptions);

            this.TokenLifetimeDays = azureBlobStoreConfigurations.TokenLifetimeDays;
        }

        public BlobContainerClient GetBlobContainerClient(string container) =>
            BlobServiceClient.GetBlobContainerClient(container);

        public DataLakeFileSystemClient GetDataLakeFileSystemClient(string container) =>
            DataLakeServiceClient.GetFileSystemClient(container);

        public BlobClient GetBlobClient(
            BlobContainerClient blobContainerClient, string fileName) =>
            blobContainerClient.GetBlobClient(fileName);

        public async ValueTask CreateFileAsync(BlobClient blobClient, Stream input) =>
            await blobClient.UploadAsync(input);

        public async ValueTask RetrieveFileAsync(BlobClient blobClient, Stream output) =>
            await blobClient.DownloadToAsync(output);

        public async ValueTask DeleteFileAsync(BlobClient blobClient) =>
            await blobClient.DeleteAsync(DeleteSnapshotsOption.None);

        public async ValueTask CreateContainerAsync(string container) =>
            await BlobServiceClient.CreateBlobContainerAsync(container);

        public async ValueTask<AsyncPageable<BlobItem>> GetBlobsAsync(BlobContainerClient blobContainerClient) =>
            blobContainerClient.GetBlobsAsync();


        public async ValueTask<List<string>> ListContainerAsync(string container)
        {
            List<string> fileNames = new List<string>();

            BlobContainerClient containerClient = BlobServiceClient
                .GetBlobContainerClient(container);

            AsyncPageable<BlobItem> blobItems = containerClient.GetBlobsAsync();

            await foreach (BlobItem blobItem in blobItems)
            {
                fileNames.Add(blobItem.Name);
            }

            return fileNames;
        }

        public async ValueTask CreateDirectoryAsync(
            DataLakeFileSystemClient dataLakeFileSystemClient, string directory) =>
            await dataLakeFileSystemClient.CreateDirectoryAsync(directory);

        public async ValueTask<List<BlobSignedIdentifier>> CreateAccessPoliciesAsync(
            List<string> policyNames, DateTimeOffset currentDateTimeOffset)
        {
            string timestamp = currentDateTimeOffset.ToString("yyyyMMddHHmmss");

            List<BlobSignedIdentifier> signedIdentifiers = new List<BlobSignedIdentifier>();

            foreach (string policyName in policyNames)
            {
                string permissions = ConvertPolicyNameToPermissions(policyName.ToLower());

                var blobSignedIdentifier = new BlobSignedIdentifier
                {
                    Id = $"{policyName}_{timestamp}",
                    AccessPolicy = new BlobAccessPolicy
                    {
                        PolicyStartsOn = currentDateTimeOffset,
                        PolicyExpiresOn = currentDateTimeOffset.AddDays(TokenLifetimeDays),
                        Permissions = permissions
                    }
                };

                signedIdentifiers.Add(blobSignedIdentifier);
            }

            return signedIdentifiers;
        }

        public async ValueTask AssignAccessPoliciesToContainerAsync(
            BlobContainerClient blobContainerClient, List<BlobSignedIdentifier> signedIdentifiers) =>
            await blobContainerClient.SetAccessPolicyAsync(permissions: signedIdentifiers);

        public async ValueTask<BlobContainerAccessPolicy> GetAccessPolicyAsync(
            BlobContainerClient blobContainerClient) =>
            await blobContainerClient.GetAccessPolicyAsync();

        public async ValueTask RemoveAccessPoliciesFromContainerAsync(BlobContainerClient containerClient)
        {
            List<BlobSignedIdentifier> emptySignedIdentifiers = new List<BlobSignedIdentifier>();
            await containerClient.SetAccessPolicyAsync(permissions: emptySignedIdentifiers);
        }

        public async ValueTask<string> GetDownloadLinkAsync(
            BlobClient blobClient, BlobSasBuilder blobSasBuilder, DateTimeOffset expiresOn)
        {
            var userDelegationKey = GetUserDelegationKey(DateTimeOffset.UtcNow, expiresOn);

            var blobUriBuilder = new BlobUriBuilder(blobClient.Uri)
            {
                Sas = blobSasBuilder.ToSasQueryParameters(userDelegationKey, BlobServiceClient.AccountName)
            };

            return blobUriBuilder.ToUri().ToString();
        }

        public async ValueTask<string> CreateDirectorySasTokenAsync(
            string container, string directoryPath, string accessPolicyIdentifier, DateTimeOffset expiresOn)
        {
            var directorySasBuilder = new DataLakeSasBuilder()
            {
                Identifier = accessPolicyIdentifier,
                Resource = "d",
                Path = directoryPath,
                IsDirectory = true,
                FileSystemName = container,
                StartsOn = DateTimeOffset.UtcNow,
                ExpiresOn = expiresOn
            };

            var sasQueryParameters = directorySasBuilder.ToSasQueryParameters(
                StorageSharedKeyCredential);

            return sasQueryParameters.ToString();
        }

        private Response<UserDelegationKey> GetUserDelegationKey(
            DateTimeOffset? startsOn,
            DateTimeOffset expiresOn,
            CancellationToken cancellationToken = default) =>
            BlobServiceClient.GetUserDelegationKey(DateTimeOffset.UtcNow, expiresOn, cancellationToken);

        public BlobSasBuilder GetBlobSasBuilder(string blobName, string blobContainerName, DateTimeOffset expiresOn)
        {
            var blobSasBuilder = new BlobSasBuilder()
            {
                BlobContainerName = blobContainerName,
                BlobName = blobName,
                Resource = "b",
                StartsOn = DateTimeOffset.UtcNow,
                ExpiresOn = expiresOn
            };

            blobSasBuilder.SetPermissions(BlobSasPermissions.Read);

            return blobSasBuilder;
        }

        private string ConvertPolicyNameToPermissions(string policyName) => policyName switch
        {
            "read" => "rl",
            "write" => "w",
            "delete" => "d",
            "fullaccess" => "rlwd",
            _ => ""
        };
    }
}
