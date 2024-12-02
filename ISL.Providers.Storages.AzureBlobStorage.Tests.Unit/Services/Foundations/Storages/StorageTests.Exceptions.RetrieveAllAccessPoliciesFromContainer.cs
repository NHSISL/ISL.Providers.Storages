// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using FluentAssertions;
using ISL.Providers.Storages.AzureBlobStorage.Models.Foundations.Files.Exceptions;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ISL.Providers.Storages.AzureBlobStorage.Tests.Unit.Services.Foundations.Storages
{
    public partial class StorageTests
    {
        [Theory]
        [MemberData(nameof(DependencyValidationExceptions))]
        public async Task ShouldThrowDependencyValidationExceptionOnRetrieveAllAccessPoliciesFromContainerAsync(
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

            this.blobStorageBrokerMock.Setup(broker =>
                broker.GetBlobContainerClient(inputContainer))
                    .Throws(dependencyValidationException);

            // when
            ValueTask<List<string>> retrieveAllAccessPoliciesTask =
                this.storageService.RetrieveAllAccessPoliciesFromContainerAsync(inputContainer);

            StorageDependencyValidationException actualStorageDependencyValidationException =
                await Assert.ThrowsAsync<StorageDependencyValidationException>(
                    testCode: retrieveAllAccessPoliciesTask.AsTask);

            // then
            actualStorageDependencyValidationException
                .Should().BeEquivalentTo(expectedStorageDependencyValidationException);

            this.blobStorageBrokerMock.Verify(broker =>
                broker.GetBlobContainerClient(inputContainer),
                    Times.Once);

            this.blobStorageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(DependencyExceptions))]
        public async Task ShouldThrowDependencyExceptionOnRetrieveAllAccessPoliciesFromContainerAsync(
            Exception dependencyException)
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

            this.blobStorageBrokerMock.Setup(broker =>
                broker.GetBlobContainerClient(inputContainer))
                    .Throws(dependencyException);

            // when
            ValueTask<List<string>> retrieveAllAccessPoliciesTask =
                this.storageService.RetrieveAllAccessPoliciesFromContainerAsync(inputContainer);

            StorageDependencyException actualStorageDependencyException =
                await Assert.ThrowsAsync<StorageDependencyException>(
                    testCode: retrieveAllAccessPoliciesTask.AsTask);

            // then
            actualStorageDependencyException
                .Should().BeEquivalentTo(expectedStorageDependencyException);

            this.blobStorageBrokerMock.Verify(broker =>
                broker.GetBlobContainerClient(inputContainer),
                    Times.Once);

            this.blobStorageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnRetrieveAllAccessPoliciesFromContainerAsync()
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

            this.blobStorageBrokerMock.Setup(broker =>
                broker.GetBlobContainerClient(inputContainer))
                    .Throws(someException);

            // when
            ValueTask<List<string>> retrieveAllAccessPoliciesTask =
                this.storageService.RetrieveAllAccessPoliciesFromContainerAsync(inputContainer);

            StorageServiceException actualStorageServiceException =
                await Assert.ThrowsAsync<StorageServiceException>(
                    testCode: retrieveAllAccessPoliciesTask.AsTask);

            // then
            actualStorageServiceException
                .Should().BeEquivalentTo(expectedStorageServiceException);

            this.blobStorageBrokerMock.Verify(broker =>
                broker.GetBlobContainerClient(inputContainer),
                    Times.Once);

            this.blobStorageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
