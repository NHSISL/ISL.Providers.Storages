using FluentAssertions;
using ISL.Providers.Storages.AzureBlobStorage.Models.Foundations.Files.Exceptions;
using ISL.Providers.Storages.AzureBlobStorage.Models.Providers.Exceptions;
using Moq;
using System.Threading.Tasks;
using Xeptions;

namespace ISL.Providers.Storages.AzureBlobStorage.Tests.Unit.Providers.AzureBlobStorage
{
    public partial class AzureBlobStorageTests
    {
        [Fact]
        public async Task ShouldThrowProviderValidationExceptionOnRemoveAccessPolicyByNameAsync()
        {
            // given
            string randomContainer = GetRandomString();
            string randomPolicyName = GetRandomString();
            string inputContainer = randomContainer;
            string inputPolicyName = randomPolicyName;

            var storageValidationException = new StorageValidationException(
                message: "Storage validation error occurred, please fix errors and try again.",
                innerException: new Xeption());

            var expectedAzureBlobStorageProviderValidationException =
                new AzureBlobStorageProviderValidationException(
                    message: "Azure blob storage provider validation error occurred, fix errors and try again.",
                    innerException: (Xeption)storageValidationException.InnerException,
                    data: storageValidationException.InnerException.Data);

            this.storageServiceMock.Setup(service =>
                service.RemoveAccessPolicyByNameAsync(inputContainer, inputPolicyName))
                    .ThrowsAsync(storageValidationException);

            // when
            ValueTask retrievePolicyTask =
                this.azureBlobStorageProvider.RemoveAccessPolicyByNameAsync(inputContainer, inputPolicyName);

            AzureBlobStorageProviderValidationException actualAzureBlobStorageProviderValidationException =
                await Assert.ThrowsAsync<AzureBlobStorageProviderValidationException>(
                    testCode: retrievePolicyTask.AsTask);

            // then
            actualAzureBlobStorageProviderValidationException
                .Should().BeEquivalentTo(expectedAzureBlobStorageProviderValidationException);

            this.storageServiceMock.Verify(service =>
                service.RemoveAccessPolicyByNameAsync(inputContainer, inputPolicyName),
                    Times.Once);

            this.storageServiceMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task
            ShouldThrowProviderValidationExceptionOnRemoveAccessPolicyByNameDependencyValidationAsync()
        {
            // given
            string randomContainer = GetRandomString();
            string randomPolicyName = GetRandomString();
            string inputContainer = randomContainer;
            string inputPolicyName = randomPolicyName;

            var storageDependencyValidationException = new StorageDependencyValidationException(
                message: "Storage dependency validation error occurred, please fix errors and try again.",
                innerException: new Xeption());

            var expectedAzureBlobStorageProviderDependencyValidationException =
                new AzureBlobStorageProviderValidationException(
                    message: "Azure blob storage provider validation error occurred, fix errors and try again.",
                    innerException: (Xeption)storageDependencyValidationException.InnerException,
                    data: storageDependencyValidationException.InnerException.Data);

            this.storageServiceMock.Setup(service =>
                service.RemoveAccessPolicyByNameAsync(inputContainer, inputPolicyName))
                    .ThrowsAsync(storageDependencyValidationException);

            // when
            ValueTask retrievePolicyTask =
                this.azureBlobStorageProvider.RemoveAccessPolicyByNameAsync(inputContainer, inputPolicyName);

            AzureBlobStorageProviderValidationException
                actualAzureBlobStorageProviderDependencyValidationException =
                    await Assert.ThrowsAsync<AzureBlobStorageProviderValidationException>(
                        testCode: retrievePolicyTask.AsTask);

            // then
            actualAzureBlobStorageProviderDependencyValidationException
                .Should().BeEquivalentTo(expectedAzureBlobStorageProviderDependencyValidationException);

            this.storageServiceMock.Verify(service =>
                service.RemoveAccessPolicyByNameAsync(inputContainer, inputPolicyName),
                    Times.Once);

            this.storageServiceMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowProviderDependencyExceptionOnRemoveAccessPolicyByNameAsync()
        {
            // given
            string randomContainer = GetRandomString();
            string randomPolicyName = GetRandomString();
            string inputContainer = randomContainer;
            string inputPolicyName = randomPolicyName;

            var storageDependencyException = new StorageDependencyException(
                message: "Storage dependency error occurred, please fix errors and try again.",
                innerException: new Xeption());

            var expectedAzureBlobStorageProviderDependencyException =
                new AzureBlobStorageProviderDependencyException(
                    message: "Azure blob storage provider dependency error occurred, " +
                            "contact support.",
                    innerException: (Xeption)storageDependencyException.InnerException);

            this.storageServiceMock.Setup(service =>
                service.RemoveAccessPolicyByNameAsync(inputContainer, inputPolicyName))
                    .ThrowsAsync(storageDependencyException);

            // when
            ValueTask retrievePolicyTask =
                this.azureBlobStorageProvider.RemoveAccessPolicyByNameAsync(inputContainer, inputPolicyName);

            AzureBlobStorageProviderDependencyException
                actualAzureBlobStorageProviderDependencyException =
                    await Assert.ThrowsAsync<AzureBlobStorageProviderDependencyException>(
                        testCode: retrievePolicyTask.AsTask);

            // then
            actualAzureBlobStorageProviderDependencyException
                .Should().BeEquivalentTo(expectedAzureBlobStorageProviderDependencyException);

            this.storageServiceMock.Verify(service =>
                service.RemoveAccessPolicyByNameAsync(inputContainer, inputPolicyName),
                    Times.Once);

            this.storageServiceMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowProviderServiceExceptionOnRemoveAccessPolicyByNameAsync()
        {
            // given
            string randomContainer = GetRandomString();
            string randomPolicyName = GetRandomString();
            string inputContainer = randomContainer;
            string inputPolicyName = randomPolicyName;

            var storageServiceException = new StorageServiceException(
                message: "Storage service error occurred, please fix errors and try again.",
                innerException: new Xeption());

            var expectedAzureBlobStorageProviderServiceException =
                new AzureBlobStorageProviderServiceException(
                    message: "Azure blob storage provider service error occurred, " +
                            "contact support.",
                    innerException: (Xeption)storageServiceException.InnerException);

            this.storageServiceMock.Setup(service =>
                service.RemoveAccessPolicyByNameAsync(inputContainer, inputPolicyName))
                    .ThrowsAsync(storageServiceException);

            // when
            ValueTask retrievePolicyTask =
                this.azureBlobStorageProvider.RemoveAccessPolicyByNameAsync(inputContainer, inputPolicyName);

            AzureBlobStorageProviderServiceException
                actualAzureBlobStorageProviderServiceException =
                    await Assert.ThrowsAsync<AzureBlobStorageProviderServiceException>(
                        testCode: retrievePolicyTask.AsTask);

            // then
            actualAzureBlobStorageProviderServiceException
                .Should().BeEquivalentTo(expectedAzureBlobStorageProviderServiceException);

            this.storageServiceMock.Verify(service =>
                service.RemoveAccessPolicyByNameAsync(inputContainer, inputPolicyName),
                Times.Once);

            this.storageServiceMock.VerifyNoOtherCalls();
        }
    }
}
