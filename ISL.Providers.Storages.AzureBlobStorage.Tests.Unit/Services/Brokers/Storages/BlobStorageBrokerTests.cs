using Azure.Storage.Blobs;
using ISL.Providers.Storages.AzureBlobStorage.Brokers.Storages.Blobs;
using ISL.Providers.Storages.AzureBlobStorage.Models;
using System;
using System.Threading.Tasks;

namespace ISL.Providers.Storages.AzureBlobStorage.Tests.Unit.Services.Brokers.Storages
{
    public class BlobStorageBrokerTests
    {
        private readonly BlobStorageBroker blobStorageBroker;

        public BlobStorageBrokerTests()
        {
            this.blobStorageBroker = new BlobStorageBroker(new AzureBlobStoreConfigurations
            {
                ServiceUri = "https://sadldevdigitalteam.blob.core.windows.net",
                //SubscriptionId = "4a847aff-f8d7-4e49-b35d-3b61e606a1a2",
                SubscriptionId = "2f7a9b80-2e65-4ed6-9851-2f727effb3a1",
                ResourceGroupName = "thre",
                StorageAccountName = "egtdhbb"
            });
        }


        [Fact]
        public async Task TestLogicAndFlow()
        {
            string containerName = "test";
            string path = "outbox";
            string permissions = "read";
            DateTimeOffset startsOn = DateTimeOffset.UtcNow;
            DateTimeOffset expiresOn = DateTimeOffset.UtcNow.AddYears(1);


            BlobClient blobClient =
                        this.blobStorageBroker.BlobServiceClient
                            .GetBlobContainerClient(containerName)
                            .GetBlobClient(path);

            //ar test = this.blobStorageBroker.BlobServiceClient.AccountName;
            var sasBuilder = this.blobStorageBroker.GetBlobSasBuilder(path, containerName, expiresOn, permissions);
            var blobUriBuilder = this.blobStorageBroker.GetBlobUriBuilder(blobClient.Uri);
            var userDelegationKey = this.blobStorageBroker.GetUserDelegationKey(startsOn, expiresOn, default);
            var sasToken = sasBuilder.ToSasQueryParameters(userDelegationKey, blobClient.AccountName);
            blobUriBuilder.Sas = sasToken;

            string stringToken = sasToken.ToString();

            var url = $"{blobClient.Uri}?{stringToken}";

            Console.WriteLine($"Sas toke {sasToken.ToString()}");



        }
    }
}
