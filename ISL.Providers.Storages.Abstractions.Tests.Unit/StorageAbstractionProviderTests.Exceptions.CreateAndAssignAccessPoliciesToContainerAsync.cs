﻿using FluentAssertions;
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
                provider.CreateAndAssignAccessPoliciesToContainerAsync(It.IsAny<List<string>>(), It.IsAny<string>()))
                    .ThrowsAsync(someStorageValidationException);

            // when
            ValueTask createAndAssignAccessPoliciesToContainerTask =
                this.storageAbstractionProvider
                    .CreateAndAssignAccessPoliciesToContainerAsync(It.IsAny<List<string>>(), It.IsAny<string>());

            StorageProviderValidationException actualStorageValidationProviderException =
                await Assert.ThrowsAsync<StorageProviderValidationException>(
                    testCode: createAndAssignAccessPoliciesToContainerTask.AsTask);

            // then
            actualStorageValidationProviderException.Should().BeEquivalentTo(
                expectedStorageValidationProviderException);

            this.storageProviderMock.Verify(provider =>
                provider.CreateAndAssignAccessPoliciesToContainerAsync(It.IsAny<List<string>>(), It.IsAny<string>()),
                    Times.Once);

            this.storageProviderMock.VerifyNoOtherCalls();
        }
    }
}