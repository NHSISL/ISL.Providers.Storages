// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using FluentAssertions;
using ISL.Providers.Storages.Abstractions.Models.Exceptions;
using ISL.Providers.Storages.Abstractions.Tests.Unit.Models.Exceptions;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xeptions;

namespace ISL.Providers.Storage.Abstractions.Tests.Unit
{
    public partial class StorageAbstractionProviderTests
    {
        [Fact]
        public async Task
            ShouldThrowValidationExceptionOnCreateSasTokenWithAccessPolicyAsyncWhenTypeIsStorageValidationException()
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
                service.CreateSasTokenAsync(
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<DateTimeOffset>()))
                        .ThrowsAsync(someStorageValidationException);

            // when
            ValueTask<string> createSasTokenTask =
                this.storageAbstractionProvider.CreateSasTokenAsync(
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<DateTimeOffset>());

            StorageProviderValidationException actualAzureBlobStorageProviderValidationException =
                await Assert.ThrowsAsync<StorageProviderValidationException>(
                    testCode: createSasTokenTask.AsTask);

            // then
            actualAzureBlobStorageProviderValidationException
                .Should().BeEquivalentTo(expectedStorageValidationProviderException);

            this.storageProviderMock.Verify(service =>
                service.CreateSasTokenAsync(
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<DateTimeOffset>()),
                        Times.Once);

