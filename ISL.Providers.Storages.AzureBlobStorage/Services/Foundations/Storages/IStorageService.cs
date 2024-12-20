﻿// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using ISL.Providers.Storages.Abstractions.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace ISL.Providers.Storages.AzureBlobStorage.Services.Foundations.Storages
{
    internal interface IStorageService
    {
        ValueTask CreateFileAsync(Stream input, string fileName, string container);
        ValueTask RetrieveFileAsync(Stream output, string fileName, string container);
        ValueTask DeleteFileAsync(string fileName, string container);
        ValueTask<List<string>> ListFilesInContainerAsync(string container);
        ValueTask<string> GetDownloadLinkAsync(string fileName, string container, DateTimeOffset expiresOn);
        ValueTask CreateDirectoryAsync(string container, string directory);
        ValueTask CreateContainerAsync(string container);
        ValueTask<List<string>> RetrieveAllContainersAsync();
        ValueTask DeleteContainerAsync(string container);
        ValueTask CreateAndAssignAccessPoliciesAsync(string container, List<Policy> policies);

        ValueTask<string> CreateSasTokenAsync(
             string container,
             string path,
             string accessPolicyIdentifier,
             DateTimeOffset expiresOn);

        ValueTask<string> CreateSasTokenAsync(
            string container,
            string path,
            DateTimeOffset expiresOn,
            List<string> permissions);

        ValueTask<List<string>> RetrieveListOfAllAccessPoliciesAsync(string container);
        ValueTask<List<Policy>> RetrieveAllAccessPoliciesAsync(string container);
        ValueTask<Policy> RetrieveAccessPolicyByNameAsync(string container, string policyName);
        ValueTask RemoveAllAccessPoliciesAsync(string container);
        ValueTask RemoveAccessPolicyByNameAsync(string container, string policyName);
    }
}
