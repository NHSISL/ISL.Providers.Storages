using FluentAssertions;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ISL.Providers.Storage.Abstractions.Tests.Unit
{
    public partial class StorageAbstractionProviderTests
    {
        [Fact]
        public async Task ShouldListContainerAsync()
        {
            // given
            string randomContainer = GetRandomString();
            string inputContainer = randomContainer;
            List<string> randomStringList = GetRandomStringList();
            List<string> outputFiles = randomStringList;
            List<string> expectedFiles = outputFiles;

            this.storageProviderMock.Setup(provider =>
                provider.ListFilesInContainerAsync(inputContainer))
                    .ReturnsAsync(outputFiles);

            // when
            List<string> actualFiles = await this.storageAbstractionProvider
                .ListFilesInContainerAsync(inputContainer);

            // then
            actualFiles.Should().BeEquivalentTo(expectedFiles);

            this.storageProviderMock.Verify(provider =>
                provider.ListFilesInContainerAsync(inputContainer),
                    Times.Once);

            this.storageProviderMock.VerifyNoOtherCalls();
        }
    }
}
