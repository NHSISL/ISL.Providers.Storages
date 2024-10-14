using Azure;
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
            //string randomFileName = "file";
            string randomContainer = GetRandomString();
            //string randomContainer = "container";
            string randomUrl = GetRandomString();
            string inputFileName = randomFileName.DeepClone();
            string inputContainer = randomContainer.DeepClone();
            string blobClientContainer = inputContainer.DeepClone();
            string blobClientBlobName = inputFileName.DeepClone();
            DateTimeOffset currentDateTimeOffset = DateTimeOffset.UtcNow;
            DateTimeOffset futureDateTimeOffset = GetRandomFutureDateTimeOffset();
            DateTimeOffset inputExpiresOn = futureDateTimeOffset;
            string randomDownloadLink = "http://mytest.com/";
            string outputDowloadLink = randomDownloadLink.DeepClone();
            string expectedDowloadLink = outputDowloadLink.DeepClone();
            var key = CreateUserDelegationKey();

            //this.blobClientMock
            //    .SetupGet(m => m.BlobContainerName)
            //        .Returns(blobClientContainer);

            //this.blobClientMock
            //    .SetupGet(m => m.Name)
            //        .Returns(blobClientBlobName);

            this.blobServiceClientMock.Setup(client =>
                client.GetBlobContainerClient(inputContainer))
                    .Returns(blobContainerClientMock.Object);

            this.blobContainerClientMock.Setup(client =>
                client.GetBlobClient(inputFileName))
                    .Returns(blobClientMock.Object);

            this.blobStorageBrokerMock.Setup(client =>
                client.GetUserDelegationKey(It.IsAny<DateTimeOffset>(), inputExpiresOn, default))
                    .Returns(Response.FromValue(key, blobClientResponseMock.Object));

            //this.blobStorageBrokerMock.Setup(client =>
            //    client.GetBlobSasBuilder(
            //        blobClientMock.Object.Name,
            //        blobClientMock.Object.BlobContainerName,
            //        inputExpiresOn))
            //            .Returns(blobSasBuilderMock.Object);

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

            //this.blobStorageBrokerMock.Verify(client =>
            //    client.GetBlobSasBuilder(
            //        blobClientMock.Object.Name,
            //        blobClientMock.Object.BlobContainerName,
            //        inputExpiresOn),
            //            Times.Once);

            this.blobStorageBrokerMock.Verify(client =>
                client.GetBlobUriBuilder(
                    It.IsAny<Uri>()),
                        Times.Once);

            //this.blobClientMock.Verify(client =>
            // client.BlobContainerName


            //this.blobUriBuilderMock.Verify(builder =>
            //    builder.ToUri(),
            //        Times.Once);


            /// Not 100% sure how to account for all these calls. The tests fail on this combination of calls 
            /// if you comment out the following three verifications
            //this.blobClientMock.Verify(client =>
            //    client.BlobContainerName, Times.Exactly(4));

            //this.blobClientMock.Verify(client =>
            //    client.Name, Times.Exactly(4));

            this.blobClientMock.Verify(client =>
                client.Uri, Times.Once);

            this.blobServiceClientMock.VerifyNoOtherCalls();
            this.blobContainerClientMock.VerifyNoOtherCalls();
            this.blobClientMock.VerifyNoOtherCalls();
            this.blobStorageBrokerMock.VerifyNoOtherCalls();
        }
    }
}
