using Xeptions;

namespace ISL.Providers.Storages.AzureBlobStorage.Models.Foundations.Files.Exceptions
{
    public class InvalidArgumentFileException : Xeption
    {
        public InvalidArgumentFileException(string message)
            : base(message)
        { }
    }
}
