using Azure;
using Azure.Storage.Blobs.Models;
using FluentAssertions;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ISL.Providers.Storages.AzureBlobStorage.Tests.Unit.Services.Foundations.Files
{
    public partial class StorageTests
    {
        [Fact]
        public async Task ShouldRetrieveAllAccessPoliciesFromContainerAsync()
        {
            // given
            string randomString = GetRandomString();
            string inputContainer = randomString;
            BlobContainerAccessPolicy randomBlobContainerAccessPolicy = CreateRandomBlobContainerAccessPolicy();
            BlobContainerAccessPolicy outputBlobContainerAccessPolicy = randomBlobContainerAccessPolicy;
            var expectedResult = new List<string>();

            foreach (var signedIdentifier in outputBlobContainerAccessPolicy.SignedIdentifiers)
            {
                expectedResult.Add(signedIdentifier.Id);
            }

            this.blobServiceClientMock.Setup(client =>
                client.GetBlobContainerClient(inputContainer))
                    .Returns(blobContainerClientMock.Object);

            this.blobContainerClientMock.Setup(client =>
                client.GetAccessPolicyAsync(null, default))
                    .Returns(Task.FromResult(Response
                        .FromValue(outputBlobContainerAccessPolicy, blobClientResponseMock.Object)));

            // when
            List<string> actualResult = await this.storageService
                .RetrieveAllAccessPoliciesFromContainerAsync(inputContainer);

            // then
            actualResult.Should().BeEquivalentTo(expectedResult);

            this.blobServiceClientMock.Verify(client =>
                client.GetBlobContainerClient(inputContainer),
                    Times.Once);

            this.blobContainerClientMock.Verify(client =>
                client.GetAccessPolicyAsync(null, default),
                    Times.Once);

            this.blobServiceClientMock.VerifyNoOtherCalls();
            this.blobContainerClientMock.VerifyNoOtherCalls();
            this.blobClientMock.VerifyNoOtherCalls();
            this.blobStorageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
