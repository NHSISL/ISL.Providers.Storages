// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using FluentAssertions;
using ISL.Providers.Storages.AzureBlobStorage.Models.Foundations.Files.Exceptions;
using ISL.Providers.Storages.AzureBlobStorage.Services.Foundations.Storages;
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
        public async Task ShouldThrowDependencyValidationExceptionOnCreateSasTokenWithAccessPolicyAsync(
            Exception dependencyValidationException)
        {
            // given
            string someContainer = GetRandomString();
            string somePath = GetRandomString();
            string somePolicyIdentifier = GetRandomString();
            DateTimeOffset someFutureDateTimeOffset = GetRandomFutureDateTimeOffset();

            var failedStorageDependencyValidationException =
                new FailedStorageDependencyValidationException(
                    message: "Failed storage dependency validation error occurred, please contact support.",
                    innerException: dependencyValidationException);

            var expectedStorageDependencyValidationException =
                new StorageDependencyValidationException(
                    message: "Storage dependency validation error occurred, please fix errors and try again.",
                    innerException: failedStorageDependencyValidationException);

            this.blobStorageBrokerMock.Setup(broker =>
                broker.CreateSasTokenAsync(
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    someFutureDateTimeOffset,
                    It.IsAny<bool>(),
                    It.IsAny<string>()))
                        .Throws(dependencyValidationException);

            // when
            ValueTask<string> createDirectorySasTokenTask =
                this.storageService.CreateSasTokenAsync(
                    someContainer,
                    somePath,
                    somePolicyIdentifier,
                    someFutureDateTimeOffset);

            StorageDependencyValidationException actualStorageDependencyValidationException =
                await Assert.ThrowsAsync<StorageDependencyValidationException>(
                    testCode: createDirectorySasTokenTask.AsTask);

            // then
            actualStorageDependencyValidationException
                .Should().BeEquivalentTo(expectedStorageDependencyValidationException);

            this.blobStorageBrokerMock.Verify(broker =>
                broker.CreateSasTokenAsync(
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    someFutureDateTimeOffset,
                    It.IsAny<bool>(),
                    It.IsAny<string>()),
                        Times.Once);

            this.blobStorageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(DependencyExceptions))]
        public async Task
            ShouldThrowDependencyExceptionOnCreateSasTokenWithAccessPolicyAsync(Exception dependencyException)
        {
            // given
            string someContainer = GetRandomString();
            string somePath = GetRandomString();
            string somePolicyIdentifier = GetRandomString();
            DateTimeOffset someFutureDateTimeOffset = GetRandomFutureDateTimeOffset();

            var failedStorageDependencyException =
                new FailedStorageDependencyException(
                    message: "Failed storage dependency error occurred, please contact support.",
                    innerException: dependencyException);

            var expectedStorageDependencyException =
                new StorageDependencyException(
                    message: "Storage dependency error occurred, please fix errors and try again.",
                    innerException: failedStorageDependencyException);

            this.blobStorageBrokerMock.Setup(broker =>
                broker.CreateSasTokenAsync(
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<DateTimeOffset>(),
                    It.IsAny<bool>(),
                    It.IsAny<string>()))
                        .Throws(dependencyException);

            // when
            ValueTask<string> createDirectorySasTokenTask =
                this.storageService.CreateSasTokenAsync(
                    someContainer,
                    somePath,
                    somePolicyIdentifier,
                    someFutureDateTimeOffset);

            StorageDependencyException actualStorageDependencyException =
                await Assert.ThrowsAsync<StorageDependencyException>(
                    testCode: createDirectorySasTokenTask.AsTask);

            // then
            actualStorageDependencyException
                .Should().BeEquivalentTo(expectedStorageDependencyException);

            this.blobStorageBrokerMock.Verify(broker =>
                broker.CreateSasTokenAsync(
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<DateTimeOffset>(),
                    It.IsAny<bool>(),
                    It.IsAny<string>()),
                        Times.Once);

            this.blobStorageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnCreateSasTokenAsync()
        {
            // given
            Exception someException = new Exception();
            string someContainer = GetRandomString();
            string somePath = GetRandomString();
            string somePolicyIdentifier = GetRandomString();
            DateTimeOffset someFutureDateTimeOffset = GetRandomFutureDateTimeOffset();

            var failedStorageServiceException =
                new FailedStorageServiceException(
                    message: "Failed storage service error occurred, please contact support.",
                    innerException: someException);

            var expectedStorageServiceException =
                new StorageServiceException(
                    message: "Storage service error occurred, please fix errors and try again.",
                    innerException: failedStorageServiceException);

            this.blobStorageBrokerMock.Setup(broker =>
                broker.CreateSasTokenAsync(
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<DateTimeOffset>(),
                    It.IsAny<bool>(),
                    It.IsAny<string>()))
                        .Throws(someException);

            // when
            ValueTask<string> createDirectorySasTokenTask =
                this.storageService.CreateSasTokenAsync(
                    someContainer,
                    somePath,
                    somePolicyIdentifier,
                    someFutureDateTimeOffset);

            StorageServiceException actualStorageServiceException =
                await Assert.ThrowsAsync<StorageServiceException>(
                    testCode: createDirectorySasTokenTask.AsTask);

            // then
            actualStorageServiceException
                .Should().BeEquivalentTo(expectedStorageServiceException);

            this.blobStorageBrokerMock.Verify(broker =>
                broker.CreateSasTokenAsync(
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<DateTimeOffset>(),
                    It.IsAny<bool>(),
                    It.IsAny<string>()),
                        Times.Once);

            this.blobStorageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(DependencyValidationExceptions))]
        public async Task ShouldThrowDependencyValidationExceptionOnCreateSasTokenWithPermissionsListAsync(
            Exception dependencyValidationException)
        {
            // given
            var storageServiceMock = new Mock<StorageService>(
                this.blobStorageBrokerMock.Object,
                this.dateTimeBrokerMock.Object)
            { CallBase = true };

            string someContainer = GetRandomString();
            string somePath = GetRandomString();
            DateTimeOffset someFutureDateTimeOffset = GetRandomFutureDateTimeOffset();
            List<string> somePermissions = GetRandomPermissionsList();

            var failedStorageDependencyValidationException =
                new FailedStorageDependencyValidationException(
                    message: "Failed storage dependency validation error occurred, please contact support.",
                    innerException: dependencyValidationException);

            var expectedStorageDependencyValidationException =
                new StorageDependencyValidationException(
                    message: "Storage dependency validation error occurred, please fix errors and try again.",
                    innerException: failedStorageDependencyValidationException);

            storageServiceMock.Setup(service =>
                service.ConvertToPermissionsString(somePermissions))
                    .Throws(dependencyValidationException);

            StorageService storageService = storageServiceMock.Object;

            // when
            ValueTask<string> createDirectorySasTokenTask =
                storageService.CreateSasTokenAsync(
                    someContainer,
                    somePath,
                    someFutureDateTimeOffset,
                    somePermissions);

            StorageDependencyValidationException actualStorageDependencyValidationException =
                await Assert.ThrowsAsync<StorageDependencyValidationException>(
                    testCode: createDirectorySasTokenTask.AsTask);

            // then
            actualStorageDependencyValidationException
                .Should().BeEquivalentTo(expectedStorageDependencyValidationException);

            storageServiceMock.Verify(service =>
                service.ConvertToPermissionsString(somePermissions),
                    Times.Once);

            this.blobStorageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(DependencyExceptions))]
        public async Task
            ShouldThrowDependencyExceptionOnCreateSasTokenWithPermissionsListAsync(Exception dependencyException)
        {
            // given
            var storageServiceMock = new Mock<StorageService>(
                this.blobStorageBrokerMock.Object,
                this.dateTimeBrokerMock.Object)
            { CallBase = true };

            string someContainer = GetRandomString();
            string somePath = GetRandomString();
            DateTimeOffset someFutureDateTimeOffset = GetRandomFutureDateTimeOffset();
            List<string> somePermissions = GetRandomPermissionsList();

            var failedStorageDependencyException =
                new FailedStorageDependencyException(
                    message: "Failed storage dependency error occurred, please contact support.",
                    innerException: dependencyException);

            var expectedStorageDependencyException =
                new StorageDependencyException(
                    message: "Storage dependency error occurred, please fix errors and try again.",
                    innerException: failedStorageDependencyException);

            storageServiceMock.Setup(service =>
                service.ConvertToPermissionsString(somePermissions))
                    .Throws(dependencyException);

            StorageService storageService = storageServiceMock.Object;

            // when
            ValueTask<string> createDirectorySasTokenTask =
                storageService.CreateSasTokenAsync(
                    someContainer,
                    somePath,
                    someFutureDateTimeOffset,
                    somePermissions);

            StorageDependencyException actualStorageDependencyException =
                await Assert.ThrowsAsync<StorageDependencyException>(
                    testCode: createDirectorySasTokenTask.AsTask);

            // then
            actualStorageDependencyException
                .Should().BeEquivalentTo(expectedStorageDependencyException);

            storageServiceMock.Verify(service =>
                service.ConvertToPermissionsString(somePermissions),
                    Times.Once);

            this.blobStorageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnCreateSasTokenWithPermissionsListAsync()
        {
            // given
            var storageServiceMock = new Mock<StorageService>(
                this.blobStorageBrokerMock.Object,
                this.dateTimeBrokerMock.Object)
            { CallBase = true };

            Exception someException = new Exception();
            string someContainer = GetRandomString();
            string somePath = GetRandomString();
            DateTimeOffset someFutureDateTimeOffset = GetRandomFutureDateTimeOffset();
            List<string> somePermissions = GetRandomPermissionsList();

            var failedStorageServiceException =
                new FailedStorageServiceException(
                    message: "Failed storage service error occurred, please contact support.",
                    innerException: someException);

            var expectedStorageServiceException =
                new StorageServiceException(
                    message: "Storage service error occurred, please fix errors and try again.",
                    innerException: failedStorageServiceException);

            storageServiceMock.Setup(service =>
                service.ConvertToPermissionsString(somePermissions))
                    .Throws(someException);

            StorageService storageService = storageServiceMock.Object;

            // when
            ValueTask<string> createDirectorySasTokenTask =
                storageService.CreateSasTokenAsync(
                    someContainer,
                    somePath,
                    someFutureDateTimeOffset,
                    somePermissions);

            StorageServiceException actualStorageServiceException =
                await Assert.ThrowsAsync<StorageServiceException>(
                    testCode: createDirectorySasTokenTask.AsTask);

            // then
            actualStorageServiceException
                .Should().BeEquivalentTo(expectedStorageServiceException);

            storageServiceMock.Verify(service =>
                service.ConvertToPermissionsString(somePermissions),
                    Times.Once);

            this.blobStorageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
