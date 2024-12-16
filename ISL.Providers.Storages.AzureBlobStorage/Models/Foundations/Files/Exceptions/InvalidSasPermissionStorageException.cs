// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using Xeptions;

namespace ISL.Providers.Storages.AzureBlobStorage.Models.Foundations.Files.Exceptions
{
    public class InvalidSasPermissionStorageException : Xeption
    {
        public InvalidSasPermissionStorageException(string message)
            : base(message)
        { }
    }
}
