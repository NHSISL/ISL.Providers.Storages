﻿// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using Xeptions;

namespace ISL.Providers.Storages.AzureBlobStorage.Models.Foundations.Files.Exceptions
{
    public class StorageValidationException : Xeption
    {
        public StorageValidationException(string message, Xeption innerException)
            : base(message, innerException)
        { }
    }
}
