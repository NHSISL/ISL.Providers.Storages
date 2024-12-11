using System.IO;
using System.Threading.Tasks;

namespace ISL.Providers.Storages.AzureBlobStorage.Tests.Acceptance
{
    public partial class AzureBlobStorageProviderTests
    {
        [Fact]
        public async Task ShouldCreateFolderAsync()
        {
            // given
            string randomContainer = GetRandomString();
            string randomFolder = GetRandomString();
            string inputContainer = randomContainer.ToLower();
            string inputFolder = randomFolder;
            MemoryStream outputStream = new MemoryStream();
            await this.azureBlobStorageProvider.CreateContainerAsync(inputContainer);

            // when
            await this.azureBlobStorageProvider.CreateFolderInContainerAsync(inputContainer, inputFolder);

            // then
            await this.azureBlobStorageProvider.RetrieveFileAsync(outputStream, inputFolder, inputContainer);
            await this.azureBlobStorageProvider.DeleteContainerAsync(inputContainer);
        }
    }
}
