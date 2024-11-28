using FluentAssertions;
using Moq;
using System;
using System.Threading.Tasks;

namespace ISL.Providers.Storage.Abstractions.Tests.Unit
{
    public partial class StorageAbstractionProviderTests
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

            this.storageProviderMock.Setup(service =>
                service.CreateDirectorySasTokenAsync(
                    inputContainer, inputDirectoryPath, inputAccessPolicyIdentifier, inputDateTimeOffset))
                        .ReturnsAsync(outputSasToken);

            // when
            string actualSasToken = await this.storageAbstractionProvider
                .CreateDirectorySasTokenAsync(
                    inputContainer, inputDirectoryPath, inputAccessPolicyIdentifier, inputDateTimeOffset);

            // then
            actualSasToken.Should().BeEquivalentTo(expectedSasToken);

            this.storageProviderMock.Verify(service =>
                service.CreateDirectorySasTokenAsync(
                    inputContainer, inputDirectoryPath, inputAccessPolicyIdentifier, inputDateTimeOffset),
                        Times.Once);

            this.storageProviderMock.VerifyNoOtherCalls();
        }
    }
}
