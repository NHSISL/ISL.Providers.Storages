using Azure;
using Azure.Identity;
using Azure.Storage.Blobs;
using ISL.Providers.Storages.AzureBlobStorage.Brokers.Storages.Blobs;
using ISL.Providers.Storages.AzureBlobStorage.Services.Foundations.Storages;
using Microsoft.WindowsAzure.Storage;
using Moq;
using System;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using Tynamix.ObjectFiller;

namespace ISL.Providers.Storages.AzureBlobStorage.Tests.Unit.Services.Foundations.Files
{
    public partial class StorageTests
    {
        private readonly Mock<IBlobStorageBroker> blobStorageBrokerMock;
        private readonly Mock<BlobServiceClient> blobServiceClientMock;
        private readonly Mock<BlobContainerClient> blobContainerClientMock;
        private readonly Mock<BlobClient> blobClientMock;
        private readonly StorageService storageService;

        public StorageTests()
        {
            this.blobStorageBrokerMock = new Mock<IBlobStorageBroker>();
            this.blobServiceClientMock = new Mock<BlobServiceClient>();
            this.blobContainerClientMock = new Mock<BlobContainerClient>();
            this.blobClientMock = new Mock<BlobClient>();

            this.blobStorageBrokerMock.Setup(broker =>
                broker.blobServiceClient)
                    .Returns(blobServiceClientMock.Object);

            this.storageService = new StorageService(
                this.blobStorageBrokerMock.Object);
        }

        private static string GetRandomString() =>
            new MnemonicString().GetValue();

        public byte[] CreateRandomData()
        {
            string randomMessage = GetRandomString();
            return Encoding.UTF8.GetBytes(randomMessage);
        }

        public class ZeroLengthStream : MemoryStream
        {
            public override long Length => 0;
        }

        public class HasLengthStream : MemoryStream
        {
            public override long Length => 1;
        }

        public static TheoryData<Stream, string> GetStreamLengthZero()
        {
            Stream stream = new ZeroLengthStream();

            return new TheoryData<Stream, string>
            {
                { stream, " " }
            };
        }
        private static Azure.Response CreateAzureResponse() =>
            (Azure.Response)RuntimeHelpers.GetUninitializedObject(type: typeof(Azure.Response));




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
    }
}
