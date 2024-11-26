using Moq;
using System.Threading.Tasks;

namespace ISL.Providers.Storages.AzureBlobStorage.Tests.Unit.Providers.AzureBlobStorage
{
    public partial class AzureBlobStorageTests
    {
        [Fact]
        public async Task ShouldDeleteFileAsync()
        {
            // given
            string randomFileName = GetRandomString();
            string randomContainer = GetRandomString();
            string inputFileName = randomFileName;
            string inputContainer = randomContainer;

            // when
            await this.azureBlobStorageProvider.DeleteFileAsync(
                inputFileName, inputContainer);

            // then
            this.storageServiceMock.Verify(service =>
                service.DeleteFileAsync(inputFileName, inputContainer),
                    Times.Once);

            this.storageServiceMock.VerifyNoOtherCalls();
        }
    }
}
