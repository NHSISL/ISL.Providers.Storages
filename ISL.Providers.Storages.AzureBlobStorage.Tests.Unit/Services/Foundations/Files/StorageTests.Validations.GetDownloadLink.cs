// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using FluentAssertions;
using ISL.Providers.Storages.AzureBlobStorage.Models.Foundations.Files.Exceptions;
using System;
using System.Threading.Tasks;

namespace ISL.Providers.Storages.AzureBlobStorage.Tests.Unit.Services.Foundations.Files
{
    public partial class StorageTests
    {
        [Theory]
        [MemberData(nameof(GetInvalidSasArguments))]
        public async Task ShouldThrowValidationExceptionOnGetDownloadLinkIfArgumentInvalidAsync(
            string invalidText, DateTimeOffset invalidDateTimeOffset)
        {
            // given
            string invalidFileName = invalidText;
            string invalidContainer = invalidText;
            DateTimeOffset invalidExpiresOn = invalidDateTimeOffset;

            var invalidArgumentStorageException =
                new InvalidArgumentStorageException(
                    message: "Invalid storage service argument(s), please fix the errors and try again.");

            invalidArgumentStorageException.AddData(
                key: "ExpiresOn",
                values: "Date is invalid");

            invalidArgumentStorageException.AddData(
                key: "FileName",
                values: "Text is invalid");

            invalidArgumentStorageException.AddData(
                key: "Container",
                values: "Text is invalid");

            var expectedStorageValidationException =
                new StorageValidationException(
                    message: "Storage validation error occurred, please fix errors and try again.",
                    innerException: invalidArgumentStorageException);

            // when
            ValueTask<string> getDownloadLinkTask =
                this.storageService.GetDownloadLinkAsync(invalidFileName, invalidContainer, invalidExpiresOn);

            StorageValidationException actualStorageValidationException =
                await Assert.ThrowsAsync<StorageValidationException>(getDownloadLinkTask.AsTask);

            // then
            actualStorageValidationException
                .Should().BeEquivalentTo(expectedStorageValidationException);

            this.blobStorageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
