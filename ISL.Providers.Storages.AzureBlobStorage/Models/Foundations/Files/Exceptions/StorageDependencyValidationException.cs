// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using Xeptions;

namespace ISL.Providers.Storages.AzureBlobStorage.Models.Foundations.Files.Exceptions
{
    public class StorageDependencyValidationException : Xeption
    {
        public StorageDependencyValidationException(string message, Xeption innerException)
            : base(message, innerException)
        { }
    }
}
