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
            ShouldThrowValidationExceptionOnRetrieveAllAccessPoliciesFromContainerAsyncWhenTypeIStorageValidationException()
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
                provider.RetrieveAllAccessPoliciesFromContainerAsync(It.IsAny<string>()))
                    .ThrowsAsync(someStorageValidationException);

            // when
            ValueTask<List<string>> retrieveAllAccessPoliciesFromContainerAsyncTask =
                this.storageAbstractionProvider
                    .RetrieveAllAccessPoliciesFromContainerAsync(It.IsAny<string>());

            StorageProviderValidationException actualStorageValidationProviderException =
                await Assert.ThrowsAsync<StorageProviderValidationException>(
                    testCode: retrieveAllAccessPoliciesFromContainerAsyncTask.AsTask);

            // then
            actualStorageValidationProviderException.Should().BeEquivalentTo(
                expectedStorageValidationProviderException);

            this.storageProviderMock.Verify(provider =>
                provider.RetrieveAllAccessPoliciesFromContainerAsync(It.IsAny<string>()),
                    Times.Once);

            this.storageProviderMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task
            ShouldThrowDependencyValidationExceptionOnRetrieveAllAccessPoliciesFromContainerAsyncWhenTypeIStorageDependencyValidationException()
        {
            // given
            var someException = new Xeption();

            var someStorageValidationException =
                new SomeStorageDependencyValidationException(
                    message: "Some storage provider dependency validation exception occurred",
                    innerException: someException,
                    data: someException.Data);

            StorageProviderDependencyValidationException expectedStorageDependencyValidationProviderException =
                new StorageProviderDependencyValidationException(
                    message: "Storage provider dependency validation errors occurred, please try again.",
                    innerException: someStorageValidationException);

            this.storageProviderMock.Setup(provider =>
                provider.RetrieveAllAccessPoliciesFromContainerAsync(It.IsAny<string>()))
                    .ThrowsAsync(someStorageValidationException);

            // when
            ValueTask<List<string>> retrieveAllAccessPoliciesFromContainerAsyncTask =
                this.storageAbstractionProvider
                    .RetrieveAllAccessPoliciesFromContainerAsync(It.IsAny<string>());

            StorageProviderDependencyValidationException actualStorageValidationProviderException =
                await Assert.ThrowsAsync<StorageProviderDependencyValidationException>(
                    testCode: retrieveAllAccessPoliciesFromContainerAsyncTask.AsTask);

            // then
            actualStorageValidationProviderException.Should().BeEquivalentTo(
                expectedStorageDependencyValidationProviderException);

            this.storageProviderMock.Verify(provider =>
                provider.RetrieveAllAccessPoliciesFromContainerAsync(It.IsAny<string>()),
                    Times.Once);

            this.storageProviderMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task
            ShouldThrowDependencyExceptionOnRetrieveAllAccessPoliciesFromContainerAsyncWhenTypeIStorageDependencyException()
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
                provider.RetrieveAllAccessPoliciesFromContainerAsync(It.IsAny<string>()))
                    .ThrowsAsync(someStorageValidationException);

            // when
            ValueTask<List<string>> retrieveAllAccessPoliciesFromContainerAsyncTask =
                this.storageAbstractionProvider
                    .RetrieveAllAccessPoliciesFromContainerAsync(It.IsAny<string>());

            StorageProviderDependencyException actualStorageDependencyProviderException =
                await Assert.ThrowsAsync<StorageProviderDependencyException>(
                    testCode: retrieveAllAccessPoliciesFromContainerAsyncTask.AsTask);

            // then
            actualStorageDependencyProviderException.Should().BeEquivalentTo(
                expectedStorageDependencyProviderException);

            this.storageProviderMock.Verify(provider =>
                provider.RetrieveAllAccessPoliciesFromContainerAsync(It.IsAny<string>()),
                    Times.Once);

            this.storageProviderMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task
            ShouldThrowServiceExceptionOnRetrieveAllAccessPoliciesFromContainerAsyncWhenTypeIStorageServiceException()
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
                provider.RetrieveAllAccessPoliciesFromContainerAsync(It.IsAny<string>()))
                    .ThrowsAsync(someStorageValidationException);

            // when
            ValueTask<List<string>> retrieveAllAccessPoliciesFromContainerAsyncTask =
                this.storageAbstractionProvider
                    .RetrieveAllAccessPoliciesFromContainerAsync(It.IsAny<string>());

            StorageProviderServiceException actualStorageServiceProviderException =
                await Assert.ThrowsAsync<StorageProviderServiceException>(
                    testCode: retrieveAllAccessPoliciesFromContainerAsyncTask.AsTask);

            // then
            actualStorageServiceProviderException.Should().BeEquivalentTo(
                expectedStorageServiceProviderException);

            this.storageProviderMock.Verify(provider =>
                provider.RetrieveAllAccessPoliciesFromContainerAsync(It.IsAny<string>()),
                    Times.Once);

            this.storageProviderMock.VerifyNoOtherCalls();
        }
    }
}
