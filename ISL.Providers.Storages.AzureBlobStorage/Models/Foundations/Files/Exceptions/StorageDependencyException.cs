using System;
using Xeptions;

namespace ISL.Providers.Storages.AzureBlobStorage.Models.Foundations.Files.Exceptions
{
    public class StorageDependencyException : Xeption
    {
        public StorageDependencyException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}
