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
        public string FileSystemUri { get; set; }
        public string StorageAccountName { get; set; }
        // TODO Remove and move to secure location
        public string StorageAccountAccessKey { get; set; }
    }
}
