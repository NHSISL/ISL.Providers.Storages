// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using Azure.Storage.Blobs.Models;
using FluentAssertions;
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
        public async Task ShouldThrowDependencyValidationExceptionOnCreateContainerAsync(
            Exception dependencyValidationException)
        {
            // given
            string randomString = GetRandomString();
            string someContainer = randomString;
            string inputContainer = someContainer;

            var failedStorageDependencyValidationException =
                new FailedStorageDependencyValidationException(
                    message: "Failed storage dependency validation error occurred, please contact support.",
                    innerException: dependencyValidationException);

            var expectedStorageDependencyValidationException =
                new StorageDependencyValidationException(
                    message: "Storage dependency validation error occurred, please fix errors and try again.",
                    innerException: failedStorageDependencyValidationException);

            this.blobServiceClientMock.Setup(client =>
                client.CreateBlobContainerAsync(inputContainer, PublicAccessType.None, null, default))
                    .Throws(dependencyValidationException);

            // when
            ValueTask createContainerTask =
                this.storageService.CreateContainerAsync(inputContainer);

            StorageDependencyValidationException actualStorageDependencyValidationException =
                await Assert.ThrowsAsync<StorageDependencyValidationException>(testCode: createContainerTask.AsTask);

            // then
            actualStorageDependencyValidationException
                .Should().BeEquivalentTo(expectedStorageDependencyValidationException);

            this.blobServiceClientMock.Verify(client =>
                client.CreateBlobContainerAsync(inputContainer, PublicAccessType.None, null, default),
                    Times.Once);

            this.blobServiceClientMock.VerifyNoOtherCalls();
            this.dataLakeServiceClientMock.VerifyNoOtherCalls();
            this.dataLakeFileSystemClientMock.VerifyNoOtherCalls();
            this.blobContainerClientMock.VerifyNoOtherCalls();
            this.blobClientMock.VerifyNoOtherCalls();
            this.blobStorageBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(DependencyExceptions))]
        public async Task ShouldThrowDependencyExceptionOnCreateContainerAsync(Exception dependencyException)
        {
            // given
            string randomString = GetRandomString();
            string someContainer = randomString;
            string inputContainer = someContainer;

            var failedStorageDependencyException =
                new FailedStorageDependencyException(
                    message: "Failed storage dependency error occurred, please contact support.",
                    innerException: dependencyException);

            var expectedStorageDependencyException =
                new StorageDependencyException(
                    message: "Storage dependency error occurred, please fix errors and try again.",
                    innerException: failedStorageDependencyException);

            this.blobServiceClientMock.Setup(client =>
                client.CreateBlobContainerAsync(inputContainer, PublicAccessType.None, null, default))
                    .Throws(dependencyException);

            // when
            ValueTask createContainerTask =
                this.storageService.CreateContainerAsync(inputContainer);

            StorageDependencyException actualStorageDependencyException =
                await Assert.ThrowsAsync<StorageDependencyException>(testCode: createContainerTask.AsTask);

            // then
            actualStorageDependencyException
                .Should().BeEquivalentTo(expectedStorageDependencyException);

            this.blobServiceClientMock.Verify(client =>
                client.CreateBlobContainerAsync(inputContainer, PublicAccessType.None, null, default),
                    Times.Once);

            this.blobServiceClientMock.VerifyNoOtherCalls();
            this.dataLakeServiceClientMock.VerifyNoOtherCalls();
            this.dataLakeFileSystemClientMock.VerifyNoOtherCalls();
            this.blobContainerClientMock.VerifyNoOtherCalls();
            this.blobClientMock.VerifyNoOtherCalls();
            this.blobStorageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnCreateContainerAsync()
        {
            // given
            Exception someException = new Exception();
            string randomString = GetRandomString();
            string someContainer = randomString;
            string inputContainer = someContainer;

            var failedStorageServiceException =
                new FailedStorageServiceException(
                    message: "Failed storage service error occurred, please contact support.",
                    innerException: someException);

            var expectedStorageServiceException =
                new StorageServiceException(
                    message: "Storage service error occurred, please fix errors and try again.",
                    innerException: failedStorageServiceException);

            this.blobServiceClientMock.Setup(client =>
                client.CreateBlobContainerAsync(inputContainer, PublicAccessType.None, null, default))
                    .Throws(someException);

            // when
            ValueTask createContainerTask =
                this.storageService.CreateContainerAsync(inputContainer);

            StorageServiceException actualStorageServiceException =
                await Assert.ThrowsAsync<StorageServiceException>(testCode: createContainerTask.AsTask);

            // then
            actualStorageServiceException
                .Should().BeEquivalentTo(expectedStorageServiceException);

            this.blobServiceClientMock.Verify(client =>
                client.CreateBlobContainerAsync(inputContainer, PublicAccessType.None, null, default),
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
