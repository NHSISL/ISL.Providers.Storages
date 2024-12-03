using FluentAssertions;
using System.Collections.Generic;
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
            string inputContainer = randomContainer.ToLower();

            // when
            await this.azureBlobStorageProvider.CreateContainerAsync(inputContainer);

            // then
            List<string> actualContainers =
                await this.azureBlobStorageProvider.RetrieveAllContainersAsync();

            actualContainers.Should().Contain(inputContainer);
            await this.azureBlobStorageProvider.DeleteContainerAsync(inputContainer);
        }
    }
}
