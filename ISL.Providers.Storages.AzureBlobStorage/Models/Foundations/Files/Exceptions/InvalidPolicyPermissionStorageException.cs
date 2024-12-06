// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using Xeptions;

namespace ISL.Providers.Storages.AzureBlobStorage.Models.Foundations.Files.Exceptions
{
    public class InvalidPolicyPermissionStorageException : Xeption
    {
        public InvalidPolicyPermissionStorageException(string message)
            : base(message)
        { }
    }
}
