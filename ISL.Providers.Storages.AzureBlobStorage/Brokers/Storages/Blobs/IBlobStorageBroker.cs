// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using Azure;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
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
        BlobClient GetBlobClient(BlobContainerClient blobContainerClient, string fileName);
        BlobSasBuilder GetBlobSasBuilder(string blobName, string blobContainerName, DateTimeOffset expiresOn);
        ValueTask CreateFileAsync(BlobClient blobClient, Stream input);
        ValueTask RetrieveFileAsync(BlobClient blobClient, Stream output);
        ValueTask DeleteFileAsync(BlobClient blobClient);
        ValueTask CreateContainerAsync(string container);
        ValueTask CreateDirectoryAsync(string container, string directory);
        ValueTask<AsyncPageable<BlobItem>> GetBlobsAsync(BlobContainerClient blobContainerClient);

        ValueTask<string> GetDownloadLinkAsync(
            BlobClient blobClient, BlobSasBuilder blobSasBuilder, DateTimeOffset expiresOn);

        ValueTask CreateAndAssignAccessPoliciesToContainerAsync(
            string container, List<string> policyNames, DateTimeOffset currentDateTimeOffset);

        ValueTask<List<string>> RetrieveAllAccessPoliciesFromContainerAsync(string container);
        ValueTask RemoveAccessPoliciesFromContainerAsync(string container);

        ValueTask<string> CreateDirectorySasTokenAsync(
            string container,
            string directoryPath,
            string accessPolicyIdentifier,
            DateTimeOffset expiresOn);
    }
}
