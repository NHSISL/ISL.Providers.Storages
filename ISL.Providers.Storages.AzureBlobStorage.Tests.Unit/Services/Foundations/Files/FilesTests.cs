using Azure.Storage.Blobs;
using ISL.Providers.Storages.AzureBlobStorage.Brokers.Storages.Blobs;
using ISL.Providers.Storages.AzureBlobStorage.Services.Foundations.Storages;
using Moq;
using System.IO;
using Tynamix.ObjectFiller;

namespace ISL.Providers.Storages.AzureBlobStorage.Tests.Unit.Services.Foundations.Files
{
    public partial class FilesTests
    {
        private readonly Mock<IBlobStorageBroker> blobStorageBrokerMock;
        private readonly Mock<BlobServiceClient> blobServiceClientMock;
        private readonly Mock<BlobContainerClient> blobContainerClientMock;
        private readonly Mock<BlobClient> blobClientMock;
        private readonly StorageService storageService;


        public FilesTests()
        {
            this.blobStorageBrokerMock = new Mock<IBlobStorageBroker>();
            this.blobServiceClientMock = new Mock<BlobServiceClient>();
            this.blobContainerClientMock = new Mock<BlobContainerClient>();
            this.blobClientMock = new Mock<BlobClient>();

            this.blobStorageBrokerMock.Setup(broker =>
                broker.BlobServiceClient)
                    .Returns(blobServiceClientMock.Object);

            this.storageService = new StorageService(
                this.blobStorageBrokerMock.Object);
        }

        private static string GetRandomStringWithLength(int length) =>
            new MnemonicString(wordCount: 1, wordMinLength: length, wordMaxLength: length).GetValue();

        private static string GetRandomString() =>
            new MnemonicString().GetValue();

        public class TestStream : MemoryStream
        {
            public override long Length => 0;
        }

        public static TheoryData<Stream, string> GetStreamLengthZero()
        {
            Stream stream = new TestStream();

            return new TheoryData<Stream, string>
            {
                { stream, " " }
            };
        }

    }
}
