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
        public async Task ShouldListFilesInContainerAsync()
        {
            // given
            List<string> randomFileNameList = GetRandomFileNameList();
            string randomContainer = GetRandomString();
            List<string> inputFileNameList = randomFileNameList;
            List<string> expectedFileNameList = inputFileNameList;
            string inputContainer = randomContainer.ToLower();
            await this.azureBlobStorageProvider.CreateContainerAsync(inputContainer);

            foreach (string inputFileName in inputFileNameList)
            {
                string randomString = GetRandomString();
                byte[] randomBytes = Encoding.UTF8.GetBytes(randomString);
                MemoryStream inputStream = new MemoryStream(randomBytes);

                await this.azureBlobStorageProvider.CreateFileAsync(
                    inputStream, inputFileName, inputContainer);
            }

            // when
            List<string> actualFileNameList =
                await this.azureBlobStorageProvider.ListFilesInContainerAsync(inputContainer);

            // then
            actualFileNameList.Should().BeEquivalentTo(expectedFileNameList);
            await this.azureBlobStorageProvider.DeleteContainerAsync(inputContainer);
        }
    }
}
