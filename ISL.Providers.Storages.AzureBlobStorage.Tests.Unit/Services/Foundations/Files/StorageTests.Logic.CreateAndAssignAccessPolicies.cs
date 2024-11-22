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

            List<BlobSignedIdentifier> inputSignedIdentifiers = SetupSignedIdentifiers(randomDateTimeOffset);

            List<string> inputPolicyNames = new List<string>
            {
                "reader",
                "writer"
            };

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(randomDateTimeOffset);

            this.blobServiceClientMock.Setup(client =>
                client.GetBlobContainerClient(inputContainer))
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

            this.blobServiceClientMock.Verify(client =>
                client.GetBlobContainerClient(inputContainer),
                    Times.Once);

            this.blobStorageBrokerMock.Verify(broker =>
                broker.TokenLifetimeDays,
                    Times.Exactly(2));

            this.blobContainerClientMock.Verify(client =>
                client.SetAccessPolicyAsync(
                    PublicAccessType.None,
                    It.Is(SameBlobSignedIdentifierListAs(inputSignedIdentifiers)),
                    null,
                    default),
                        Times.Once);

            this.blobServiceClientMock.VerifyNoOtherCalls();
            this.blobContainerClientMock.VerifyNoOtherCalls();
            this.blobClientMock.VerifyNoOtherCalls();
            this.blobStorageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
