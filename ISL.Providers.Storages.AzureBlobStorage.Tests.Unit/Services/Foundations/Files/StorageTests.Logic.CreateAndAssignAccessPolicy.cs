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
        public async Task ShouldCreateAndAssignAccessPolicyAsync()
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
                broker.TokenLifetimeYears)
                    .Returns(1);

            // when
            await this.storageService.CreateAndAssignAccessPolicyToContainerAsync(inputContainer, inputPolicyNames);

            // then
            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.blobServiceClientMock.Verify(client =>
                client.CreateBlobContainerAsync(inputContainer, PublicAccessType.None, null, default),
                    Times.Once);

            this.blobContainerClientMock.Verify(client =>
                client.SetAccessPolicyAsync(PublicAccessType.None, inputSignedIdentifiers, null, default),
                    Times.Once);
        }
    }
}
