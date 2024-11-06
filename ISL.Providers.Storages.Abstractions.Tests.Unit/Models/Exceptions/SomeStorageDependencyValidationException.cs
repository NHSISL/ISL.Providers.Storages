// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Collections;
using ISL.Providers.Storages.Abstractions.Models.Exceptions;
using Xeptions;

namespace ISL.Providers.Storages.Abstractions.Tests.Unit.Models.Exceptions
{
    public class SomeStorageDependencyValidationException : Xeption, IStorageProviderDependencyValidationException
    {
        public SomeStorageDependencyValidationException(string message, Xeption innerException, IDictionary data)
            : base(message, innerException, data)
        { }
    }
}
