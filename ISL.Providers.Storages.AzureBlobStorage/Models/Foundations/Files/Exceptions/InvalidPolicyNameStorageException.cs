// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using Xeptions;

namespace ISL.Providers.Storages.AzureBlobStorage.Models.Foundations.Files.Exceptions
{
    public class InvalidPolicyNameStorageException : Xeption
    {
        public InvalidPolicyNameStorageException(string message)
            : base(message)
        { }
    }
}
