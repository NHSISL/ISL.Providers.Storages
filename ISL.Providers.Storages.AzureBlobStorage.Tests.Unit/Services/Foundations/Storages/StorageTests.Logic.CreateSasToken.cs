// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using FluentAssertions;
using Moq;
using System;
using System.Threading.Tasks;

namespace ISL.Providers.Storages.AzureBlobStorage.Tests.Unit.Services.Foundations.Storages
{
    public partial class StorageTests
    {
        [Theory]
        [MemberData(nameof(PathRelatedInputs))]
        public async Task ShouldCreateSasTokenAsync(
            string path,
            bool isDirectory,
            string resource)
        {
            // given
            string randomContainer = GetRandomString();
            string randomAccessPolicyIdentifier = GetRandomString();
            DateTimeOffset randomDateTimeOffset = GetRandomFutureDateTimeOffset();
            string randomSasToken = GetRandomString();
            string inputContainer = randomContainer;
            string inputPath = path;
            string inputAccessPolicyIdentifier = randomAccessPolicyIdentifier;
            DateTimeOffset inputExpiresOn = randomDateTimeOffset;
            bool inputIsDirectory = isDirectory;
            string inputResource = resource;
            string outputSasToken = randomSasToken;
            string expectedSasToken = outputSasToken;

            this.blobStorageBrokerMock.Setup(broker =>
                broker.CreateSasTokenAsync(
                    inputContainer,
                    inputPath,
                    inputAccessPolicyIdentifier,
                    inputExpiresOn,
                    inputIsDirectory,
                    inputResource))
                        .ReturnsAsync(outputSasToken);

            // when
            var actualSasToken = await this.storageService.CreateSasTokenAsync(
                inputContainer,
                inputPath,
                inputAccessPolicyIdentifier,
                inputExpiresOn);

            // then
            actualSasToken.Should().BeEquivalentTo(expectedSasToken);

            this.blobStorageBrokerMock.Verify(broker =>
                broker.CreateSasTokenAsync(
                    inputContainer,
                    inputPath,
                    inputAccessPolicyIdentifier,
                    inputExpiresOn,
                    inputIsDirectory,
                    inputResource),
                        Times.Once);

            this.blobStorageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
