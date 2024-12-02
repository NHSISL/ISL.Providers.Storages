// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using Moq;
using System.IO;
using System.Threading.Tasks;

namespace ISL.Providers.Storages.AzureBlobStorage.Tests.Unit.Services.Foundations.Storages
{
    public partial class StorageTests
    {
        [Fact]
        public async Task ShouldCreateFileAsync()
        {
            // given
            string randomFileName = GetRandomString();
            string randomContainer = GetRandomString();
            Stream randomStream = new HasLengthStream();
            string inputFileName = randomFileName;
            string inputContainer = randomContainer;
            Stream inputStream = randomStream;

            this.blobStorageBrokerMock.Setup(broker =>
                broker.GetBlobContainerClient(inputContainer))
                    .Returns(blobContainerClientMock.Object);

            this.blobStorageBrokerMock.Setup(broker =>
                broker.GetBlobClient(blobContainerClientMock.Object, inputFileName))
                    .Returns(blobClientMock.Object);

            // when
            await this.storageService.CreateFileAsync(inputStream, inputFileName, inputContainer);

            // then
            this.blobStorageBrokerMock.Verify(broker =>
                broker.GetBlobContainerClient(inputContainer),
                    Times.Once);

            this.blobStorageBrokerMock.Verify(broker =>
                broker.GetBlobClient(blobContainerClientMock.Object, inputFileName),
                    Times.Once);

            this.blobStorageBrokerMock.Verify(broker =>
                broker.CreateFileAsync(blobClientMock.Object, inputStream),
                    Times.Once);

            this.blobStorageBrokerMock.VerifyNoOtherCalls();
        }
    }
}
