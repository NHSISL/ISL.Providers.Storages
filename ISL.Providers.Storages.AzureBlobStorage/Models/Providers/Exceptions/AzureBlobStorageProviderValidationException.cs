using ISL.Providers.Storages.Abstractions.Models.Exceptions;
using System.Collections;
using Xeptions;

namespace ISL.Providers.Storages.AzureBlobStorage.Models.Providers.Exceptions
{
    public class AzureBlobStorageProviderValidationException : Xeption, IStorageProviderValidationException
    {
        /// <summary>
        /// This exception is thrown when a validation error occurs while using the storage provider.
        /// For example, if required data is missing or invalid.
        /// </summary>
        public AzureBlobStorageProviderValidationException(string message, Xeption innerException, IDictionary data)
            : base(message: message, innerException, data)
        { }
    }
}
