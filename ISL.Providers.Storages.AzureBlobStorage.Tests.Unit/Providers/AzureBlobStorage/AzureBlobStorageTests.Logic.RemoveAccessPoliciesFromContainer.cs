using Moq;
using System.Threading.Tasks;

namespace ISL.Providers.Storages.AzureBlobStorage.Tests.Unit.Providers.AzureBlobStorage
{
    public partial class AzureBlobStorageTests
    {
        [Fact]
        public async Task ShouldShouldRemoveAccessPoliciesAsync()
        {
            // given
            string randomContainer = GetRandomString();
            string inputContainer = randomContainer;

            // when
            await this.azureBlobStorageProvider.RemoveAccessPoliciesAsync(
                inputContainer);

            // then
            this.storageServiceMock.Verify(service =>
                service.RemoveAccessPoliciesAsync(inputContainer),
                    Times.Once);

            this.storageServiceMock.VerifyNoOtherCalls();
        }
    }
}
