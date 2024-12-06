using FluentAssertions;
using System.Collections.Generic;

namespace ISL.Providers.Storages.AzureBlobStorage.Tests.Unit.Services.Foundations.Storages
{
    public partial class StorageTests
    {
        [Theory]
        [MemberData(nameof(ConvertToPermissionsListInputsAndExpected))]
        public void ShouldConvertToPermissionsList(string permissionsString, List<string> permissionsList)
        {
            // given
            string inputPermissionsString = permissionsString;
            List<string> expectedPermissionsList = permissionsList;

            // when
            List<string> actualPermissionsList = this.storageService.ConvertToPermissionsList(inputPermissionsString);

            // then
            actualPermissionsList.Should().BeEquivalentTo(expectedPermissionsList);
        }
    }
}
