// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.IO;
using System.Threading.Tasks;

namespace ISL.Providers.Storages.Abstractions
{
    public interface IStoragesOperations
    {
        ValueTask CreateFileAsync(Stream input, string fileName, string container);
        ValueTask RetrieveFileAsync(Stream output, string fileName, string container);
        ValueTask DeleteFileAsync(string fileName, string container);
        ValueTask<string> GetDownloadLinkAsync(string fileName, string container, DateTimeOffset expiresOn);

        ValueTask CreateContainerAsync(string container);
        ValueTask ListContainerAsync(string container);

        ValueTask GetContainerACLAsync(string container);
        ValueTask SetContainerACLAsync(string container);

        ValueTask CreateAndAssignContainerRoleAsync(string container, string roleName);
        ValueTask CreateAndAssignManagedIdentityToRoleAsync(string identity, string roleName);
    }
}
