// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using Azure;
using Azure.Core.Pipeline;
using Azure.Storage;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Files.DataLake;
using Azure.Storage.Sas;
using ISL.Providers.Storages.AzureBlobStorage.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace ISL.Providers.Storages.AzureBlobStorage.Brokers.Storages.Blobs
{
    internal class BlobStorageBroker : IBlobStorageBroker
    {
        private readonly BlobServiceClient BlobServiceClient;
        private readonly DataLakeServiceClient DataLakeServiceClient;
        private readonly StorageSharedKeyCredential StorageSharedKeyCredential;
        public int TokenLifetimeDays { get; private set; }

        public BlobStorageBroker(AzureBlobStoreConfigurations azureBlobStoreConfigurations)
        {
            var blobServiceClientOptions = new BlobClientOptions()
            {
                Transport = new HttpClientTransport(new HttpClient { Timeout = new TimeSpan(1, 0, 0) }),
                Retry = { NetworkTimeout = new TimeSpan(1, 0, 0) },
                EnableTenantDiscovery = true
            };

            var dataLakeClientOptions = new DataLakeClientOptions()
            {
                Transport = new HttpClientTransport(new HttpClient { Timeout = new TimeSpan(1, 0, 0) }),
                Retry = { NetworkTimeout = new TimeSpan(1, 0, 0) },
                EnableTenantDiscovery = true
            };

            this.StorageSharedKeyCredential = new StorageSharedKeyCredential(
                accountName: azureBlobStoreConfigurations.StorageAccountName,
                accountKey: azureBlobStoreConfigurations.StorageAccountAccessKey);

            this.BlobServiceClient = new BlobServiceClient(
                serviceUri: new Uri(azureBlobStoreConfigurations.ServiceUri),
                credential: this.StorageSharedKeyCredential,
                options: blobServiceClientOptions);

            this.DataLakeServiceClient = new DataLakeServiceClient(
                serviceUri: new Uri(azureBlobStoreConfigurations.ServiceUri),
                credential: this.StorageSharedKeyCredential,
                dataLakeClientOptions);

            this.TokenLifetimeDays = azureBlobStoreConfigurations.TokenLifetimeDays;
        }

        public BlobContainerClient GetBlobContainerClient(string container) =>
            BlobServiceClient.GetBlobContainerClient(container);

        public DataLakeFileSystemClient GetDataLakeFileSystemClient(string container) =>
            DataLakeServiceClient.GetFileSystemClient(container);

        public BlobClient GetBlobClient(
            BlobContainerClient blobContainerClient, string fileName) =>
            blobContainerClient.GetBlobClient(fileName);

        public async ValueTask CreateFileAsync(BlobClient blobClient, Stream input) =>
            await blobClient.UploadAsync(input);

        public async ValueTask RetrieveFileAsync(BlobClient blobClient, Stream output) =>
            await blobClient.DownloadToAsync(output);

        public async ValueTask DeleteFileAsync(BlobClient blobClient) =>
            await blobClient.DeleteAsync(DeleteSnapshotsOption.None);

        public async ValueTask CreateContainerAsync(string container) =>
            await BlobServiceClient.CreateBlobContainerAsync(container);

        public async ValueTask<AsyncPageable<BlobContainerItem>> RetrieveAllContainersAsync() =>
            BlobServiceClient.GetBlobContainersAsync();

        public async ValueTask DeleteContainerAsync(string container) =>
            await BlobServiceClient.DeleteBlobContainerAsync(container);

        public async ValueTask<AsyncPageable<BlobItem>> GetBlobsAsync(BlobContainerClient blobContainerClient) =>
            blobContainerClient.GetBlobsAsync();

        public async ValueTask CreateDirectoryAsync(
            DataLakeFileSystemClient dataLakeFileSystemClient,
            string directory) =>
            await dataLakeFileSystemClient.CreateDirectoryAsync(directory);

        public async ValueTask AssignAccessPoliciesToContainerAsync(
            BlobContainerClient blobContainerClient, List<BlobSignedIdentifier> signedIdentifiers) =>
            await blobContainerClient.SetAccessPolicyAsync(permissions: signedIdentifiers);

        public async ValueTask<BlobContainerAccessPolicy> GetAccessPolicyAsync(
            BlobContainerClient blobContainerClient) =>
            await blobContainerClient.GetAccessPolicyAsync();

        public async ValueTask RemoveAllAccessPoliciesAsync(BlobContainerClient containerClient)
        {
            List<BlobSignedIdentifier> emptySignedIdentifiers = new List<BlobSignedIdentifier>();
            await containerClient.SetAccessPolicyAsync(permissions: emptySignedIdentifiers);
        }

        public async ValueTask<string> GetDownloadLinkAsync(
            BlobClient blobClient, BlobSasBuilder blobSasBuilder, DateTimeOffset expiresOn)
        {
            var blobUriBuilder = new BlobUriBuilder(blobClient.Uri)
            {
                Sas = blobSasBuilder.ToSasQueryParameters(StorageSharedKeyCredential)
            };

            return blobUriBuilder.ToUri().ToString();
        }

        public async ValueTask<string> CreateDirectorySasTokenAsync(
            string container,
            string directoryPath,
            string accessPolicyIdentifier)
        {
            var directorySasBuilder = new DataLakeSasBuilder()
            {
                Identifier = accessPolicyIdentifier,
                Resource = "d",
                Path = directoryPath,
                IsDirectory = true,
                FileSystemName = container,
            };

            var sasQueryParameters = directorySasBuilder.ToSasQueryParameters(
                StorageSharedKeyCredential);

            return sasQueryParameters.ToString();
        }

        public BlobSasBuilder GetBlobSasBuilder(string blobName, string blobContainerName, DateTimeOffset startsOn, DateTimeOffset expiresOn)
        {
            var blobSasBuilder = new BlobSasBuilder()
            {
                BlobContainerName = blobContainerName,
                BlobName = blobName,
                Resource = "b",
                StartsOn = startsOn,
                ExpiresOn = expiresOn
            };

            blobSasBuilder.SetPermissions(BlobSasPermissions.Read);

            return blobSasBuilder;
        }
    }
}
