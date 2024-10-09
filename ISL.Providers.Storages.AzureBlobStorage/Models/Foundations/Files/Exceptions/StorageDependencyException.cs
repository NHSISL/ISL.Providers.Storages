using Xeptions;

namespace ISL.Providers.Storages.AzureBlobStorage.Models.Foundations.Files.Exceptions
{
    public class StorageDependencyException : Xeption
    {
        public StorageDependencyException(string message, Xeption innerException)
            : base(message, innerException)
        { }
    }
}
