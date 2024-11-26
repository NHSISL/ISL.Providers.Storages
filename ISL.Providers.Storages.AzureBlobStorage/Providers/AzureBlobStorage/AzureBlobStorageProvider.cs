using ISL.Providers.Storages.AzureBlobStorage.Brokers.DateTimes;
using ISL.Providers.Storages.AzureBlobStorage.Brokers.Storages.Blobs;
using ISL.Providers.Storages.AzureBlobStorage.Models;
using ISL.Providers.Storages.AzureBlobStorage.Models.Foundations.Files.Exceptions;
using ISL.Providers.Storages.AzureBlobStorage.Models.Providers.Exceptions;
using ISL.Providers.Storages.AzureBlobStorage.Services.Foundations.Files;
using ISL.Providers.Storages.AzureBlobStorage.Services.Foundations.Storages;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
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

        internal AzureBlobStorageProvider(IStorageService storageService)
        {
            this.storageService = storageService;
        }

        /// <summary>
        /// Creates a file in the storage container.
        /// </summary>
        /// <param name="input">The <see cref="Stream"/> containing the file data to be uploaded.</param>
        /// <param name="fileName">The name of the file to create in the container.</param>
        /// <param name="container">The name of the storage container where the file will be stored.</param>
        /// <returns>A <see cref="ValueTask"/> representing the asynchronous operation.</returns>
        /// <exception cref="AzureBlobStorageProviderValidationException" />
        /// <exception cref="AzureBlobStorageProviderDependencyValidationException" />
        /// <exception cref="AzureBlobStorageProviderDependencyException" />
        /// <exception cref="AzureBlobStorageProviderServiceException" />
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

        /// <summary>
        /// Retrieves a file from the storage container.
        /// </summary>
        /// <param name="output">The <see cref="Stream"/> containing the file data to be downloaded.</param>
        /// <param name="fileName">The name of the file to retrieve in the container.</param>
        /// <param name="container">The name of the storage container where the file is located.</param>
        /// <returns>A <see cref="ValueTask"/> representing the asynchronous operation.</returns>
        /// <exception cref="AzureBlobStorageProviderValidationException" />
        /// <exception cref="AzureBlobStorageProviderDependencyValidationException" />
        /// <exception cref="AzureBlobStorageProviderDependencyException" />
        /// <exception cref="AzureBlobStorageProviderServiceException" />
        public async ValueTask RetrieveFileAsync(Stream output, string fileName, string container)
        {
            try
            {
                await this.storageService.RetrieveFileAsync(output, fileName, container);
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
        /// <summary>
        /// Asynchronously deletes a file from the specified storage container.
        /// </summary>
        /// <param name="fileName">The name of the file to delete.</param>
        /// <param name="container">The name of the storage container where the file is located.</param>
        /// <returns>A <see cref="ValueTask"/> representing the asynchronous operation.</returns>
        /// <exception cref="AzureBlobStorageProviderValidationException" />
        /// <exception cref="AzureBlobStorageProviderDependencyValidationException" />
        /// <exception cref="AzureBlobStorageProviderDependencyException" />
        /// <exception cref="AzureBlobStorageProviderServiceException" />
        public async ValueTask DeleteFileAsync(string fileName, string container) =>
            await this.storageService.DeleteFileAsync(fileName, container);

        public ValueTask<string> GetDownloadLinkAsync(string fileName, string container, DateTimeOffset expiresOn) =>
            throw new NotImplementedException();

        public ValueTask<List<string>> ListContainerAsync(string container) =>
            throw new NotImplementedException();

        public ValueTask<string> GetAccessTokenAsync(
            string path,
            string container,
            string accessLevel,
            DateTimeOffset expiresOn) =>
            throw new NotImplementedException();

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
