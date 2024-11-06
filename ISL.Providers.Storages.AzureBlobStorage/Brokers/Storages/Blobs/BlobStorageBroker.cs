// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using Azure;
using Azure.Core.Pipeline;
using Azure.Identity;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
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
        //private readonly DataLakeServiceClient dataLakeServiceClient;
        public BlobStorageBroker(AzureBlobStoreConfigurations azureBlobStoreConfigurations)
        {
            var blobServiceClientOptions = new BlobClientOptions()
            {
                Transport = new HttpClientTransport(new HttpClient { Timeout = new TimeSpan(1, 0, 0) }),
                Retry = { NetworkTimeout = new TimeSpan(1, 0, 0) },
                EnableTenantDiscovery = true
            };

            this.BlobServiceClient = new BlobServiceClient(
                    serviceUri: new Uri(azureBlobStoreConfigurations.ServiceUri),
                    credential: new DefaultAzureCredential());
            //credential: new DefaultAzureCredential(
            //        new DefaultAzureCredentialOptions
            //        {
            //            VisualStudioTenantId = azureBlobStoreConfigurations.SubscriptionId,
            //        }),
            //        options: blobServiceClientOptions);
        }

        public Response<UserDelegationKey> GetUserDelegationKey(
            DateTimeOffset? startsOn,
            DateTimeOffset expiresOn,
            CancellationToken cancellationToken = default) =>
            this.BlobServiceClient.GetUserDelegationKey(DateTime.UtcNow, DateTime.UtcNow.AddMinutes(45));

        public BlobSasBuilder GetBlobSasBuilder(string path, string blobContainerName, DateTimeOffset expiresOn)
        {
            var blobSasBuilder = new BlobSasBuilder()
            {
                BlobContainerName = blobContainerName,
                BlobName = path,
                Resource = "d",
                StartsOn = DateTimeOffset.UtcNow,
                ExpiresOn = expiresOn
            };

            blobSasBuilder.SetPermissions(BlobSasPermissions.Read | BlobSasPermissions.List);

            return blobSasBuilder;
        }

        public BlobSasBuilder GetBlobSasBuilder(string path, string blobContainerName, DateTimeOffset expiresOn, string permissions)
        {
            var blobSasBuilder = new BlobSasBuilder()
            {
                BlobContainerName = blobContainerName,
                BlobName = path,
                Resource = "d",
                StartsOn = DateTimeOffset.UtcNow,
                ExpiresOn = expiresOn
            };

            //blobSasBuilder.SetPermissions(permissions switch
            //{
            //    "read" => BlobSasPermissions.Read | BlobSasPermissions.List,
            //    "write" => BlobSasPermissions.Write,
            //    "full" => BlobSasPermissions.Read | BlobSasPermissions.Write | BlobSasPermissions.List,
            //    _ => BlobSasPermissions.Read
            //});

            blobSasBuilder.SetPermissions(BlobSasPermissions.Read | BlobSasPermissions.List);

            return blobSasBuilder;
        }

        public BlobUriBuilder GetBlobUriBuilder(Uri uri) =>
            new BlobUriBuilder(uri);
    }
}
