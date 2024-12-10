// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using Azure.Storage.Blobs.Models;
using FluentAssertions;
using ISL.Providers.Storages.AzureBlobStorage.Models.Foundations.Files.Exceptions;
using Moq;
using System.Threading.Tasks;

namespace ISL.Providers.Storages.AzureBlobStorage.Tests.Unit.Services.Foundations.Storages
{
    public partial class StorageTests
    {
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public async Task ShouldThrowValidationExceptionOnRemoveAccessPolicyByNameIfArgumentsInvalidAndLogItAsync(
            string invalidText)
        {
            // given
            string invalidContainer = invalidText;
            string invalidPolicyName = invalidText;

            var invalidArgumentStorageException =
                new InvalidArgumentStorageException(
                    message: "Invalid storage service argument(s), please fix the errors and try again.");

            invalidArgumentStorageException.AddData(
                key: "Container",
                values: "Text is invalid");

            invalidArgumentStorageException.AddData(
                key: "PolicyName",
                values: "Text is invalid");

            var expectedStorageValidationException =
                new StorageValidationException(
                    message: "Storage validation error occurred, please fix errors and try again.",
                    innerException: invalidArgumentStorageException);

            // when
            ValueTask retrieveAccessPolicyTask =
                this.storageService.RemoveAccessPolicyByNameAsync(invalidContainer, invalidPolicyName);

            StorageValidationException actualStorageValidationException =
                await Assert.ThrowsAsync<StorageValidationException>(
                    testCode: retrieveAccessPolicyTask.AsTask);

            // then
            actualStorageValidationException
                .Should().BeEquivalentTo(expectedStorageValidationException);

            this.blobStorageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnRemoveByNameIfNotExistsAndLogItAsync()
        {
            string someContainer = GetRandomString();
            string somePolicyName = GetRandomString();

            BlobContainerAccessPolicy randomBlobContainerAccessPolicy =
                CreateRandomBlobContainerAccessPolicy();

            BlobContainerAccessPolicy outputBlobContainerAccessPolicy = randomBlobContainerAccessPolicy;

            var accessPolicyNotFoundStorageException =
                new AccessPolicyNotFoundStorageException(
                    message: "Access policy with the provided name was not found on this container.");

            var expectedStorageValidationException =
                new StorageValidationException(
                    message: "Storage validation error occurred, please fix errors and try again.",
                    innerException: accessPolicyNotFoundStorageException);

            this.blobStorageBrokerMock.Setup(broker =>
                broker.GetBlobContainerClient(someContainer))
                    .Returns(blobContainerClientMock.Object);

            this.blobStorageBrokerMock.Setup(broker =>
                broker.GetAccessPolicyAsync(blobContainerClientMock.Object))
                    .ReturnsAsync(outputBlobContainerAccessPolicy);

            // when
            ValueTask retrieveAccessPolicyTask =
                this.storageService.RemoveAccessPolicyByNameAsync(someContainer, somePolicyName);

            StorageValidationException actualStorageValidationException =
                await Assert.ThrowsAsync<StorageValidationException>(
                    testCode: retrieveAccessPolicyTask.AsTask);

            // then
            actualStorageValidationException
                .Should().BeEquivalentTo(expectedStorageValidationException);

            this.blobStorageBrokerMock.Verify(broker =>
                broker.GetBlobContainerClient(someContainer),
                    Times.Once);

            this.blobStorageBrokerMock.Verify(broker =>
                broker.GetAccessPolicyAsync(blobContainerClientMock.Object),
                    Times.Once);

            this.blobStorageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
