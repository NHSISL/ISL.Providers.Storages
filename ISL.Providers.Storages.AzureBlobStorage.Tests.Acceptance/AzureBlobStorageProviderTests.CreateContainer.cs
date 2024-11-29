using System.Threading.Tasks;

namespace ISL.Providers.Storages.AzureBlobStorage.Tests.Acceptance
{
    public partial class AzureBlobStorageProviderTests
    {
        [Fact]
        public async Task ShouldCreateContainerAsync()
        {
            // given
            string randomContainer = GetRandomString();
            string inputContainer = "testcontainer";

            // when
            await this.azureBlobStorageProvider.CreateContainerAsync(inputContainer);

            // then
        }
    }
}
