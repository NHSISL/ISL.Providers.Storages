using FluentAssertions;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ISL.Providers.Storages.AzureBlobStorage.Tests.Unit.Providers.AzureBlobStorage
{
    public partial class AzureBlobStorageTests
    {
        [Fact]
        public async Task ShouldListFilesInContainerAsync()
        {
            // given
            string randomContainer = GetRandomString();
            List<string> randomStringList = GetRandomStringList();
            string inputContainer = randomContainer;
            List<string> outputFileList = randomStringList;
            List<string> expectedFileList = outputFileList;

            this.storageServiceMock.Setup(service =>
                service.ListFilesInContainerAsync(inputContainer))
                    .ReturnsAsync(outputFileList);

            // when
            List<string> actualFileList = await this.azureBlobStorageProvider
                .ListFilesInContainerAsync(inputContainer);

            // then
            actualFileList.Should().BeEquivalentTo(expectedFileList);

            this.storageServiceMock.Verify(service =>
                service.ListFilesInContainerAsync(inputContainer),
                    Times.Once);

            this.storageServiceMock.VerifyNoOtherCalls();
        }
    }
}
