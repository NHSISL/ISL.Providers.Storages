// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using Moq;
using System.IO;
using System.Threading.Tasks;

namespace ISL.Providers.Storages.AzureBlobStorage.Tests.Unit.Services.Foundations.Files
{
    public partial class StorageTests
    {
        [Fact]
        public async Task ShouldCreateFileAsync()
        {
            // given
            string randomFileName = GetRandomString();
            string randomContainer = GetRandomString();
            Stream randomStream = new HasLengthStream();
            string inputFileName = randomFileName;
            string inputContainer = randomContainer;
            Stream inputStream = randomStream;

            // when
            await this.storageService.CreateFileAsync(inputStream, inputFileName, inputContainer);

            // then
            this.blobStorageBrokerMock.Verify(broker =>
                broker.CreateFileAsync(inputStream, inputFileName, inputContainer),
                    Times.Once);

            this.blobStorageBrokerMock.VerifyNoOtherCalls();
        }
    }
}
