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
        BlobContainerClient GetBlobContainerClient(string container);
        DataLakeFileSystemClient GetDataLakeFileSystemClient(string container);
        BlobClient GetBlobClient(BlobContainerClient blobContainerClient, string fileName);

        BlobSasBuilder GetBlobSasBuilder(
            string blobName,
            string blobContainerName,
            DateTimeOffset startsOn,
            DateTimeOffset expiresOn);

        ValueTask CreateFileAsync(BlobClient blobClient, Stream input);
        ValueTask RetrieveFileAsync(BlobClient blobClient, Stream output);
        ValueTask DeleteFileAsync(BlobClient blobClient);

        ValueTask<string> GetDownloadLinkAsync(
            BlobClient blobClient,
            BlobSasBuilder blobSasBuilder,
            DateTimeOffset expiresOn);

        ValueTask CreateDirectoryAsync(DataLakeFileSystemClient dataLakeFileSystemClient, string directory);
        ValueTask CreateContainerAsync(string container);
        ValueTask<AsyncPageable<BlobContainerItem>> RetrieveAllContainersAsync();
        ValueTask DeleteContainerAsync(string container);
        ValueTask<AsyncPageable<BlobItem>> GetBlobsAsync(BlobContainerClient blobContainerClient);

        ValueTask AssignAccessPoliciesToContainerAsync(
            BlobContainerClient blobContainerClient,
            List<BlobSignedIdentifier> signedIdentifiers);

        ValueTask<BlobContainerAccessPolicy> GetAccessPolicyAsync(BlobContainerClient blobContainerClient);
        ValueTask RemoveAllAccessPoliciesAsync(BlobContainerClient containerClient);

        ValueTask<string> CreateSasTokenAsync(
            string container,
            string path,
            string accessPolicyIdentifier,
            DateTimeOffset expiresOn,
            bool isDirectory,
            string resource);

        ValueTask<string> CreateSasTokenAsync(
            string container,
            string path,
            DateTimeOffset expiresOn,
            string permissionsString,
            bool isDirectory,
            string resource);
    }
}
