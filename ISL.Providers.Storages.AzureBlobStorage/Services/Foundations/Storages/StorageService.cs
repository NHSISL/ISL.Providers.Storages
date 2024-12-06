﻿// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using Azure;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Files.DataLake;
using Azure.Storage.Sas;
using ISL.Providers.Storages.Abstractions.Models;
using ISL.Providers.Storages.AzureBlobStorage.Brokers.DateTimes;
using ISL.Providers.Storages.AzureBlobStorage.Brokers.Storages.Blobs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ISL.Providers.Storages.AzureBlobStorage.Services.Foundations.Storages
{
    internal partial class StorageService : IStorageService
    {
        private readonly IBlobStorageBroker blobStorageBroker;
        private readonly IDateTimeBroker dateTimeBroker;

        public StorageService(IBlobStorageBroker blobStorageBroker, IDateTimeBroker dateTimeBroker)
        {
            this.blobStorageBroker = blobStorageBroker;
            this.dateTimeBroker = dateTimeBroker;
        }

        public ValueTask CreateFileAsync(Stream input, string fileName, string container) =>
        TryCatch(async () =>
        {
            ValidateStorageArgumentsOnCreate(input, fileName, container);
            BlobContainerClient blobContainerClient = this.blobStorageBroker.GetBlobContainerClient(container);
            BlobClient blobClient = this.blobStorageBroker.GetBlobClient(blobContainerClient, fileName);
            await this.blobStorageBroker.CreateFileAsync(blobClient, input);
        });

        public ValueTask RetrieveFileAsync(Stream output, string fileName, string container) =>
        TryCatch(async () =>
        {
            ValidateStorageArgumentsOnRetrieve(output, fileName, container);
            BlobContainerClient blobContainerClient = this.blobStorageBroker.GetBlobContainerClient(container);
            BlobClient blobClient = this.blobStorageBroker.GetBlobClient(blobContainerClient, fileName);
            await this.blobStorageBroker.RetrieveFileAsync(blobClient, output);
        });

        public ValueTask DeleteFileAsync(string fileName, string container) =>
        TryCatch(async () =>
        {
            ValidateStorageArgumentsOnDelete(fileName, container);
            BlobContainerClient blobContainerClient = this.blobStorageBroker.GetBlobContainerClient(container);
            BlobClient blobClient = this.blobStorageBroker.GetBlobClient(blobContainerClient, fileName);
            await this.blobStorageBroker.DeleteFileAsync(blobClient);
        });

        public ValueTask<List<string>> ListFilesInContainerAsync(string container) =>
        TryCatch(async () =>
        {
            ValidateContainerName(container);
            BlobContainerClient blobContainerClient = this.blobStorageBroker.GetBlobContainerClient(container);
            AsyncPageable<BlobItem> blobItems = await this.blobStorageBroker.GetBlobsAsync(blobContainerClient);
            List<string> fileNames = new List<string>();

            await foreach (BlobItem blobItem in blobItems)
            {
                fileNames.Add(blobItem.Name);
            }

            return fileNames;
        });

        public ValueTask<string> GetDownloadLinkAsync(string fileName, string container, DateTimeOffset expiresOn) =>
        TryCatch(async () =>
        {
            ValidateStorageArgumentsOnGetDownloadLink(fileName, container, expiresOn);
            BlobContainerClient blobContainerClient = this.blobStorageBroker.GetBlobContainerClient(container);
            BlobClient blobClient = this.blobStorageBroker.GetBlobClient(blobContainerClient, fileName);
            BlobSasBuilder sasBuilder = this.blobStorageBroker.GetBlobSasBuilder(fileName, container, expiresOn);

            string downloadLink = await this.blobStorageBroker
                .GetDownloadLinkAsync(blobClient, sasBuilder, expiresOn);

            return downloadLink;
        });

        public ValueTask CreateContainerAsync(string container) =>
        TryCatch(async () =>
        {
            ValidateContainerName(container);
            await this.blobStorageBroker.CreateContainerAsync(container);
        });

        public ValueTask<List<string>> RetrieveAllContainersAsync() =>
        TryCatch(async () =>
        {
            AsyncPageable<BlobContainerItem> asyncPageableBlobContainerItem =
                await this.blobStorageBroker.RetrieveAllContainersAsync();

            List<string> containerNames = new List<string>();

            await foreach (var blobContainerItem in asyncPageableBlobContainerItem)
            {
                containerNames.Add(blobContainerItem.Name);
            }

            return containerNames;
        });

        public ValueTask DeleteContainerAsync(string container) =>
        TryCatch(async () =>
        {
            ValidateContainerName(container);
            await this.blobStorageBroker.DeleteContainerAsync(container);
        });

        public ValueTask CreateDirectoryAsync(string container, string directory) =>
        TryCatch(async () =>
        {
            ValidateStorageArgumentsOnCreateDirectory(container, directory);

            DataLakeFileSystemClient dataLakeFileSystemClient =
                this.blobStorageBroker.GetDataLakeFileSystemClient(container);

            await this.blobStorageBroker.CreateDirectoryAsync(dataLakeFileSystemClient, directory);
        });

        public ValueTask CreateAndAssignAccessPoliciesAsync(string container, List<Policy> policies) =>
        TryCatch(async () =>
        {
            ValidateStorageArgumentsOnCreateAccessPolicy(container, policies);
            DateTimeOffset currentDateTimeOffset = await this.dateTimeBroker.GetCurrentDateTimeOffsetAsync();
            BlobContainerClient blobContainerClient = this.blobStorageBroker.GetBlobContainerClient(container);
            List<BlobSignedIdentifier> signedIdentifiers = new List<BlobSignedIdentifier>();

            foreach (Policy policy in policies)
            {
                ValidatePermissions(policy.Permissions);
                string permissions = ConvertToPermissionsString(policy.Permissions);

                var blobSignedIdentifier = new BlobSignedIdentifier
                {
                    Id = policy.PolicyName,

                    AccessPolicy = new BlobAccessPolicy
                    {
                        PolicyStartsOn = currentDateTimeOffset,
                        PolicyExpiresOn = currentDateTimeOffset.AddDays(this.blobStorageBroker.TokenLifetimeDays),
                        Permissions = permissions
                    }
                };

                signedIdentifiers.Add(blobSignedIdentifier);
            }

            await this.blobStorageBroker.AssignAccessPoliciesToContainerAsync(
                blobContainerClient, signedIdentifiers);
        });

        public ValueTask<List<string>> RetrieveListOfAllAccessPoliciesAsync(string container) =>
        TryCatch(async () =>
        {
            ValidateStorageArgumentsOnRetrieveAllAccessPolicies(container);
            BlobContainerClient blobContainerClient = this.blobStorageBroker.GetBlobContainerClient(container);

            BlobContainerAccessPolicy containerAccessPolicy =
                await blobStorageBroker.GetAccessPolicyAsync(blobContainerClient);

            List<string> signedIdentifiers = new List<string>();

            foreach (var signedIdentifier in containerAccessPolicy.SignedIdentifiers)
            {
                signedIdentifiers.Add(signedIdentifier.Id);
            }

            return signedIdentifiers;
        });

        public ValueTask<Policy> RetrieveAccessPolicyByNameAsync(string container, string policyName) =>
        TryCatch(async () =>
        {
            ValidateStorageArgumentsOnRetrieveAccessPolicyByName(container, policyName);
            BlobContainerClient blobContainerClient = this.blobStorageBroker.GetBlobContainerClient(container);

            BlobContainerAccessPolicy containerAccessPolicy =
                await blobStorageBroker.GetAccessPolicyAsync(blobContainerClient);

            List<BlobSignedIdentifier> signedIdentifiers = new List<BlobSignedIdentifier>();

            foreach (var signedIdentifier in containerAccessPolicy.SignedIdentifiers)
            {
                signedIdentifiers.Add(signedIdentifier);
            }

            ValidateAccessPolicyExists(policyName, signedIdentifiers);

            BlobSignedIdentifier matchedSignedIdentifier = signedIdentifiers.First(
                signedIdentifier => signedIdentifier.Id == policyName);

            Policy returnedPolicy = new Policy
            {
                PolicyName = matchedSignedIdentifier.Id,
                Permissions = ConvertToPermissionsList(matchedSignedIdentifier.AccessPolicy.Permissions)
            };

            return returnedPolicy;
        });

        public ValueTask RemoveAccessPoliciesFromContainerAsync(string container) =>
        TryCatch(async () =>
        {
            ValidateStorageArgumentsOnRemoveAccessPolicies(container);
            BlobContainerClient blobContainerClient = this.blobStorageBroker.GetBlobContainerClient(container);
            await this.blobStorageBroker.RemoveAccessPoliciesFromContainerAsync(blobContainerClient);
        });

        public ValueTask<string> CreateDirectorySasTokenAsync(
             string container, string directoryPath, string accessPolicyIdentifier, DateTimeOffset expiresOn) =>
        TryCatch(async () =>
        {
            ValidateStorageArgumentsOnCreateDirectorySasToken(
                container, directoryPath, accessPolicyIdentifier, expiresOn);

            DateTimeOffset dateTimeOffset = await this.dateTimeBroker.GetCurrentDateTimeOffsetAsync();

            var sasToken = await this.blobStorageBroker.CreateDirectorySasTokenAsync(
                container, directoryPath, accessPolicyIdentifier, expiresOn);

            return sasToken;
        });

        virtual internal string ConvertToPermissionsString(List<string> permissions)
        {
            var permissionsMap = new Dictionary<string, char>(StringComparer.OrdinalIgnoreCase)
            {
                { "read", 'r' },
                { "add", 'a' },
                { "create", 'c' },
                { "write", 'w' },
                { "delete", 'd' },
                { "list", 'l' }
            };

            return new string(permissionsMap
                .Where(entry => permissions
                .Contains(entry.Key, StringComparer.OrdinalIgnoreCase))
                .Select(entry => entry.Value)
                .ToArray());
        }

        virtual internal List<string> ConvertToPermissionsList(string permissionsString)
        {
            var permissionsMap = new Dictionary<string, string>
            {
                { "r", "read" },
                { "a", "add" },
                { "c", "create" },
                { "w", "write" },
                { "d", "delete" },
                { "l", "list" }
            };

            List<string> lettersList = permissionsString.Select(c => c.ToString()).ToList();

            return lettersList
                .Where(permissionsMap.ContainsKey)
                .Select(letter => permissionsMap[letter])
                .ToList();
        }
    }
}
