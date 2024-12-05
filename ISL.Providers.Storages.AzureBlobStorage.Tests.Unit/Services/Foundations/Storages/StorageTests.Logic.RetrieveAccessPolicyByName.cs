using Azure.Storage.Blobs.Models;
using FluentAssertions;
using ISL.Providers.Storages.Abstractions.Models;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ISL.Providers.Storages.AzureBlobStorage.Tests.Unit.Services.Foundations.Storages
{
    public partial class StorageTests
    {
        [Fact]
        public async Task ShouldRetrieveAccessPolicyByNameAsync()
        {
            // given
            string randomContainer = GetRandomString();
            string randomPolicyName = GetRandomString();
            string inputContainer = randomContainer;
            string inputPolicyName = randomPolicyName;
            string expectedPolicyName = inputPolicyName;

            BlobContainerAccessPolicy randomBlobContainerAccessPolicy =
                CreateRandomBlobContainerAccessPolicy(inputPolicyName);

            BlobContainerAccessPolicy outputBlobContainerAccessPolicy = randomBlobContainerAccessPolicy;

            BlobSignedIdentifier matchedSignedIdentifier =
                outputBlobContainerAccessPolicy.SignedIdentifiers
                    .FirstOrDefault(signedIdentifier => signedIdentifier.Id == expectedPolicyName);

            List<string> permissionsList = ConvertToPermissionsList(
                matchedSignedIdentifier.AccessPolicy.Permissions);

            Policy expectedPolicy = new Policy()
            {
                PolicyName = expectedPolicyName,
                Permissions = permissionsList
            };

            this.blobStorageBrokerMock.Setup(broker =>
                broker.GetBlobContainerClient(inputContainer))
                    .Returns(blobContainerClientMock.Object);

            this.blobStorageBrokerMock.Setup(broker =>
                broker.GetAccessPolicyAsync(blobContainerClientMock.Object))
                    .ReturnsAsync(outputBlobContainerAccessPolicy);

            // when
            Policy actualPolicy = await this.storageService
                .RetrieveAccessPolicyByNameAsync(inputContainer, inputPolicyName);

            // then
            actualPolicy.Should().BeEquivalentTo(expectedPolicy);

            this.blobStorageBrokerMock.Verify(broker =>
                broker.GetBlobContainerClient(inputContainer),
                    Times.Once);

            this.blobStorageBrokerMock.Verify(broker =>
                broker.GetAccessPolicyAsync(blobContainerClientMock.Object),
                    Times.Once);

            this.blobStorageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
