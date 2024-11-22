// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

namespace ISL.Providers.Storages.AzureBlobStorage.Models
{
    public class AzureBlobStoreConfigurations
    {
        public string ServiceUri { get; set; }
        public string AzureTenantId { get; set; }
        public int TokenLifetimeDays { get; set; }
        public string StorageAccountName { get; set; }
        public string StorageAccountKey { get; set; }
    }
}
