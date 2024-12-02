﻿using FluentAssertions;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace ISL.Providers.Storages.AzureBlobStorage.Tests.Acceptance
{
    public partial class AzureBlobStorageProviderTests
    {
        [Fact]
        public async Task ShouldRetrieveFileAsync()
        {
            // given
            string randomFileName = GetRandomString();
            randomFileName = randomFileName + ".csv";
            string inputFileName = randomFileName;
            string randomString = GetRandomString();
            byte[] randomBytes = Encoding.UTF8.GetBytes(randomString);
            string inputContainer = "testcontainer";
            MemoryStream inputStream = new MemoryStream(randomBytes);
            MemoryStream outputStream = new MemoryStream();
            MemoryStream actualOutputStream = outputStream;

            // when
            await this.azureBlobStorageProvider.CreateFileAsync(inputStream, inputFileName, inputContainer);
            await this.azureBlobStorageProvider.RetrieveFileAsync(actualOutputStream, inputFileName, inputContainer);

            // then
            actualOutputStream.Length.Should().BeGreaterThan(0);
            await this.azureBlobStorageProvider.DeleteFileAsync(inputFileName, inputContainer);
        }
    }
}