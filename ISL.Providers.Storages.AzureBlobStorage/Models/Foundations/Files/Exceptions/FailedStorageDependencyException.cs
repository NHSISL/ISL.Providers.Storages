// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using Xeptions;

namespace ISL.Providers.Storages.AzureBlobStorage.Models.Foundations.Files.Exceptions
{
    public class FailedStorageDependencyException : Xeption
    {
        public FailedStorageDependencyException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}
