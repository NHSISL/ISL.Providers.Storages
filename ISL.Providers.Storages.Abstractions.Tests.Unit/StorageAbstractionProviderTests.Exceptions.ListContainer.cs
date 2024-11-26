// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using FluentAssertions;
using ISL.Providers.Storages.Abstractions.Models.Exceptions;
using ISL.Providers.Storages.Abstractions.Tests.Unit.Models.Exceptions;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xeptions;

namespace ISL.Providers.Storage.Abstractions.Tests.Unit
{
    public partial class StorageAbstractionProviderTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionOnListContainerWhenTypeIStorageValidationException()
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

            this.storageProviderMock.Setup(provider =>
                provider.ListFilesInContainerAsync(It.IsAny<string>()))
                    .ThrowsAsync(someStorageValidationException);

            // when
            ValueTask<List<string>> listContainerTask =
                this.storageAbstractionProvider
                    .ListFilesInContainerAsync(It.IsAny<string>());

            StorageProviderValidationException actualStorageValidationProviderException =
                await Assert.ThrowsAsync<StorageProviderValidationException>(testCode: listContainerTask.AsTask);

            // then
            actualStorageValidationProviderException.Should().BeEquivalentTo(
                expectedStorageValidationProviderException);

            this.storageProviderMock.Verify(provider =>
                provider.ListFilesInContainerAsync(It.IsAny<string>()),
                    Times.Once);

            this.storageProviderMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyValidationExceptionOnListContainerWhenTypeIStorageDependencyValidationException()
        {
            // given
            var someException = new Xeption();

            var someStorageValidationException =
                new SomeStorageDependencyValidationException(
                    message: "Some storage provider dependency validation exception occurred",
                    innerException: someException,
                    data: someException.Data);

            StorageProviderValidationException expectedStorageValidationProviderException =
                new StorageProviderValidationException(
                    message: "Storage provider validation errors occurred, please try again.",
                    innerException: someStorageValidationException);

            this.storageProviderMock.Setup(provider =>
                provider.ListFilesInContainerAsync(It.IsAny<string>()))
                    .ThrowsAsync(someStorageValidationException);

            // when
            ValueTask<List<string>> listContainerTask =
                this.storageAbstractionProvider
                    .ListFilesInContainerAsync(It.IsAny<string>());

            StorageProviderValidationException actualStorageValidationProviderException =
                await Assert.ThrowsAsync<StorageProviderValidationException>(testCode: listContainerTask.AsTask);

            // then
            actualStorageValidationProviderException.Should().BeEquivalentTo(
                expectedStorageValidationProviderException);

            this.storageProviderMock.Verify(provider =>
                provider.ListFilesInContainerAsync(It.IsAny<string>()),
                    Times.Once);

            this.storageProviderMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyExceptionOnListContainerWhenTypeIStorageDependencyException()
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
                provider.ListFilesInContainerAsync(It.IsAny<string>()))
                    .ThrowsAsync(someStorageValidationException);

            // when
            ValueTask<List<string>> listContainerTask =
                this.storageAbstractionProvider
                    .ListFilesInContainerAsync(It.IsAny<string>());

            StorageProviderDependencyException actualStorageDependencyProviderException =
                await Assert.ThrowsAsync<StorageProviderDependencyException>(testCode: listContainerTask.AsTask);

            // then
            actualStorageDependencyProviderException.Should().BeEquivalentTo(
                expectedStorageDependencyProviderException);

            this.storageProviderMock.Verify(provider =>
                provider.ListFilesInContainerAsync(It.IsAny<string>()),
                    Times.Once);

            this.storageProviderMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnListContainerWhenTypeIStorageServiceException()
        {
            // given
            var someException = new Xeption();

            var someStorageValidationException =
                new SomeStorageServiceException(
                    message: "Some storage provider service exception occurred",
                    innerException: someException);

            StorageProviderServiceException expectedStorageServiceProviderException =
                new StorageProviderServiceException(
                    message: "Storage provider service error occurred, contact support.",
                    innerException: someStorageValidationException);

            this.storageProviderMock.Setup(provider =>
                provider.ListFilesInContainerAsync(It.IsAny<string>()))
                    .ThrowsAsync(someStorageValidationException);

            // when
            ValueTask<List<string>> listContainerTask =
                this.storageAbstractionProvider
                    .ListFilesInContainerAsync(It.IsAny<string>());

            StorageProviderServiceException actualStorageServiceProviderException =
                await Assert.ThrowsAsync<StorageProviderServiceException>(testCode: listContainerTask.AsTask);

            // then
            actualStorageServiceProviderException.Should().BeEquivalentTo(
                expectedStorageServiceProviderException);

            this.storageProviderMock.Verify(provider =>
                provider.ListFilesInContainerAsync(It.IsAny<string>()),
                    Times.Once);

            this.storageProviderMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowUncatagorizedServiceExceptionOnListContainerWhenTypeIsNotExpected()
        {
            // given
            var someException = new Xeption();

            var uncatagorizedStorageProviderException =
                new UncatagorizedStorageProviderException(
                    message: "Storage provider not properly implemented. Uncatagorized errors found, " +
                            "contact the storage provider owner for support.",
                    innerException: someException,
                    data: someException.Data);

            StorageProviderServiceException expectedStorageServiceProviderException =
                new StorageProviderServiceException(
                    message: "Uncatagorized storage provider service error occurred, contact support.",
                    innerException: uncatagorizedStorageProviderException);

            this.storageProviderMock.Setup(provider =>
                provider.ListFilesInContainerAsync(It.IsAny<string>()))
                    .ThrowsAsync(someException);

            // when
            ValueTask<List<string>> listContainerTask =
                this.storageAbstractionProvider
                    .ListFilesInContainerAsync(It.IsAny<string>());

            StorageProviderServiceException actualStorageServiceProviderException =
                await Assert.ThrowsAsync<StorageProviderServiceException>(testCode: listContainerTask.AsTask);

            // then
            actualStorageServiceProviderException.Should().BeEquivalentTo(
                expectedStorageServiceProviderException);

            this.storageProviderMock.Verify(provider =>
                provider.ListFilesInContainerAsync(It.IsAny<string>()),
                    Times.Once);

            this.storageProviderMock.VerifyNoOtherCalls();
        }
    }
}
