﻿// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

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
        ValueTask CreateAndAssignAccessPoliciesToContainerAsync(string container, List<string> policyNames);

        ValueTask<string> CreateDirectorySasTokenAsync(
             string container, string directoryPath, string accessPolicyIdentifier, DateTimeOffset expiresOn);

        ValueTask<List<string>> RetrieveAllAccessPoliciesFromContainerAsync(string container);
        ValueTask RemoveAccessPoliciesFromContainerAsync(string container);
    }
}