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
        [Fact]
        public async Task ShouldCreateDirectorySasTokenAsync()
        {
            // given
            string randomContainer = GetRandomString();
            string randomPath = GetRandomString();
            string randomAccessPolicyIdentifier = GetRandomString();
            DateTimeOffset randomDateTimeOffset = GetRandomFutureDateTimeOffset();
            string randomSasToken = GetRandomString();
            string inputContainer = randomContainer;
            string inputPath = randomPath;
            string inputAccessPolicyIdentifier = randomAccessPolicyIdentifier;
            DateTimeOffset inputExpiresOn = randomDateTimeOffset;
            string outputSasToken = randomSasToken;
            string expectedSasToken = outputSasToken;

            this.blobStorageBrokerMock.Setup(broker =>
                broker.CreateSasTokenAsync(
                    inputContainer,
                    inputPath,
                    inputAccessPolicyIdentifier,
                    inputExpiresOn,
                    true,
                    "d"))
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
                    true,
                    "d"),
                        Times.Once);

            this.blobStorageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
