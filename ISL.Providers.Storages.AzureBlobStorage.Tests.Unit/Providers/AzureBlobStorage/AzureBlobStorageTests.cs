using ISL.Providers.Storages.AzureBlobStorage.Brokers.DateTimes;
using ISL.Providers.Storages.AzureBlobStorage.Brokers.Storages.Blobs;
using ISL.Providers.Storages.AzureBlobStorage.Models;
using ISL.Providers.Storages.AzureBlobStorage.Providers.AzureBlobStorage;
using ISL.Providers.Storages.AzureBlobStorage.Services.Foundations.Files;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System;
using System.IO;
using Tynamix.ObjectFiller;

namespace ISL.Providers.Storages.AzureBlobStorage.Tests.Unit.Providers.AzureBlobStorage
{
    public partial class AzureBlobStorageTests
    {
        private readonly Mock<IBlobStorageBroker> blobStorageBrokerMock;
        private readonly Mock<IDateTimeBroker> dateTimeBrokerMock;
        private readonly Mock<IStorageService> storageServiceMock;
        private readonly Mock<AzureBlobStoreConfigurations> azureBlobStoreConfigurationsMock;
        private readonly AzureBlobStorageProvider azureBlobStorageProvider;

        public AzureBlobStorageTests()
        {
            this.blobStorageBrokerMock = new Mock<IBlobStorageBroker>();
            this.dateTimeBrokerMock = new Mock<IDateTimeBroker>();
            this.storageServiceMock = new Mock<IStorageService>();
            this.azureBlobStoreConfigurationsMock = new Mock<AzureBlobStoreConfigurations>();

            var serviceCollection = new ServiceCollection()
                .AddTransient<IBlobStorageBroker>(broker =>
                {
                    return blobStorageBrokerMock.Object;
                })
                .AddTransient<IDateTimeBroker>(broker =>
                {
                    return dateTimeBrokerMock.Object;
                })
                .AddTransient<IStorageService>(service =>
                {
                    return storageServiceMock.Object;
                })
                .AddSingleton(azureBlobStoreConfigurationsMock.Object);

            IServiceProvider serviceProvider = serviceCollection.BuildServiceProvider();
            this.azureBlobStorageProvider = new AzureBlobStorageProvider(serviceProvider);
        }

        private static string GetRandomString() =>
            new MnemonicString().GetValue();

        public class HasLengthStream : MemoryStream
        {
            public override long Length => 1;
        }
    }
}
