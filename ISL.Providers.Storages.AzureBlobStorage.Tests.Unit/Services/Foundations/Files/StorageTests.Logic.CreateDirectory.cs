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
            string randomDirectory = GetRandomString();
            string inputDirectory = randomDirectory;

            // when
            await this.storageService.CreateDirectoryAsync(inputDirectory);

            // then
            this.dataLakeFileSystemClientMock.Verify(client =>
                client.CreateDirectoryAsync(inputDirectory, null, default),
                    Times.Once);

            this.blobServiceClientMock.VerifyNoOtherCalls();
            this.dataLakeFileSystemClientMock.VerifyNoOtherCalls();
            this.blobContainerClientMock.VerifyNoOtherCalls();
            this.blobClientMock.VerifyNoOtherCalls();
            this.blobStorageBrokerMock.VerifyNoOtherCalls();
        }
    }
}
