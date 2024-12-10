using Azure.Storage.Blobs.Models;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ISL.Providers.Storages.AzureBlobStorage.Tests.Unit.Services.Foundations.Storages
{
    public partial class StorageTests
    {
        [Fact]
        public async Task ShouldRemoveAccessPolicyByNameAsync()
        {
            // given
            string randomContainer = GetRandomString();
            string randomPolicyName = GetRandomString();
            string inputContainer = randomContainer;
            string inputPolicyName = randomPolicyName;

            BlobContainerAccessPolicy randomBlobContainerAccessPolicy =
                CreateRandomBlobContainerAccessPolicy(inputPolicyName);

            BlobContainerAccessPolicy outputBlobContainerAccessPolicy = randomBlobContainerAccessPolicy;

            List<BlobSignedIdentifier> outputBlobSignedIdentifiers =
                outputBlobContainerAccessPolicy.SignedIdentifiers.ToList();

            BlobSignedIdentifier matchedBlobSignedIdentifier = outputBlobSignedIdentifiers
                .First(signedIdentifier => signedIdentifier.Id == inputPolicyName);

            outputBlobSignedIdentifiers.Remove(matchedBlobSignedIdentifier);
            List<BlobSignedIdentifier> inputBlobSignedIdentifiers = outputBlobSignedIdentifiers;

            this.blobStorageBrokerMock.Setup(broker =>
                broker.GetBlobContainerClient(inputContainer))
                    .Returns(blobContainerClientMock.Object);

            this.blobStorageBrokerMock.Setup(broker =>
                broker.GetAccessPolicyAsync(blobContainerClientMock.Object))
                    .ReturnsAsync(outputBlobContainerAccessPolicy);

            // when
            await this.storageService
                .RemoveAccessPolicyByNameAsync(inputContainer, inputPolicyName);

            // then
            this.blobStorageBrokerMock.Verify(broker =>
                broker.GetBlobContainerClient(inputContainer),
                    Times.Once());

            this.blobStorageBrokerMock.Verify(broker =>
                broker.GetAccessPolicyAsync(blobContainerClientMock.Object),
                    Times.Once);

            this.blobStorageBrokerMock.Verify(broker =>
                broker.AssignAccessPoliciesToContainerAsync(
                    blobContainerClientMock.Object,
                    It.Is(SameBlobSignedIdentifierListAs(inputBlobSignedIdentifiers))),
                        Times.Once);

            this.blobStorageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
