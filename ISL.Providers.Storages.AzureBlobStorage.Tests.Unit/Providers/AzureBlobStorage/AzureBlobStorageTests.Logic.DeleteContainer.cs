using Moq;
using System.Threading.Tasks;

namespace ISL.Providers.Storages.AzureBlobStorage.Tests.Unit.Providers.AzureBlobStorage
{
    public partial class AzureBlobStorageTests
    {
        [Fact]
        public async Task ShouldDeleteContainerAsync()
        {
            // given
            string randomContainer = GetRandomString();
            string inputContainer = randomContainer;

            // when
            await this.azureBlobStorageProvider.DeleteContainerAsync(inputContainer);

            // then
            this.storageServiceMock.Verify(service =>
                service.DeleteContainerAsync(inputContainer),
                    Times.Once);

            this.storageServiceMock.VerifyNoOtherCalls();
        }
    }
}
