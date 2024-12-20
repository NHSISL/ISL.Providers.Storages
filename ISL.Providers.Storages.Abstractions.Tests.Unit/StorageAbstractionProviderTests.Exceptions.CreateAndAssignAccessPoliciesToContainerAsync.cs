﻿// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using FluentAssertions;
using ISL.Providers.Storages.Abstractions.Models;
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
            ShouldThrowValidationExceptionOnCreateAndAssignAccessPoliciesAsyncWhenTypeIsStorageValidationException()
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
                provider.CreateAndAssignAccessPoliciesAsync(It.IsAny<string>(), It.IsAny<List<Policy>>()))
                    .ThrowsAsync(someStorageValidationException);

            // when
            ValueTask createAndAssignAccessPoliciesToContainerTask =
                this.storageAbstractionProvider
                    .CreateAndAssignAccessPoliciesAsync(It.IsAny<string>(), It.IsAny<List<Policy>>());

            StorageProviderValidationException actualStorageValidationProviderException =
                await Assert.ThrowsAsync<StorageProviderValidationException>(
                    testCode: createAndAssignAccessPoliciesToContainerTask.AsTask);

            // then
            actualStorageValidationProviderException.Should().BeEquivalentTo(
                expectedStorageValidationProviderException);

            this.storageProviderMock.Verify(provider =>
                provider.CreateAndAssignAccessPoliciesAsync(It.IsAny<string>(), It.IsAny<List<Policy>>()),
                    Times.Once);

            this.storageProviderMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task
            ShouldThrowDependencyExceptionOnCreateAndAssignAccessPoliciesAsyncWhenTypeIsStorageDependencyException()
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
                provider.CreateAndAssignAccessPoliciesAsync(It.IsAny<string>(), It.IsAny<List<Policy>>()))
                    .ThrowsAsync(someStorageValidationException);

            // when
            ValueTask createAndAssignAccessPoliciesAsyncTask =
                this.storageAbstractionProvider
                    .CreateAndAssignAccessPoliciesAsync(It.IsAny<string>(), It.IsAny<List<Policy>>());

            StorageProviderDependencyException actualStorageDependencyProviderException =
                await Assert.ThrowsAsync<StorageProviderDependencyException>(
                    testCode: createAndAssignAccessPoliciesAsyncTask.AsTask);

            // then
            actualStorageDependencyProviderException.Should().BeEquivalentTo(
                expectedStorageDependencyProviderException);

            this.storageProviderMock.Verify(provider =>
                provider.CreateAndAssignAccessPoliciesAsync(It.IsAny<string>(), It.IsAny<List<Policy>>()),
                    Times.Once);

            this.storageProviderMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task
            ShouldThrowServiceExceptionOnCreateAndAssignAccessPoliciesAsyncWhenTypeIsStorageServiceException()
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
                provider.CreateAndAssignAccessPoliciesAsync(It.IsAny<string>(), It.IsAny<List<Policy>>()))
                    .ThrowsAsync(someStorageValidationException);

            // when
            ValueTask createAndAssignAccessPoliciesAsyncTask =
                this.storageAbstractionProvider
                    .CreateAndAssignAccessPoliciesAsync(It.IsAny<string>(), It.IsAny<List<Policy>>());

            StorageProviderServiceException actualStorageServiceProviderException =
                await Assert.ThrowsAsync<StorageProviderServiceException>(
                    testCode: createAndAssignAccessPoliciesAsyncTask.AsTask);

            // then
            actualStorageServiceProviderException.Should().BeEquivalentTo(
                expectedStorageServiceProviderException);

            this.storageProviderMock.Verify(provider =>
                provider.CreateAndAssignAccessPoliciesAsync(It.IsAny<string>(), It.IsAny<List<Policy>>()),
                    Times.Once);

            this.storageProviderMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task
            ShouldThrowUncatagorizedServiceExceptionOnCreateAndAssignAccessPoliciesAsyncWhenTypeIsNotExpected()
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
                provider.CreateAndAssignAccessPoliciesAsync(It.IsAny<string>(), It.IsAny<List<Policy>>()))
                    .ThrowsAsync(someException);

            // when
            ValueTask createAndAssignAccessPoliciesAsyncTask =
                this.storageAbstractionProvider
                    .CreateAndAssignAccessPoliciesAsync(It.IsAny<string>(), It.IsAny<List<Policy>>());

            StorageProviderServiceException actualStorageServiceProviderException =
                await Assert.ThrowsAsync<StorageProviderServiceException>(
                    testCode: createAndAssignAccessPoliciesAsyncTask.AsTask);

            // then
            actualStorageServiceProviderException.Should().BeEquivalentTo(
                expectedStorageServiceProviderException);

            this.storageProviderMock.Verify(provider =>
                provider.CreateAndAssignAccessPoliciesAsync(It.IsAny<string>(), It.IsAny<List<Policy>>()),
                    Times.Once);

            this.storageProviderMock.VerifyNoOtherCalls();
        }
    }
}
