// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

namespace ISL.Providers.Storages.AzureBlobStorage.Models
{
    public class AzureBlobStoreConfigurations
    {
        public string ServiceUri { get; set; }
        public string SubscriptionId { get; set; }
        public string ResourceGroupName { get; set; }
        public string StorageAccountName { get; set; }
    }
}
