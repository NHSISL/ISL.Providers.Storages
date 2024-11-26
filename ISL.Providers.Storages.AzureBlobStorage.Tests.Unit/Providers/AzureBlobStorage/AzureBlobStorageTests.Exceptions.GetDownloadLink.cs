using FluentAssertions;
using ISL.Providers.Storages.AzureBlobStorage.Models.Foundations.Files.Exceptions;
using ISL.Providers.Storages.AzureBlobStorage.Models.Providers.Exceptions;
using Moq;
using System;
using System.Threading.Tasks;
using Xeptions;

namespace ISL.Providers.Storages.AzureBlobStorage.Tests.Unit.Providers.AzureBlobStorage
{
    public partial class AzureBlobStorageTests
    {
        [Fact]
        public async Task ShouldThrowProviderValidationExceptionOnGetDownloadLink()
        {
            // given
            string randomFileName = GetRandomString();
            string randomContainer = GetRandomString();
            DateTimeOffset randomDateTimeOffset = GetRandomFutureDateTimeOffset();
            string inputFileName = randomFileName;
            string inputContainer = randomContainer;
            DateTimeOffset inputDateTimeOffset = randomDateTimeOffset;

            var storageValidationException = new StorageValidationException(
                message: "Storage validation error occurred, please fix errors and try again.",
                innerException: new Xeption());

            var expectedAzureBlobStorageProviderValidationException =
                new AzureBlobStorageProviderValidationException(
                    message: "Azure blob storage provider validation error occurred, fix errors and try again.",
                    innerException: (Xeption)storageValidationException.InnerException,
                    data: storageValidationException.InnerException.Data);

            this.storageServiceMock.Setup(service =>
                service.GetDownloadLinkAsync(inputFileName, inputContainer, inputDateTimeOffset))
                    .ThrowsAsync(storageValidationException);

            // when
            ValueTask<string> getDownloadLinkTask =
                this.azureBlobStorageProvider.GetDownloadLinkAsync(inputFileName, inputContainer, inputDateTimeOffset);

            AzureBlobStorageProviderValidationException actualAzureBlobStorageProviderValidationException =
                await Assert.ThrowsAsync<AzureBlobStorageProviderValidationException>(
                    testCode: getDownloadLinkTask.AsTask);

            // then
            actualAzureBlobStorageProviderValidationException
                .Should().BeEquivalentTo(expectedAzureBlobStorageProviderValidationException);

            this.storageServiceMock.Verify(service =>
                service.GetDownloadLinkAsync(inputFileName, inputContainer, inputDateTimeOffset),
                Times.Once);

            this.storageServiceMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowProviderDependencyValidationExceptionOnGetDownloadLink()
        {
            // given
            string randomFileName = GetRandomString();
            string randomContainer = GetRandomString();
            DateTimeOffset randomDateTimeOffset = GetRandomFutureDateTimeOffset();
            string inputFileName = randomFileName;
            string inputContainer = randomContainer;
            DateTimeOffset inputDateTimeOffset = randomDateTimeOffset;

            var storageDependencyValidationException = new StorageDependencyValidationException(
                message: "Storage dependency validation error occurred, please fix errors and try again.",
                innerException: new Xeption());

            var expectedAzureBlobStorageProviderDependencyValidationException =
                new AzureBlobStorageProviderDependencyValidationException(
                    message: "Azure blob storage provider dependency validation error occurred, " +
                            "fix errors and try again.",
                    innerException: (Xeption)storageDependencyValidationException.InnerException,
                    data: storageDependencyValidationException.InnerException.Data);

            this.storageServiceMock.Setup(service =>
                service.GetDownloadLinkAsync(inputFileName, inputContainer, inputDateTimeOffset))
                    .ThrowsAsync(storageDependencyValidationException);

            // when
            ValueTask<string> getDownloadLinkTask =
                this.azureBlobStorageProvider.GetDownloadLinkAsync(inputFileName, inputContainer, inputDateTimeOffset);

            AzureBlobStorageProviderDependencyValidationException
                actualAzureBlobStorageProviderDependencyValidationException =
                    await Assert.ThrowsAsync<AzureBlobStorageProviderDependencyValidationException>(
                        testCode: getDownloadLinkTask.AsTask);

            // then
            actualAzureBlobStorageProviderDependencyValidationException
                .Should().BeEquivalentTo(expectedAzureBlobStorageProviderDependencyValidationException);

            this.storageServiceMock.Verify(service =>
                service.GetDownloadLinkAsync(inputFileName, inputContainer, inputDateTimeOffset),
                Times.Once);

            this.storageServiceMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowProviderDependencyExceptionOnGetDownloadLink()
        {
            // given
            string randomFileName = GetRandomString();
            string randomContainer = GetRandomString();
            DateTimeOffset randomDateTimeOffset = GetRandomFutureDateTimeOffset();
            string inputFileName = randomFileName;
            string inputContainer = randomContainer;
            DateTimeOffset inputDateTimeOffset = randomDateTimeOffset;

            var storageDependencyException = new StorageDependencyException(
                message: "Storage dependency error occurred, please fix errors and try again.",
                innerException: new Xeption());

            var expectedAzureBlobStorageProviderDependencyException =
                new AzureBlobStorageProviderDependencyException(
                    message: "Azure blob storage provider dependency error occurred, " +
                            "contact support.",
                    innerException: (Xeption)storageDependencyException.InnerException);

            this.storageServiceMock.Setup(service =>
                service.GetDownloadLinkAsync(inputFileName, inputContainer, inputDateTimeOffset))
                    .ThrowsAsync(storageDependencyException);

            // when
            ValueTask<string> getDownloadLinkTask =
                this.azureBlobStorageProvider.GetDownloadLinkAsync(inputFileName, inputContainer, inputDateTimeOffset);

            AzureBlobStorageProviderDependencyException
                actualAzureBlobStorageProviderDependencyException =
                    await Assert.ThrowsAsync<AzureBlobStorageProviderDependencyException>(
                        testCode: getDownloadLinkTask.AsTask);

            // then
            actualAzureBlobStorageProviderDependencyException
                .Should().BeEquivalentTo(expectedAzureBlobStorageProviderDependencyException);

            this.storageServiceMock.Verify(service =>
                service.GetDownloadLinkAsync(inputFileName, inputContainer, inputDateTimeOffset),
                Times.Once);

            this.storageServiceMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowProviderServiceExceptionOnGetDownloadLink()
        {
            // given
            string randomFileName = GetRandomString();
            string randomContainer = GetRandomString();
            DateTimeOffset randomDateTimeOffset = GetRandomFutureDateTimeOffset();
            string inputFileName = randomFileName;
            string inputContainer = randomContainer;
            DateTimeOffset inputDateTimeOffset = randomDateTimeOffset;

            var storageServiceException = new StorageServiceException(
                message: "Storage service error occurred, please fix errors and try again.",
                innerException: new Xeption());

            var expectedAzureBlobStorageProviderServiceException =
                new AzureBlobStorageProviderServiceException(
                    message: "Azure blob storage provider service error occurred, " +
                            "contact support.",
                    innerException: (Xeption)storageServiceException.InnerException);

            this.storageServiceMock.Setup(service =>
                service.GetDownloadLinkAsync(inputFileName, inputContainer, inputDateTimeOffset))
                    .ThrowsAsync(storageServiceException);

            // when
            ValueTask<string> getDownloadLinkTask =
                this.azureBlobStorageProvider.GetDownloadLinkAsync(inputFileName, inputContainer, inputDateTimeOffset);

            AzureBlobStorageProviderServiceException
                actualAzureBlobStorageProviderServiceException =
                    await Assert.ThrowsAsync<AzureBlobStorageProviderServiceException>(
                        testCode: getDownloadLinkTask.AsTask);

            // then
            actualAzureBlobStorageProviderServiceException
                .Should().BeEquivalentTo(expectedAzureBlobStorageProviderServiceException);

            this.storageServiceMock.Verify(service =>
                service.GetDownloadLinkAsync(inputFileName, inputContainer, inputDateTimeOffset),
                Times.Once);

            this.storageServiceMock.VerifyNoOtherCalls();
        }
    }
}
