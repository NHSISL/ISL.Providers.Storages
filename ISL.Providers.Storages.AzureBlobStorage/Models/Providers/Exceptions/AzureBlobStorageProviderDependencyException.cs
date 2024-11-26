using ISL.Providers.Storages.Abstractions.Models.Exceptions;
using Xeptions;

namespace ISL.Providers.Storages.AzureBlobStorage.Models.Providers.Exceptions
{
    public class AzureBlobStorageProviderDependencyException : Xeption, IStorageProviderDependencyException
    {
        /// <summary>
        /// This exception is thrown when a dependency error occurs while using the storage provider.
        /// For example, if a required dependency is unavailable or incompatible.
        /// </summary>
        public AzureBlobStorageProviderDependencyException(string message, Xeption innerException)
            : base(message: message, innerException)
        { }
    }
}
