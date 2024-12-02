using Moq;
using System.Threading.Tasks;

namespace ISL.Providers.Storages.AzureBlobStorage.Tests.Unit.Services.Foundations.Storages
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

            this.blobStorageBrokerMock.Setup(broker =>
                broker.GetDataLakeFileSystemClient(inputContainer))
                    .Returns(dataLakeFileSystemClientMock.Object);

            this.blobStorageBrokerMock.Setup(broker =>
                broker.CreateDirectoryAsync(dataLakeFileSystemClientMock.Object, inputDirectory));

            // when
            await this.storageService.CreateDirectoryAsync(inputContainer, inputDirectory);

            // then
            this.blobStorageBrokerMock.Verify(broker =>
                broker.GetDataLakeFileSystemClient(inputContainer),
                    Times.Once);

            this.blobStorageBrokerMock.Verify(broker =>
                broker.CreateDirectoryAsync(dataLakeFileSystemClientMock.Object, inputDirectory),
                    Times.Once);

            this.blobStorageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
