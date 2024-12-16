using FluentAssertions;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace ISL.Providers.Storages.AzureBlobStorage.Tests.Acceptance
{
    public partial class AzureBlobStorageProviderTests
    {
        [Fact]
        public async Task ShouldDeleteFileAsync()
        {
            // given
            string randomFileName = GetRandomString();
            string randomContainer = GetRandomString();
            randomFileName = randomFileName + ".csv";
            string inputFileName = randomFileName;
            string inputContainer = randomContainer.ToLower();
            string randomString = GetRandomString();
            byte[] randomBytes = Encoding.UTF8.GetBytes(randomString);
            MemoryStream inputStream = new MemoryStream(randomBytes);
            MemoryStream outputStream = new MemoryStream();
            MemoryStream actualOutputStream = outputStream;
            await this.azureBlobStorageProvider.CreateContainerAsync(inputContainer);

            await this.azureBlobStorageProvider
                .CreateFileAsync(inputStream, inputFileName, inputContainer);

            // when
            await this.azureBlobStorageProvider.DeleteFileAsync(inputFileName, inputContainer);

            // then
            List<string> actualFileNames =
                await this.azureBlobStorageProvider.ListFilesInContainerAsync(inputContainer);

            actualFileNames.Should().NotContain(inputFileName);
            await this.azureBlobStorageProvider.DeleteContainerAsync(inputContainer);
        }
    }
}
