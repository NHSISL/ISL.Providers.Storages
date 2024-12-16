// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using Azure;
using Azure.Identity;
using Azure.Storage;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Files.DataLake;
using Azure.Storage.Sas;
using ISL.Providers.Storages.Abstractions.Models;
using ISL.Providers.Storages.AzureBlobStorage.Brokers.DateTimes;
using ISL.Providers.Storages.AzureBlobStorage.Brokers.Storages.Blobs;
using ISL.Providers.Storages.AzureBlobStorage.Services.Foundations.Storages;
using KellermanSoftware.CompareNetObjects;
using Microsoft.WindowsAzure.Storage;
using Moq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using Tynamix.ObjectFiller;

namespace ISL.Providers.Storages.AzureBlobStorage.Tests.Unit.Services.Foundations.Storages
{
    public partial class StorageTests
    {
        private readonly Mock<IBlobStorageBroker> blobStorageBrokerMock;
        private readonly Mock<IDateTimeBroker> dateTimeBrokerMock;
        private readonly Mock<BlobServiceClient> blobServiceClientMock;
        private readonly Mock<DataLakeServiceClient> dataLakeServiceClientMock;
        private readonly Mock<DataLakeFileSystemClient> dataLakeFileSystemClientMock;
        private readonly Mock<BlobSasBuilder> blobSasBuilderMock;
        private readonly Mock<BlobUriBuilder> blobUriBuilderMock;
        private readonly Mock<BlobContainerClient> blobContainerClientMock;
        private readonly Mock<BlobClient> blobClientMock;
        private readonly Mock<Response> blobClientResponseMock;
        private readonly StorageService storageService;
        private readonly ICompareLogic compareLogic;

        public StorageTests()
        {
            this.blobStorageBrokerMock = new Mock<IBlobStorageBroker>();
            this.dateTimeBrokerMock = new Mock<IDateTimeBroker>();
            this.blobServiceClientMock = new Mock<BlobServiceClient>();
            this.dataLakeServiceClientMock = new Mock<DataLakeServiceClient>();
            this.dataLakeFileSystemClientMock = new Mock<DataLakeFileSystemClient>();
            this.blobSasBuilderMock = new Mock<BlobSasBuilder>();
            this.blobUriBuilderMock = new Mock<BlobUriBuilder>(new Uri("http://mytest.com/"));
            this.blobContainerClientMock = new Mock<BlobContainerClient>();
            this.blobClientMock = new Mock<BlobClient>();
            this.blobClientResponseMock = new Mock<Response>();
            this.compareLogic = new CompareLogic();

            this.storageService = new StorageService(
                this.blobStorageBrokerMock.Object,
                this.dateTimeBrokerMock.Object);
        }

        private static string GetRandomString() =>
            new MnemonicString().GetValue();

        private static string GetRandomStringWithLengthOf(int length)
        {
            string result = new MnemonicString(wordCount: 1, wordMinLength: length, wordMaxLength: length).GetValue();

            return result.Length > length ? result.Substring(0, length) : result;
        }

        private static int GetRandomNumber() =>
            new IntRange(max: 15, min: 2).GetValue();

        private static int GetRandomNumber(int max, int min) =>
            new IntRange(max: max, min: min).GetValue();

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

        private static DateTimeOffset GetRandomDateTimeOffset() =>
            new DateTimeRange(earliestDate: new DateTime()).GetValue();

        private static DateTimeOffset GetRandomFutureDateTimeOffset()
        {
            DateTime futureStartDate = DateTimeOffset.UtcNow.AddDays(1).Date;
            int randomDaysInFuture = GetRandomNumber();
            DateTime futureEndDate = futureStartDate.AddDays(randomDaysInFuture).Date;

            return new DateTimeRange(earliestDate: futureStartDate, latestDate: futureEndDate).GetValue();
        }

