using Moq;
using System.Threading.Tasks;

namespace ISL.Providers.Storages.AzureBlobStorage.Tests.Unit.Providers.AzureBlobStorage
{
    public partial class AzureBlobStorageTests
    {
        [Fact]
        public async Task ShouldRemoveAccessPolicyByNameAsync()
        {
            // given
            string randomContainer = GetRandomString();
            string randomPolicyName = GetRandomString();
            string inputContainer = randomContainer;
            string inputPolicyName = randomPolicyName;

            // when
            await this.azureBlobStorageProvider
                .RemoveAccessPolicyByNameAsync(inputContainer, inputPolicyName);

            // then
            this.storageServiceMock.Verify(service =>
                service.RemoveAccessPolicyByNameAsync(inputContainer, inputPolicyName),
                    Times.Once);

            this.storageServiceMock.VerifyNoOtherCalls();
        }
    }
}
