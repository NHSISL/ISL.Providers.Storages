using FluentAssertions;
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
        public async Task ShouldThrowProviderValidationExceptionOnRetrieveAllAccessPoliciesAsync()
        {
            // given
            string randomContainer = GetRandomString();
            string inputContainer = randomContainer;

            var storageValidationException = new StorageValidationException(
                message: "Storage validation error occurred, please fix errors and try again.",
                innerException: new Xeption());

            var expectedAzureBlobStorageProviderValidationException =
                new AzureBlobStorageProviderValidationException(
                    message: "Azure blob storage provider validation error occurred, fix errors and try again.",
                    innerException: (Xeption)storageValidationException.InnerException,
                    data: storageValidationException.InnerException.Data);

            this.storageServiceMock.Setup(service =>
                service.RetrieveAllAccessPoliciesAsync(inputContainer))
                    .ThrowsAsync(storageValidationException);

            // when
            ValueTask<List<Policy>> retrievePoliciesTask =
                this.azureBlobStorageProvider.RetrieveAllAccessPoliciesAsync(inputContainer);

            AzureBlobStorageProviderValidationException actualAzureBlobStorageProviderValidationException =
                await Assert.ThrowsAsync<AzureBlobStorageProviderValidationException>(
                    testCode: retrievePoliciesTask.AsTask);

            // then
            actualAzureBlobStorageProviderValidationException
                .Should().BeEquivalentTo(expectedAzureBlobStorageProviderValidationException);

            this.storageServiceMock.Verify(service =>
                service.RetrieveAllAccessPoliciesAsync(inputContainer),
                    Times.Once);

            this.storageServiceMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task
            ShouldThrowProviderValidationExceptionOnRRetrieveAllAccessPoliciesDependencyValidationAsync()
        {
            // given
            string randomContainer = GetRandomString();
            string inputContainer = randomContainer;

            var storageDependencyValidationException = new StorageDependencyValidationException(
                message: "Storage dependency validation error occurred, please fix errors and try again.",
                innerException: new Xeption());

            var expectedAzureBlobStorageProviderDependencyValidationException =
                new AzureBlobStorageProviderValidationException(
                    message: "Azure blob storage provider validation error occurred, fix errors and try again.",
                    innerException: (Xeption)storageDependencyValidationException.InnerException,
                    data: storageDependencyValidationException.InnerException.Data);

            this.storageServiceMock.Setup(service =>
                service.RetrieveAllAccessPoliciesAsync(inputContainer))
                    .ThrowsAsync(storageDependencyValidationException);

            // when
            ValueTask<List<Policy>> retrievePoliciesTask =
                this.azureBlobStorageProvider.RetrieveAllAccessPoliciesAsync(inputContainer);

            AzureBlobStorageProviderValidationException
                actualAzureBlobStorageProviderDependencyValidationException =
                    await Assert.ThrowsAsync<AzureBlobStorageProviderValidationException>(
                        testCode: retrievePoliciesTask.AsTask);

            // then
            actualAzureBlobStorageProviderDependencyValidationException
                .Should().BeEquivalentTo(expectedAzureBlobStorageProviderDependencyValidationException);

            this.storageServiceMock.Verify(service =>
                service.RetrieveAllAccessPoliciesAsync(inputContainer),
                    Times.Once);

            this.storageServiceMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowProviderDependencyExceptionOnRetrieveAllAccessPoliciesAsync()
        {
            // given
            string randomContainer = GetRandomString();
            string inputContainer = randomContainer;

            var storageDependencyException = new StorageDependencyException(
                message: "Storage dependency error occurred, please fix errors and try again.",
                innerException: new Xeption());

            var expectedAzureBlobStorageProviderDependencyException =
                new AzureBlobStorageProviderDependencyException(
                    message: "Azure blob storage provider dependency error occurred, " +
                            "contact support.",
                    innerException: (Xeption)storageDependencyException.InnerException);

            this.storageServiceMock.Setup(service =>
                service.RetrieveAllAccessPoliciesAsync(inputContainer))
                    .ThrowsAsync(storageDependencyException);

            // when
            ValueTask<List<Policy>> retrievePoliciesTask =
                this.azureBlobStorageProvider.RetrieveAllAccessPoliciesAsync(inputContainer);

            AzureBlobStorageProviderDependencyException
                actualAzureBlobStorageProviderDependencyException =
                    await Assert.ThrowsAsync<AzureBlobStorageProviderDependencyException>(
                        testCode: retrievePoliciesTask.AsTask);

            // then
            actualAzureBlobStorageProviderDependencyException
                .Should().BeEquivalentTo(expectedAzureBlobStorageProviderDependencyException);

            this.storageServiceMock.Verify(service =>
                service.RetrieveAllAccessPoliciesAsync(inputContainer),
                    Times.Once);

            this.storageServiceMock.VerifyNoOtherCalls();
        }
    }
}
