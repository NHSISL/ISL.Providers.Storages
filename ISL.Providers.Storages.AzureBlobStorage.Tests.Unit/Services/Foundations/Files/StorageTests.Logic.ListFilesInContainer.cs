using Azure.Storage.Blobs.Models;
using FluentAssertions;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ISL.Providers.Storages.AzureBlobStorage.Tests.Unit.Services.Foundations.Files
{
    public partial class StorageTests
    {
        [Fact]
        public async Task ShouldListFilesInContainerAsync()
        {
            // given
            string randomContainer = GetRandomString();
            string inputContainer = randomContainer;
            var randomAsyncPageable = CreateAsyncPageableBlobItem();
            var outputAsyncPageable = randomAsyncPageable;
            List<string> expectedFileNames = new List<string>();

            await foreach (BlobItem blobItem in outputAsyncPageable)
            {
                expectedFileNames.Add(blobItem.Name);
            }

            this.blobServiceClientMock.Setup(client =>
                client.GetBlobContainerClient(inputContainer))
                    .Returns(blobContainerClientMock.Object);

            this.blobContainerClientMock.Setup(client =>
                client.GetBlobsAsync(BlobTraits.None, BlobStates.None, null, default))
                    .Returns(outputAsyncPageable);

            // when
            var actualFileNames = await this.storageService.ListFilesInContainerAsync(inputContainer);

            // then
            actualFileNames.Should().BeEquivalentTo(expectedFileNames);

            this.blobServiceClientMock.Verify(client =>
                client.GetBlobContainerClient(inputContainer),
                    Times.Once);

            this.blobContainerClientMock.Verify(client =>
                client.GetBlobsAsync(BlobTraits.None, BlobStates.None, null, default),
                    Times.Once);

            this.blobServiceClientMock.VerifyNoOtherCalls();
            this.blobContainerClientMock.VerifyNoOtherCalls();
            this.blobClientMock.VerifyNoOtherCalls();
            this.blobStorageBrokerMock.VerifyNoOtherCalls();
        }
    }
}
