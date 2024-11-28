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
            await this.storageService.CreateDirectoryAsync(inputContainer, inputDirectory);

            // then
            this.blobStorageBrokerMock.Verify(broker =>
                broker.CreateDirectoryAsync(inputContainer, inputDirectory),
                    Times.Once);

            this.blobStorageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
