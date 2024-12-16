// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using FluentAssertions;
using ISL.Providers.Storages.AzureBlobStorage.Models.Foundations.Files.Exceptions;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ISL.Providers.Storages.AzureBlobStorage.Tests.Unit.Services.Foundations.Storages
{
    public partial class StorageTests
    {
        [Theory]
        [MemberData(nameof(GetInvalidSasArguments))]
        public async Task ShouldThrowValidationExceptionOnCreateSasTokenWithAccessPolicyIfArgumentInvalidAsync(
            string invalidText, DateTimeOffset invalidDateTimeOffset)
        {
            // given
            string invalidContainer = invalidText;
            string invalidPath = invalidText;
            string invalidAccessPolicyIdentifier = invalidText;

            var invalidArgumentStorageException =
                new InvalidArgumentStorageException(
                    message: "Invalid storage service argument(s), please fix the errors and try again.");

            invalidArgumentStorageException.AddData(
                key: "Path",
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
                    invalidPath,
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

        [Theory]
        [MemberData(nameof(GetInvalidSasArguments))]
        public async Task ShouldThrowValidationExceptionOnCreateSasTokenWithPermissionsListIfArgumentInvalidAsync(
            string invalidText, DateTimeOffset invalidDateTimeOffset)
        {
            // given
            string invalidContainer = invalidText;
            string invalidPath = invalidText;
            string invalidAccessPolicyIdentifier = invalidText;
            List<string> somePermissionsList = GetRandomPermissionsList();

            var invalidArgumentStorageException =
                new InvalidArgumentStorageException(
                    message: "Invalid storage service argument(s), please fix the errors and try again.");

            invalidArgumentStorageException.AddData(
                key: "Path",
                values: "Text is invalid");

            invalidArgumentStorageException.AddData(
                key: "Container",
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
                    invalidPath,
                    invalidDateTimeOffset,
                    somePermissionsList);

            StorageValidationException actualStorageValidationException =
                await Assert.ThrowsAsync<StorageValidationException>(createSasTokenTask.AsTask);

            // then
            actualStorageValidationException
                .Should().BeEquivalentTo(expectedStorageValidationException);

            this.blobStorageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(NullAndEmptyStringList))]
        public async Task
            ShouldThrowValidationExceptionOnCreateSasTokenWithPermissionsListIfInputListArgumentInvalidAsync(
                List<string> invalidList)
        {
            // given
            string someContainer = GetRandomString();
            string somePath = GetRandomString();
            DateTimeOffset someFutureDateTimeOffset = GetRandomFutureDateTimeOffset();
            List<string> invalidPermissionsList = invalidList;

            var invalidArgumentStorageException =
                new InvalidArgumentStorageException(
                    message: "Invalid storage service argument(s), please fix the errors and try again.");

            invalidArgumentStorageException.AddData(
                key: "Permissions",
                values: "List is invalid");

            var expectedStorageValidationException =
                new StorageValidationException(
                    message: "Storage validation error occurred, please fix errors and try again.",
                    innerException: invalidArgumentStorageException);

            // when
            ValueTask<string> createSasTokenTask =
                this.storageService.CreateSasTokenAsync(
                    someContainer,
                    somePath,
                    someFutureDateTimeOffset,
                    invalidPermissionsList);

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
