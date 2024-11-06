// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Collections;
using Xeptions;

namespace ISL.Providers.Storages.Abstractions.Models.Exceptions
{
    public class StorageProviderServiceException : Xeption
    {
        public StorageProviderServiceException(string message, Xeption innerException)
            : base(message, innerException)
        { }

        public StorageProviderServiceException(string message, Xeption innerException, IDictionary data)
            : base(message, innerException, data)
        { }
    }
}
