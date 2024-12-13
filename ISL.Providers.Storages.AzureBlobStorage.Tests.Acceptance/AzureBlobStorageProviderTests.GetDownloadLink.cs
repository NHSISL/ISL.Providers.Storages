using FluentAssertions;
using System;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ISL.Providers.Storages.AzureBlobStorage.Tests.Acceptance
{
    public partial class AzureBlobStorageProviderTests
    {
        [Fact]
        public async Task ShouldGetDownloadLink()
        {
            // given
            string randomFileName = GetRandomString();
            string randomContainer = GetRandomString();
            randomFileName = randomFileName + ".csv";
            string inputFileName = randomFileName;
            string inputContainer = randomContainer.ToLower();
            string randomString = GetRandomString();
            string inputString = randomString;
            byte[] inputBytes = Encoding.UTF8.GetBytes(inputString);
            MemoryStream inputStream = new MemoryStream(inputBytes);
            DateTimeOffset dateTimeOffset = DateTimeOffset.UtcNow;
            DateTimeOffset inputExpiresOn = dateTimeOffset.AddMinutes(1);
            await this.azureBlobStorageProvider.CreateContainerAsync(inputContainer);

            await this.azureBlobStorageProvider
                .CreateFileAsync(inputStream, inputFileName, inputContainer);

            // when
            string actualDownloadLink = await this.azureBlobStorageProvider
                .GetDownloadLinkAsync(inputFileName, inputContainer, inputExpiresOn);

            // then
            HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.GetAsync(actualDownloadLink);
            var actualContent = await response.Content.ReadAsStringAsync();
            actualContent.Should().BeEquivalentTo(inputString);
            await this.azureBlobStorageProvider.DeleteContainerAsync(inputContainer);
        }
    }
}
