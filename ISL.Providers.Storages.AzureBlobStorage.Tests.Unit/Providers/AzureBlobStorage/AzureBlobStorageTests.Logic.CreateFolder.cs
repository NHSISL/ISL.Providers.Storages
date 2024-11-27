using Moq;
using System.Threading.Tasks;

namespace ISL.Providers.Storages.AzureBlobStorage.Tests.Unit.Providers.AzureBlobStorage
{
    public partial class AzureBlobStorageTests
    {
        [Fact]
        public async Task ShouldCreateFolderInContainerAsync()
        {
            // given
            string randomContainer = GetRandomString();
            string randomFolder = GetRandomString();
            string inputContainer = randomContainer;
            string inputFolder = randomFolder;

            // when
            await this.azureBlobStorageProvider.CreateFolderInContainerAsync(inputContainer, inputFolder);

            // then
            this.storageServiceMock.Verify(service =>
                service.CreateDirectoryAsync(inputContainer, inputFolder),
                    Times.Once);

            this.storageServiceMock.VerifyNoOtherCalls();
        }
    }
}