            this.storageProviderMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task
            ShouldThrowDependencyExceptionOnCreateSasTokenWithAccessPolicyAsyncWhenTypeIsStorageDependencyException()
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
                provider.CreateSasTokenAsync(
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<DateTimeOffset>()))
                    .ThrowsAsync(someStorageValidationException);

            // when
            ValueTask<string> createSasTokenTask =
                this.storageAbstractionProvider.CreateSasTokenAsync(
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<DateTimeOffset>());

            StorageProviderDependencyException actualStorageDependencyProviderException =
                await Assert.ThrowsAsync<StorageProviderDependencyException>(
                    testCode: createSasTokenTask.AsTask);

            // then
            actualStorageDependencyProviderException.Should().BeEquivalentTo(
                expectedStorageDependencyProviderException);

            this.storageProviderMock.Verify(provider =>
                provider.CreateSasTokenAsync(
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<DateTimeOffset>()),
                        Times.Once);

            this.storageProviderMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task
            ShouldThrowServiceExceptionOnCreateSasTokenWithAccessPolicyAsyncWhenTypeIsStorageServiceException()
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
                 provider.CreateSasTokenAsync(
                     It.IsAny<string>(),
                     It.IsAny<string>(),
                     It.IsAny<string>(),
                     It.IsAny<DateTimeOffset>()))
                        .ThrowsAsync(someStorageValidationException);

            // when
            ValueTask<string> createSasTokenTask =
                this.storageAbstractionProvider.CreateSasTokenAsync(
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<DateTimeOffset>());

            StorageProviderServiceException actualStorageServiceProviderException =
                await Assert.ThrowsAsync<StorageProviderServiceException>(
                    testCode: createSasTokenTask.AsTask);

            // then
            actualStorageServiceProviderException.Should().BeEquivalentTo(
                expectedStorageServiceProviderException);

            this.storageProviderMock.Verify(provider =>
                provider.CreateSasTokenAsync(
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<DateTimeOffset>()),
                        Times.Once);

            this.storageProviderMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task
            ShouldThrowUncatagorizedServiceExceptionOnCreateSasTokenWithAccessPolicyAsyncWhenTypeIsNotExpected()
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
                 provider.CreateSasTokenAsync(
                     It.IsAny<string>(),
                     It.IsAny<string>(),
                     It.IsAny<string>(),
                     It.IsAny<DateTimeOffset>()))
                        .ThrowsAsync(someException);

            // when
            ValueTask<string> createSasTokenTask =
                this.storageAbstractionProvider.CreateSasTokenAsync(
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<DateTimeOffset>());

            StorageProviderServiceException actualStorageServiceProviderException =
                await Assert.ThrowsAsync<StorageProviderServiceException>(
                    testCode: createSasTokenTask.AsTask);

            // then
            actualStorageServiceProviderException.Should().BeEquivalentTo(
                expectedStorageServiceProviderException);

            this.storageProviderMock.Verify(provider =>
                provider.CreateSasTokenAsync(
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<DateTimeOffset>()),
                        Times.Once);

            this.storageProviderMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task
            ShouldThrowValidationExceptionOnCreateSasTokenWithPermissionListAsyncWhenTypeIsStorageValidationException()
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
                service.CreateSasTokenAsync(
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<DateTimeOffset>(),
                    It.IsAny<List<string>>()))
                        .ThrowsAsync(someStorageValidationException);

            // when
            ValueTask<string> createSasTokenTask =
                this.storageAbstractionProvider.CreateSasTokenAsync(
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<DateTimeOffset>(),
                    It.IsAny<List<string>>());

            StorageProviderValidationException actualAzureBlobStorageProviderValidationException =
                await Assert.ThrowsAsync<StorageProviderValidationException>(
                    testCode: createSasTokenTask.AsTask);

            // then
            actualAzureBlobStorageProviderValidationException
                .Should().BeEquivalentTo(expectedStorageValidationProviderException);

            this.storageProviderMock.Verify(service =>
                service.CreateSasTokenAsync(
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<DateTimeOffset>(),
                    It.IsAny<List<string>>()),
                        Times.Once);

            this.storageProviderMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task
            ShouldThrowDependencyExceptionOnCreateSasTokenWithPermissionListAsyncWhenTypeIsStorageDependencyException()
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
                provider.CreateSasTokenAsync(
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<DateTimeOffset>(),
                    It.IsAny<List<string>>()))
                        .ThrowsAsync(someStorageValidationException);

            // when
            ValueTask<string> createSasTokenTask =
                this.storageAbstractionProvider.CreateSasTokenAsync(
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<DateTimeOffset>(),
                    It.IsAny<List<string>>());

            StorageProviderDependencyException actualStorageDependencyProviderException =
                await Assert.ThrowsAsync<StorageProviderDependencyException>(
                    testCode: createSasTokenTask.AsTask);

            // then
            actualStorageDependencyProviderException.Should().BeEquivalentTo(
                expectedStorageDependencyProviderException);

            this.storageProviderMock.Verify(provider =>
                provider.CreateSasTokenAsync(
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<DateTimeOffset>(),
                    It.IsAny<List<string>>()),
                        Times.Once);

            this.storageProviderMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task
            ShouldThrowServiceExceptionOnCreateSasTokenWithPermissionListAsyncWhenTypeIsStorageServiceException()
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
                 provider.CreateSasTokenAsync(
                     It.IsAny<string>(),
                     It.IsAny<string>(),
                     It.IsAny<DateTimeOffset>(),
                     It.IsAny<List<string>>()))
                        .ThrowsAsync(someStorageValidationException);

            // when
            ValueTask<string> createSasTokenTask =
                this.storageAbstractionProvider.CreateSasTokenAsync(
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<DateTimeOffset>(),
                    It.IsAny<List<string>>());

            StorageProviderServiceException actualStorageServiceProviderException =
                await Assert.ThrowsAsync<StorageProviderServiceException>(
                    testCode: createSasTokenTask.AsTask);

            // then
            actualStorageServiceProviderException.Should().BeEquivalentTo(
                expectedStorageServiceProviderException);

            this.storageProviderMock.Verify(provider =>
                provider.CreateSasTokenAsync(
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<DateTimeOffset>(),
                    It.IsAny<List<string>>()),
                        Times.Once);

            this.storageProviderMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task
            ShouldThrowUncatagorizedServiceExceptionOnCreateSasTokenWithPermissionListAsyncWhenTypeIsNotExpected()
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
                 provider.CreateSasTokenAsync(
                     It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<DateTimeOffset>(),
                    It.IsAny<List<string>>()))
                    .ThrowsAsync(someException);

            // when
            ValueTask<string> createSasTokenTask =
                this.storageAbstractionProvider.CreateSasTokenAsync(
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<DateTimeOffset>(),
                    It.IsAny<List<string>>());

            StorageProviderServiceException actualStorageServiceProviderException =
                await Assert.ThrowsAsync<StorageProviderServiceException>(
                    testCode: createSasTokenTask.AsTask);

            // then
            actualStorageServiceProviderException.Should().BeEquivalentTo(
                expectedStorageServiceProviderException);

            this.storageProviderMock.Verify(provider =>
                provider.CreateSasTokenAsync(
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<DateTimeOffset>(),
                    It.IsAny<List<string>>()),
                        Times.Once);

            this.storageProviderMock.VerifyNoOtherCalls();
        }
    }
}
