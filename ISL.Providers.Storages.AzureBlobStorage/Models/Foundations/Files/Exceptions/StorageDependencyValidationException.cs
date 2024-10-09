using System;
using Xeptions;

namespace ISL.Providers.Storages.AzureBlobStorage.Models.Foundations.Files.Exceptions
{
    public class StorageDependencyValidationException : Xeption
    {
        public StorageDependencyValidationException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}
