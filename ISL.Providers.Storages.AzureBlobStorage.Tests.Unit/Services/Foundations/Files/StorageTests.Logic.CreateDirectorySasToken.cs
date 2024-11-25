// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using Azure.Storage;
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
            string randomAccountName = GetRandomString();
            string randomAccountKey = GetRandomString();
            string randomSasToken = GetRandomString();
            string inputContainer = randomContainer;
            string inputDirectoryPath = randomDirectoryPath;
            string inputAccessPolicyIdentifier = randomAccessPolicyIdentifier;
            DateTimeOffset currentDateTimeOffset = DateTimeOffset.UtcNow;
            DateTimeOffset futureDateTimeOffset = GetRandomFutureDateTimeOffset();
            DateTimeOffset inputExpiresOn = futureDateTimeOffset;

            StorageSharedKeyCredential randomStorageSharedKeyCredential =
                new StorageSharedKeyCredential("VGhpcyBpcyBhIHRlc3Q=", "VGhpcyBpcyBhIHRlc3Q=");

            StorageSharedKeyCredential outputStorageSharedKeyCredential = randomStorageSharedKeyCredential;
            string outputSasToken = randomSasToken;
            string expectedSasToken = outputSasToken;

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

            //this.dataLakeSasBuilderMock.Setup(builder =>
            //    builder.ToSasQueryParameters(It.Is(SameStorageSharedKeyCredentialAs(
            //        outputStorageSharedKeyCredential))).ToString())
            //            .Returns(outputSasToken);

            // when
            var actualSasToken = await this.storageService.CreateDirectorySasToken(inputContainer, inputDirectoryPath, inputAccessPolicyIdentifier, inputExpiresOn);

            // then
            actualSasToken.Should().BeEquivalentTo(expectedSasToken);

            this.blobStorageBrokerMock.Verify(broker =>
                broker.GetDataLakeSasBuilder(
                    inputContainer,
                    inputDirectoryPath,
                    inputAccessPolicyIdentifier,
                    inputExpiresOn),
                        Times.Once);

            this.blobStorageBrokerMock.Verify(broker =>
                broker.StorageSharedKeyCredential,
                    Times.Once);

            this.dataLakeSasBuilderMock.Verify(builder =>
                builder.ToSasQueryParameters(It.Is(SameStorageSharedKeyCredentialAs(
                    outputStorageSharedKeyCredential))).ToString(), Times.Once);

            this.blobServiceClientMock.VerifyNoOtherCalls();
            this.blobContainerClientMock.VerifyNoOtherCalls();
            this.blobClientMock.VerifyNoOtherCalls();
            this.blobStorageBrokerMock.VerifyNoOtherCalls();
        }
    }
}
