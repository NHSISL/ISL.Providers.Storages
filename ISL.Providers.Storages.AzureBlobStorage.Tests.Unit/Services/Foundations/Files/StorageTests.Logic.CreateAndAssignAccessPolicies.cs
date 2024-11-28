using Azure.Storage.Blobs.Models;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ISL.Providers.Storages.AzureBlobStorage.Tests.Unit.Services.Foundations.Files
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
            DateTimeOffset inputDateTimeOffset = randomDateTimeOffset;
            List<string> inputPolicyNames = GetPolicyNames();
            List<BlobSignedIdentifier> outputSignedIdentifiers = SetupSignedIdentifiers(inputDateTimeOffset);


            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(randomDateTimeOffset);

            this.blobStorageBrokerMock.Setup(broker =>
                broker.GetBlobContainerClient(inputContainer))
                    .Returns(blobContainerClientMock.Object);

            this.blobStorageBrokerMock.Setup(broker =>
                broker.CreateAccessPoliciesAsync(inputPolicyNames, inputDateTimeOffset))
                    .ReturnsAsync(outputSignedIdentifiers);

            // when
            await this.storageService
                .CreateAndAssignAccessPoliciesToContainerAsync(inputContainer, inputPolicyNames);

            // then
            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.blobStorageBrokerMock.Verify(broker =>
                broker.GetBlobContainerClient(inputContainer),
                    Times.Once);

            this.blobStorageBrokerMock.Verify(broker =>
                broker.CreateAccessPoliciesAsync(inputPolicyNames, inputDateTimeOffset),
                    Times.Once());

            this.blobStorageBrokerMock.Verify(broker =>
                broker.AssignAccessPoliciesToContainerAsync(
                    blobContainerClientMock.Object, outputSignedIdentifiers),
                        Times.Once());

            this.blobStorageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
