using Moq;
using System.Threading.Tasks;

namespace ISL.Providers.Storage.Abstractions.Tests.Unit
{
    public partial class StorageAbstractionProviderTests
    {
        [Fact]
        public async Task ShouldCreateFolderInToContainerAsync()
        {
            // given
            string randomContainer = GetRandomString();
            string randomDirectory = GetRandomString();
            string inputContainer = randomContainer;
            string inputDirectory = randomDirectory;

            // when
            await this.storageAbstractionProvider
                .CreateFolderInContainerAsync(inputContainer, inputDirectory);

            // then
            this.storageProviderMock.Verify(provider =>
                provider.CreateFolderInContainerAsync(inputContainer, inputDirectory),
                    Times.Once);

            this.storageProviderMock.VerifyNoOtherCalls();
        }
    }
}
