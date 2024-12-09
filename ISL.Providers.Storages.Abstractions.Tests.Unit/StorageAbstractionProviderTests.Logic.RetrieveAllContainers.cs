using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ISL.Providers.Storage.Abstractions.Tests.Unit
{
    public partial class StorageAbstractionProviderTests
    {
        [Fact]
        public async Task ShouldRetrieveAllContainersAsync()
        {
            // given
            List<string> randomContainers = GetRandomStringList();
            List<string> outputContainers = randomContainers;
            List<string> expectedContainers = outputContainers;

            this.storageProviderMock.Setup(provider =>
                provider.RetrieveAllContainersAsync())
                    .ReturnsAsync(outputContainers);

            // when
            List<string> actualContainers = await this.storageAbstractionProvider
                .RetrieveAllContainersAsync();

            // then
            this.storageProviderMock.Verify(provider =>
                provider.RetrieveAllContainersAsync(),
                    Times.Once);

            this.storageProviderMock.VerifyNoOtherCalls();
        }
    }
}
