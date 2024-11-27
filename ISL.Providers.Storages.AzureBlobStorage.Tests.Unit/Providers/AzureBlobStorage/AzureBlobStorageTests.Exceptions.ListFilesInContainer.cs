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
        public async Task ShouldThrowProviderValidationExceptionOnListFilesInContainer()
        {
            // given
            string randomContainer = GetRandomString();
            string inputContainer = randomContainer;

            var storageValidationException = new StorageValidationException(
                message: "Storage validation error occurred, please fix errors and try again.",
                innerException: new Xeption());

            var expectedAzureBlobStorageProviderValidationException =
                new AzureBlobStorageProviderValidationException(
                    message: "Azure blob storage provider validation error occurred, fix errors and try again.",
                    innerException: (Xeption)storageValidationException.InnerException,
                    data: storageValidationException.InnerException.Data);

            this.storageServiceMock.Setup(service =>
                service.ListFilesInContainerAsync(inputContainer))
                    .ThrowsAsync(storageValidationException);

            // when
            ValueTask<List<string>> listFilesTask =
                this.azureBlobStorageProvider.ListFilesInContainerAsync(inputContainer);

            AzureBlobStorageProviderValidationException actualAzureBlobStorageProviderValidationException =
                await Assert.ThrowsAsync<AzureBlobStorageProviderValidationException>(
                    testCode: listFilesTask.AsTask);

            // then
            actualAzureBlobStorageProviderValidationException
                .Should().BeEquivalentTo(expectedAzureBlobStorageProviderValidationException);

            this.storageServiceMock.Verify(service =>
                service.ListFilesInContainerAsync(inputContainer),
                    Times.Once);

            this.storageServiceMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowProviderValidationExceptionOnListFilesInContainerDependencyValidation()
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
                service.ListFilesInContainerAsync(inputContainer))
                    .ThrowsAsync(storageDependencyValidationException);

            // when
            ValueTask<List<string>> listFilesTask =
                this.azureBlobStorageProvider.ListFilesInContainerAsync(inputContainer);

            AzureBlobStorageProviderValidationException
                actualAzureBlobStorageProviderValidationException =
                    await Assert.ThrowsAsync<AzureBlobStorageProviderValidationException>(
                        testCode: listFilesTask.AsTask);

            // then
            actualAzureBlobStorageProviderValidationException
                .Should().BeEquivalentTo(expectedAzureBlobStorageProviderValidationException);

            this.storageServiceMock.Verify(service =>
                service.ListFilesInContainerAsync(inputContainer),
                    Times.Once);

            this.storageServiceMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowProviderDependencyExceptionOnListFilesInContainer()
        {
            // given
            string randomContainer = GetRandomString();
            string inputContainer = randomContainer;

            var storageDependencyException = new StorageDependencyException(
                message: "Storage dependency error occurred, please fix errors and try again.",
                innerException: new Xeption());

            var expectedAzureBlobStorageProviderDependencyException =
                new AzureBlobStorageProviderDependencyException(
                    message: "Azure blob storage provider dependency error occurred, " +
                        "contact support.",
                    innerException: (Xeption)storageDependencyException.InnerException);

            this.storageServiceMock.Setup(service =>
                service.ListFilesInContainerAsync(inputContainer))
                    .ThrowsAsync(storageDependencyException);

            // when
            ValueTask<List<string>> listFilesTask =
                this.azureBlobStorageProvider.ListFilesInContainerAsync(inputContainer);

            AzureBlobStorageProviderDependencyException
                actualAzureBlobStorageProviderDependencyException =
                    await Assert.ThrowsAsync<AzureBlobStorageProviderDependencyException>(
                        testCode: listFilesTask.AsTask);

            // then
            actualAzureBlobStorageProviderDependencyException
                .Should().BeEquivalentTo(expectedAzureBlobStorageProviderDependencyException);

            this.storageServiceMock.Verify(service =>
                service.ListFilesInContainerAsync(inputContainer),
                    Times.Once);

            this.storageServiceMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowProviderServiceExceptionOnListFilesInContainer()
        {
            // given
            string randomContainer = GetRandomString();
            string inputContainer = randomContainer;

            var storageServiceException = new StorageServiceException(
                message: "Storage service error occurred, please fix errors and try again.",
                innerException: new Xeption());

            var expectedAzureBlobStorageProviderServiceException =
                new AzureBlobStorageProviderServiceException(
                    message: "Azure blob storage provider service error occurred, " +
                        "contact support.",
                    innerException: (Xeption)storageServiceException.InnerException);

            this.storageServiceMock.Setup(service =>
                service.ListFilesInContainerAsync(inputContainer))
                    .ThrowsAsync(storageServiceException);

            // when
            ValueTask<List<string>> listFilesTask =
                this.azureBlobStorageProvider.ListFilesInContainerAsync(inputContainer);

            AzureBlobStorageProviderServiceException
                actualAzureBlobStorageProviderServiceException =
                    await Assert.ThrowsAsync<AzureBlobStorageProviderServiceException>(
                        testCode: listFilesTask.AsTask);

            // then
            actualAzureBlobStorageProviderServiceException
                .Should().BeEquivalentTo(expectedAzureBlobStorageProviderServiceException);

            this.storageServiceMock.Verify(service =>
                service.ListFilesInContainerAsync(inputContainer),
                    Times.Once);

            this.storageServiceMock.VerifyNoOtherCalls();
        }
    }
}
