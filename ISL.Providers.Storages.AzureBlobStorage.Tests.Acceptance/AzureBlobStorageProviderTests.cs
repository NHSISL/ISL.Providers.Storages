using ISL.Providers.Storages.Abstractions.Models;
using ISL.Providers.Storages.AzureBlobStorage.Models;
using ISL.Providers.Storages.AzureBlobStorage.Providers.AzureBlobStorage;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using Tynamix.ObjectFiller;

namespace ISL.Providers.Storages.AzureBlobStorage.Tests.Acceptance
{
    public partial class AzureBlobStorageProviderTests
    {
        private readonly IAzureBlobStorageProvider azureBlobStorageProvider;
        private readonly IConfiguration configuration;

        public AzureBlobStorageProviderTests()
        {
            var configurationBuilder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile("appsettings.Development.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables();

            configuration = configurationBuilder.Build();

            AzureBlobStoreConfigurations azureBlobStoreConfigurations = configuration
                .GetSection("AzureBlobStoreConfigurations").Get<AzureBlobStoreConfigurations>();

            this.azureBlobStorageProvider = new AzureBlobStorageProvider(azureBlobStoreConfigurations);
        }

        private static int GetRandomNumber() =>
            new IntRange(max: 15, min: 2).GetValue();

        private static string GetRandomString() =>
            new MnemonicString().GetValue();

        private static string GetRandomStringWithLengthOf(int length)
        {
            string result = new MnemonicString(
                wordCount: 1, wordMinLength: length, wordMaxLength: length).GetValue();

            return result.Length > length ? result.Substring(0, length) : result;
        }

        private static List<string> GetRandomFileNameList()
        {
            int randomNumber = GetRandomNumber();
            List<string> randomStringList = new List<string>();

            for (int index = 0; index < randomNumber; index++)
            {
                string randomString = GetRandomStringWithLengthOf(randomNumber);
                string randomFileName = randomString + ".csv";
                randomStringList.Add(randomFileName);
            }

            return randomStringList;
        }

        private static List<Policy> GetPolicies() =>
            new List<Policy>
            {
                new Policy
                {
                    PolicyName = "read",
                    Permissions = new List<string>
                    {
                        "read",
                        "list"
                    }
                },
                new Policy
                {
                    PolicyName = "write",
                    Permissions = new List<string>
                    {
                        "write",
                        "add",
                        "create"
                    }
                },
            };
    }
}
