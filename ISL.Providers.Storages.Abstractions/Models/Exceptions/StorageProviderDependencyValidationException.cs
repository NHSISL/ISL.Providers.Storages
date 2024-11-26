using System.Collections;
using Xeptions;

namespace ISL.Providers.Storages.Abstractions.Models.Exceptions
{
    public class StorageProviderDependencyValidationException : Xeption
    {
        public StorageProviderDependencyValidationException(string message, Xeption innerException)
        : base(message, innerException)
        { }

        public StorageProviderDependencyValidationException(string message, Xeption innerException, IDictionary data)
            : base(message, innerException, data)
        { }
    }
}
