using Azure.Storage.Blobs.Models;
using FluentAssertions;
using ISL.Providers.Storages.Abstractions.Models;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ISL.Providers.Storages.AzureBlobStorage.Tests.Unit.Services.Foundations.Storages
{
    public partial class StorageTests
    {
        [Fact]
        public async Task ShouldRetrieveAllAccessPoliciesAsync()
        {
            // given
            string randomString = GetRandomString();
            string inputContainer = randomString;
            BlobContainerAccessPolicy randomBlobContainerAccessPolicy = CreateRandomBlobContainerAccessPolicy();
            BlobContainerAccessPolicy outputBlobContainerAccessPolicy = randomBlobContainerAccessPolicy;
            var expectedAccessPolicies = new List<Policy>();

            foreach (var signedIdentifier in outputBlobContainerAccessPolicy.SignedIdentifiers)
            {
                Policy policy = new Policy
                {
                    PolicyName = signedIdentifier.Id,
                    Permissions = ConvertToPermissionsList(signedIdentifier.AccessPolicy.Permissions)
                };

                expectedAccessPolicies.Add(policy);
            }

            this.blobStorageBrokerMock.Setup(broker =>
                broker.GetBlobContainerClient(inputContainer))
                    .Returns(blobContainerClientMock.Object);

            this.blobStorageBrokerMock.Setup(broker =>
                broker.GetAccessPolicyAsync(blobContainerClientMock.Object))
                    .ReturnsAsync(outputBlobContainerAccessPolicy);

            // when
            List<Policy> actualAccessPolicies = await this.storageService
                .RetrieveAllAccessPoliciesAsync(inputContainer);

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
