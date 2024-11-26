using Moq;
using System.IO;
using System.Threading.Tasks;

namespace ISL.Providers.Storages.AzureBlobStorage.Tests.Unit.Providers.AzureBlobStorage
{
    public partial class AzureBlobStorageTests
    {
        [Fact]
        public async Task ShouldRetrieveFileAsync()
        {
            // given
            string randomFileName = GetRandomString();
            string randomContainer = GetRandomString();
            Stream randomStream = new ZeroLengthStream();
            string inputFileName = randomFileName;
            string inputContainer = randomContainer;
            Stream outputStream = randomStream;

            // when
            await this.azureBlobStorageProvider.RetrieveFileAsync(
                outputStream, inputFileName, inputContainer);

            // then
            this.storageServiceMock.Verify(service =>
                service.RetrieveFileAsync(outputStream, inputFileName, inputContainer),
                    Times.Once);

            this.storageServiceMock.VerifyNoOtherCalls();
        }
    }
}
