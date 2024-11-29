using FluentAssertions;
using ISL.Providers.Storages.AzureBlobStorage.Models.Foundations.Files.Exceptions;
using ISL.Providers.Storages.AzureBlobStorage.Models.Providers.Exceptions;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xeptions;

namespace ISL.Providers.Storages.AzureBlobStorage.Tests.Unit.Providers.AzureBlobStorage
{
    public partial class AzureBlobStorageTests
    {
        [Fact]
        public async Task ShouldThrowProviderValidationExceptionOnRetrieveAllContainers()
        {
            // given
            var storageValidationException = new StorageValidationException(
                message: "Storage validation error occurred, please fix errors and try again.",
                innerException: new Xeption());

            var expectedAzureBlobStorageProviderValidationException =
                new AzureBlobStorageProviderValidationException(
                    message: "Azure blob storage provider validation error occurred, fix errors and try again.",
                    innerException: (Xeption)storageValidationException.InnerException,
                    data: storageValidationException.InnerException.Data);

            this.storageServiceMock.Setup(service =>
                service.RetrieveAllContainersAsync())
                    .ThrowsAsync(storageValidationException);

            // when
            ValueTask<List<string>> retrieveContainersTask =
                this.azureBlobStorageProvider.RetrieveAllContainersAsync();

            AzureBlobStorageProviderValidationException actualAzureBlobStorageProviderValidationException =
                await Assert.ThrowsAsync<AzureBlobStorageProviderValidationException>(
                    testCode: retrieveContainersTask.AsTask);

            // then
            actualAzureBlobStorageProviderValidationException
                .Should().BeEquivalentTo(expectedAzureBlobStorageProviderValidationException);

            this.storageServiceMock.Verify(service =>
                service.RetrieveAllContainersAsync(),
                    Times.Once);

            this.storageServiceMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowProviderValidationExceptionOnRetrieveAllContainersDependencyValidation()
        {
            // given
            string randomContainer = GetRandomString();
            string inputContainer = randomContainer;

            var storageDependencyValidationException = new StorageDependencyValidationException(
                message: "Storage dependency validation error occurred, please fix errors and try again.",
                innerException: new Xeption());

            var expectedAzureBlobStorageProviderValidationException =
                new AzureBlobStorageProviderValidationException(
                    message: "Azure blob storage provider validation error occurred, fix errors and try again.",
                    innerException: (Xeption)storageDependencyValidationException.InnerException,
                    data: storageDependencyValidationException.InnerException.Data);

            this.storageServiceMock.Setup(service =>
                service.RetrieveAllContainersAsync())
                    .ThrowsAsync(storageDependencyValidationException);

            // when
            ValueTask<List<string>> retrieveContainersTask =
                this.azureBlobStorageProvider.RetrieveAllContainersAsync();

            AzureBlobStorageProviderValidationException
                actualAzureBlobStorageProviderValidationException =
                    await Assert.ThrowsAsync<AzureBlobStorageProviderValidationException>(
                        testCode: retrieveContainersTask.AsTask);

            // then
            actualAzureBlobStorageProviderValidationException
                .Should().BeEquivalentTo(expectedAzureBlobStorageProviderValidationException);

            this.storageServiceMock.Verify(service =>
                service.RetrieveAllContainersAsync(),
                    Times.Once);

            this.storageServiceMock.VerifyNoOtherCalls();
        }
    }
}
