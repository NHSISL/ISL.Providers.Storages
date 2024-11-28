﻿// ---------------------------------------------------------
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
using System.Threading;
using System.Threading.Tasks;

namespace ISL.Providers.Storages.AzureBlobStorage.Brokers.Storages.Blobs
{
    internal interface IBlobStorageBroker
    {
        BlobServiceClient BlobServiceClient { get; }
        DataLakeServiceClient DataLakeServiceClient { get; }

        Response<UserDelegationKey> GetUserDelegationKey(
            DateTimeOffset? startsOn,
            DateTimeOffset expiresOn,
            CancellationToken cancellationToken = default(CancellationToken));

        BlobSasBuilder GetBlobSasBuilder(
            string blobName,
            string blobContainerName,
            DateTimeOffset expiresOn);

        ValueTask CreateFileAsync(Stream input, string fileName, string container);
        ValueTask RetrieveFileAsync(Stream output, string fileName, string container);
        ValueTask DeleteFileAsync(string fileName, string container);
        ValueTask CreateContainerAsync(string container);
        ValueTask CreateDirectoryAsync(string container, string directory);
        ValueTask<List<string>> ListContainerAsync(string container);
        ValueTask<string> GetDownloadLinkAsync(string fileName, string container, DateTimeOffset expiresOn);

        ValueTask CreateAndAssignAccessPoliciesToContainerAsync(
            string container, List<string> policyNames, DateTimeOffset currentDateTimeOffset);

        ValueTask<List<string>> RetrieveAllAccessPoliciesFromContainerAsync(string container);
        ValueTask RemoveAccessPoliciesFromContainerAsync(string container);

        ValueTask<string> GetSasTokenAsync(
            string container,
            string directoryPath,
            string accessPolicyIdentifier,
            DateTimeOffset expiresOn);
    }
}
