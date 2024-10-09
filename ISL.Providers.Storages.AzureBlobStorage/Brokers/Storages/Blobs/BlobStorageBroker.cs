﻿// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using Azure;
using Azure.Core.Pipeline;
using Azure.Identity;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using ISL.Providers.Storages.AzureBlobStorage.Models;
using System;
using System.Net.Http;
using System.Threading;

namespace ISL.Providers.Storages.AzureBlobStorage.Brokers.Storages.Blobs
{
    internal class BlobStorageBroker : IBlobStorageBroker
    {
        public BlobServiceClient blobServiceClient { get; private set; }

        public BlobStorageBroker(AzureBlobStoreConfigurations azureBlobStoreConfigurations)
        {
            var blobServiceClientOptions = new BlobClientOptions()
            {
                Transport = new HttpClientTransport(new HttpClient { Timeout = new TimeSpan(1, 0, 0) }),
                Retry = { NetworkTimeout = new TimeSpan(1, 0, 0) },
                EnableTenantDiscovery = true
            };

            this.blobServiceClient = new BlobServiceClient(
                    serviceUri: new Uri(azureBlobStoreConfigurations.ServiceUri),
                    credential: new DefaultAzureCredential(
                        new DefaultAzureCredentialOptions
                        {
                            VisualStudioTenantId = azureBlobStoreConfigurations.AzureTenantId,
                        }),
                    options: blobServiceClientOptions);
        }

        public Response<UserDelegationKey> GetUserDelegationKey(
            DateTimeOffset? startsOn,
            DateTimeOffset expiresOn,
            CancellationToken cancellationToken = default) =>
            blobServiceClient.GetUserDelegationKey(DateTimeOffset.UtcNow, expiresOn, cancellationToken);
    }
}
