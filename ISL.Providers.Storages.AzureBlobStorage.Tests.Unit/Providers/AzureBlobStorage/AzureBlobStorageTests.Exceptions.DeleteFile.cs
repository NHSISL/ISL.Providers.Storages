﻿using FluentAssertions;
using ISL.Providers.Storages.AzureBlobStorage.Models.Foundations.Files.Exceptions;
using ISL.Providers.Storages.AzureBlobStorage.Models.Providers.Exceptions;
using Moq;
using System.Threading.Tasks;
using Xeptions;

namespace ISL.Providers.Storages.AzureBlobStorage.Tests.Unit.Providers.AzureBlobStorage
{
    public partial class AzureBlobStorageTests
    {
        [Fact]
        public async Task ShouldThrowProviderValidationExceptionOnDeleteFile()
        {
            // given
            string randomFileName = GetRandomString();
            string randomContainer = GetRandomString();
            string inputFileName = randomFileName;
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
                service.DeleteFileAsync(inputFileName, inputContainer))
                    .ThrowsAsync(storageValidationException);

            // when
            ValueTask deleteFileTask =
                this.azureBlobStorageProvider.DeleteFileAsync(inputFileName, inputContainer);

            AzureBlobStorageProviderValidationException actualAzureBlobStorageProviderValidationException =
                await Assert.ThrowsAsync<AzureBlobStorageProviderValidationException>(
                    testCode: deleteFileTask.AsTask);

            // then
            actualAzureBlobStorageProviderValidationException
                .Should().BeEquivalentTo(expectedAzureBlobStorageProviderValidationException);

            this.storageServiceMock.Verify(service =>
                service.DeleteFileAsync(inputFileName, inputContainer),
                Times.Once);

            this.storageServiceMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowProviderDependencyValidationExceptionOnDeleteFile()
        {
            // given
            string randomFileName = GetRandomString();
            string randomContainer = GetRandomString();
            string inputFileName = randomFileName;
            string inputContainer = randomContainer;

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
                service.DeleteFileAsync(inputFileName, inputContainer))
                    .ThrowsAsync(storageDependencyValidationException);

            // when
            ValueTask createFileTask =
                this.azureBlobStorageProvider.DeleteFileAsync(inputFileName, inputContainer);

            AzureBlobStorageProviderDependencyValidationException
                actualAzureBlobStorageProviderDependencyValidationException =
                    await Assert.ThrowsAsync<AzureBlobStorageProviderDependencyValidationException>(
                        testCode: createFileTask.AsTask);

            // then
            actualAzureBlobStorageProviderDependencyValidationException
                .Should().BeEquivalentTo(expectedAzureBlobStorageProviderDependencyValidationException);

            this.storageServiceMock.Verify(service =>
                service.DeleteFileAsync(inputFileName, inputContainer),
                Times.Once);

            this.storageServiceMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowProviderDependencyExceptionOnDeleteFile()
        {
            // given
            string randomFileName = GetRandomString();
            string randomContainer = GetRandomString();
            string inputFileName = randomFileName;
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
                service.DeleteFileAsync(inputFileName, inputContainer))
                    .ThrowsAsync(storageDependencyException);

            // when
            ValueTask deleteFileTask =
                this.azureBlobStorageProvider.DeleteFileAsync(inputFileName, inputContainer);

            AzureBlobStorageProviderDependencyException
                actualAzureBlobStorageProviderDependencyException =
                    await Assert.ThrowsAsync<AzureBlobStorageProviderDependencyException>(
                        testCode: deleteFileTask.AsTask);

            // then
            actualAzureBlobStorageProviderDependencyException
                .Should().BeEquivalentTo(expectedAzureBlobStorageProviderDependencyException);

            this.storageServiceMock.Verify(service =>
                service.DeleteFileAsync(inputFileName, inputContainer),
                Times.Once);

            this.storageServiceMock.VerifyNoOtherCalls();
        }
    }
}