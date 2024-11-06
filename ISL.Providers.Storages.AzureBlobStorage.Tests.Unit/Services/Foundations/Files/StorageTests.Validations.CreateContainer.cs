﻿// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Threading.Tasks;
using FluentAssertions;
using ISL.Providers.Storages.AzureBlobStorage.Models.Foundations.Files.Exceptions;

namespace ISL.Providers.Storages.AzureBlobStorage.Tests.Unit.Services.Foundations.Files
{
    public partial class StorageTests
    {
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public async Task ShouldThrowValidationExceptionOnCreateContainerIfArgumentsInvalidAsync(string invalidText)
        {
            // given
            string invalidContainer = invalidText;

            var invalidArgumentStorageException =
                new InvalidArgumentStorageException(
                    message: "Invalid storage service argument(s), please fix the errors and try again.");

            invalidArgumentStorageException.AddData(
                key: "Container",
                values: "Text is invalid");

            var expectedStorageValidationException =
                new StorageValidationException(
                    message: "Storage validation error occurred, please fix errors and try again.",
                    innerException: invalidArgumentStorageException);

            // when
            ValueTask createContainerTask =
                this.storageService.CreateContainerAsync(invalidContainer);

            StorageValidationException actualStorageValidationException =
                await Assert.ThrowsAsync<StorageValidationException>(testCode: createContainerTask.AsTask);

            // then
            actualStorageValidationException
                .Should().BeEquivalentTo(expectedStorageValidationException);

            this.blobServiceClientMock.VerifyNoOtherCalls();
            this.blobContainerClientMock.VerifyNoOtherCalls();
            this.blobClientMock.VerifyNoOtherCalls();
            this.blobStorageBrokerMock.VerifyNoOtherCalls();
        }
    }
}
