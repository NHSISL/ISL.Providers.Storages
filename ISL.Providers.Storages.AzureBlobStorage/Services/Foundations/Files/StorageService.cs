// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using Azure;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using ISL.Providers.Storages.AzureBlobStorage.Brokers.Storages.Blobs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace ISL.Providers.Storages.AzureBlobStorage.Services.Foundations.Storages
{
    internal partial class StorageService : IStorageService
    {
        private readonly IBlobStorageBroker blobStorageBroker;

        internal StorageService(IBlobStorageBroker blobStorageBroker)
        {
            this.blobStorageBroker = blobStorageBroker;
        }

        public ValueTask CreateFileAsync(Stream input, string fileName, string container) =>
            TryCatch(async () =>
            {
                ValidateStorageArgumentsOnCreate(input, fileName, container);

                BlobClient blobClient =
                    this.blobStorageBroker.BlobServiceClient
                        .GetBlobContainerClient(container)
                        .GetBlobClient(fileName);

                await blobClient.UploadAsync(input);
            });

        public ValueTask RetrieveFileAsync(Stream output, string fileName, string container) =>
            TryCatch(async () =>
            {
                ValidateStorageArgumentsOnRetrieve(output, fileName, container);

                BlobClient blobClient =
                    this.blobStorageBroker.BlobServiceClient
                        .GetBlobContainerClient(container)
                        .GetBlobClient(fileName);

                await blobClient.DownloadToAsync(output);
            });

        public ValueTask DeleteFileAsync(string fileName, string container) =>
            TryCatch(async () =>
            {
                ValidateStorageArgumentsOnDelete(fileName, container);

                BlobClient blobClient =
                    this.blobStorageBroker.BlobServiceClient
                        .GetBlobContainerClient(container)
                        .GetBlobClient(fileName);

                await blobClient.DeleteAsync(DeleteSnapshotsOption.None);
            });

        public ValueTask<List<string>> ListFilesInContainerAsync(string container) =>
            TryCatch(async () =>
            {
                ValidateContainerName(container);
                List<string> fileNames = new List<string>();

                BlobContainerClient containerClient =
                    this.blobStorageBroker.BlobServiceClient
                        .GetBlobContainerClient(container);

                AsyncPageable<BlobItem> blobItems = containerClient.GetBlobsAsync();

                await foreach (BlobItem blobItem in blobItems)
                {
                    fileNames.Add(blobItem.Name);
                }

                return fileNames;
            });


        public ValueTask<string> GetDownloadLinkAsync(string fileName, string container, DateTimeOffset expiresOn) =>
            TryCatch(async () =>
            {
                //ValidateStorageArgumentsOnGetDownloadLink(fileName, container, expiresOn);

                //BlobClient blobClient =
                //        this.blobStorageBroker.BlobServiceClient
                //            .GetBlobContainerClient(container)
                //            .GetBlobClient(fileName);


                //var userDelegationKey = this.blobStorageBroker.GetUserDelegationKey(DateTimeOffset.UtcNow, expiresOn);
                //var sasBuilder = this.blobStorageBroker.GetBlobSasBuilder(fileName, container, expiresOn);
                //var blobUriBuilder = this.blobStorageBroker.GetBlobUriBuilder(blobClient.Uri);
                //blobUriBuilder.Sas = sasBuilder.ToSasQueryParameters(userDelegationKey, blobClient.AccountName);

                //return blobUriBuilder.ToUri().ToString();
                return "blobUriBuilder.ToUri().ToString()";
            });

        public ValueTask CreateContainerAsync(string container) =>
            TryCatch(async () =>
            {
                ValidateContainerName(container);
                await this.blobStorageBroker.BlobServiceClient.CreateBlobContainerAsync(container);
            });

        public ValueTask SetContainerACLAsync(string container, string accessType, string permissions) =>
            throw new NotImplementedException();

        //public ValueTask CreateAndAssignRoleToContainerAsync(string roleName, string container) =>
        //    throw new NotImplementedException();

        //public async ValueTask CreateAndAssignRoleToContainerAsync(string roleName, string container)
        //{
        //    var containerId = $"";

        //    //this.blobStorageBroker.BlobServiceClient.GetBlobContainerClient(container).GetProperties()
        //    // check that the role name is one of the pre determined options e.g. Reader, Contributor

        //    // Store the role id guid?

        //    // what to do about principal id and subscription id?

        //    //this.blobStorageBroker.BlobServiceClient.
        //    this.blobStorageBroker.BlobServiceClient.GetBlobContainerClient(container).SetAccessPolicyAsync();

        //    var client = new ArmClient(new DefaultAzureCredential());
        //    var test = client.GetAuthorizationRoleDefinitions();
        //    string scope = "/subscriptions/<subscription-Id>/resourceGroups/venkatesan-rg";
        //    string roleAssignmentName = "acdd7xxxx81ae7";
        //    ResourceIdentifier roleAssignmentResourceId = RoleAssignmentResource.CreateResourceIdentifier(scope, roleAssignmentName);
        //    RoleAssignmentResource roleAssignment = client.GetRoleAssignmentResource(roleAssignmentResourceId);


        //    RoleAssignmentCreateOrUpdateContent content = new RoleAssignmentCreateOrUpdateContent(new ResourceIdentifier("/subscriptions/xxxxx/providers/Microsoft.Authorization/roleDefinitions/<roledefinitionid>"), Guid.Parse("user-object-id"))
        //    {
        //        PrincipalType = RoleManagementPrincipalType.User,
        //    };
        //    var lro = await roleAssignment.UpdateAsync(WaitUntil.Completed, content);
        //    RoleAssignmentResource result = lro.Value;
        //    RoleAssignmentData resourceData = result.Data;
        //    //var authorizationClient = armClient.GetRoleDefinition(new ResourceIdentifier($"/subscriptions/{subscriptionId}"));
        //    //var authorizationClient = armClient.GetSubscriptionResource(new ResourceIdentifier($"/subscriptions"));
        //    //var authorizationClient = armClient.
        //    //authorizationClient.
        //    //var authorizer = armClient.GetAuthorizationRoleDefinition(new ResourceIdentifier(""), new ResourceIdentifier(""));
        //    //authorizer.
        //}

        //public ValueTask CreateAndAssignManagedIdentityToRoleAsync(string identity, string roleName) =>
        //throw new NotImplementedException();

        //public async ValueTask TestCreateAndAssignRoleToContainerAsync(string roleName, string container)
        //{
        //    var containerId = $"";

        //    //this.blobStorageBroker.BlobServiceClient.GetBlobContainerClient(container).GetProperties()
        //    // check that the role name is one of the pre determined options e.g. Reader, Contributor

        //    // Store the role id guid?

        //    // what to do about principal id and subscription id?

        //    //this.blobStorageBroker.BlobServiceClient.
        //    this.blobStorageBroker.BlobServiceClient.GetBlobContainerClient(container).SetAccessPolicyAsync();

        //    var client = new ArmClient(new DefaultAzureCredential());
        //    var subscription = client.GetSubscriptionResource(new ResourceIdentifier("storageAccountId"));
        //    subscription.GetRoleAssignments().CreateOrUpdate(
        //        "cope", "roleAssignmentId", new RoleAssignmentCreateOrUpdateContent
        //        {
        //            PrincipalId = "principalId",
        //            RoleDefinitionId = "roleDefinitionId"
        //        }););
        //    var storageAccount = client.GetStorageAccountResource(new ResourceIdentifier("storageAccountId"));
        //    var blobService = storageAccount.GetBlobService();
        //    var container = blobService.GetBlobContainer("containerName");
        //    string scope = "/subscriptions/<subscription-Id>/resourceGroups/venkatesan-rg";

        //    var roleDefinition = new RoleDefinition()
        //    {
        //        RoleName = roleName,
        //        Description = $"Custom role for {roleName}",
        //        AssignableScopes = new[] { containerId }, // Scope to the specific container
        //        Permissions =
        //    {
        //        new Permission()
        //        {
        //            Actions = actions,
        //            NotActions = notActions
        //        }
        //    }
        //    };

        //}


    }
}
