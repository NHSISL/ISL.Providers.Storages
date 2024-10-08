using Xeptions;

namespace ISL.Providers.Storages.AzureBlobStorage.Models.Foundations.Files.Exceptions
{
    public class FileValidationException : Xeption
    {
        public FileValidationException(string message, Xeption innerException)
            : base(message, innerException)
        { }
    }
}
