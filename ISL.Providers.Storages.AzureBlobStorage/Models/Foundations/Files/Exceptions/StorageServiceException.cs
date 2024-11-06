// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using Xeptions;

namespace ISL.Providers.Storages.AzureBlobStorage.Models.Foundations.Files.Exceptions
{
    public class StorageServiceException : Xeption
    {
        public StorageServiceException(string message, Xeption innerException)
            : base(message, innerException)
        { }
    }
}
