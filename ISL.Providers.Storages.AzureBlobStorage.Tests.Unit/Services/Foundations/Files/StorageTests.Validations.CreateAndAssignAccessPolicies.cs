// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using FluentAssertions;
using ISL.Providers.Storages.AzureBlobStorage.Models.Foundations.Files.Exceptions;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ISL.Providers.Storages.AzureBlobStorage.Tests.Unit.Services.Foundations.Files
{
    public partial class StorageTests
    {
        [Theory]
        [MemberData(nameof(InvalidPolicyArguments))]
        public async Task
            ShouldThrowValidationExceptionOnCreateAccessPolicyIfArgumentsInvalidAsync(
            string invalidText, List<string> invalidList)
        {
            // given
            string invalidContainer = invalidText;
            List<string> invalidPolicyNames = invalidList;

            var invalidArgumentStorageException =
                new InvalidArgumentStorageException(
                    message: "Invalid storage service argument(s), please fix the errors and try again.");

            invalidArgumentStorageException.AddData(
                key: "Container",
                values: "Text is invalid");

            invalidArgumentStorageException.AddData(
                key: "PolicyNames",
                values: "List is invalid");

            var expectedStorageValidationException =
                new StorageValidationException(
                    message: "Storage validation error occurred, please fix errors and try again.",
                    innerException: invalidArgumentStorageException);

            // when
            ValueTask createAccessPolicyTask =
                this.storageService.CreateAndAssignAccessPoliciesToContainerAsync(invalidContainer, invalidPolicyNames);

            StorageValidationException actualStorageValidationException =
                await Assert.ThrowsAsync<StorageValidationException>(testCode: createAccessPolicyTask.AsTask);

            // then
            actualStorageValidationException
                .Should().BeEquivalentTo(expectedStorageValidationException);

            this.blobServiceClientMock.VerifyNoOtherCalls();
            this.blobContainerClientMock.VerifyNoOtherCalls();
            this.blobClientMock.VerifyNoOtherCalls();
            this.blobStorageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnCreateAccessPolicyIfPolicyNamesInvalidAsync()
        {
            // given
            string randomString = GetRandomString();
            string inputContainer = randomString;
            List<string> randomPolicyNames = GetRandomStringList();
            List<string> invalidPolicyNames = randomPolicyNames;

            var invalidPolicyNameStorageException =
                new InvalidPolicyNameStorageException(
                    message: "Invalid policy name, only read, write, delete and fullaccess privileges " +
                        "are supported at this time.");

            var expectedStorageValidationException =
                new StorageValidationException(
                    message: "Storage validation error occurred, please fix errors and try again.",
                    innerException: invalidPolicyNameStorageException);

            // when
            ValueTask createAccessPolicyTask =
                this.storageService.CreateAndAssignAccessPoliciesToContainerAsync(inputContainer, invalidPolicyNames);

            StorageValidationException actualStorageValidationException =
                await Assert.ThrowsAsync<StorageValidationException>(testCode: createAccessPolicyTask.AsTask);

            // then
            actualStorageValidationException
                .Should().BeEquivalentTo(expectedStorageValidationException);

            this.blobServiceClientMock.VerifyNoOtherCalls();
            this.blobContainerClientMock.VerifyNoOtherCalls();
            this.blobClientMock.VerifyNoOtherCalls();
            this.blobStorageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
