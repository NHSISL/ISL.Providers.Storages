using FluentAssertions;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ISL.Providers.Storages.AzureBlobStorage.Tests.Unit.Providers.AzureBlobStorage
{
    public partial class AzureBlobStorageTests
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

            this.storageServiceMock.Setup(service =>
                service.CreateSasTokenAsync(
                    inputContainer,
                    inputPath,
                    inputAccessPolicyIdentifier,
                    inputExpiresOn))
                        .ReturnsAsync(outputSasToken);

            // when
            string actualSasToken = await this.azureBlobStorageProvider
                .CreateSasTokenAsync(
                    inputContainer,
                    inputPath,
                    inputAccessPolicyIdentifier,
                    inputExpiresOn);

            // then
            actualSasToken.Should().BeEquivalentTo(expectedSasToken);

            this.storageServiceMock.Verify(service =>
                service.CreateSasTokenAsync(
                    inputContainer,
                    inputPath,
                    inputAccessPolicyIdentifier,
                    inputExpiresOn),
                        Times.Once);

            this.storageServiceMock.VerifyNoOtherCalls();
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

            this.storageServiceMock.Setup(service =>
                service.CreateSasTokenAsync(
                    inputContainer,
                    inputPath,
                    inputExpiresOn,
                    inputPermissionsList))
                        .ReturnsAsync(outputSasToken);

            // when
            string actualSasToken = await this.azureBlobStorageProvider
                .CreateSasTokenAsync(
                    inputContainer,
                    inputPath,
                    inputExpiresOn,
                    inputPermissionsList);

            // then
            actualSasToken.Should().BeEquivalentTo(expectedSasToken);

            this.storageServiceMock.Verify(service =>
                service.CreateSasTokenAsync(
                    inputContainer,
                    inputPath,
                    inputExpiresOn,
                    inputPermissionsList),
                        Times.Once);

            this.storageServiceMock.VerifyNoOtherCalls();
        }
    }
}
