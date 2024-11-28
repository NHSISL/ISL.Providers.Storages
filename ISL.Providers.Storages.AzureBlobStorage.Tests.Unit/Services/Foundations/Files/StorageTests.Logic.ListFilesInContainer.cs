// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using FluentAssertions;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ISL.Providers.Storages.AzureBlobStorage.Tests.Unit.Services.Foundations.Files
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
            List<string> outputFileNames = GetRandomStringList();
            List<string> expectedFileNames = outputFileNames;

            this.blobStorageBrokerMock.Setup(broker =>
                broker.ListContainerAsync(inputContainer))
                    .ReturnsAsync(outputFileNames);

            // when
            var actualFileNames = await this.storageService.ListFilesInContainerAsync(inputContainer);

            // then
            actualFileNames.Should().BeEquivalentTo(expectedFileNames);

            this.blobStorageBrokerMock.Verify(broker =>
                broker.ListContainerAsync(inputContainer),
                    Times.Once);

            this.blobStorageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
