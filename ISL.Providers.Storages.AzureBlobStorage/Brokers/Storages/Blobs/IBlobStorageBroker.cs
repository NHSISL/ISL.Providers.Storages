// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using Azure;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Files.DataLake;
using Azure.Storage.Sas;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace ISL.Providers.Storages.AzureBlobStorage.Brokers.Storages.Blobs
{
    internal interface IBlobStorageBroker
    {
        int TokenLifetimeDays { get; }
        BlobContainerClient GetBlobContainerClient(string container);
        DataLakeFileSystemClient GetDataLakeFileSystemClient(string container);
        BlobClient GetBlobClient(BlobContainerClient blobContainerClient, string fileName);
        BlobSasBuilder GetBlobSasBuilder(string blobName, string blobContainerName, DateTimeOffset expiresOn);
        ValueTask CreateFileAsync(BlobClient blobClient, Stream input);
        ValueTask RetrieveFileAsync(BlobClient blobClient, Stream output);
        ValueTask DeleteFileAsync(BlobClient blobClient);

        ValueTask<string> GetDownloadLinkAsync(
            BlobClient blobClient, BlobSasBuilder blobSasBuilder, DateTimeOffset expiresOn);

        ValueTask CreateDirectoryAsync(DataLakeFileSystemClient dataLakeFileSystemClient, string directory);
        ValueTask CreateContainerAsync(string container);
        ValueTask<AsyncPageable<BlobContainerItem>> RetrieveAllContainersAsync();
        ValueTask<AsyncPageable<BlobItem>> GetBlobsAsync(BlobContainerClient blobContainerClient);

        ValueTask AssignAccessPoliciesToContainerAsync(
            BlobContainerClient blobContainerClient, List<BlobSignedIdentifier> signedIdentifiers);

        ValueTask<BlobContainerAccessPolicy> GetAccessPolicyAsync(BlobContainerClient blobContainerClient);
        ValueTask RemoveAccessPoliciesFromContainerAsync(BlobContainerClient containerClient);

        ValueTask<string> CreateDirectorySasTokenAsync(
            string container,
            string directoryPath,
            string accessPolicyIdentifier,
            DateTimeOffset expiresOn);
    }
}
