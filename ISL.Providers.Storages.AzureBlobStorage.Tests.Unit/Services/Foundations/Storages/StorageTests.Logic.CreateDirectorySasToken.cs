// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using FluentAssertions;
using Moq;
using System.Threading.Tasks;

namespace ISL.Providers.Storages.AzureBlobStorage.Tests.Unit.Services.Foundations.Storages
{
    public partial class StorageTests
    {
        [Fact]
        public async Task ShouldCreateDirectorySasTokenAsync()
        {
            // given
            string randomContainer = GetRandomString();
            string randomDirectoryPath = GetRandomString();
            string randomAccessPolicyIdentifier = GetRandomString();
            string randomSasToken = GetRandomString();
            string inputContainer = randomContainer;
            string inputDirectoryPath = randomDirectoryPath;
            string inputAccessPolicyIdentifier = randomAccessPolicyIdentifier;
            string outputSasToken = randomSasToken;
            string expectedSasToken = outputSasToken;

            this.blobStorageBrokerMock.Setup(broker =>
                broker.CreateDirectorySasTokenAsync(
                    inputContainer,
                    inputDirectoryPath,
                    inputAccessPolicyIdentifier))
                        .ReturnsAsync(outputSasToken);

            // when
            var actualSasToken = await this.storageService.CreateDirectorySasTokenAsync(
                inputContainer, inputDirectoryPath, inputAccessPolicyIdentifier);

            // then
            actualSasToken.Should().BeEquivalentTo(expectedSasToken);

            this.blobStorageBrokerMock.Verify(broker =>
                broker.CreateDirectorySasTokenAsync(
                    inputContainer,
                    inputDirectoryPath,
                    inputAccessPolicyIdentifier),
                        Times.Once);

            this.blobStorageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
