﻿using FluentAssertions;
using ISL.Providers.Storages.Abstractions.Models;
using ISL.Providers.Storages.AzureBlobStorage.Models.Foundations.Files.Exceptions;
using ISL.Providers.Storages.AzureBlobStorage.Models.Providers.Exceptions;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xeptions;

namespace ISL.Providers.Storages.AzureBlobStorage.Tests.Unit.Providers.AzureBlobStorage
{
    public partial class AzureBlobStorageTests
    {
        [Fact]
        public async Task ShouldThrowProviderValidationExceptionOnCreateAndAssignAccessPoliciesToContainer()
        {
            // given
            string randomContainer = GetRandomString();
            string inputContainer = randomContainer;
            List<Policy> inputPolicies = GetPolicies();

            var storageValidationException = new StorageValidationException(
                message: "Storage validation error occurred, please fix errors and try again.",
                innerException: new Xeption());

            var expectedAzureBlobStorageProviderValidationException =
                new AzureBlobStorageProviderValidationException(
                    message: "Azure blob storage provider validation error occurred, fix errors and try again.",
                    innerException: (Xeption)storageValidationException.InnerException,
                    data: storageValidationException.InnerException.Data);

            this.storageServiceMock.Setup(service =>
                service.CreateAndAssignAccessPoliciesAsync(inputContainer, inputPolicies))
                    .ThrowsAsync(storageValidationException);

            // when
            ValueTask createAndAssignAccessPoliciesTask =
                this.azureBlobStorageProvider.CreateAndAssignAccessPoliciesAsync(
                    inputContainer, inputPolicies);

            AzureBlobStorageProviderValidationException actualAzureBlobStorageProviderValidationException =
                await Assert.ThrowsAsync<AzureBlobStorageProviderValidationException>(
                    testCode: createAndAssignAccessPoliciesTask.AsTask);

            // then
            actualAzureBlobStorageProviderValidationException
                .Should().BeEquivalentTo(expectedAzureBlobStorageProviderValidationException);

            this.storageServiceMock.Verify(service =>
                service.CreateAndAssignAccessPoliciesAsync(inputContainer, inputPolicies),
                    Times.Once);

            this.storageServiceMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task
            ShouldThrowProviderValidationExceptionOnCreateAndAssignAccessPoliciesToContainerDependencyValidation()
        {
            // given
            string randomContainer = GetRandomString();
            string inputContainer = randomContainer;
            List<Policy> inputPolicies = GetPolicies();

            var storageDependencyValidationException = new StorageDependencyValidationException(
                message: "Storage dependency validation error occurred, please fix errors and try again.",
                innerException: new Xeption());

            var expectedAzureBlobStorageProviderValidationException =
                new AzureBlobStorageProviderValidationException(
                    message: "Azure blob storage provider validation error occurred, fix errors and try again.",
                    innerException: (Xeption)storageDependencyValidationException.InnerException,
                    data: storageDependencyValidationException.InnerException.Data);

            this.storageServiceMock.Setup(service =>
                service.CreateAndAssignAccessPoliciesAsync(inputContainer, inputPolicies))
                    .ThrowsAsync(storageDependencyValidationException);

            // when
            ValueTask createAndAssignAccessPoliciesTask =
                this.azureBlobStorageProvider.CreateAndAssignAccessPoliciesAsync(
                    inputContainer, inputPolicies);

            AzureBlobStorageProviderValidationException
                actualAzureBlobStorageProviderValidationException =
                    await Assert.ThrowsAsync<AzureBlobStorageProviderValidationException>(
                        testCode: createAndAssignAccessPoliciesTask.AsTask);

            // then
            actualAzureBlobStorageProviderValidationException
                .Should().BeEquivalentTo(expectedAzureBlobStorageProviderValidationException);

            this.storageServiceMock.Verify(service =>
                service.CreateAndAssignAccessPoliciesAsync(inputContainer, inputPolicies),
                    Times.Once);

            this.storageServiceMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowProviderDependencyExceptionOnCreateAndAssignAccessPoliciesToContainer()
        {
            // given
            string randomContainer = GetRandomString();
            string inputContainer = randomContainer;
            List<Policy> inputPolicies = GetPolicies();

            var storageDependencyException = new StorageDependencyException(
                message: "Storage dependency error occurred, please fix errors and try again.",
                innerException: new Xeption());

            var expectedAzureBlobStorageProviderDependencyException =
                new AzureBlobStorageProviderDependencyException(
                    message: "Azure blob storage provider dependency error occurred, " +
                        "contact support.",
                    innerException: (Xeption)storageDependencyException.InnerException);

            this.storageServiceMock.Setup(service =>
                service.CreateAndAssignAccessPoliciesAsync(inputContainer, inputPolicies))
                    .ThrowsAsync(storageDependencyException);

            // when
            ValueTask createAndAssignAccessPoliciesTask =
                this.azureBlobStorageProvider.CreateAndAssignAccessPoliciesAsync(
                    inputContainer, inputPolicies);

            AzureBlobStorageProviderDependencyException
                actualAzureBlobStorageProviderDependencyException =
                    await Assert.ThrowsAsync<AzureBlobStorageProviderDependencyException>(
                        testCode: createAndAssignAccessPoliciesTask.AsTask);

            // then
            actualAzureBlobStorageProviderDependencyException
                .Should().BeEquivalentTo(expectedAzureBlobStorageProviderDependencyException);

            this.storageServiceMock.Verify(service =>
                service.CreateAndAssignAccessPoliciesAsync(inputContainer, inputPolicies),
                    Times.Once);

            this.storageServiceMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowProviderServiceExceptionOnCreateAndAssignAccessPoliciesToContainer()
        {
            // given
            string randomContainer = GetRandomString();
            string inputContainer = randomContainer;
            List<Policy> inputPolicies = GetPolicies();

            var storageServiceException = new StorageServiceException(
                message: "Storage service error occurred, please fix errors and try again.",
                innerException: new Xeption());

            var expectedAzureBlobStorageProviderServiceException =
                new AzureBlobStorageProviderServiceException(
                    message: "Azure blob storage provider service error occurred, " +
                        "contact support.",
                    innerException: (Xeption)storageServiceException.InnerException);

            this.storageServiceMock.Setup(service =>
                service.CreateAndAssignAccessPoliciesAsync(inputContainer, inputPolicies))
                    .ThrowsAsync(storageServiceException);

            // when
            ValueTask createAndAssignAccessPoliciesTask =
                this.azureBlobStorageProvider.CreateAndAssignAccessPoliciesAsync(
                    inputContainer, inputPolicies);

            AzureBlobStorageProviderServiceException
                actualAzureBlobStorageProviderServiceException =
                    await Assert.ThrowsAsync<AzureBlobStorageProviderServiceException>(
                        testCode: createAndAssignAccessPoliciesTask.AsTask);

            // then
            actualAzureBlobStorageProviderServiceException
                .Should().BeEquivalentTo(expectedAzureBlobStorageProviderServiceException);

            this.storageServiceMock.Verify(service =>
                service.CreateAndAssignAccessPoliciesAsync(inputContainer, inputPolicies),
                    Times.Once);

            this.storageServiceMock.VerifyNoOtherCalls();
        }
    }
}

