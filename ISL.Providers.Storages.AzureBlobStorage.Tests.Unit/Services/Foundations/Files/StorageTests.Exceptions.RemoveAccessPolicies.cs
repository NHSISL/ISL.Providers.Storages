// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

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
        public async Task ShouldThrowDependencyValidationExceptionOnRemoveAccessPoliciesAsync(
            Exception dependencyValidationException)
        {
            // given
            string randomString = GetRandomString();
            string inputContainer = randomString;

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
            ValueTask removeAccessPolicyTask =
                this.storageService.RemoveAccessPoliciesFromContainerAsync(inputContainer);

            StorageDependencyValidationException actualStorageDependencyValidationException =
                await Assert.ThrowsAsync<StorageDependencyValidationException>(
                    testCode: removeAccessPolicyTask.AsTask);

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
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(DependencyExceptions))]
        public async Task ShouldThrowDependencyExceptionOnRemoveAccessPoliciesAsync(Exception dependencyException)
        {
            // given
            string randomString = GetRandomString();
            string inputContainer = randomString; ;

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
            ValueTask removeAccessPolicyTask =
                this.storageService.RemoveAccessPoliciesFromContainerAsync(inputContainer);

            StorageDependencyException actualStorageDependencyException =
                await Assert.ThrowsAsync<StorageDependencyException>(testCode: removeAccessPolicyTask.AsTask);

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
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
