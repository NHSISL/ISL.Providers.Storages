using Xeptions;

namespace ISL.Providers.Storages.AzureBlobStorage.Models.Foundations.Files.Exceptions
{
    public class AccessPolicyNotFoundStorageException : Xeption
    {
        public AccessPolicyNotFoundStorageException(string message) : base(message)
        { }
    }
}
