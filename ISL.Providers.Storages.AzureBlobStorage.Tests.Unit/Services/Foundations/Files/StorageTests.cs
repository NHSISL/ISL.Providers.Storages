// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using Azure;
using Azure.Identity;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Sas;
using ISL.Providers.Storages.AzureBlobStorage.Brokers.DateTimes;
using ISL.Providers.Storages.AzureBlobStorage.Brokers.Storages.Blobs;
using ISL.Providers.Storages.AzureBlobStorage.Services.Foundations.Storages;
using KellermanSoftware.CompareNetObjects;
using Microsoft.WindowsAzure.Storage;
using Moq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Text;
using Tynamix.ObjectFiller;

namespace ISL.Providers.Storages.AzureBlobStorage.Tests.Unit.Services.Foundations.Files
{
    public partial class StorageTests
    {
        private readonly Mock<IBlobStorageBroker> blobStorageBrokerMock;
        private readonly Mock<IDateTimeBroker> dateTimeBrokerMock;
        private readonly Mock<BlobServiceClient> blobServiceClientMock;
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
            this.blobSasBuilderMock = new Mock<BlobSasBuilder>();
            this.blobUriBuilderMock = new Mock<BlobUriBuilder>(new Uri("http://mytest.com/"));
            this.blobContainerClientMock = new Mock<BlobContainerClient>();
            this.blobClientMock = new Mock<BlobClient>();
            this.blobClientResponseMock = new Mock<Response>();
            this.compareLogic = new CompareLogic();

            this.blobStorageBrokerMock.Setup(broker =>
                broker.BlobServiceClient)
                    .Returns(blobServiceClientMock.Object);

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

        public byte[] CreateRandomData()
        {
            string randomMessage = GetRandomString();

            return Encoding.UTF8.GetBytes(randomMessage);
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

        public static TheoryData<string, DateTimeOffset> GetInvalidDownloadArguments()
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

        public static List<BlobSignedIdentifier> SetupSignedIdentifiers(DateTimeOffset createdDateTimeOffset)
        {
            string timestamp = createdDateTimeOffset.ToString("yyyyMMddHHmms");

            List<BlobSignedIdentifier> signedIdentifiers = new List<BlobSignedIdentifier>
            {
                new BlobSignedIdentifier
                {
                    Id = $"reader_{timestamp}",
                    AccessPolicy = new BlobAccessPolicy
                    {
                        PolicyStartsOn = createdDateTimeOffset,
                        PolicyExpiresOn = createdDateTimeOffset.AddYears(1),
                        Permissions = "rl"
                    }
                },
                new BlobSignedIdentifier
                {
                    Id = $"writer_{timestamp}",
                    AccessPolicy = new BlobAccessPolicy
                    {
                        PolicyStartsOn = createdDateTimeOffset,
                        PolicyExpiresOn = createdDateTimeOffset.AddYears(1),
                        Permissions = "w"
                    }
                }
            };

            return signedIdentifiers;
        }

        public static BlobContainerAccessPolicy CreateRandomBlobContainerAccessPolicy() =>
            CreateBlobContainerAccessPolicyFiller().Create();

        private static Filler<BlobContainerAccessPolicy> CreateBlobContainerAccessPolicyFiller()
        {
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            var filler = new Filler<BlobContainerAccessPolicy>();

            filler.Setup()
                .OnType<DateTimeOffset>().Use(randomDateTimeOffset)
                .OnType<DateTimeOffset?>().Use(randomDateTimeOffset)
                .OnProperty(policy => policy.ETag).Use(new ETag(GetRandomString()));

            return filler;
        }

        private static Filler<BlobSignedIdentifier> CreateBlobSignedIdentifierFiller(string signedIdentifierId)
        {

            var filler = new Filler<BlobSignedIdentifier>();

            filler.Setup()
                .OnProperty(signedIdentifier => signedIdentifier.Id).Use(signedIdentifierId);

            return filler;
        }

        private Expression<Func<List<BlobSignedIdentifier>, bool>> SameBlobSignedIdentifierListAs(
            List<BlobSignedIdentifier> expectedList) =>
                actualList => this.compareLogic.Compare(expectedList, actualList).AreEqual;

        private static UserDelegationKey CreateUserDelegationKey() =>
            new Mock<UserDelegationKey>().Object;

        //private static ETag CreateUserDelegationKey() =>
        //    new Mock<ETag<T>>().Object;

        private static AuthenticationFailedException CreateAuthenticationFailedException() =>
        (AuthenticationFailedException)RuntimeHelpers.GetUninitializedObject(type: typeof(AuthenticationFailedException));

        private static ArgumentException CreateArgumentException() =>
            (ArgumentException)RuntimeHelpers.GetUninitializedObject(type: typeof(ArgumentException));

        private static RequestFailedException CreateRequestFailedException() =>
            (RequestFailedException)RuntimeHelpers.GetUninitializedObject(type: typeof(RequestFailedException));

        private static StorageException CreateStorageException() =>
            (StorageException)RuntimeHelpers.GetUninitializedObject(type: typeof(StorageException));

        private static OperationCanceledException CreateOperationCanceledException() =>
            (OperationCanceledException)RuntimeHelpers.GetUninitializedObject(type: typeof(OperationCanceledException));

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

        public static TheoryData<string, List<string>> InvalidPolicyArguments()
        {
            List<string> emptyStringList = new List<string>
            {
                ""
            };

            List<string> blankStringList = new List<string>
            {
                " "
            };

            return new TheoryData<string, List<string>>
            {
                { null, null },
                { "", emptyStringList },
                { " ", blankStringList }
            };
        }
    }
}
