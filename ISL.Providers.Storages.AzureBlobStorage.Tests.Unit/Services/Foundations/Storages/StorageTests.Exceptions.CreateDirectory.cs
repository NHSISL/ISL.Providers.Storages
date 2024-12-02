// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using FluentAssertions;
using ISL.Providers.Storages.AzureBlobStorage.Models.Foundations.Files.Exceptions;
using Moq;
using System;
using System.Threading.Tasks;

namespace ISL.Providers.Storages.AzureBlobStorage.Tests.Unit.Services.Foundations.Storages
{
    public partial class StorageTests
    {
        [Theory]
        [MemberData(nameof(DependencyValidationExceptions))]
        public async Task ShouldThrowDependencyValidationExceptionOnCreateDirectoryAsync(
            Exception dependencyValidationException)
        {
            // given
            string randomContainerString = GetRandomString();
            string inputContainer = randomContainerString;
            string randomDirectoryString = GetRandomString();
            string inputDirectory = randomDirectoryString;

            var failedStorageDependencyValidationException =
                new FailedStorageDependencyValidationException(
                    message: "Failed storage dependency validation error occurred, please contact support.",
                    innerException: dependencyValidationException);

            var expectedStorageDependencyValidationException =
                new StorageDependencyValidationException(
                    message: "Storage dependency validation error occurred, please fix errors and try again.",
                    innerException: failedStorageDependencyValidationException);

            this.blobStorageBrokerMock.Setup(broker =>
                broker.GetDataLakeFileSystemClient(inputContainer))
                    .Throws(dependencyValidationException);

            // when
            ValueTask createDirectoryTask =
                this.storageService.CreateDirectoryAsync(inputContainer, inputDirectory);

            StorageDependencyValidationException actualStorageDependencyValidationException =
                await Assert.ThrowsAsync<StorageDependencyValidationException>(
                    testCode: createDirectoryTask.AsTask);

            // then
            actualStorageDependencyValidationException
                .Should().BeEquivalentTo(expectedStorageDependencyValidationException);

            this.blobStorageBrokerMock.Verify(broker =>
                broker.GetDataLakeFileSystemClient(inputContainer),
                    Times.Once);

            this.blobStorageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(DependencyExceptions))]
        public async Task ShouldThrowDependencyExceptionOnCreateDirectoryAsync(Exception dependencyException)
        {
            // given
            string randomContainerString = GetRandomString();
            string inputContainer = randomContainerString;
            string randomDirectoryString = GetRandomString();
            string inputDirectory = randomDirectoryString;

            var failedStorageDependencyException =
                new FailedStorageDependencyException(
                    message: "Failed storage dependency error occurred, please contact support.",
                    innerException: dependencyException);

            var expectedStorageDependencyException =
                new StorageDependencyException(
                    message: "Storage dependency error occurred, please fix errors and try again.",
                    innerException: failedStorageDependencyException);

            this.blobStorageBrokerMock.Setup(broker =>
                broker.GetDataLakeFileSystemClient(inputContainer))
                    .Throws(dependencyException);

            // when
            ValueTask createDirectoryTask =
                this.storageService.CreateDirectoryAsync(inputContainer, inputDirectory);

            StorageDependencyException actualStorageDependencyException =
                await Assert.ThrowsAsync<StorageDependencyException>(testCode: createDirectoryTask.AsTask);

            // then
            actualStorageDependencyException
                .Should().BeEquivalentTo(expectedStorageDependencyException);

            this.blobStorageBrokerMock.Verify(broker =>
                broker.GetDataLakeFileSystemClient(inputContainer),
                    Times.Once);

            this.blobStorageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnCreateDirectoryAsync()
        {
            // given
            Exception someException = new Exception();
            string randomContainerString = GetRandomString();
            string inputContainer = randomContainerString;
            string randomDirectoryString = GetRandomString();
            string inputDirectory = randomDirectoryString;

            var failedStorageServiceException =
                new FailedStorageServiceException(
                    message: "Failed storage service error occurred, please contact support.",
                    innerException: someException);

            var expectedStorageServiceException =
                new StorageServiceException(
                    message: "Storage service error occurred, please fix errors and try again.",
                    innerException: failedStorageServiceException);

            this.blobStorageBrokerMock.Setup(broker =>
                broker.GetDataLakeFileSystemClient(inputContainer))
                    .Throws(someException);

            // when
            ValueTask createDirectoryTask =
                this.storageService.CreateDirectoryAsync(inputContainer, inputDirectory);

            StorageServiceException actualStorageServiceException =
                await Assert.ThrowsAsync<StorageServiceException>(testCode: createDirectoryTask.AsTask);

            // then
            actualStorageServiceException
                .Should().BeEquivalentTo(expectedStorageServiceException);

            this.blobStorageBrokerMock.Verify(broker =>
                broker.GetDataLakeFileSystemClient(inputContainer),
                    Times.Once);

            this.blobStorageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
