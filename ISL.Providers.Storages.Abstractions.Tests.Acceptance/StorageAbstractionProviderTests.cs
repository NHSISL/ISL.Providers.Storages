// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using ISL.Providers.Storages.Abstractions;
using Microsoft.Extensions.Configuration;
using Moq;
using Tynamix.ObjectFiller;

namespace ISL.Providers.ReIdentification.Abstractions.Tests.Acceptance
{
    public partial class StorageAbstractionProviderTests
    {
        private readonly Mock<IStorageProvider> storageProviderMock;
        private readonly IStorageAbstractionProvider storageAbstractionProvider;
        private readonly IConfiguration configuration;

        public StorageAbstractionProviderTests()
        {
            var configurationBuilder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile("appsettings.Development.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables();

            configuration = configurationBuilder.Build();
            storageProviderMock = new Mock<IStorageProvider>();

            this.storageAbstractionProvider =
                new StorageAbstractionProvider(storageProviderMock.Object);
        }

        private static string GetRandomString()
        {
            int length = GetRandomNumber();

            return new MnemonicString(wordCount: 1, wordMinLength: length, wordMaxLength: length).GetValue();
        }

        private static int GetRandomNumber() =>
            new IntRange(min: 2, max: 10).GetValue();
    }
}