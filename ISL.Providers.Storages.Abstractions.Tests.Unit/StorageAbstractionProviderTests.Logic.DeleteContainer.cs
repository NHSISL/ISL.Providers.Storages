using Moq;
using System.Threading.Tasks;

namespace ISL.Providers.Storage.Abstractions.Tests.Unit
{
    public partial class StorageAbstractionProviderTests
    {
        [Fact]
        public async Task ShouldDeleteContainerAsync()
        {
            // given
            string randomContainer = GetRandomString();
            string inputContainer = randomContainer;

            // when
            await this.storageAbstractionProvider
                .DeleteContainerAsync(inputContainer);

            // then
            this.storageProviderMock.Verify(provider =>
                provider.DeleteContainerAsync(inputContainer),
                    Times.Once);

            this.storageProviderMock.VerifyNoOtherCalls();
        }
    }
}
