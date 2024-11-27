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
        public async Task ShouldThrowProviderValidationExceptionOnCreateDirectorySasToken()
        {
            // given
            string randomContainer = GetRandomString();
            string randomDirectoryPath = GetRandomString();
            string randomAccessPolicyIdentifier = GetRandomString();
            DateTimeOffset randomDateTimeOffset = GetRandomFutureDateTimeOffset();
            string randomSasToken = GetRandomString();
            string inputDirectoryPath = randomDirectoryPath;
            string inputContainer = randomContainer;
            string inputAccessPolicyIdentifier = randomAccessPolicyIdentifier;
            DateTimeOffset inputDateTimeOffset = randomDateTimeOffset;
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
                    inputContainer, inputDirectoryPath, inputAccessPolicyIdentifier, inputDateTimeOffset))
                        .ThrowsAsync(someStorageValidationException);

            // when
            ValueTask<string> createDirectorySasTokenTask =
                this.storageAbstractionProvider.CreateDirectorySasTokenAsync(
                    inputContainer, inputDirectoryPath, inputAccessPolicyIdentifier, inputDateTimeOffset);

            StorageProviderValidationException actualAzureBlobStorageProviderValidationException =
                await Assert.ThrowsAsync<StorageProviderValidationException>(
                    testCode: createDirectorySasTokenTask.AsTask);

            // then
            actualAzureBlobStorageProviderValidationException
                .Should().BeEquivalentTo(expectedStorageValidationProviderException);

            this.storageProviderMock.Verify(service =>
                service.CreateDirectorySasTokenAsync(
                    inputContainer, inputDirectoryPath, inputAccessPolicyIdentifier, inputDateTimeOffset),
                        Times.Once);

            this.storageProviderMock.VerifyNoOtherCalls();
        }
    }
}
