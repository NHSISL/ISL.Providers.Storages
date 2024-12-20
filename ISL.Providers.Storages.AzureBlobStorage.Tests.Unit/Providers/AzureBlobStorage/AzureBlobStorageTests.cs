﻿using ISL.Providers.Storages.Abstractions.Models;
using ISL.Providers.Storages.AzureBlobStorage.Providers.AzureBlobStorage;
using ISL.Providers.Storages.AzureBlobStorage.Services.Foundations.Storages;
using Moq;
using System;
using System.Collections.Generic;
using System.IO;
using Tynamix.ObjectFiller;

namespace ISL.Providers.Storages.AzureBlobStorage.Tests.Unit.Providers.AzureBlobStorage
{
    public partial class AzureBlobStorageTests
    {
        private readonly Mock<IStorageService> storageServiceMock;
        private readonly AzureBlobStorageProvider azureBlobStorageProvider;

        public AzureBlobStorageTests()
        {
            this.storageServiceMock = new Mock<IStorageService>();
            this.azureBlobStorageProvider = new AzureBlobStorageProvider(storageServiceMock.Object);
        }

        private static string GetRandomString() =>
            new MnemonicString().GetValue();

        private static string GetRandomStringWithLengthOf(int length)
        {
            string result = new MnemonicString(wordCount: 1, wordMinLength: length, wordMaxLength: length).GetValue();

            return result.Length > length ? result.Substring(0, length) : result;
        }

        private static DateTimeOffset GetRandomFutureDateTimeOffset()
        {
            DateTime futureStartDate = DateTimeOffset.UtcNow.AddDays(1).Date;
            int randomDaysInFuture = GetRandomNumber();
            DateTime futureEndDate = futureStartDate.AddDays(randomDaysInFuture).Date;

            return new DateTimeRange(earliestDate: futureStartDate, latestDate: futureEndDate).GetValue();
        }

        private static int GetRandomNumber() =>
            new IntRange(max: 15, min: 2).GetValue();

        private static List<string> GetRandomStringList()
        {
            int randomNumber = GetRandomNumber();
            List<string> randomStringList = new List<string>();

            for (int index = 0; index < randomNumber; index++)
            {
                string randomString = GetRandomStringWithLengthOf(randomNumber);
                randomStringList.Add(randomString);
            }

            return randomStringList;
        }

        private static List<Policy> GetPolicies() =>
            new List<Policy>
            {
                new Policy
                {
                    PolicyName = "read",
                    Permissions = new List<string>
                    {
                        "Read",
                        "list"
                    }
                },
                new Policy
                {
                    PolicyName = "write",
                    Permissions = new List<string>
                    {
                        "write",
                        "add",
                        "Create"
                    }
                },
            };

        private static Policy GetPolicy(string policyName) =>
            new Policy
            {
                PolicyName = policyName,
                Permissions = new List<string>
                {
                    "Read",
                    "list"
                }
            };

        private static List<string> GetRandomPermissionsList()
        {
            List<string> returnedList = new List<string>();

            List<string> permissionsList = new List<string>
            {
                "read",
                "write",
                "delete",
                "list",
                "add",
                "create"
            };

            var rng = new Random();
            int index = rng.Next(1, permissionsList.Count);

            for (int i = 0; i < index; i++)
            {
                returnedList.Add(permissionsList[i]);
            }

            return returnedList;
        }

        public class HasLengthStream : MemoryStream
        {
            public override long Length => 1;
        }

        public class ZeroLengthStream : MemoryStream
        {
            public override long Length => 0;
        }
    }
}