        private static string GetRandomPermissionsString()
        {
            List<string> permissionsStringList = new List<string>
            {
                "r",
                "a",
                "c",
                "w",
                "d",
                "l",
                "rl",
                "acw",
                "acd",
                "wd",
                "racwdl"
            };

            var rng = new Random();
            int index = rng.Next(permissionsStringList.Count);

            return permissionsStringList[index];
        }

        public class ZeroLengthStream : MemoryStream
        {
            public override long Length => 0;
        }

        public class HasLengthStream : MemoryStream
        {
            public override long Length => 1;
        }

        public static TheoryData<Stream, string> InvalidArgumentsStreamLengthZero()
        {
            Stream stream = new ZeroLengthStream();

            return new TheoryData<Stream, string>
            {
                { null, null },
                { stream, "" },
                { stream, " " }
            };
        }

        public static TheoryData<Stream, string> InvalidArgumentsStreamHasLength()
        {
            Stream stream = new HasLengthStream();

            return new TheoryData<Stream, string>
            {
                { null, null },
                { stream, ""},
                { stream, " " }
            };
        }

        public static TheoryData<string, DateTimeOffset> GetInvalidSasArguments()
        {
            DateTimeOffset defaultDateTimeOffset = default;
            DateTimeOffset pastDateTimeOffset = DateTimeOffset.MinValue;

            return new TheoryData<string, DateTimeOffset>
            {
                { null, defaultDateTimeOffset },
                { "", defaultDateTimeOffset },
                { " ", pastDateTimeOffset },
            };
        }

        public static TheoryData<string, List<string>> ConvertToPermissionsListInputsAndExpected() =>
            new TheoryData<string, List<string>>
            {
                { "r" , new List<string> { "read" }},
                { "a" , new List<string> { "add" }},
                { "c" , new List<string> { "create" }},
                { "w" , new List<string> { "write" }},
                { "d" , new List<string> { "delete" }},
                { "l" , new List<string> { "list" }},
                { "rcd" , new List<string> { "read", "create", "delete" }},
                { "ac" , new List<string> {"add", "create" }},
                { "racwdl" , new List<string> { "read", "add", "create", "write", "delete", "list" }},
            };

        private static AsyncPageable<BlobItem> CreateAsyncPageableBlobItem()
        {
            List<BlobItem> blobItems = CreateBlobItems();
            Page<BlobItem> page = Page<BlobItem>.FromValues(blobItems, null, new Mock<Response>().Object);
            AsyncPageable<BlobItem> pages = AsyncPageable<BlobItem>.FromPages(new[] { page });

            return pages;
        }

        private static List<BlobItem> CreateBlobItems()
        {
            List<BlobItem> blobItems = new List<BlobItem>();
            int randomNumber = GetRandomNumber();

            for (int index = 0; index < randomNumber; index++)
            {
                string blobItemName = GetRandomString();
                BlobItem blobItem = BlobsModelFactory.BlobItem(name: blobItemName);
                blobItems.Add(blobItem);
            }

            return blobItems;
        }

        private static AsyncPageable<BlobContainerItem> CreateAsyncPageableBlobContainerItem()
        {
            List<BlobContainerItem> blobItems = CreateBlobContainerItems();
            Page<BlobContainerItem> page = Page<BlobContainerItem>.FromValues(blobItems, null, new Mock<Response>().Object);
            AsyncPageable<BlobContainerItem> pages = AsyncPageable<BlobContainerItem>.FromPages(new[] { page });

            return pages;
        }

