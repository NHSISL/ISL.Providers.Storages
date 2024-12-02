using Azure.Storage.Blobs.Models;
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
            DateTimeOffset inputDateTimeOffset = randomDateTimeOffset;
            List<string> inputPolicyNames = GetPolicyNames();
            List<BlobSignedIdentifier> inputSignedIdentifiers = SetupSignedIdentifiers(inputDateTimeOffset);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(randomDateTimeOffset);

            this.blobStorageBrokerMock.Setup(broker =>
                broker.GetBlobContainerClient(inputContainer))
                    .Returns(blobContainerClientMock.Object);

            this.blobStorageBrokerMock.Setup(broker =>
                broker.TokenLifetimeDays)
                    .Returns(365);

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
                broker.TokenLifetimeDays,
                    Times.Exactly(inputPolicyNames.Count));

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
