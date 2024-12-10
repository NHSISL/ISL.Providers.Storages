using Moq;
using System.Threading.Tasks;

namespace ISL.Providers.Storages.AzureBlobStorage.Tests.Unit.Services.Foundations.Storages
{
    public partial class StorageTests
    {
        [Fact]
        public async Task ShouldRemoveAccessPolicyAsync()
        {
            // given
            string randomString = GetRandomString();
            string inputContainer = randomString;

            this.blobStorageBrokerMock.Setup(broker =>
                broker.GetBlobContainerClient(inputContainer))
                    .Returns(blobContainerClientMock.Object);

            // when
            await this.storageService.RemoveAllAccessPoliciesAsync(inputContainer);

            // then
            this.blobStorageBrokerMock.Verify(broker =>
                broker.GetBlobContainerClient(inputContainer),
                    Times.Once());

            this.blobStorageBrokerMock.Verify(broker =>
                broker.RemoveAllAccessPoliciesAsync(blobContainerClientMock.Object),
                    Times.Once);

            this.blobStorageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
