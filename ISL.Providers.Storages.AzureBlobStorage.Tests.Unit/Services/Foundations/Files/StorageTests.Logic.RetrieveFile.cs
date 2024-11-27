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
            byte[] randomoutputData = CreateRandomData();
            Stream returnedOutputStream = new MemoryStream(randomoutputData);
            var mockResponse = new Mock<Azure.Response>();

            this.blobServiceClientMock.Setup(client =>
                client.GetBlobContainerClient(inputContainer))
                    .Returns(blobContainerClientMock.Object);

            this.blobContainerClientMock.Setup(client =>
                client.GetBlobClient(inputFileName))
                    .Returns(blobClientMock.Object);

            this.blobClientMock.Setup(client =>
                client.DownloadToAsync(outputStream))
                    .Callback<Stream>((output) =>
                    {
                        returnedOutputStream.Position = 0;
                        returnedOutputStream.CopyTo(outputStream);
                    }).Returns(Task.FromResult(mockResponse.Object));

            // when
            await this.storageService.RetrieveFileAsync(outputStream, inputFileName, inputContainer);

            // then
            this.blobServiceClientMock.Verify(client =>
                client.GetBlobContainerClient(inputContainer),
                    Times.Once);

            this.blobContainerClientMock.Verify(client =>
                client.GetBlobClient(inputFileName),
                    Times.Once);

            this.blobClientMock.Verify(client =>
                client.DownloadToAsync(outputStream),
                    Times.Once);

            this.blobServiceClientMock.VerifyNoOtherCalls();
            this.dataLakeServiceClientMock.VerifyNoOtherCalls();
            this.dataLakeFileSystemClientMock.VerifyNoOtherCalls();
            this.blobContainerClientMock.VerifyNoOtherCalls();
            this.blobClientMock.VerifyNoOtherCalls();
            this.blobStorageBrokerMock.VerifyNoOtherCalls();
        }
    }
}
