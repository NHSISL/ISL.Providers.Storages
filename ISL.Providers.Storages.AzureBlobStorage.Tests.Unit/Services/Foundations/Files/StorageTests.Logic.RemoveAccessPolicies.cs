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

            // when
            await this.storageService.RemoveAccessPoliciesFromContainerAsync(inputContainer);

            // then
            this.blobStorageBrokerMock.Verify(broker =>
                broker.RemoveAccessPoliciesFromContainerAsync(inputContainer),
                    Times.Once());

            this.blobStorageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
