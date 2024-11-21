using FluentAssertions;

namespace ISL.Providers.Storages.AzureBlobStorage.Tests.Unit.Services.Foundations.Files
{
    public partial class StorageTests
    {
        [Theory]
        [InlineData("reader", "rl")]
        [InlineData("writer", "w")]
        [InlineData("randomstring", "")]
        public void ShouldConvertPolicyNameToPermissions(string maybePolicyString, string outputString)
        {
            // given
            string inputString = maybePolicyString;
            string expectedOutput = outputString;

            // when
            string actualOutput = this.storageService.ConvertPolicyNameToPermissions(inputString);

            // then
            actualOutput.Should().BeEquivalentTo(expectedOutput);
        }
    }
}
