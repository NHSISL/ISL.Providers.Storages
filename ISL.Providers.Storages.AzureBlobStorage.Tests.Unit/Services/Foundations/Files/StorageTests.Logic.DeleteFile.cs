using Azure.Storage.Blobs.Models;
using Moq;
using System.Threading.Tasks;

namespace ISL.Providers.Storages.AzureBlobStorage.Tests.Unit.Services.Foundations.Files
{
    public partial class StorageTests
    {
        [Fact]
        public async Task ShouldDeleteFileAsync()
        {
            // given
            string randomFileName = GetRandomString();
            string randomContainer = GetRandomString();
            string inputFileName = randomFileName;
            string inputContainer = randomContainer;

            this.blobServiceClientMock.Setup(client =>
                client.GetBlobContainerClient(inputContainer))
                    .Returns(blobContainerClientMock.Object);

            this.blobContainerClientMock.Setup(client =>
                client.GetBlobClient(inputFileName))
                    .Returns(blobClientMock.Object);

            // when
            await this.storageService.DeleteFileAsync(inputFileName, inputContainer);

            // then
            this.blobServiceClientMock.Verify(client =>
                client.GetBlobContainerClient(inputContainer),
                    Times.Once);

            this.blobContainerClientMock.Verify(client =>
                client.GetBlobClient(inputFileName),
                    Times.Once);

            this.blobClientMock.Verify(client =>
                client.DeleteAsync(DeleteSnapshotsOption.None, null, default),
                    Times.Once);

            this.blobServiceClientMock.VerifyNoOtherCalls();
            this.blobContainerClientMock.VerifyNoOtherCalls();
            this.blobClientMock.VerifyNoOtherCalls();
            this.blobStorageBrokerMock.VerifyNoOtherCalls();
        }
    }
}
