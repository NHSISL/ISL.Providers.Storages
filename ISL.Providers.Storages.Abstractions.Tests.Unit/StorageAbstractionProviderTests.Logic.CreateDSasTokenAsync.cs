using FluentAssertions;
using Moq;
using System;
using System.Threading.Tasks;

namespace ISL.Providers.Storage.Abstractions.Tests.Unit
{
    public partial class StorageAbstractionProviderTests
    {
        [Fact]
        public async Task ShouldCreateSasTokenAsync()
        {
            // given
            string randomContainer = GetRandomString();
            string randomPath = GetRandomString();
            string randomAccessPolicyIdentifier = GetRandomString();
            DateTimeOffset randomDateTimeOffset = GetRandomFutureDateTimeOffset();
            string randomSasToken = GetRandomString();
            string inputPath = randomPath;
            string inputContainer = randomContainer;
            string inputAccessPolicyIdentifier = randomAccessPolicyIdentifier;
            DateTimeOffset inputExpiresOn = randomDateTimeOffset;
            string outputSasToken = randomSasToken;
            string expectedSasToken = outputSasToken;

            this.storageProviderMock.Setup(service =>
                service.CreateSasTokenAsync(
                    inputContainer,
                    inputPath,
                    inputAccessPolicyIdentifier,
                    inputExpiresOn))
                        .ReturnsAsync(outputSasToken);

            // when
            string actualSasToken = await this.storageAbstractionProvider
                .CreateSasTokenAsync(
                    inputContainer,
                    inputPath,
                    inputAccessPolicyIdentifier,
                    inputExpiresOn);

            // then
            actualSasToken.Should().BeEquivalentTo(expectedSasToken);

            this.storageProviderMock.Verify(service =>
                service.CreateSasTokenAsync(
                    inputContainer,
                    inputPath,
                    inputAccessPolicyIdentifier,
                    inputExpiresOn),
                    Times.Once);
        }
    }
}
