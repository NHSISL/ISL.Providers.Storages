using Moq;
using System.IO;
using System.Threading.Tasks;

namespace ISL.Providers.Storages.AzureBlobStorage.Tests.Unit.Providers.AzureBlobStorage
{
    public partial class AzureBlobStorageTests
    {
        [Fact]
        public async Task ShouldCreateFileAsync()
        {
            // given
            string randomFileName = GetRandomString();
            string randomContainer = GetRandomString();
            Stream randomStream = new HasLengthStream();
            string inputFileName = randomFileName;
            string inputContainer = randomContainer;
            Stream inputStream = randomStream;

            // when
            await this.azureBlobStorageProvider.CreateFileAsync(
                inputStream, inputFileName, inputContainer);

            // then
            this.storageServiceMock.Verify(service =>
                service.CreateFileAsync(inputStream, inputFileName, inputContainer),
                    Times.Once);

            this.storageServiceMock.VerifyNoOtherCalls();
        }
    }
}
