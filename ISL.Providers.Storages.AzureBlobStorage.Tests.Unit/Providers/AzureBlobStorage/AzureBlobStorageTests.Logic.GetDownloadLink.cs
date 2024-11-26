using FluentAssertions;
using Moq;
using System;
using System.Threading.Tasks;

namespace ISL.Providers.Storages.AzureBlobStorage.Tests.Unit.Providers.AzureBlobStorage
{
    public partial class AzureBlobStorageTests
    {
        [Fact]
        public async Task ShouldGetDownloadLinkAsync()
        {
            // given
            string randomFileName = GetRandomString();
            string randomContainer = GetRandomString();
            DateTimeOffset randomDateTimeOffset = GetRandomFutureDateTimeOffset();
            string randomDownloadLink = GetRandomString();
            string inputFileName = randomFileName;
            string inputContainer = randomContainer;
            DateTimeOffset inputDateTimeOffset = randomDateTimeOffset;
            string outputDownloadLink = randomDownloadLink;
            string expectedDownloadLink = outputDownloadLink;

            this.storageServiceMock.Setup(service =>
                service.GetDownloadLinkAsync(inputFileName, inputContainer, inputDateTimeOffset))
                    .ReturnsAsync(outputDownloadLink);

            // when
            string actualDownloadLink = await this.azureBlobStorageProvider
                .GetDownloadLinkAsync(inputFileName, inputContainer, inputDateTimeOffset);

            // then
            actualDownloadLink.Should().Be(expectedDownloadLink);

            this.storageServiceMock.Verify(service =>
                service.GetDownloadLinkAsync(inputFileName, inputContainer, inputDateTimeOffset),
                    Times.Once);

            this.storageServiceMock.VerifyNoOtherCalls();
        }
    }
}