        private static List<BlobContainerItem> CreateBlobContainerItems()
        {
            List<BlobContainerItem> blobContainerItems = new List<BlobContainerItem>();
            int randomNumber = GetRandomNumber();

            for (int index = 0; index < randomNumber; index++)
            {
                BlobContainerProperties properties = BlobsModelFactory.BlobContainerProperties(GetRandomDateTimeOffset(), new ETag());
                string blobContainerItemName = GetRandomString();
                BlobContainerItem blobContainerItem = BlobsModelFactory.BlobContainerItem(blobContainerItemName, properties);
                blobContainerItems.Add(blobContainerItem);
            }

            return blobContainerItems;
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

        private static List<BlobSignedIdentifier> SetupSignedIdentifiers(DateTimeOffset createdDateTimeOffset)
        {
            List<BlobSignedIdentifier> signedIdentifiers = new List<BlobSignedIdentifier>
            {
                new BlobSignedIdentifier
                {
                    Id = $"read",
                    AccessPolicy = new BlobAccessPolicy
                    {
                        Permissions = "rl"
                    }
                },
                new BlobSignedIdentifier
                {
                    Id = $"write",
                    AccessPolicy = new BlobAccessPolicy
                    {
                        Permissions = "acw"
                    }
                },
            };
            return signedIdentifiers;
        }

        private static BlobContainerAccessPolicy CreateRandomBlobContainerAccessPolicy()
        {
            string randomPolicyName = GetRandomString();

            return CreateBlobContainerAccessPolicyFiller(randomPolicyName).Create();
        }

        private static BlobContainerAccessPolicy CreateRandomBlobContainerAccessPolicy(string inputPolicyName) =>
            CreateBlobContainerAccessPolicyFiller(inputPolicyName).Create();

        private static Filler<BlobContainerAccessPolicy> CreateBlobContainerAccessPolicyFiller(string inputPolicyName)
        {
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            var filler = new Filler<BlobContainerAccessPolicy>();

            filler.Setup()
                .OnType<DateTimeOffset>().Use(randomDateTimeOffset)
                .OnType<DateTimeOffset?>().Use(randomDateTimeOffset)
                .OnProperty(policy => policy.ETag).Use(new ETag(GetRandomString()))
                .OnProperty(policy => policy.SignedIdentifiers).Use(CreateRandomBlobSignedIdentifierEnumerable(inputPolicyName));

            return filler;
        }

        private static IEnumerable<BlobSignedIdentifier> CreateRandomBlobSignedIdentifierEnumerable(string inputPolicyName)
        {
            int randomSignedIdentifierNumber = GetRandomNumber(5, 1);
            int updatedSignedIdentifierIndex = GetRandomNumber(randomSignedIdentifierNumber, 1) - 1;
            List<BlobSignedIdentifier> blobSignedIdentifierList = new List<BlobSignedIdentifier>();

            for (int index = 0; index < randomSignedIdentifierNumber; index++)
            {
                BlobSignedIdentifier randomBlobSignedIdentifier = CreateRandomBlobSignedIdentifier();

                if (index == updatedSignedIdentifierIndex)
                {
                    randomBlobSignedIdentifier.Id = inputPolicyName;
                }

                blobSignedIdentifierList.Add(randomBlobSignedIdentifier);
            }

            return blobSignedIdentifierList;
        }

        private static BlobSignedIdentifier CreateRandomBlobSignedIdentifier() =>
            CreateBlobSignedIdentifierFiller().Create();

        private static Filler<BlobSignedIdentifier> CreateBlobSignedIdentifierFiller()
        {
            var filler = new Filler<BlobSignedIdentifier>();

            filler.Setup()
                .OnProperty(signedIdentifier => signedIdentifier.AccessPolicy).Use(CreateRandomBlobAccessPolicy());

            return filler;
        }

        private static BlobAccessPolicy CreateRandomBlobAccessPolicy() =>
            CreateBlobAccessPolicyFiller().Create();

        private static Filler<BlobAccessPolicy> CreateBlobAccessPolicyFiller()
        {
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            var filler = new Filler<BlobAccessPolicy>();

            filler.Setup()
                .OnType<DateTimeOffset>().Use(randomDateTimeOffset)
                .OnType<DateTimeOffset?>().Use(randomDateTimeOffset)
                .OnProperty(accessPolicy => accessPolicy.Permissions).Use(GetRandomPermissionsString());

            return filler;
        }


        private static List<string> GetPolicyNames() =>
            new List<string>
            {
                "read",
                "write",
                "delete",
                "fullaccess"
            };

        private static List<string> ConvertToPermissionsList(string permissionsString)
        {
            var permissionsMap = new Dictionary<string, string>
            {
                { "r", "read" },
                { "a", "add" },
                { "c", "create" },
                { "w", "write" },
                { "d", "delete" },
                { "l", "list" }
            };

            List<string> lettersList = permissionsString.Select(c => c.ToString()).ToList();

            return lettersList
                .Where(permissionsMap.ContainsKey)
                .Select(letter => permissionsMap[letter])
                .ToList();
        }

        private Expression<Func<List<BlobSignedIdentifier>, bool>> SameBlobSignedIdentifierListAs(
            List<BlobSignedIdentifier> expectedList) =>
                actualList => this.compareLogic.Compare(expectedList, actualList).AreEqual;

        private Expression<Func<Uri, bool>> SameUriAs(
            Uri expectedUri) =>
                actualUri => this.compareLogic.Compare(expectedUri, actualUri).AreEqual;

        private Expression<Func<StorageSharedKeyCredential, bool>> SameStorageSharedKeyCredentialAs(
            StorageSharedKeyCredential expectedCredential) =>
                actualCredential => this.compareLogic.Compare(expectedCredential, actualCredential).AreEqual;

        private static UserDelegationKey CreateUserDelegationKey() =>
            new Mock<UserDelegationKey>().Object;

        private static AuthenticationFailedException CreateAuthenticationFailedException() =>
        (AuthenticationFailedException)RuntimeHelpers.GetUninitializedObject(
            type: typeof(AuthenticationFailedException));

        private static ArgumentException CreateArgumentException() =>
            (ArgumentException)RuntimeHelpers.GetUninitializedObject(type: typeof(ArgumentException));

        private static RequestFailedException CreateRequestFailedException() =>
            (RequestFailedException)RuntimeHelpers.GetUninitializedObject(type: typeof(RequestFailedException));

        private static StorageException CreateStorageException() =>
            (StorageException)RuntimeHelpers.GetUninitializedObject(type: typeof(StorageException));

        private static OperationCanceledException CreateOperationCanceledException() =>
            (OperationCanceledException)RuntimeHelpers.GetUninitializedObject(
                type: typeof(OperationCanceledException));

        private static TimeoutException CreateTimeoutException() =>
            (TimeoutException)RuntimeHelpers.GetUninitializedObject(type: typeof(TimeoutException));

        private static IOException CreateIOException() =>
            (IOException)RuntimeHelpers.GetUninitializedObject(type: typeof(IOException));

        public static TheoryData<Exception> DependencyValidationExceptions()
        {
            AuthenticationFailedException someAuthenticationFailedException = CreateAuthenticationFailedException();
            ArgumentException someArgumentException = CreateArgumentException();

            return new TheoryData<Exception>
            {
                { someAuthenticationFailedException },
                { someArgumentException }
            };
        }

        public static TheoryData<Exception> DependencyExceptions()
        {
            RequestFailedException someRequestFailedException = CreateRequestFailedException();
            StorageException someStorageException = CreateStorageException();
            OperationCanceledException someOperationCanceledException = CreateOperationCanceledException();
            TimeoutException someTimeoutException = CreateTimeoutException();
            IOException someIOException = CreateIOException();

            return new TheoryData<Exception>
            {
                { someRequestFailedException },
                { someStorageException },
                { someOperationCanceledException },
                { someTimeoutException },
                { someIOException }
            };
        }

        public static TheoryData<List<Policy>> NullAndEmptyList() =>
            new TheoryData<List<Policy>>
            {
                { null },
                { new List<Policy>() }
            };

        public static TheoryData<string, bool, string> PathRelatedInputs()
        {
            string randomString = GetRandomString();
            string directoryPath = randomString;
            string blobPath = randomString + ".csv";

            return new TheoryData<string, bool, string>
            {
                { directoryPath, true, "d" },
                { blobPath, false, "b" }
            };
        }
    }
}
