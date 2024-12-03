using FluentAssertions;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace ISL.Providers.Storages.AzureBlobStorage.Tests.Acceptance
{
    public partial class AzureBlobStorageProviderTests
    {
        [Fact]
        public async Task ShouldCreateFileAsync()
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

            // when
            await this.azureBlobStorageProvider.CreateFileAsync(
                inputStream, inputFileName, inputContainer);

            await this.azureBlobStorageProvider.RetrieveFileAsync(
                actualOutputStream, inputFileName, inputContainer);

            // then
            actualOutputStream.Length.Should().BeGreaterThan(0);
            await this.azureBlobStorageProvider.DeleteContainerAsync(inputContainer);
        }
    }
}
