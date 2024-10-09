using System;
using Xeptions;

namespace ISL.Providers.Storages.AzureBlobStorage.Models.Foundations.Files.Exceptions
{
    public class FailedStorageDependencyException : Xeption
    {
        public FailedStorageDependencyException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}
