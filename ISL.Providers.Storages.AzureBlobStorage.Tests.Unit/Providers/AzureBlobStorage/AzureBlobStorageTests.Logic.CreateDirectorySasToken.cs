using FluentAssertions;
using Moq;
using System.Threading.Tasks;

namespace ISL.Providers.Storages.AzureBlobStorage.Tests.Unit.Providers.AzureBlobStorage
{
    public partial class AzureBlobStorageTests
    {
        [Fact]
        public async Task ShouldCreateDirectorySasTokenAsync()
        {
            // given
            string randomContainer = GetRandomString();
            string randomDirectoryPath = GetRandomString();
            string randomAccessPolicyIdentifier = GetRandomString();
            string randomSasToken = GetRandomString();
            string inputDirectoryPath = randomDirectoryPath;
            string inputContainer = randomContainer;
            string inputAccessPolicyIdentifier = randomAccessPolicyIdentifier;
            string outputSasToken = randomSasToken;
            string expectedSasToken = outputSasToken;

            this.storageServiceMock.Setup(service =>
                service.CreateDirectorySasTokenAsync(inputContainer, inputDirectoryPath, inputAccessPolicyIdentifier))
                    .ReturnsAsync(outputSasToken);

            // when
            string actualSasToken = await this.azureBlobStorageProvider
                .CreateDirectorySasTokenAsync(inputContainer, inputDirectoryPath, inputAccessPolicyIdentifier);

            // then
            actualSasToken.Should().BeEquivalentTo(expectedSasToken);

            this.storageServiceMock.Verify(service =>
                service.CreateDirectorySasTokenAsync(inputContainer, inputDirectoryPath, inputAccessPolicyIdentifier),
                    Times.Once);

            this.storageServiceMock.VerifyNoOtherCalls();
        }
    }
}
