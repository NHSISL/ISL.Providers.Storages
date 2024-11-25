// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using Azure.Storage.Blobs.Models;
using Moq;
using System.Threading.Tasks;

namespace ISL.Providers.Storages.AzureBlobStorage.Tests.Unit.Services.Foundations.Files
{
    public partial class StorageTests
    {
        [Fact]
        public async Task ShouldCreateContainerAsync()
        {
            // given
            string randomContainer = GetRandomString();
            string inputContainer = randomContainer;

            // when
            await this.storageService.CreateContainerAsync(inputContainer);

            // then
            this.blobServiceClientMock.Verify(client =>
                client.CreateBlobContainerAsync(inputContainer, PublicAccessType.None, null, default),
                    Times.Once);

            this.blobServiceClientMock.VerifyNoOtherCalls();
            this.dataLakeServiceClientMock.VerifyNoOtherCalls();
            this.dataLakeFileSystemClientMock.VerifyNoOtherCalls();
            this.blobContainerClientMock.VerifyNoOtherCalls();
            this.blobClientMock.VerifyNoOtherCalls();
            this.blobStorageBrokerMock.VerifyNoOtherCalls();
        }
    }
}
