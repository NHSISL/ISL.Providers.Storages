// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using ISL.Providers.Storages.Abstractions;
using Moq;
using System;
using Tynamix.ObjectFiller;

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

        private static string GetRandomString() =>
            new MnemonicString().GetValue();

        private static int GetRandomNumber() =>
            new IntRange(max: 15, min: 2).GetValue();

        private static DateTimeOffset GetRandomFutureDateTimeOffset()
        {
            DateTime futureStartDate = DateTimeOffset.UtcNow.AddDays(1).Date;
            int randomDaysInFuture = GetRandomNumber();
            DateTime futureEndDate = futureStartDate.AddDays(randomDaysInFuture).Date;

            return new DateTimeRange(earliestDate: futureStartDate, latestDate: futureEndDate).GetValue();
        }
    }
}
