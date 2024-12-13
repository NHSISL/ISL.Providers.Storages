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
        public async Task ShouldGetDownloadLinkAsync()
        {
            // given
            string randomFileName = GetRandomString();
            string randomContainer = GetRandomString();
            string randomDownloadLink = GetRandomString();
            string inputFileName = randomFileName;
            string inputContainer = randomContainer;
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            DateTimeOffset inputStartsOn = randomDateTimeOffset.AddMinutes(-1);
            DateTimeOffset futureDateTimeOffset = GetRandomFutureDateTimeOffset();
            DateTimeOffset inputExpiresOn = futureDateTimeOffset;
            string outputDowloadLink = randomDownloadLink;
            string expectedDowloadLink = outputDowloadLink;

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(randomDateTimeOffset);

            this.blobStorageBrokerMock.Setup(broker =>
                broker.GetBlobContainerClient(inputContainer))
                    .Returns(blobContainerClientMock.Object);

            this.blobStorageBrokerMock.Setup(broker =>
                broker.GetBlobClient(blobContainerClientMock.Object, inputFileName))
                    .Returns(blobClientMock.Object);

            this.blobStorageBrokerMock.Setup(broker =>
                broker.GetBlobSasBuilder(inputFileName, inputContainer, inputStartsOn, inputExpiresOn))
                    .Returns(blobSasBuilderMock.Object);

            this.blobStorageBrokerMock.Setup(client =>
                client.GetDownloadLinkAsync(
                    blobClientMock.Object,
                    blobSasBuilderMock.Object,
                    inputExpiresOn))
                        .ReturnsAsync(outputDowloadLink);

            // when
            var actualDownloadLink = await this.storageService
                .GetDownloadLinkAsync(inputFileName, inputContainer, inputExpiresOn);

            // then
            actualDownloadLink.Should().BeEquivalentTo(expectedDowloadLink);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.blobStorageBrokerMock.Verify(broker =>
                broker.GetBlobContainerClient(inputContainer),
                    Times.Once);

            this.blobStorageBrokerMock.Verify(broker =>
                broker.GetBlobClient(blobContainerClientMock.Object, inputFileName),
                    Times.Once);

            this.blobStorageBrokerMock.Verify(broker =>
                broker.GetBlobSasBuilder(inputFileName, inputContainer, inputStartsOn, inputExpiresOn),
                    Times.Once);

            this.blobStorageBrokerMock.Verify(client =>
                client.GetDownloadLinkAsync(
                    blobClientMock.Object,
                    blobSasBuilderMock.Object,
                    inputExpiresOn),
                        Times.Once);

            this.blobStorageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
