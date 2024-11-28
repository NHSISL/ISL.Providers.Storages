// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using FluentAssertions;
using Moq;
using System;
using System.Threading.Tasks;

namespace ISL.Providers.Storages.AzureBlobStorage.Tests.Unit.Services.Foundations.Files
{
    public partial class StorageTests
    {
        [Fact]
        public async Task ShouldCreateDirectorySasTokenAsync()
        {
            // given
            string randomContainer = GetRandomString();
            string randomDirectoryPath = GetRandomString();
            string randomAccessPolicyIdentifier = GetRandomString();
            string randomSasToken = GetRandomString();
            string inputContainer = randomContainer;
            string inputDirectoryPath = randomDirectoryPath;
            string inputAccessPolicyIdentifier = randomAccessPolicyIdentifier;
            DateTimeOffset currentDateTimeOffset = DateTimeOffset.UtcNow;
            DateTimeOffset futureDateTimeOffset = GetRandomFutureDateTimeOffset();
            DateTimeOffset inputExpiresOn = futureDateTimeOffset;
            string outputSasToken = randomSasToken;
            string expectedSasToken = outputSasToken;

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(currentDateTimeOffset);

            this.blobStorageBrokerMock.Setup(broker =>
                broker.GetSasTokenAsync(
                    inputContainer,
                    inputDirectoryPath,
                    inputAccessPolicyIdentifier,
                    inputExpiresOn))
                        .ReturnsAsync(outputSasToken);

            // when
            var actualSasToken = await this.storageService.CreateDirectorySasTokenAsync(
                inputContainer, inputDirectoryPath, inputAccessPolicyIdentifier, inputExpiresOn);

            // then
            actualSasToken.Should().BeEquivalentTo(expectedSasToken);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.blobStorageBrokerMock.Verify(broker =>
                broker.GetSasTokenAsync(
                    inputContainer,
                    inputDirectoryPath,
                    inputAccessPolicyIdentifier,
                    inputExpiresOn),
                        Times.Once);

            this.blobStorageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
