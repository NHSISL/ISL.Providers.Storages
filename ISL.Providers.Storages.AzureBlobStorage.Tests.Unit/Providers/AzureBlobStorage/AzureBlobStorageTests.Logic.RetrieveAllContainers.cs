using FluentAssertions;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ISL.Providers.Storages.AzureBlobStorage.Tests.Unit.Providers.AzureBlobStorage
{
    public partial class AzureBlobStorageTests
    {
        [Fact]
        public async Task ShouldRetrieveAllContainersAsync()
        {
            // given
            List<string> randomStringList = GetRandomStringList();
            List<string> outputContainerNames = randomStringList;
            List<string> expectedContainerNames = outputContainerNames;

            this.storageServiceMock.Setup(service =>
                service.RetrieveAllContainersAsync())
                    .ReturnsAsync(outputContainerNames);

            // when
            List<string> actualContainerNames =
                await this.azureBlobStorageProvider.RetrieveAllContainersAsync();

            // then
            actualContainerNames.Should().BeEquivalentTo(expectedContainerNames);

            this.storageServiceMock.Verify(service =>
                service.RetrieveAllContainersAsync(),
                    Times.Once);

            this.storageServiceMock.VerifyNoOtherCalls();
        }
    }
}
