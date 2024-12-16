using Azure;
using Azure.Storage.Files.DataLake;
using ISL.Providers.Storages.Abstractions.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace ISL.Providers.Storages.AzureBlobStorage.Tests.Acceptance
{
    public partial class AzureBlobStorageProviderTests
    {
        [Fact]
        public async Task ShouldCreateSasTokenWritePermissionsAsync()
        {
            // given
            string randomPolicyName = GetRandomString();
            string randomContainer = GetRandomString();
            string randomFolder = GetRandomString();
            string randomSubFolder = GetRandomString();
            string randomFileName = GetRandomString();
            DateTimeOffset randomDateTimeOffset = GetRandomFutureDateTimeOffset();
            string inputPolicyName = randomPolicyName;
            string inputContainer = randomContainer.ToLower();
            string inputDirectory = randomFolder + "/" + randomSubFolder;
            DateTimeOffset inputExpiresOn = randomDateTimeOffset;
            randomFileName = randomFileName + ".csv";
            string inputFileName = randomFileName;

            List<Policy> inputAccessPolicyList = new List<Policy>
            {
                new Policy
                {
                    PolicyName = inputPolicyName,
                    Permissions = new List<string>
                    {
                        "write",
                        "add",
                        "Create"
                    }
                },
            };

            await this.azureBlobStorageProvider.CreateContainerAsync(inputContainer);

            await this.azureBlobStorageProvider.CreateAndAssignAccessPoliciesAsync(
                inputContainer,
                inputAccessPolicyList);

            // when
            var actualSasToken = await this.azureBlobStorageProvider.CreateSasTokenAsync(
                inputContainer,
                inputDirectory,
                inputPolicyName,
                inputExpiresOn);

            // then
            Uri uri = new Uri(this.serviceUri + "/" + inputContainer + "/" + inputDirectory + "/subfolder");
            AzureSasCredential sasCredential = new AzureSasCredential(actualSasToken);

            DataLakeDirectoryClient sasAuthenticatedDataLakeDirectoryClient =
                new DataLakeDirectoryClient(uri, sasCredential);

            await sasAuthenticatedDataLakeDirectoryClient.CreateFileAsync(inputFileName);
            await this.azureBlobStorageProvider.DeleteContainerAsync(inputContainer);
        }

        [Fact]
        public async Task ShouldCreateSasTokenReadPermissionsAsync()
        {
            // given
            string randomPolicyName = GetRandomString();
            string randomContainer = GetRandomString();
            string randomFolder = GetRandomString();
            string randomSubFolder = GetRandomString();
            string randomFileName = GetRandomString();
            DateTimeOffset randomDateTimeOffset = GetRandomFutureDateTimeOffset();
            string inputPolicyName = randomPolicyName;
            string inputContainer = randomContainer.ToLower();
            string inputDirectory = randomFolder + "/" + randomSubFolder;
            randomFileName = randomFileName + ".csv";
            string inputFileName = randomFileName;
            DateTimeOffset inputExpiresOn = randomDateTimeOffset;
            string randomString = GetRandomString();
            byte[] randomBytes = Encoding.UTF8.GetBytes(randomString);
            MemoryStream inputStream = new MemoryStream(randomBytes);
            string inputDirectoryFilePath = inputDirectory + "/" + inputFileName;

            List<Policy> inputAccessPolicyList = new List<Policy>
            {
                new Policy
                {
                    PolicyName = inputPolicyName,
                    Permissions = new List<string>
                    {
                        "Read",
                        "list"
                    }
                },
            };

            await this.azureBlobStorageProvider.CreateContainerAsync(inputContainer);

            await this.azureBlobStorageProvider.CreateFileAsync(inputStream, inputDirectoryFilePath, inputContainer);

            await this.azureBlobStorageProvider.CreateAndAssignAccessPoliciesAsync(
                inputContainer,
                inputAccessPolicyList);

            // when
            var actualSasToken = await this.azureBlobStorageProvider.CreateSasTokenAsync(
                inputContainer,
                inputDirectory,
                inputPolicyName,
                inputExpiresOn);

            // then
            Uri uri = new Uri(this.serviceUri + "/" + inputContainer + "/" + inputDirectoryFilePath);
            AzureSasCredential sasCredential = new AzureSasCredential(actualSasToken);

            DataLakeFileClient sasAuthenticatedDataLakeFileClient =
                new DataLakeFileClient(uri, sasCredential);

            await sasAuthenticatedDataLakeFileClient.OpenReadAsync();
            await this.azureBlobStorageProvider.DeleteContainerAsync(inputContainer);
        }
    }
}
