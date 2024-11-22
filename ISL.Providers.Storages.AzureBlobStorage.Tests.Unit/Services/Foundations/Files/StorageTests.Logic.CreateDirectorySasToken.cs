// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using Azure.Storage;
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

            StorageSharedKeyCredential randomStorageSharedKeyCredential =
                CreateRandomStorageSharedKeyCredential();

            StorageSharedKeyCredential outputStorageSharedKeyCredential = randomStorageSharedKeyCredential;

            string outputSasToken = randomSasToken;
            string outputDowloadLink = mockDownloadLink.DeepClone();
            string expectedDowloadLink = outputDowloadLink.DeepClone();
            UserDelegationKey randomKey = CreateUserDelegationKey();
            UserDelegationKey outputKey = randomKey;

            this.blobStorageBrokerMock.Setup(broker =>
                broker.GetDataLakeSasBuilder(
                    inputContainer,
                    inputDirectoryPath,
                    inputAccessPolicyIdentifier,
                    inputExpiresOn))
                        .Returns(dataLakeSasBuilderMock.Object);

            this.blobStorageBrokerMock.Setup(broker =>
                broker.StorageSharedKeyCredential)
                    .Returns(outputStorageSharedKeyCredential);

            this.dataLakeSasBuilderMock.Setup(builder =>
                builder.ToSasQueryParameters(It.Is(SameStorageSharedKeyCredentialAs(outputStorageSharedKeyCredential)))).Returns(outputSasToken);

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
