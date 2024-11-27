using FluentAssertions;
using ISL.Providers.Storages.Abstractions.Models.Exceptions;
using ISL.Providers.Storages.Abstractions.Tests.Unit.Models.Exceptions;
using Moq;
using System;
using System.Threading.Tasks;
using Xeptions;

namespace ISL.Providers.Storage.Abstractions.Tests.Unit
{
    public partial class StorageAbstractionProviderTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionOnCreateDirectorySasTokenAsyncWhenTypeIStorageValidationException()
        {
            // given
            var someException = new Xeption();

            var someStorageValidationException =
                new SomeStorageValidationException(
                    message: "Some storage provider validation exception occurred",
                    innerException: someException,
                    data: someException.Data);

            StorageProviderValidationException expectedStorageValidationProviderException =
                new StorageProviderValidationException(
                    message: "Storage provider validation errors occurred, please try again.",
                    innerException: someStorageValidationException);

            this.storageProviderMock.Setup(service =>
                service.CreateDirectorySasTokenAsync(
                    It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<DateTimeOffset>()))
                        .ThrowsAsync(someStorageValidationException);

            // when
            ValueTask<string> createDirectorySasTokenTask =
                this.storageAbstractionProvider.CreateDirectorySasTokenAsync(
                    It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<DateTimeOffset>());

            StorageProviderValidationException actualAzureBlobStorageProviderValidationException =
                await Assert.ThrowsAsync<StorageProviderValidationException>(
                    testCode: createDirectorySasTokenTask.AsTask);

            // then
            actualAzureBlobStorageProviderValidationException
                .Should().BeEquivalentTo(expectedStorageValidationProviderException);

            this.storageProviderMock.Verify(service =>
                service.CreateDirectorySasTokenAsync(
                    It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<DateTimeOffset>()),
                        Times.Once);

            this.storageProviderMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task
            ShouldThrowDependencyValidationExceptionOnCreateDirectorySasTokenAsyncWhenTypeIStorageDependencyValidationException()
        {
            // given
            var someException = new Xeption();

            var someStorageValidationException =
                new SomeStorageDependencyValidationException(
                    message: "Some storage provider dependency validation exception occurred",
                    innerException: someException,
                    data: someException.Data);

            StorageProviderDependencyValidationException expectedStorageValidationProviderException =
                new StorageProviderDependencyValidationException(
                    message: "Storage provider dependency validation errors occurred, please try again.",
                    innerException: someStorageValidationException);

            this.storageProviderMock.Setup(provider =>
                provider.CreateDirectorySasTokenAsync(
                    It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<DateTimeOffset>()))
                    .ThrowsAsync(someStorageValidationException);

            // when
            ValueTask<string> createDirectorySasTokenTask =
                this.storageAbstractionProvider.CreateDirectorySasTokenAsync(
                    It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<DateTimeOffset>());

            StorageProviderDependencyValidationException actualStorageValidationProviderException =
                await Assert.ThrowsAsync<StorageProviderDependencyValidationException>(
                    testCode: createDirectorySasTokenTask.AsTask);

            // then
            actualStorageValidationProviderException.Should().BeEquivalentTo(
                expectedStorageValidationProviderException);

            this.storageProviderMock.Verify(provider =>
                provider.CreateDirectorySasTokenAsync(
                    It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<DateTimeOffset>()),
                        Times.Once);

            this.storageProviderMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task
            ShouldThrowDependencyExceptionOnCreateDirectorySasTokenAsyncWhenTypeIStorageDependencyException()
        {
            // given
            var someException = new Xeption();

            var someStorageValidationException =
                new SomeStorageDependencyException(
                    message: "Some storage provider dependency exception occurred",
                    innerException: someException);

            StorageProviderDependencyException expectedStorageDependencyProviderException =
                new StorageProviderDependencyException(
                    message: "Storage provider dependency error occurred, contact support.",
                    innerException: someStorageValidationException);

            this.storageProviderMock.Setup(provider =>
                provider.CreateDirectorySasTokenAsync(
                    It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<DateTimeOffset>()))
                    .ThrowsAsync(someStorageValidationException);

            // when
            ValueTask<string> createDirectorySasTokenTask =
                this.storageAbstractionProvider.CreateDirectorySasTokenAsync(
                    It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<DateTimeOffset>());

            StorageProviderDependencyException actualStorageDependencyProviderException =
                await Assert.ThrowsAsync<StorageProviderDependencyException>(
                    testCode: createDirectorySasTokenTask.AsTask);

            // then
            actualStorageDependencyProviderException.Should().BeEquivalentTo(
                expectedStorageDependencyProviderException);

            this.storageProviderMock.Verify(provider =>
                provider.CreateDirectorySasTokenAsync(
                    It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<DateTimeOffset>()),
                        Times.Once);

            this.storageProviderMock.VerifyNoOtherCalls();
        }
    }
}
