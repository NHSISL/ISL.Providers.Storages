using FluentAssertions;
using Moq;
using System;
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
            DateTimeOffset randomDateTimeOffset = GetRandomFutureDateTimeOffset();
            string randomSasToken = GetRandomString();
            string inputDirectoryPath = randomDirectoryPath;
            string inputContainer = randomContainer;
            string inputAccessPolicyIdentifier = randomAccessPolicyIdentifier;
            DateTimeOffset inputDateTimeOffset = randomDateTimeOffset;
            string outputSasToken = randomSasToken;
            string expectedSasToken = outputSasToken;

            this.storageServiceMock.Setup(service =>
                service.CreateDirectorySasTokenAsync(
                    inputContainer, inputDirectoryPath, inputAccessPolicyIdentifier, inputDateTimeOffset))
                        .ReturnsAsync(outputSasToken);

            // when
            string actualSasToken = await this.azureBlobStorageProvider
                .CreateDirectorySasTokenAsync(
                    inputContainer, inputDirectoryPath, inputAccessPolicyIdentifier, inputDateTimeOffset);

            // then
            actualSasToken.Should().BeEquivalentTo(expectedSasToken);

            this.storageServiceMock.Verify(service =>
                service.CreateDirectorySasTokenAsync(
                    inputContainer, inputDirectoryPath, inputAccessPolicyIdentifier, inputDateTimeOffset),
                        Times.Once);

            this.storageServiceMock.VerifyNoOtherCalls();
        }
    }
}
