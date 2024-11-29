// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using Moq;
using System.IO;
using System.Threading.Tasks;

namespace ISL.Providers.Storages.AzureBlobStorage.Tests.Unit.Services.Foundations.Files
{
    public partial class StorageTests
    {
        [Fact]
        public async Task ShouldRetrieveFileAsync()
        {
            // given
            string randomFileName = GetRandomString();
            string randomContainer = GetRandomString();
            string inputFileName = randomFileName;
            string inputContainer = randomContainer;
            Stream outputStream = new MemoryStream();

            this.blobStorageBrokerMock.Setup(broker =>
                broker.GetBlobContainerClient(inputContainer))
                    .Returns(blobContainerClientMock.Object);

            this.blobStorageBrokerMock.Setup(broker =>
                broker.GetBlobClient(blobContainerClientMock.Object, inputFileName))
                    .Returns(blobClientMock.Object);

            // when
            await this.storageService.RetrieveFileAsync(outputStream, inputFileName, inputContainer);

            // then
            this.blobStorageBrokerMock.Verify(broker =>
                broker.GetBlobContainerClient(inputContainer),
                    Times.Once);

            this.blobStorageBrokerMock.Verify(broker =>
                broker.GetBlobClient(blobContainerClientMock.Object, inputFileName),
                    Times.Once);

            this.blobStorageBrokerMock.Verify(broker =>
                broker.RetrieveFileAsync(blobClientMock.Object, outputStream),
                    Times.Once);

            this.blobStorageBrokerMock.VerifyNoOtherCalls();
        }
    }
}
