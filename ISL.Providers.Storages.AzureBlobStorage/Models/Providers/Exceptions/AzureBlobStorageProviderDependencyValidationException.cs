using ISL.Providers.Storages.Abstractions.Models.Exceptions;
using System.Collections;
using Xeptions;

namespace ISL.Providers.Storages.AzureBlobStorage.Models.Providers.Exceptions
{
    public class AzureBlobStorageProviderDependencyValidationException : Xeption, IStorageProviderDependencyValidationException
    {
        /// <summary>
        /// This exception is thrown when a dependency validation error occurs while using the storage provider.
        /// For example, if an external dependency used by the provider requires data that is missing or invalid.
        /// </summary>
        public AzureBlobStorageProviderDependencyValidationException(string message, Xeption innerException, IDictionary data)
            : base(message: message, innerException, data)
        { }
    }
}
