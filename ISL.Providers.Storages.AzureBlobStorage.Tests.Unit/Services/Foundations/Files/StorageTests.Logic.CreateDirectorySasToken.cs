// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using Azure.Storage;
using Azure.Storage.Sas;
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
            string randomAccountName = GetRandomStringWithLengthOf(10);
            string randomAccountKey = GetRandomStringWithLengthOf(10);
            string randomSasToken = GetRandomString();
            string inputContainer = randomContainer;
            string inputDirectoryPath = randomDirectoryPath;
            string inputAccessPolicyIdentifier = randomAccessPolicyIdentifier;
            string inputAccountName = randomAccountName + "==";
            string inputAccountKey = randomAccountKey + "==";
            DateTimeOffset currentDateTimeOffset = DateTimeOffset.UtcNow;
            DateTimeOffset futureDateTimeOffset = GetRandomFutureDateTimeOffset();
            DateTimeOffset inputExpiresOn = futureDateTimeOffset;

            StorageSharedKeyCredential randomStorageSharedKeyCredential =
                new StorageSharedKeyCredential(inputAccountName, inputAccountKey);

            StorageSharedKeyCredential outputStorageSharedKeyCredential =
                randomStorageSharedKeyCredential;

            string outputSasToken = randomSasToken;
            string expectedSasToken = outputSasToken;

            Uri outputServiceUri = new Uri("http://mytest.com/");

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

            this.dataLakeServiceClientMock.Setup(client =>
                client.Uri).Returns(outputServiceUri);

            this.blobStorageBrokerMock.Setup(client =>
                client.GetDataLakeUriBuilder(
                    It.Is(SameUriAs(outputServiceUri)),
                    inputContainer,
                    inputDirectoryPath,
                    It.IsAny<DataLakeSasQueryParameters>()))
                        .Returns(dataLakeUriBuilderMock.Object);

            this.dataLakeUriBuilderMock.Setup(builder =>
                builder.ToString())
                    .Returns(outputSasToken);

            // when
            var actualSasToken = await this.storageService.CreateDirectorySasToken(
                inputContainer, inputDirectoryPath, inputAccessPolicyIdentifier, inputExpiresOn);

            // then
            actualSasToken.Should().BeEquivalentTo(expectedSasToken);

            //this.blobStorageBrokerMock.Verify(broker =>
            //    broker.GetDataLakeSasBuilder(
            //        inputContainer,
            //        inputDirectoryPath,
            //        inputAccessPolicyIdentifier,
            //        inputExpiresOn),
            //            Times.Once);

            this.blobStorageBrokerMock.Verify(broker =>
                broker.StorageSharedKeyCredential,
                    Times.Once);

            this.dataLakeServiceClientMock.Verify(client =>
                client.Uri,
                    Times.Once);

            //this.dataLakeSasBuilderMock.Verify(builder =>
            //    builder.ToSasQueryParameters(It.Is(SameStorageSharedKeyCredentialAs(
            //        outputStorageSharedKeyCredential))), Times.Once);

            this.blobStorageBrokerMock.Verify(client =>
                client.GetDataLakeUriBuilder(
                    It.Is(SameUriAs(outputServiceUri)),
                    inputContainer,
                    inputDirectoryPath,
                    It.IsAny<DataLakeSasQueryParameters>()),
                        Times.Once);

            this.dataLakeUriBuilderMock.Verify(builder =>
                builder.ToString(), Times.Once);

            this.blobServiceClientMock.VerifyNoOtherCalls();
            this.blobContainerClientMock.VerifyNoOtherCalls();
            this.blobClientMock.VerifyNoOtherCalls();
            this.blobStorageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
