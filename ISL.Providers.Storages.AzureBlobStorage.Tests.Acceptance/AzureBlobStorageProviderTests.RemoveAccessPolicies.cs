using FluentAssertions;
using ISL.Providers.Storages.Abstractions.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ISL.Providers.Storages.AzureBlobStorage.Tests.Acceptance
{
    public partial class AzureBlobStorageProviderTests
    {
        [Fact]
        public async Task ShouldRemoveAccessPoliciesAsync()
        {
            // given
            string randomContainer = GetRandomString();
            string inputContainer = randomContainer.ToLower();

            List<Policy> inputAccessPolicies = new List<Policy>
            {
                new Policy
                {
                    PolicyName = "read",
                    Permissions = new List<string>
                    {
                        "Read",
                        "list"
                    }
                },
                new Policy
                {
                    PolicyName = "write",
                    Permissions = new List<string>
                    {
                        "write",
                        "add",
                        "Create"
                    }
                },
            };

            await this.azureBlobStorageProvider.CreateContainerAsync(inputContainer);

            await this.azureBlobStorageProvider.CreateAndAssignAccessPoliciesAsync(
                inputContainer, inputAccessPolicies);

            // when
            await this.azureBlobStorageProvider.RemoveAccessPoliciesAsync(inputContainer);

            // then
            List<string> actualAccessPolicyNames =
                await this.azureBlobStorageProvider.RetrieveListOfAllAccessPoliciesAsync(
                    inputContainer);

            actualAccessPolicyNames.Should().BeEmpty();
            await this.azureBlobStorageProvider.DeleteContainerAsync(inputContainer);
        }
    }
}
