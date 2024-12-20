﻿using Azure.Storage.Blobs.Models;
using ISL.Providers.Storages.Abstractions.Models;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ISL.Providers.Storages.AzureBlobStorage.Tests.Unit.Services.Foundations.Storages
{
    public partial class StorageTests
    {
        [Fact]
        public async Task ShouldCreateAndAssignAccessPoliciesAsync()
        {
            // given
            string randomString = GetRandomString();
            string inputContainer = randomString;
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            DateTimeOffset inputDateTimeOffset = randomDateTimeOffset.AddMinutes(-1);
            List<Policy> inputPolicies = GetPolicies();
            List<BlobSignedIdentifier> inputSignedIdentifiers = SetupSignedIdentifiers(inputDateTimeOffset);

            this.blobStorageBrokerMock.Setup(broker =>
                broker.GetBlobContainerClient(inputContainer))
                    .Returns(blobContainerClientMock.Object);

            // when
            await this.storageService
                .CreateAndAssignAccessPoliciesAsync(inputContainer, inputPolicies);

            // then
            this.blobStorageBrokerMock.Verify(broker =>
                broker.GetBlobContainerClient(inputContainer),
                    Times.Once);

            this.blobStorageBrokerMock.Verify(broker =>
                broker.AssignAccessPoliciesToContainerAsync(
                    blobContainerClientMock.Object,
                    It.Is(SameBlobSignedIdentifierListAs(inputSignedIdentifiers))),
                        Times.Once());

            this.blobStorageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
