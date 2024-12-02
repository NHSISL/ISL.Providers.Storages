// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using FluentAssertions;
using ISL.Providers.Storages.AzureBlobStorage.Models.Foundations.Files.Exceptions;
using System.Threading.Tasks;

namespace ISL.Providers.Storages.AzureBlobStorage.Tests.Unit.Services.Foundations.Storages
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

            this.blobStorageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
