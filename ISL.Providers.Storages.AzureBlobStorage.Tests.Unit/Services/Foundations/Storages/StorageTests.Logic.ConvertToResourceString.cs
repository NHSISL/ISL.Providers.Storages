using FluentAssertions;

namespace ISL.Providers.Storages.AzureBlobStorage.Tests.Unit.Services.Foundations.Storages
{
    public partial class StorageTests
    {
        [Theory]
        [InlineData(true, "d")]
        [InlineData(false, "b")]
        public void ShouldConvertToResourceString(bool isDirectory, string resourceString)
        {
            // given
            bool inputIsDirectory = isDirectory;
            string expectedOutput = resourceString;

            // when
            string actualOutput = this.storageService.ConvertToResourceString(inputIsDirectory);

            // then
            actualOutput.Should().BeEquivalentTo(expectedOutput);
        }
    }
}
