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
        public async Task ShouldThrowDependencyValidationExceptionOnCreateDirectorySasTokenAsync(
            Exception dependencyValidationException)
        {
            // given
            string randomString = GetRandomString();
            string someContainer = randomString;
            string someDirectoryPath = randomString;
            string someAccessPolicyIdentifier = randomString;
            string inputContainer = someContainer;
            string inputDirectoryPath = someDirectoryPath;
            string inputAccessPolicyIdentifier = someAccessPolicyIdentifier;

            var failedStorageDependencyValidationException =
                new FailedStorageDependencyValidationException(
                    message: "Failed storage dependency validation error occurred, please contact support.",
                    innerException: dependencyValidationException);

            var expectedStorageDependencyValidationException =
                new StorageDependencyValidationException(
                    message: "Storage dependency validation error occurred, please fix errors and try again.",
                    innerException: failedStorageDependencyValidationException);

            this.blobStorageBrokerMock.Setup(broker =>
                broker.CreateDirectorySasTokenAsync(
                    someContainer,
                    someDirectoryPath,
                    someAccessPolicyIdentifier))
                        .Throws(dependencyValidationException);

            // when
            ValueTask<string> createDirectorySasTokenTask =
                this.storageService.CreateSasTokenAsync(
                    someContainer,
                    someDirectoryPath,
                    someAccessPolicyIdentifier);

            StorageDependencyValidationException actualStorageDependencyValidationException =
                await Assert.ThrowsAsync<StorageDependencyValidationException>(
                    testCode: createDirectorySasTokenTask.AsTask);

            // then
            actualStorageDependencyValidationException
                .Should().BeEquivalentTo(expectedStorageDependencyValidationException);

            this.blobStorageBrokerMock.Verify(broker =>
                broker.CreateDirectorySasTokenAsync(
                    someContainer,
                    someDirectoryPath,
                    someAccessPolicyIdentifier),
                    Times.Once);

            this.blobStorageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(DependencyExceptions))]
        public async Task ShouldThrowDependencyExceptionOnCreateDirectorySasTokenAsync(Exception dependencyException)
        {
            // given
            string randomString = GetRandomString();
            string someContainer = randomString;
            string someDirectoryPath = randomString;
            string someAccessPolicyIdentifier = randomString;
            string inputContainer = someContainer;
            string inputDirectoryPath = someDirectoryPath;
            string inputAccessPolicyIdentifier = someAccessPolicyIdentifier;

            var failedStorageDependencyException =
                new FailedStorageDependencyException(
                    message: "Failed storage dependency error occurred, please contact support.",
                    innerException: dependencyException);

            var expectedStorageDependencyException =
                new StorageDependencyException(
                    message: "Storage dependency error occurred, please fix errors and try again.",
                    innerException: failedStorageDependencyException);

            this.blobStorageBrokerMock.Setup(broker =>
                broker.CreateDirectorySasTokenAsync(
                    someContainer,
                    someDirectoryPath,
                    someAccessPolicyIdentifier))
                        .Throws(dependencyException);

            // when
            ValueTask<string> createDirectorySasTokenTask =
                this.storageService.CreateSasTokenAsync(
                    someContainer,
                    someDirectoryPath,
                    someAccessPolicyIdentifier);

            StorageDependencyException actualStorageDependencyException =
                await Assert.ThrowsAsync<StorageDependencyException>(
                    testCode: createDirectorySasTokenTask.AsTask);

            // then
            actualStorageDependencyException
                .Should().BeEquivalentTo(expectedStorageDependencyException);

            this.blobStorageBrokerMock.Verify(broker =>
                broker.CreateDirectorySasTokenAsync(
                    someContainer,
                    someDirectoryPath,
                    someAccessPolicyIdentifier),
                    Times.Once);

            this.blobStorageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnCreateDirectorySasTokenAsync()
        {
            // given
            Exception someException = new Exception();
            string randomString = GetRandomString();
            string someContainer = randomString;
            string someDirectoryPath = randomString;
            string someAccessPolicyIdentifier = randomString;
            string inputContainer = someContainer;
            string inputDirectoryPath = someDirectoryPath;
            string inputAccessPolicyIdentifier = someAccessPolicyIdentifier;

            var failedStorageServiceException =
                new FailedStorageServiceException(
                    message: "Failed storage service error occurred, please contact support.",
                    innerException: someException);

            var expectedStorageServiceException =
                new StorageServiceException(
                    message: "Storage service error occurred, please fix errors and try again.",
                    innerException: failedStorageServiceException);

            this.blobStorageBrokerMock.Setup(broker =>
                broker.CreateDirectorySasTokenAsync(
                    someContainer,
                    someDirectoryPath,
                    someAccessPolicyIdentifier))
                        .Throws(someException);

            // when
            ValueTask<string> createDirectorySasTokenTask =
                this.storageService.CreateSasTokenAsync(
                    someContainer,
                    someDirectoryPath,
                    someAccessPolicyIdentifier);

            StorageServiceException actualStorageServiceException =
                await Assert.ThrowsAsync<StorageServiceException>(
                    testCode: createDirectorySasTokenTask.AsTask);

            // then
            actualStorageServiceException
                .Should().BeEquivalentTo(expectedStorageServiceException);

            this.blobStorageBrokerMock.Verify(broker =>
                broker.CreateDirectorySasTokenAsync(
                    someContainer,
                    someDirectoryPath,
                    someAccessPolicyIdentifier),
                        Times.Once);

            this.blobStorageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
