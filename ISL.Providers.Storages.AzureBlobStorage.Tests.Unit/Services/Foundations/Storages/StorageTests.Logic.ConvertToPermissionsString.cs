using FluentAssertions;
using System.Collections.Generic;

namespace ISL.Providers.Storages.AzureBlobStorage.Tests.Unit.Services.Foundations.Storages
{
    public partial class StorageTests
    {
        [Fact]
        public void ShouldConvertToPermissionsString()
        {
            // given
            List<string> inputPermissionsList = new List<string>
            {
                "Add",
                "list",
                "DELETE",
                "reAd",
                "Create",
                "write"
            };

            string expectedOutput = "racwdl";

            // when
            string actualOutput = this.storageService.ConvertToPermissionsString(inputPermissionsList);

            // then
            actualOutput.Should().BeEquivalentTo(expectedOutput);
        }
    }
}
