﻿// ---------------------------------------------------------
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
        public int TokenLifetimeDays { get; private set; }
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

        public BlobUriBuilder GetBlobUriBuilder(Uri uri) =>
            new BlobUriBuilder(uri);
    }
}
