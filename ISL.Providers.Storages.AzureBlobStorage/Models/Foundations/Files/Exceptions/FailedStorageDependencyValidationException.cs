using System;
using Xeptions;

namespace ISL.Providers.Storages.AzureBlobStorage.Models.Foundations.Files.Exceptions
{
    public class FailedStorageDependencyValidationException : Xeption
    {
        public FailedStorageDependencyValidationException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}
