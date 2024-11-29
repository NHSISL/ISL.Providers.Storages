// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using Azure.Storage.Blobs.Models;
using FluentAssertions;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ISL.Providers.Storages.AzureBlobStorage.Tests.Unit.Services.Foundations.Storages
{
    public partial class StorageTests
    {
        [Fact]
        public async Task ShouldListFilesInContainerAsync()
        {
            // given
            string randomContainer = GetRandomString();
            string inputContainer = randomContainer;
            var randomAsyncPageable = CreateAsyncPageableBlobItem();
            var outputAsyncPageable = randomAsyncPageable;
            List<string> expectedFileNames = new List<string>();

            await foreach (BlobItem blobItem in outputAsyncPageable)
            {
                expectedFileNames.Add(blobItem.Name);
            }

            this.blobStorageBrokerMock.Setup(broker =>
                broker.GetBlobContainerClient(inputContainer))
                    .Returns(blobContainerClientMock.Object);

            this.blobStorageBrokerMock.Setup(broker =>
                broker.GetBlobsAsync(blobContainerClientMock.Object))
                    .ReturnsAsync(outputAsyncPageable);

            // when
            var actualFileNames = await this.storageService.ListFilesInContainerAsync(inputContainer);

            // then
            actualFileNames.Should().BeEquivalentTo(expectedFileNames);

            this.blobStorageBrokerMock.Verify(broker =>
                broker.GetBlobContainerClient(inputContainer),
                    Times.Once);

            this.blobStorageBrokerMock.Verify(broker =>
                broker.GetBlobsAsync(blobContainerClientMock.Object),
                    Times.Once);

            this.blobStorageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
