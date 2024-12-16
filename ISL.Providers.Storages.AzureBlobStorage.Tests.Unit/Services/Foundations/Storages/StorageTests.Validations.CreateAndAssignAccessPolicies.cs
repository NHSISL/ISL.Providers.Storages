// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using FluentAssertions;
using ISL.Providers.Storages.Abstractions.Models;
using ISL.Providers.Storages.AzureBlobStorage.Models.Foundations.Files.Exceptions;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ISL.Providers.Storages.AzureBlobStorage.Tests.Unit.Services.Foundations.Storages
{
    public partial class StorageTests
    {
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public async Task
            ShouldThrowValidationExceptionOnCreateAccessPolicyIfArgumentsInvalidAsync(
            string invalidText)
        {
            // given
            string invalidContainer = invalidText;
            string invalidPolicyName = invalidText;
            List<Policy> inputPolicies = GetPolicies();

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
            ValueTask createAccessPolicyTask =
                this.storageService.CreateAndAssignAccessPoliciesAsync(
                    invalidContainer, inputPolicies);

            StorageValidationException actualStorageValidationException =
                await Assert.ThrowsAsync<StorageValidationException>(testCode: createAccessPolicyTask.AsTask);

            // then
            actualStorageValidationException
                .Should().BeEquivalentTo(expectedStorageValidationException);

            this.blobStorageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public async Task
            ShouldThrowValidationExceptionOnCreateAccessPolicyIfPolicyObjectArgumentsInvalidAsync(
            string invalidText)
        {
            // given
            string randomString = GetRandomString();
            string inputContainer = randomString;

            List<Policy> invalidPolicies = new List<Policy>
            {
                new Policy
                {
                    PolicyName = invalidText,
                    Permissions = new List<string>{ invalidText }
                }
            };

            List<Policy> inputPolicies = GetPolicies();

            var invalidArgumentStorageException =
                new InvalidArgumentStorageException(
                    message: "Invalid storage service argument(s), please fix the errors and try again.");

            invalidArgumentStorageException.AddData(
                key: $"{nameof(Policy)}.{nameof(Policy.PolicyName)}",
                values: "Text is invalid");

            invalidArgumentStorageException.AddData(
                key: $"{nameof(Policy)}.{nameof(Policy.Permissions)}",
                values: "List is invalid");

            var expectedStorageValidationException =
                new StorageValidationException(
                    message: "Storage validation error occurred, please fix errors and try again.",
                    innerException: invalidArgumentStorageException);

            // when
            ValueTask createAccessPolicyTask =
                this.storageService.CreateAndAssignAccessPoliciesAsync(
                    inputContainer, invalidPolicies);

            StorageValidationException actualStorageValidationException =
                await Assert.ThrowsAsync<StorageValidationException>(testCode: createAccessPolicyTask.AsTask);

            // then
            actualStorageValidationException
                .Should().BeEquivalentTo(expectedStorageValidationException);

            this.blobStorageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(NullAndEmptyPolicyList))]
        public async Task
            ShouldThrowValidationExceptionOnCreateAccessPolicyIfListNullOrEmptyInvalidAsync(
            List<Policy> invalidList)
        {
            // given
            string randomString = GetRandomString();
            string inputContainer = randomString;
            List<Policy> invalidPolicies = invalidList;

            var invalidArgumentStorageException =
                new InvalidArgumentStorageException(
                    message: "Invalid storage service argument(s), please fix the errors and try again.");

            invalidArgumentStorageException.AddData(
                key: "Policies",
                values: "List is invalid");

            var expectedStorageValidationException =
                new StorageValidationException(
                    message: "Storage validation error occurred, please fix errors and try again.",
                    innerException: invalidArgumentStorageException);

            // when
            ValueTask createAccessPolicyTask =
                this.storageService.CreateAndAssignAccessPoliciesAsync(
                    inputContainer, invalidPolicies);

            StorageValidationException actualStorageValidationException =
                await Assert.ThrowsAsync<StorageValidationException>(testCode: createAccessPolicyTask.AsTask);

            // then
            actualStorageValidationException
                .Should().BeEquivalentTo(expectedStorageValidationException);

            this.blobStorageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnCreateAccessPolicyIfPolicyNamesInvalidAsync()
        {
            // given
            string randomContainer = GetRandomString();
            string randomPolicyName = GetRandomString();
            string inputContainer = randomContainer;
            string inputPolicyName = randomPolicyName;
            List<string> randomPermissions = GetRandomStringList();
            List<string> invalidPermissions = randomPermissions;

            List<Policy> inputPolicies = new List<Policy>
            {
                new Policy
                {
                    PolicyName = inputPolicyName,
                    Permissions = invalidPermissions
                }
            };

            var InvalidPolicyPermissionStorageException =
                new InvalidPolicyPermissionStorageException(
                    message: "Invalid permission. Read, write, delete, create, add and list" +
                        "permissions are supported at this time.");

            var expectedStorageValidationException =
                new StorageValidationException(
                    message: "Storage validation error occurred, please fix errors and try again.",
                    innerException: InvalidPolicyPermissionStorageException);

            // when
            ValueTask createAccessPolicyTask =
                this.storageService.CreateAndAssignAccessPoliciesAsync(
                    inputContainer, inputPolicies);

            StorageValidationException actualStorageValidationException =
                await Assert.ThrowsAsync<StorageValidationException>(testCode: createAccessPolicyTask.AsTask);

            // then
            actualStorageValidationException
                .Should().BeEquivalentTo(expectedStorageValidationException);

            this.blobStorageBrokerMock.Verify(broker =>
                broker.GetBlobContainerClient(inputContainer),
                    Times.Once);

            this.blobStorageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
