﻿using FluentAssertions;
using ISL.Providers.Storages.AzureBlobStorage.Models.Foundations.Files.Exceptions;
using Moq;
using System;
using System.Threading.Tasks;

namespace ISL.Providers.Storages.AzureBlobStorage.Tests.Unit.Services.Foundations.Files
{
    public partial class StorageTests
    {
        [Theory]
        [MemberData(nameof(DependencyValidationExceptions))]
        public async Task ShouldThrowDependencyValidationExceptionOnGetDownloadLinkAsync(
            Exception dependencyValidationException)
        {
            // given
            string randomString = GetRandomString();
            DateTimeOffset someDateTimeOffset = GetRandomFutureDateTimeOffset();
            string someFileName = randomString;
            string someContainer = randomString;
            string inputFileName = someFileName;
            string inputContainer = someContainer;
            DateTimeOffset inputDateTimeOffset = someDateTimeOffset;

            var failedStorageDependencyValidationException =
                new FailedStorageDependencyValidationException(
                    message: "Failed storage dependency validation error occurred, please contact support.",
                    innerException: dependencyValidationException);

            var expectedStorageDependencyValidationException =
                new StorageDependencyValidationException(
                    message: "Storage dependency validation error occurred, please fix errors and try again.",
                    innerException: failedStorageDependencyValidationException);

            this.blobServiceClientMock.Setup(client =>
                client.GetBlobContainerClient(inputContainer))
                    .Throws(dependencyValidationException);

            // when
            ValueTask<string> getDownloadLinkTask =
                this.storageService.GetDownloadLinkAsync(inputFileName, inputContainer, inputDateTimeOffset);

            StorageDependencyValidationException actualStorageDependencyValidationException =
                await Assert.ThrowsAsync<StorageDependencyValidationException>(testCode: getDownloadLinkTask.AsTask);

            // then
            actualStorageDependencyValidationException
                .Should().BeEquivalentTo(expectedStorageDependencyValidationException);

            this.blobServiceClientMock.Verify(client =>
                client.GetBlobContainerClient(inputContainer),
                    Times.Once);

            this.blobServiceClientMock.VerifyNoOtherCalls();
            this.blobContainerClientMock.VerifyNoOtherCalls();
            this.blobClientMock.VerifyNoOtherCalls();
            this.blobStorageBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(DependencyExceptions))]
        public async Task ShouldThrowDependencyExceptionOnGetDownloadLinkAsync(Exception dependencyException)
        {
            // given
            string randomString = GetRandomString();
            DateTimeOffset someDateTimeOffset = GetRandomFutureDateTimeOffset();
            string someFileName = randomString;
            string someContainer = randomString;
            string inputFileName = someFileName;
            string inputContainer = someContainer;
            DateTimeOffset inputDateTimeOffset = someDateTimeOffset;

            var failedStorageDependencyException =
                new FailedStorageDependencyException(
                    message: "Failed storage dependency error occurred, please contact support.",
                    innerException: dependencyException);

            var expectedStorageDependencyException =
                new StorageDependencyException(
                    message: "Storage dependency error occurred, please fix errors and try again.",
                    innerException: failedStorageDependencyException);

            this.blobServiceClientMock.Setup(client =>
                client.GetBlobContainerClient(inputContainer))
                    .Throws(dependencyException);

            // when
            ValueTask<string> getDownloadLinkTask =
                this.storageService.GetDownloadLinkAsync(inputFileName, inputContainer, inputDateTimeOffset);

            StorageDependencyException actualStorageDependencyException =
                await Assert.ThrowsAsync<StorageDependencyException>(testCode: getDownloadLinkTask.AsTask);

            // then
            actualStorageDependencyException
                .Should().BeEquivalentTo(expectedStorageDependencyException);

            this.blobServiceClientMock.Verify(client =>
                client.GetBlobContainerClient(inputContainer),
                    Times.Once);

            this.blobServiceClientMock.VerifyNoOtherCalls();
            this.blobContainerClientMock.VerifyNoOtherCalls();
            this.blobClientMock.VerifyNoOtherCalls();
            this.blobStorageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnGetDownloadLinkAsync()
        {
            // given
            Exception someException = new Exception();
            string randomString = GetRandomString();
            DateTimeOffset someDateTimeOffset = GetRandomFutureDateTimeOffset();
            string someFileName = randomString;
            string someContainer = randomString;
            string inputFileName = someFileName;
            string inputContainer = someContainer;
            DateTimeOffset inputDateTimeOffset = someDateTimeOffset;

            var failedStorageServiceException =
                new FailedStorageServiceException(
                    message: "Failed storage service error occurred, please contact support.",
                    innerException: someException);

            var expectedStorageServiceException =
                new StorageServiceException(
                    message: "Storage service error occurred, please fix errors and try again.",
                    innerException: failedStorageServiceException);

            this.blobServiceClientMock.Setup(client =>
                client.GetBlobContainerClient(inputContainer))
                    .Throws(someException);

            // when
            ValueTask<string> getDownloadLinkTask =
                this.storageService.GetDownloadLinkAsync(inputFileName, inputContainer, inputDateTimeOffset);

            StorageServiceException actualStorageServiceException =
                await Assert.ThrowsAsync<StorageServiceException>(testCode: getDownloadLinkTask.AsTask);

            // then
            actualStorageServiceException
                .Should().BeEquivalentTo(expectedStorageServiceException);

            this.blobServiceClientMock.Verify(client =>
                client.GetBlobContainerClient(inputContainer),
                    Times.Once);

            this.blobServiceClientMock.VerifyNoOtherCalls();
            this.blobContainerClientMock.VerifyNoOtherCalls();
            this.blobClientMock.VerifyNoOtherCalls();
            this.blobStorageBrokerMock.VerifyNoOtherCalls();
        }
    }
}