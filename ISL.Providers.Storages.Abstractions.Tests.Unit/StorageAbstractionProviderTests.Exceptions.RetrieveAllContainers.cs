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
        public async Task
            ShouldThrowValidationExceptionOnRetrieveAllContainersAsyncWhenTypeIsStorageValidationException()
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
                provider.RetrieveAllContainersAsync())
                    .ThrowsAsync(someStorageValidationException);

            // when
            ValueTask<List<string>> retrieveAllContainersAsyncTask =
                this.storageAbstractionProvider
                    .RetrieveAllContainersAsync();

            StorageProviderValidationException actualStorageValidationProviderException =
                await Assert.ThrowsAsync<StorageProviderValidationException>(
                    testCode: retrieveAllContainersAsyncTask.AsTask);

            // then
            actualStorageValidationProviderException.Should().BeEquivalentTo(
                expectedStorageValidationProviderException);

            this.storageProviderMock.Verify(provider =>
                provider.RetrieveAllContainersAsync(),
                    Times.Once);

            this.storageProviderMock.VerifyNoOtherCalls();
        }
    }
}
