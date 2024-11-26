using ISL.Providers.Storages.AzureBlobStorage.Providers.AzureBlobStorage;
using ISL.Providers.Storages.AzureBlobStorage.Services.Foundations.Files;
using Moq;
using System.IO;
using Tynamix.ObjectFiller;

namespace ISL.Providers.Storages.AzureBlobStorage.Tests.Unit.Providers.AzureBlobStorage
{
    public partial class AzureBlobStorageTests
    {
        private readonly Mock<IStorageService> storageServiceMock;
        private readonly AzureBlobStorageProvider azureBlobStorageProvider;

        public AzureBlobStorageTests()
        {
            this.storageServiceMock = new Mock<IStorageService>();
            this.azureBlobStorageProvider = new AzureBlobStorageProvider(storageServiceMock.Object);
        }

        private static string GetRandomString() =>
            new MnemonicString().GetValue();

        public class HasLengthStream : MemoryStream
        {
            public override long Length => 1;
        }
    }
}
