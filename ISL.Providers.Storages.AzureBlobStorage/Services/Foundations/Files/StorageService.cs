﻿// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using Azure.Storage.Blobs;
using ISL.Providers.Storages.AzureBlobStorage.Brokers.Storages.Blobs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace ISL.Providers.Storages.AzureBlobStorage.Services.Foundations.Storages
{
    internal class StorageService : IStorageService
    {
        private readonly IBlobStorageBroker blobStorageBroker;

        internal StorageService(IBlobStorageBroker blobStorageBroker)
        {
            this.blobStorageBroker = blobStorageBroker;
        }

        public async ValueTask CreateFileAsync(Stream input, string fileName, string container)
        {
            BlobClient blobClient =
                this.blobStorageBroker.BlobServiceClient
                    .GetBlobContainerClient(container)
                    .GetBlobClient(fileName);

            await blobClient.UploadAsync(input);
        }

        public ValueTask RetrieveFileAsync(Stream output, string fileName, string container) =>
            throw new NotImplementedException();

        public ValueTask DeleteFileAsync(string fileName, string container) =>
            throw new NotImplementedException();

        public ValueTask<List<string>> ListFilesInContainerAsync(string container) =>
            throw new NotImplementedException();

        public ValueTask<string> GetDownloadLinkAsync(string fileName, string container, DateTimeOffset expiresOn) =>
            throw new NotImplementedException();

        public ValueTask CreateContainerAsync(string container) =>
            throw new NotImplementedException();

        public ValueTask SetContainerACLAsync(string container, string accessType, string permissions) =>
            throw new NotImplementedException();

        public ValueTask CreateAndAssignRoleToContainerAsync(string roleName, string container) =>
            throw new NotImplementedException();

        public ValueTask CreateAndAssignManagedIdentityToRoleAsync(string identity, string roleName) =>
            throw new NotImplementedException();
    }
}
