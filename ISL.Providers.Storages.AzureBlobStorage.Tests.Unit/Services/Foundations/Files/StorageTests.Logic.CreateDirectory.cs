using Moq;
using System.Threading.Tasks;

namespace ISL.Providers.Storages.AzureBlobStorage.Tests.Unit.Services.Foundations.Files
{
    public partial class StorageTests
    {
        [Fact]
        public async Task ShouldCreateDirectoryAsync()
        {
            // given
            string randomContainer = GetRandomString();
            string inputContainer = randomContainer;
            string randomDirectory = GetRandomString();
            string inputDirectory = randomDirectory;

            // when
            this.dataLakeServiceClientMock.Setup(service =>
                service.GetFileSystemClient(inputContainer))
                    .Returns(this.dataLakeFileSystemClientMock.Object);

            await this.storageService.CreateDirectoryAsync(inputContainer, inputDirectory);

            // then
            this.dataLakeServiceClientMock.Verify(service =>
                service.GetFileSystemClient(inputContainer),
                    Times.Once);

            this.dataLakeFileSystemClientMock.Verify(client =>
                client.CreateDirectoryAsync(inputDirectory, null, default),
                    Times.Once);

            this.blobServiceClientMock.VerifyNoOtherCalls();
            this.dataLakeServiceClientMock.VerifyNoOtherCalls();
            this.dataLakeFileSystemClientMock.VerifyNoOtherCalls();
            this.blobContainerClientMock.VerifyNoOtherCalls();
            this.blobClientMock.VerifyNoOtherCalls();
            this.blobStorageBrokerMock.VerifyNoOtherCalls();
        }
    }
}
