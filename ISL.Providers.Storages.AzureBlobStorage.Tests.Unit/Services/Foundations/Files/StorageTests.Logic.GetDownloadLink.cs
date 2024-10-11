using Azure;
using Azure.Storage.Blobs;
using FluentAssertions;
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
            string randomUrl = GetRandomString();
            string inputFileName = randomFileName;
            string inputContainer = randomContainer;
            DateTimeOffset currentDateTimeOffset = DateTimeOffset.UtcNow;
            DateTimeOffset futureDateTimeOffset = GetRandomFutureDateTimeOffset();
            DateTimeOffset inputExpiresOn = futureDateTimeOffset;
            string randomDownloadLink = GetRandomString();
            string outputDowloadLink = randomDownloadLink;
            string expectedDowloadLink = outputDowloadLink;
            var key = CreateUserDelegationKey();
            //var newResponse = Response();
            //var response = Response.FromValue(key, );
            string server = "http://www.myserver.com";
            var someUri = new Uri(server);

            var blobUriBuilderMock = new Mock<BlobUriBuilder>(new Uri(server));

            this.blobServiceClientMock.Setup(client =>
                client.GetBlobContainerClient(inputContainer))
                    .Returns(blobContainerClientMock.Object);

            this.blobContainerClientMock.Setup(client =>
                client.GetBlobClient(inputFileName))
                    .Returns(blobClientMock.Object);

            this.blobStorageBrokerMock.Setup(client =>
                client.GetUserDelegationKey(It.IsAny<DateTimeOffset>(), inputExpiresOn, default))
                    .Returns(Response.FromValue(key, blobClientResponseMock.Object));

            this.blobStorageBrokerMock.Setup(client =>
                client.GetBlobSasBuilder(
                    blobClientMock.Object.Name,
                    blobClientMock.Object.BlobContainerName,
                    inputExpiresOn))
                        .Returns(blobSasBuilderMock.Object);

            this.blobStorageBrokerMock.Setup(client =>
                client.GetBlobUriBuilder(
                    It.IsAny<Uri>()))
                        //new Uri(GetRandomString()))
                        //someUri))
                        .Returns(blobUriBuilderMock.Object);

            //this.blobUriBuilderMock.Setup(builder =>
            //    builder.ToUri())
            //        .Returns(new Uri(outputDowloadLink));

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
                client.GetUserDelegationKey(It.IsAny<DateTimeOffset>(), inputExpiresOn, default),
                    Times.Once);

            this.blobStorageBrokerMock.Verify(client =>
                client.GetBlobSasBuilder(
                    blobClientMock.Object.Name,
                    blobClientMock.Object.BlobContainerName,
                    inputExpiresOn),
                        Times.Once);

            this.blobStorageBrokerMock.Verify(client =>
                client.GetBlobUriBuilder(
                    blobClientMock.Object.Uri),
                        Times.Once);

            this.blobUriBuilderMock.Verify(builder =>
                builder.ToUri(),
                    Times.Once);

            this.blobServiceClientMock.VerifyNoOtherCalls();
            this.blobContainerClientMock.VerifyNoOtherCalls();
            this.blobClientMock.VerifyNoOtherCalls();
            this.blobStorageBrokerMock.VerifyNoOtherCalls();
        }
    }
}
