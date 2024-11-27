// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using Azure;
using Azure.Storage.Blobs.Models;
using FluentAssertions;
using Force.DeepCloner;
using Moq;
using System;
using System.Threading.Tasks;

namespace ISL.Providers.Storages.AzureBlobStorage.Tests.Unit.Services.Foundations.Files
{
    public partial class StorageTests
    {
        [Fact]
        public async Task ShouldGetDownloadLinkAsync()
        {
            // given
            string randomFileName = GetRandomString();
            string randomContainer = GetRandomString();
            string inputFileName = randomFileName.DeepClone();
            string inputContainer = randomContainer.DeepClone();
            string blobClientContainer = inputContainer.DeepClone();
            string blobClientBlobName = inputFileName.DeepClone();
            DateTimeOffset currentDateTimeOffset = DateTimeOffset.UtcNow;
            DateTimeOffset futureDateTimeOffset = GetRandomFutureDateTimeOffset();
            DateTimeOffset inputExpiresOn = futureDateTimeOffset;
            string mockDownloadLink = "http://mytest.com/";
            string outputDowloadLink = mockDownloadLink.DeepClone();
            string expectedDowloadLink = outputDowloadLink.DeepClone();
            UserDelegationKey randomKey = CreateUserDelegationKey();
            UserDelegationKey outputKey = randomKey;

            this.blobServiceClientMock.Setup(client =>
                client.GetBlobContainerClient(inputContainer))
                    .Returns(blobContainerClientMock.Object);

            this.blobContainerClientMock.Setup(client =>
                client.GetBlobClient(inputFileName))
                    .Returns(blobClientMock.Object);

            this.blobStorageBrokerMock.Setup(client =>
                client.GetUserDelegationKey(It.IsAny<DateTimeOffset>(), inputExpiresOn, default))
                    .Returns(Response.FromValue(outputKey, blobClientResponseMock.Object));

            this.blobStorageBrokerMock.Setup(client =>
                client.GetBlobSasBuilder(
                    inputFileName,
                    inputContainer,
                    inputExpiresOn))
                        .Returns(blobSasBuilderMock.Object);

            this.blobStorageBrokerMock.Setup(client =>
                client.GetBlobUriBuilder(
                    It.IsAny<Uri>()))
                        .Returns(blobUriBuilderMock.Object);

            // when
            var actualDownloadLink = await this.storageService.GetDownloadLinkAsync(inputFileName, inputContainer, inputExpiresOn);

            // then
            actualDownloadLink.Should().BeEquivalentTo(expectedDowloadLink);

            this.blobServiceClientMock.Verify(client =>
                client.GetBlobContainerClient(inputContainer),
                    Times.Once);

            this.blobContainerClientMock.Verify(client =>
                client.GetBlobClient(inputFileName),
                    Times.Once);

            this.blobStorageBrokerMock.Verify(client =>
                client.GetBlobSasBuilder(
                    inputFileName,
                    inputContainer,
                    inputExpiresOn),
                        Times.Once);

            this.blobStorageBrokerMock.Verify(client =>
                client.GetBlobUriBuilder(
                    It.IsAny<Uri>()),
                        Times.Once);

            this.blobClientMock.Verify(client =>
                client.Uri,
                    Times.Once);

            this.blobServiceClientMock.VerifyNoOtherCalls();
            this.blobContainerClientMock.VerifyNoOtherCalls();
            this.blobClientMock.VerifyNoOtherCalls();
            this.blobStorageBrokerMock.VerifyNoOtherCalls();
        }
    }
}
