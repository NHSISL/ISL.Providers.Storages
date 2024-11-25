using ISL.Providers.Storages.Abstractions.Models.Exceptions;
using Xeptions;

namespace ISL.Providers.Storages.AzureBlobStorage.Models.Providers.Exceptions
{
    public class AzureBlobStorageProviderServiceException : Xeption, IStorageProviderServiceException
    {
        /// <summary>
        /// This exception is thrown when a service error occurs while using the storage provider. 
        /// For example, if there is a problem with the server or any other service failure.
        /// </summary>
        public AzureBlobStorageProviderServiceException(string message, Xeption innerException)
            : base(message, innerException)
        { }
    }
}
