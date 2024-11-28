using Moq;
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

            this.blobStorageBrokerMock.Setup(broker =>
                broker.GetBlobContainerClient(inputContainer))
                    .Returns(blobContainerClientMock.Object);

            // when
            await this.storageService.RemoveAccessPoliciesFromContainerAsync(inputContainer);

            // then
            this.blobStorageBrokerMock.Verify(broker =>
                broker.GetBlobContainerClient(inputContainer),
                    Times.Once());

            this.blobStorageBrokerMock.Verify(broker =>
                broker.RemoveAccessPoliciesFromContainerAsync(blobContainerClientMock.Object),
                    Times.Once);

            this.blobStorageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
