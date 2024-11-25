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
using System.Net.Http;
using System.Threading;

namespace ISL.Providers.Storages.AzureBlobStorage.Brokers.Storages.Blobs
{
    internal class BlobStorageBroker : IBlobStorageBroker
    {
        public BlobServiceClient BlobServiceClient { get; private set; }
        public DataLakeServiceClient DataLakeServiceClient { get; private set; }
        public int TokenLifetimeDays { get; private set; }

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

            this.BlobServiceClient = new BlobServiceClient(
                serviceUri: new Uri(azureBlobStoreConfigurations.ServiceUri),
                credential: new DefaultAzureCredential(
                    new DefaultAzureCredentialOptions
                    {
                        VisualStudioTenantId = azureBlobStoreConfigurations.AzureTenantId,
                    }),
                options: blobServiceClientOptions);

            this.DataLakeServiceClient = new DataLakeServiceClient(
                serviceUri: new Uri(azureBlobStoreConfigurations.ServiceUri),
                credential: new StorageSharedKeyCredential(azureBlobStoreConfigurations.StorageAccountName, azureBlobStoreConfigurations.StorageAccountAccessKey),
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

        public BlobUriBuilder GetBlobUriBuilder(Uri uri) =>
            new BlobUriBuilder(uri);
    }
}
