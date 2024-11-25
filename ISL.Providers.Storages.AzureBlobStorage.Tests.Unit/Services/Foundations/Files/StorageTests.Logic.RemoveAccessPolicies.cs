using Azure.Storage.Blobs.Models;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ISL.Providers.Storages.AzureBlobStorage.Tests.Unit.Services.Foundations.Files
{
    public partial class StorageTests
    {
        [Fact]
        public async Task ShouldRemoveAccessPolicyAsync()
        {
            // given
            string randomString = GetRandomString();
            string inputContainer = randomString;
            List<BlobSignedIdentifier> emptySignedIdentifiers = new List<BlobSignedIdentifier>();
            List<BlobSignedIdentifier> inputSignedIdentifiers = emptySignedIdentifiers;

            this.blobServiceClientMock.Setup(client =>
                client.GetBlobContainerClient(inputContainer))
                    .Returns(blobContainerClientMock.Object);

            // when
            await this.storageService.RemoveAccessPoliciesFromContainerAsync(inputContainer);

            // then
            this.blobServiceClientMock.Verify(client =>
                client.GetBlobContainerClient(inputContainer),
                    Times.Once);

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
