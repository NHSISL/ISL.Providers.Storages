﻿// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using ISL.Providers.Storages.Abstractions;
using Moq;

namespace ISL.Providers.Storage.Abstractions.Tests.Unit
{
    public partial class StorageAbstractionProviderTests
    {
        private readonly Mock<IStorageProvider> storageProviderMock;
        private readonly StorageAbstractionProvider storageAbstractionProvider;

        public StorageAbstractionProviderTests()
        {
            this.storageProviderMock = new Mock<IStorageProvider>();

            this.storageAbstractionProvider =
                new StorageAbstractionProvider(this.storageProviderMock.Object);
        }
    }
}