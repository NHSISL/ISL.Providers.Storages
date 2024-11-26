using FluentAssertions;
using ISL.Providers.Storages.AzureBlobStorage.Models.Foundations.Files.Exceptions;
using ISL.Providers.Storages.AzureBlobStorage.Models.Providers.Exceptions;
using Moq;
using System.IO;
using System.Threading.Tasks;
using Xeptions;

namespace ISL.Providers.Storages.AzureBlobStorage.Tests.Unit.Providers.AzureBlobStorage
{
    public partial class AzureBlobStorageTests
    {
        [Fact]
        public async Task ShouldThrowProviderValidationExceptionOnRetrieveFile()
        {
            // given
            string randomFileName = GetRandomString();
            string randomContainer = GetRandomString();
            Stream randomStream = new ZeroLengthStream();
            string inputFileName = randomFileName;
            string inputContainer = randomContainer;
            Stream outputStream = randomStream;

            var storageValidationException = new StorageValidationException(
                message: "Storage validation error occurred, please fix errors and try again.",
                innerException: new Xeption());

            var expectedAzureBlobStorageProviderValidationException =
                new AzureBlobStorageProviderValidationException(
                    message: "Azure blob storage provider validation error occurred, fix errors and try again.",
                    innerException: (Xeption)storageValidationException.InnerException,
                    data: storageValidationException.InnerException.Data);

            this.storageServiceMock.Setup(service =>
                service.RetrieveFileAsync(outputStream, inputFileName, inputContainer))
                    .ThrowsAsync(storageValidationException);

            // when
            ValueTask retrieveFileTask =
                this.azureBlobStorageProvider.CreateFileAsync(outputStream, inputFileName, inputContainer);

            AzureBlobStorageProviderValidationException actualAzureBlobStorageProviderValidationException =
                await Assert.ThrowsAsync<AzureBlobStorageProviderValidationException>(
                    testCode: retrieveFileTask.AsTask);

            // then
            actualAzureBlobStorageProviderValidationException
                .Should().BeEquivalentTo(expectedAzureBlobStorageProviderValidationException);

            this.storageServiceMock.Verify(service =>
                service.CreateFileAsync(outputStream, inputFileName, inputContainer),
                Times.Once);

            this.storageServiceMock.VerifyNoOtherCalls();
        }
    }
}
