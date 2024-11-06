// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using ISL.Providers.Storages.Abstractions.Models.Exceptions;
using Xeptions;

namespace ISL.Providers.Storages.Abstractions.Tests.Unit.Models.Exceptions
{
    public class SomeStorageServiceException : Xeption, IStorageProviderServiceException
    {
        public SomeStorageServiceException(string message, Xeption innerException)
            : base(message, innerException)
        { }
    }
}
