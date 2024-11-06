// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections;
using Xeptions;

namespace ISL.Providers.Storages.Abstractions.Models.Exceptions
{
    public class UncatagorizedStorageProviderException : Xeption
    {
        public UncatagorizedStorageProviderException(string message, Exception innerException)
            : base(message, innerException)
        { }

        public UncatagorizedStorageProviderException(
            string message,
            Exception innerException,
            IDictionary data)
            : base(message, innerException, data)
        { }
    }
}
