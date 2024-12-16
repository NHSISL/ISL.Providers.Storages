// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using FluentAssertions;
using ISL.Providers.Storages.AzureBlobStorage.Models.Foundations.Files.Exceptions;
using System;
using System.Threading.Tasks;

namespace ISL.Providers.Storages.AzureBlobStorage.Tests.Unit.Services.Foundations.Storages
{
    public partial class StorageTests
    {
        [Theory]
        [MemberData(nameof(GetInvalidSasArguments))]
        public async Task ShouldThrowValidationExceptionOnCreateDirectorySasTokenIfArgumentInvalidAsync(
            string invalidText, DateTimeOffset invalidDateTimeOffset)
        {
            // given
            string invalidContainer = invalidText;
            string invalidDirectoryPath = invalidText;
            string invalidAccessPolicyIdentifier = invalidText;

            var invalidArgumentStorageException =
                new InvalidArgumentStorageException(
                    message: "Invalid storage service argument(s), please fix the errors and try again.");

            invalidArgumentStorageException.AddData(
                key: "DirectoryPath",
                values: "Text is invalid");

            invalidArgumentStorageException.AddData(
                key: "Container",
                values: "Text is invalid");

            invalidArgumentStorageException.AddData(
                key: "AccessPolicyIdentifier",
                values: "Text is invalid");

            invalidArgumentStorageException.AddData(
                key: "ExpiresOn",
                values: "Date is invalid");

            var expectedStorageValidationException =
                new StorageValidationException(
                    message: "Storage validation error occurred, please fix errors and try again.",
                    innerException: invalidArgumentStorageException);

            // when
            ValueTask<string> createSasTokenTask =
                this.storageService.CreateSasTokenAsync(
                    invalidContainer,
                    invalidDirectoryPath,
                    invalidAccessPolicyIdentifier,
                    invalidDateTimeOffset);

            StorageValidationException actualStorageValidationException =
                await Assert.ThrowsAsync<StorageValidationException>(createSasTokenTask.AsTask);

            // then
            actualStorageValidationException
                .Should().BeEquivalentTo(expectedStorageValidationException);

            this.blobStorageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
