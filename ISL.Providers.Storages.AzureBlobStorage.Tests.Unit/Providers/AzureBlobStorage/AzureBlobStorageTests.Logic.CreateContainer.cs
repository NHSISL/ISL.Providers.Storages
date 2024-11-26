using Moq;
using System.IO;
using System.Threading.Tasks;

namespace ISL.Providers.Storages.AzureBlobStorage.Tests.Unit.Providers.AzureBlobStorage
{
    public partial class AzureBlobStorageTests
    {
        [Fact]
        public async Task ShouldCreateContainerAsync()
        {
            // given
            string randomContainerName = GetRandomString();
            string randomContainer = GetRandomString();
            Stream randomStream = new HasLengthStream();
            string inputContainerName = randomContainerName;
            string inputContainer = randomContainer;
            Stream inputStream = randomStream;

            // when
            await this.azureBlobStorageProvider.CreateContainerAsync(inputContainer);

            // then
            this.storageServiceMock.Verify(service =>
                service.CreateContainerAsync(inputContainer),
                    Times.Once);

            this.storageServiceMock.VerifyNoOtherCalls();
        }
    }
}
