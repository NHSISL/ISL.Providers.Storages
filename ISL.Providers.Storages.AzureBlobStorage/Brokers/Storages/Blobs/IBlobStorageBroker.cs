// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using Azure;
using Azure.Storage;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Files.DataLake;
using Azure.Storage.Sas;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ISL.Providers.Storages.AzureBlobStorage.Brokers.Storages.Blobs
{
    internal interface IBlobStorageBroker
    {
        BlobServiceClient BlobServiceClient { get; }
        DataLakeServiceClient DataLakeServiceClient { get; }
        int TokenLifetimeDays { get; }
        StorageSharedKeyCredential StorageSharedKeyCredential { get; }

        Response<UserDelegationKey> GetUserDelegationKey(
            DateTimeOffset? startsOn,
            DateTimeOffset expiresOn,
            CancellationToken cancellationToken = default(CancellationToken));


        BlobSasBuilder GetBlobSasBuilder(
            string blobName,
            string blobContainerName,
            DateTimeOffset expiresOn);

        ValueTask<string> GetSasTokenAsync(
            string container,
            string directoryPath,
            string accessPolicyIdentifier,
            DateTimeOffset expiresOn);

        BlobUriBuilder GetBlobUriBuilder(Uri uri);
    }
}
