using FluentAssertions;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ISL.Providers.Storage.Abstractions.Tests.Unit
{
    public partial class StorageAbstractionProviderTests
    {
        [Fact]
        public async Task ShouldCreateSasTokenWithAccessPolicyAsync()
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

            this.storageProviderMock.Setup(provider =>
                provider.CreateSasTokenAsync(
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

            this.storageProviderMock.Verify(provider =>
                provider.CreateSasTokenAsync(
                    inputContainer,
                    inputPath,
                    inputAccessPolicyIdentifier,
                    inputExpiresOn),
                        Times.Once);

            this.storageProviderMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldCreateSasTokenWithPermissionsListAsync()
        {
            // given
            string randomContainer = GetRandomString();
            string randomPath = GetRandomString();
            DateTimeOffset randomDateTimeOffset = GetRandomFutureDateTimeOffset();
            List<string> randomPermissionsList = GetRandomPermissionsList();
            string randomSasToken = GetRandomString();
            string inputPath = randomPath;
            string inputContainer = randomContainer;
            DateTimeOffset inputExpiresOn = randomDateTimeOffset;
            List<string> inputPermissionsList = randomPermissionsList;
            string outputSasToken = randomSasToken;
            string expectedSasToken = outputSasToken;

            this.storageProviderMock.Setup(provider =>
                provider.CreateSasTokenAsync(
                    inputContainer,
                    inputPath,
                    inputExpiresOn,
                    inputPermissionsList))
                        .ReturnsAsync(outputSasToken);

            // when
            string actualSasToken = await this.storageAbstractionProvider
                .CreateSasTokenAsync(
                    inputContainer,
                    inputPath,
                    inputExpiresOn,
                    inputPermissionsList);

            // then
            actualSasToken.Should().BeEquivalentTo(expectedSasToken);

            this.storageProviderMock.Verify(provider =>
                provider.CreateSasTokenAsync(
                    inputContainer,
                    inputPath,
                    inputExpiresOn,
                    inputPermissionsList),
                        Times.Once);

            this.storageProviderMock.VerifyNoOtherCalls();
        }
    }
}
