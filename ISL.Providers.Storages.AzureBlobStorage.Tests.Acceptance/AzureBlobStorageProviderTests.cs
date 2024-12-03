using ISL.Providers.Storages.AzureBlobStorage.Models;
using ISL.Providers.Storages.AzureBlobStorage.Providers.AzureBlobStorage;
using Microsoft.Extensions.Configuration;
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

        private static string GetRandomString() =>
            new MnemonicString().GetValue();
    }
}
