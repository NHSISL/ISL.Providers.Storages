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
        public async Task
            ShouldThrowValidationExceptionOnCreateAndAssignAccessPoliciesToContainerAsyncWhenTypeIStorageValidationException()
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
                provider.CreateAndAssignAccessPoliciesToContainerAsync(It.IsAny<string>(), It.IsAny<List<string>>()))
                    .ThrowsAsync(someStorageValidationException);

            // when
            ValueTask createAndAssignAccessPoliciesToContainerTask =
                this.storageAbstractionProvider
                    .CreateAndAssignAccessPoliciesToContainerAsync(It.IsAny<string>(), It.IsAny<List<string>>());

            StorageProviderValidationException actualStorageValidationProviderException =
                await Assert.ThrowsAsync<StorageProviderValidationException>(
                    testCode: createAndAssignAccessPoliciesToContainerTask.AsTask);

            // then
            actualStorageValidationProviderException.Should().BeEquivalentTo(
                expectedStorageValidationProviderException);

            this.storageProviderMock.Verify(provider =>
                provider.CreateAndAssignAccessPoliciesToContainerAsync(It.IsAny<string>(), It.IsAny<List<string>>()),
                    Times.Once);

            this.storageProviderMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task
            ShouldThrowDependencyValidationExceptionOnCreateAndAssignAccessPoliciesToContainerAsyncWhenTypeIStorageDependencyValidationException()
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
                provider.CreateAndAssignAccessPoliciesToContainerAsync(It.IsAny<string>(), It.IsAny<List<string>>()))
                    .ThrowsAsync(someStorageValidationException);

            // when
            ValueTask createAndAssignAccessPoliciesToContainerAsyncTask =
                this.storageAbstractionProvider
                    .CreateAndAssignAccessPoliciesToContainerAsync(It.IsAny<string>(), It.IsAny<List<string>>());

            StorageProviderDependencyValidationException actualStorageValidationProviderException =
                await Assert.ThrowsAsync<StorageProviderDependencyValidationException>(
                    testCode: createAndAssignAccessPoliciesToContainerAsyncTask.AsTask);

            // then
            actualStorageValidationProviderException.Should().BeEquivalentTo(
                expectedStorageValidationProviderException);

            this.storageProviderMock.Verify(provider =>
                provider.CreateAndAssignAccessPoliciesToContainerAsync(It.IsAny<string>(), It.IsAny<List<string>>()),
                    Times.Once);

            this.storageProviderMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task
            ShouldThrowDependencyExceptionOnCreateAndAssignAccessPoliciesToContainerAsyncWhenTypeIStorageDependencyException()
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
                provider.CreateAndAssignAccessPoliciesToContainerAsync(It.IsAny<string>(), It.IsAny<List<string>>()))
                    .ThrowsAsync(someStorageValidationException);

            // when
            ValueTask createAndAssignAccessPoliciesToContainerAsyncTask =
                this.storageAbstractionProvider
                    .CreateAndAssignAccessPoliciesToContainerAsync(It.IsAny<string>(), It.IsAny<List<string>>());

            StorageProviderDependencyException actualStorageDependencyProviderException =
                await Assert.ThrowsAsync<StorageProviderDependencyException>(
                    testCode: createAndAssignAccessPoliciesToContainerAsyncTask.AsTask);

            // then
            actualStorageDependencyProviderException.Should().BeEquivalentTo(
                expectedStorageDependencyProviderException);

            this.storageProviderMock.Verify(provider =>
                provider.CreateAndAssignAccessPoliciesToContainerAsync(It.IsAny<string>(), It.IsAny<List<string>>()),
                    Times.Once);

            this.storageProviderMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task
            ShouldThrowServiceExceptionOnCreateAndAssignAccessPoliciesToContainerAsyncWhenTypeIStorageServiceException()
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
                provider.CreateAndAssignAccessPoliciesToContainerAsync(It.IsAny<string>(), It.IsAny<List<string>>()))
                    .ThrowsAsync(someStorageValidationException);

            // when
            ValueTask createAndAssignAccessPoliciesToContainerAsyncTask =
                this.storageAbstractionProvider
                    .CreateAndAssignAccessPoliciesToContainerAsync(It.IsAny<string>(), It.IsAny<List<string>>());

            StorageProviderServiceException actualStorageServiceProviderException =
                await Assert.ThrowsAsync<StorageProviderServiceException>(
                    testCode: createAndAssignAccessPoliciesToContainerAsyncTask.AsTask);

            // then
            actualStorageServiceProviderException.Should().BeEquivalentTo(
                expectedStorageServiceProviderException);

            this.storageProviderMock.Verify(provider =>
                provider.CreateAndAssignAccessPoliciesToContainerAsync(It.IsAny<string>(), It.IsAny<List<string>>()),
                    Times.Once);

            this.storageProviderMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task
            ShouldThrowUncatagorizedServiceExceptionOnCreateAndAssignAccessPoliciesToContainerAsyncWhenTypeIsNotExpected()
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
                provider.CreateAndAssignAccessPoliciesToContainerAsync(It.IsAny<string>(), It.IsAny<List<string>>()))
                    .ThrowsAsync(someException);

            // when
            ValueTask createAndAssignAccessPoliciesToContainerAsyncTask =
                this.storageAbstractionProvider
                    .CreateAndAssignAccessPoliciesToContainerAsync(It.IsAny<string>(), It.IsAny<List<string>>());

            StorageProviderServiceException actualStorageServiceProviderException =
                await Assert.ThrowsAsync<StorageProviderServiceException>(
                    testCode: createAndAssignAccessPoliciesToContainerAsyncTask.AsTask);

            // then
            actualStorageServiceProviderException.Should().BeEquivalentTo(
                expectedStorageServiceProviderException);

            this.storageProviderMock.Verify(provider =>
                provider.CreateAndAssignAccessPoliciesToContainerAsync(It.IsAny<string>(), It.IsAny<List<string>>()),
                    Times.Once);

            this.storageProviderMock.VerifyNoOtherCalls();
        }
    }
}
