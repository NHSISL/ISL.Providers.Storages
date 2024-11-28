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
        public BlobServiceClient BlobServiceClient { get; private set; }
        public DataLakeServiceClient DataLakeServiceClient { get; private set; }
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

        public Response<UserDelegationKey> GetUserDelegationKey(
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

        public async ValueTask CreateFileAsync(Stream input, string fileName, string container)
        {
            BlobClient blobClient = BlobServiceClient
                .GetBlobContainerClient(container)
                .GetBlobClient(fileName);

            await blobClient.UploadAsync(input);
        }

        public async ValueTask RetrieveFileAsync(Stream output, string fileName, string container)
        {
            BlobClient blobClient = BlobServiceClient
                .GetBlobContainerClient(container)
                .GetBlobClient(fileName);

            await blobClient.DownloadToAsync(output);
        }

        public async ValueTask DeleteFileAsync(string fileName, string container)
        {
            BlobClient blobClient = BlobServiceClient
                .GetBlobContainerClient(container)
                .GetBlobClient(fileName);

            await blobClient.DeleteAsync(DeleteSnapshotsOption.None);
        }

        public async ValueTask CreateContainerAsync(string container) =>
            await BlobServiceClient.CreateBlobContainerAsync(container);

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

        public async ValueTask CreateDirectoryAsync(string container, string directory)
        {
            DataLakeFileSystemClient dataLakeFileSystemClient = DataLakeServiceClient.GetFileSystemClient(container);
            await dataLakeFileSystemClient.CreateDirectoryAsync(directory);
        }

        public async ValueTask CreateAndAssignAccessPoliciesToContainerAsync(
            string container, List<string> policyNames, DateTimeOffset currentDateTimeOffset)
        {
            string timestamp = currentDateTimeOffset.ToString("yyyyMMddHHmmss");

            BlobContainerClient containerClient = BlobServiceClient
                .GetBlobContainerClient(container);

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

            await containerClient.SetAccessPolicyAsync(permissions: signedIdentifiers);
        }

        public async ValueTask<List<string>> RetrieveAllAccessPoliciesFromContainerAsync(string container)
        {
            BlobContainerClient containerClient = BlobServiceClient
                .GetBlobContainerClient(container);

            BlobContainerAccessPolicy containerAccessPolicy = await containerClient.GetAccessPolicyAsync();
            List<string> signedIdentifiers = new List<string>();

            foreach (var signedIdentifier in containerAccessPolicy.SignedIdentifiers)
            {
                signedIdentifiers.Add(signedIdentifier.Id);
            }

            return signedIdentifiers;
        }

        public async ValueTask RemoveAccessPoliciesFromContainerAsync(string container)
        {
            List<BlobSignedIdentifier> emptySignedIdentifiers = new List<BlobSignedIdentifier>();

            BlobContainerClient containerClient = BlobServiceClient
                .GetBlobContainerClient(container);

            await containerClient.SetAccessPolicyAsync(permissions: emptySignedIdentifiers);
        }

        public async ValueTask<string> GetDownloadLinkAsync(
            string fileName, string container, DateTimeOffset expiresOn)
        {
            BlobClient blobClient = BlobServiceClient
                    .GetBlobContainerClient(container)
                    .GetBlobClient(fileName);

            var userDelegationKey = BlobServiceClient.GetUserDelegationKey(DateTimeOffset.UtcNow, expiresOn);
            var sasBuilder = GetBlobSasBuilder(fileName, container, expiresOn);

            var blobUriBuilder = new BlobUriBuilder(blobClient.Uri)
            {
                Sas = sasBuilder.ToSasQueryParameters(userDelegationKey, BlobServiceClient.AccountName)
            };

            return blobUriBuilder.ToUri().ToString();
        }

        public async ValueTask<string> GetSasTokenAsync(
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
