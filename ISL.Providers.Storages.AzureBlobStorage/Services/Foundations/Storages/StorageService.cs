// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Azure;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Files.DataLake;
using Azure.Storage.Sas;
using ISL.Providers.Storages.Abstractions.Models;
using ISL.Providers.Storages.AzureBlobStorage.Brokers.DateTimes;
using ISL.Providers.Storages.AzureBlobStorage.Brokers.Storages.Blobs;

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
            DateTimeOffset currentDateTimeOffset = await this.dateTimeBroker.GetCurrentDateTimeOffsetAsync();
            DateTimeOffset startsOnDateTimeOffset = currentDateTimeOffset.AddMinutes(-1);
            BlobContainerClient blobContainerClient = this.blobStorageBroker.GetBlobContainerClient(container);
            BlobClient blobClient = this.blobStorageBroker.GetBlobClient(blobContainerClient, fileName);

            BlobSasBuilder sasBuilder = this.blobStorageBroker
                .GetBlobSasBuilder(fileName, container, startsOnDateTimeOffset, expiresOn);

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
                        PolicyStartsOn = policy.StartTime,
                        PolicyExpiresOn = policy.ExpiryTime,
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

        public ValueTask<List<Policy>> RetrieveAllAccessPoliciesAsync(string container) =>
        TryCatch(async () =>
        {
            ValidateStorageArgumentsOnRetrieveAllAccessPolicies(container);
            BlobContainerClient blobContainerClient = this.blobStorageBroker.GetBlobContainerClient(container);

            BlobContainerAccessPolicy containerAccessPolicy =
                await blobStorageBroker.GetAccessPolicyAsync(blobContainerClient);

            List<Policy> policies = new List<Policy>();

            foreach (var signedIdentifier in containerAccessPolicy.SignedIdentifiers)
            {
                Policy policy = new Policy
                {
                    PolicyName = signedIdentifier.Id,
                    Permissions = ConvertToPermissionsList(signedIdentifier.AccessPolicy.Permissions)
                };

                policies.Add(policy);
            }

            return policies;
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

        public ValueTask RemoveAllAccessPoliciesAsync(string container) =>
        TryCatch(async () =>
        {
            ValidateStorageArgumentsOnRemoveAccessPolicies(container);
            BlobContainerClient blobContainerClient = this.blobStorageBroker.GetBlobContainerClient(container);
            await this.blobStorageBroker.RemoveAllAccessPoliciesAsync(blobContainerClient);
        });

        public ValueTask RemoveAccessPolicyByNameAsync(string container, string policyName) =>
        TryCatch(async () =>
        {
            ValidateStorageArgumentsOnRemoveAccessPolicyByName(container, policyName);
            BlobContainerClient blobContainerClient = this.blobStorageBroker.GetBlobContainerClient(container);

            BlobContainerAccessPolicy containerAccessPolicy =
                await blobStorageBroker.GetAccessPolicyAsync(blobContainerClient);

            List<BlobSignedIdentifier> signedIdentifiers = containerAccessPolicy.SignedIdentifiers.ToList();
            ValidateAccessPolicyExists(policyName, signedIdentifiers);

            BlobSignedIdentifier matchedBlobSignedIdentifier = signedIdentifiers
                .First(signedIdentifier => signedIdentifier.Id == policyName);

            signedIdentifiers.Remove(matchedBlobSignedIdentifier);

            await this.blobStorageBroker
                .AssignAccessPoliciesToContainerAsync(blobContainerClient, signedIdentifiers);
        });

        public ValueTask<string> CreateSasTokenAsync(
             string container,
             string path,
             string accessPolicyIdentifier,
             DateTimeOffset expiresOn) =>
        TryCatch(async () =>
        {
            ValidateStorageArgumentsOnCreateDirectorySasToken(
                container,
                path,
                accessPolicyIdentifier);

            //TODO: Logic to determine if this is a file or directory

            var sasToken = await this.blobStorageBroker.CreateDirectorySasTokenAsync(
                container,
                path,
                accessPolicyIdentifier);

            return sasToken;
        });

        public ValueTask<string> CreateSasTokenAsync(
            string container,
            string path,
            DateTimeOffset expiresOn,
            string accessLevel) =>
        TryCatch(async () =>
        {
            throw new NotImplementedException();
            //ValidateStorageArgumentsOnCreateDirectorySasToken(
            //    container,
            //    path,
            //    accessPolicyIdentifier);

            ////TODO: Logic to determine if this is a file or directory

            //var sasToken = await this.blobStorageBroker.CreateDirectorySasTokenAsync(
            //    container,
            //    path,
            //    accessPolicyIdentifier);

            //return sasToken;
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
