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

            this.dataLakeServiceClientMock.Setup(client =>
                client.GetFileSystemClient(inputContainer))
                    .Throws(dependencyValidationException);

            // when
            ValueTask createDirectoryTask =
                this.storageService.CreateDirectoryAsync(inputContainer, inputDirectory);

            StorageDependencyValidationException actualStorageDependencyValidationException =
                await Assert.ThrowsAsync<StorageDependencyValidationException>(testCode: createDirectoryTask.AsTask);

            // then
            actualStorageDependencyValidationException
                .Should().BeEquivalentTo(expectedStorageDependencyValidationException);

            this.dataLakeServiceClientMock.Verify(client =>
                client.GetFileSystemClient(inputContainer),
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

            this.dataLakeServiceClientMock.Setup(client =>
                client.GetFileSystemClient(inputContainer))
                    .Throws(dependencyException);

            // when
            ValueTask createDirectoryTask =
                this.storageService.CreateDirectoryAsync(inputContainer, inputDirectory);

            StorageDependencyException actualStorageDependencyException =
                await Assert.ThrowsAsync<StorageDependencyException>(testCode: createDirectoryTask.AsTask);

            // then
            actualStorageDependencyException
                .Should().BeEquivalentTo(expectedStorageDependencyException);

            this.dataLakeServiceClientMock.Verify(client =>
                client.GetFileSystemClient(inputContainer),
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
