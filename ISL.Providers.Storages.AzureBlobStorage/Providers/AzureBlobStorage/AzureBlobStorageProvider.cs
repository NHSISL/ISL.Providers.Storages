using ISL.Providers.Storages.AzureBlobStorage.Brokers.Storages.Blobs;
using ISL.Providers.Storages.AzureBlobStorage.Models;
using ISL.Providers.Storages.AzureBlobStorage.Services.Foundations.Files;
using ISL.Providers.Storages.AzureBlobStorage.Services.Foundations.Storages;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace ISL.Providers.Storages.AzureBlobStorage.Providers.AzureBlobStorage
{
    public class AzureBlobStorageProvider : IAzureBlobStorageProvider
    {
        private IStorageService storageService;
        public AzureBlobStorageProvider(AzureBlobStoreConfigurations configurations)
        {
            IServiceProvider serviceProvider = RegisterServices(configurations);
            InitializeClients(serviceProvider);
        }

        private void InitializeClients(IServiceProvider serviceProvider) =>
            storageService = serviceProvider.GetRequiredService<IStorageService>();

        private static IServiceProvider RegisterServices(AzureBlobStoreConfigurations configurations)
        {
            var serviceCollection = new ServiceCollection()
                .AddTransient<IStorageService, StorageService>()
                .AddTransient<IBlobStorageBroker, BlobStorageBroker>()
                .AddSingleton(configurations);

            IServiceProvider serviceProvider = serviceCollection.BuildServiceProvider();

            return serviceProvider;
        }
    }
}
