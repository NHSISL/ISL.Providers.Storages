// ---------------------------------------------------------
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

        ValueTask CreateContainerAsync(string container);
        //ValueTask CreateAndAssignRoleToContainerAsync(string container, string roleName);
        //ValueTask CreateAndAssignManagedIdentityToRoleAsync(string identity, string roleName);
    }
}
