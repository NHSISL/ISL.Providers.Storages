using Azure.Storage.Blobs.Models;
using FluentAssertions;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ISL.Providers.Storages.AzureBlobStorage.Tests.Unit.Services.Foundations.Storages
{
    public partial class StorageTests
    {
        [Fact]
        public async Task ShouldRetrieveAllAccessPoliciesFromContainerAsync()
        {
            // given
            string randomString = GetRandomString();
            List<string> randomStringList = GetRandomStringList();
            string inputContainer = randomString;
            BlobContainerAccessPolicy randomBlobContainerAccessPolicy = CreateRandomBlobContainerAccessPolicy();
            BlobContainerAccessPolicy outputBlobContainerAccessPolicy = randomBlobContainerAccessPolicy;
            var expectedAccessPolicies = new List<string>();

            foreach (var signedIdentifier in outputBlobContainerAccessPolicy.SignedIdentifiers)
            {
                expectedAccessPolicies.Add(signedIdentifier.Id);
            }

            this.blobStorageBrokerMock.Setup(broker =>
                broker.GetBlobContainerClient(inputContainer))
                    .Returns(blobContainerClientMock.Object);

            this.blobStorageBrokerMock.Setup(broker =>
                broker.GetAccessPolicyAsync(blobContainerClientMock.Object))
                    .ReturnsAsync(outputBlobContainerAccessPolicy);

            // when
            List<string> actualAccessPolicies = await this.storageService
                .RetrieveAllAccessPoliciesFromContainerAsync(inputContainer);

            // then
            actualAccessPolicies.Should().BeEquivalentTo(expectedAccessPolicies);

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
