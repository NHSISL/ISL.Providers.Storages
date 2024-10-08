using FluentAssertions;
using ISL.Providers.Storages.AzureBlobStorage.Models.Foundations.Files.Exceptions;
using System.IO;
using System.Threading.Tasks;

namespace ISL.Providers.Storages.AzureBlobStorage.Tests.Unit.Services.Foundations.Files
{
    public partial class FilesTests
    {
        [Theory]
        [InlineData(null, null)]
        [InlineData(null, "")]
        [InlineData(null, " ")]
        [MemberData(nameof(GetStreamLengthZero))]
        public async Task ShouldThrowValidationExceptionOnCreateIfStringArgumentsInvalidAndLogItAsync(Stream invalidStream, string invalidText)
        {
            // given
            string invalidFileName = invalidText;
            string invalidContainer = invalidText;
            Stream invalidInputStream = invalidStream;

            var invalidArgumentFilesException = new InvalidArgumentFileException(
                message: "Invalid files service argument(s), please fix the errors and try again.");

            invalidArgumentFilesException.AddData(
                key: "Input",
                values: "Input is invalid");

            invalidArgumentFilesException.AddData(
                key: "FileName",
                values: "FileName is invalid");

            invalidArgumentFilesException.AddData(
                key: "Container",
                values: "Container is invalid");

            var expectedFileValidationException = new FileValidationException(
                message: "File validation error occuured, please fix errors and try again.",
                innerException: invalidArgumentFilesException);

            // when
            ValueTask createFileTask = this.storageService.CreateFileAsync(invalidInputStream, invalidFileName, invalidContainer);

            FileValidationException actualFileValidationException =
                await Assert.ThrowsAsync<FileValidationException>(createFileTask.AsTask);

            // then
            actualFileValidationException.Should().BeEquivalentTo(expectedFileValidationException);

            this.blobServiceClientMock.VerifyNoOtherCalls();
            this.blobContainerClientMock.VerifyNoOtherCalls();
            this.blobClientMock.VerifyNoOtherCalls();
            this.blobStorageBrokerMock.VerifyNoOtherCalls();
        }
    }
}
