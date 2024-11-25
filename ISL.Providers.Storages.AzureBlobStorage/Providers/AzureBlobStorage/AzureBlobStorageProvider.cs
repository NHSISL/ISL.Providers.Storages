using ISL.Providers.Storages.AzureBlobStorage.Brokers.DateTimes;
using ISL.Providers.Storages.AzureBlobStorage.Brokers.Storages.Blobs;
using ISL.Providers.Storages.AzureBlobStorage.Models;
using ISL.Providers.Storages.AzureBlobStorage.Models.Foundations.Files.Exceptions;
using ISL.Providers.Storages.AzureBlobStorage.Models.Providers.Exceptions;
using ISL.Providers.Storages.AzureBlobStorage.Services.Foundations.Files;
using ISL.Providers.Storages.AzureBlobStorage.Services.Foundations.Storages;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;
using System.Threading.Tasks;
using Xeptions;

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

        public async ValueTask CreateFileAsync(Stream input, string fileName, string container)
        {
            try
            {
                await storageService.CreateFileAsync(input, fileName, container);
            }
            catch (StorageValidationException storageValidationException)
            {
                throw CreateProviderValidationException(
                    storageValidationException.InnerException as Xeption);
            }
            catch (StorageDependencyValidationException storageDependencyValidationException)
            {
                throw CreateProviderDependencyValidationException(
                    storageDependencyValidationException.InnerException as Xeption);
            }
            catch (StorageDependencyException storageDependencyException)
            {
                throw CreateProviderDependencyException(
                    storageDependencyException.InnerException as Xeption);
            }
            catch (StorageServiceException storageServiceException)
            {
                throw CreateProviderServiceException(
                    storageServiceException.InnerException as Xeption);
            }
        }

        private static AzureBlobStorageProviderValidationException CreateProviderValidationException(
            Xeption innerException)
        {
            return new AzureBlobStorageProviderValidationException(
                message: "Azure blob storage provider validation error occurred, fix errors and try again.",
                innerException,
                data: innerException.Data);
        }

        private static AzureBlobStorageProviderDependencyValidationException
            CreateProviderDependencyValidationException(Xeption innerException)
        {
            return new AzureBlobStorageProviderDependencyValidationException(
                message: "Azure blob storage provider dependency validation error occurred, " +
                    "fix errors and try again.",
                innerException,
                data: innerException.Data);
        }

        private static AzureBlobStorageProviderDependencyException CreateProviderDependencyException(
            Xeption innerException)
        {
            return new AzureBlobStorageProviderDependencyException(
                message: "Azure blob storage provider dependency error occurred, contact support.",
                innerException);
        }

        private static AzureBlobStorageProviderServiceException CreateProviderServiceException(
            Xeption innerException)
        {
            return new AzureBlobStorageProviderServiceException(
                message: "Azure blob storage provider service error occurred, contact support.",
                innerException);
        }

        private void InitializeClients(IServiceProvider serviceProvider) =>
            storageService = serviceProvider.GetRequiredService<IStorageService>();

        private static IServiceProvider RegisterServices(AzureBlobStoreConfigurations configurations)
        {
            var serviceCollection = new ServiceCollection()
                .AddTransient<IBlobStorageBroker, BlobStorageBroker>()
                .AddTransient<IDateTimeBroker, DateTimeBroker>()
                .AddTransient<IStorageService, StorageService>()
                .AddSingleton(configurations);

            IServiceProvider serviceProvider = serviceCollection.BuildServiceProvider();

            return serviceProvider;
        }
    }
}
