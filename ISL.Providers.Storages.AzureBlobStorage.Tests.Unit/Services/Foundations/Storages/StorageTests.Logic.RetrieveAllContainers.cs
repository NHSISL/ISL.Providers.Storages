// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using Azure;
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
        public async Task ShouldRetrieveAllContainersAsync()
        {
            // given
            string randomContainer = GetRandomString();
            string inputContainer = randomContainer;

            AsyncPageable<BlobContainerItem> randomAsyncPageableBlobContainerItem =
                CreateAsyncPageableBlobContainerItem();

            AsyncPageable<BlobContainerItem> outputAsyncPageableBlobContainerItem =
                randomAsyncPageableBlobContainerItem;

            List<string> expectedContainers = new List<string>();

            await foreach (var blobContainerItem in outputAsyncPageableBlobContainerItem)
            {
                expectedContainers.Add(blobContainerItem.Name);
            }

            this.blobStorageBrokerMock.Setup(broker =>
                broker.RetrieveAllContainersAsync())
                    .ReturnsAsync(outputAsyncPageableBlobContainerItem);

            // when
            List<string> actualContainers = await this.storageService.RetrieveAllContainersAsync();

            // then
            actualContainers.Should().BeEquivalentTo(expectedContainers);

            this.blobStorageBrokerMock.Verify(broker =>
                broker.RetrieveAllContainersAsync(),
                    Times.Once);

            this.blobStorageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
