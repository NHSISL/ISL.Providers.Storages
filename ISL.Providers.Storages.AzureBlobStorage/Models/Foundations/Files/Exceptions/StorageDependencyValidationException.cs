using Xeptions;

namespace ISL.Providers.Storages.AzureBlobStorage.Models.Foundations.Files.Exceptions
{
    public class StorageDependencyValidationException : Xeption
    {
        public StorageDependencyValidationException(string message, Xeption innerException)
            : base(message, innerException)
        { }
    }
}
