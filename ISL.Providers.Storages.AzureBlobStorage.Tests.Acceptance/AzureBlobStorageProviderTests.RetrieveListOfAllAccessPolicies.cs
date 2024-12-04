using FluentAssertions;
using ISL.Providers.Storages.Abstractions.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ISL.Providers.Storages.AzureBlobStorage.Tests.Acceptance
{
    public partial class AzureBlobStorageProviderTests
    {
        [Fact]
        public async Task ShouldRetrieveListOfAllAccessPoliciesAsync()
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
            List<string> actualAccessPolicyNames =
                await this.azureBlobStorageProvider.RetrieveListOfAllAccessPoliciesAsync(
                    inputContainer);

            // then
            actualAccessPolicyNames.Count.Should().Be(inputAccessPolicies.Count);

            foreach (string policyName in actualAccessPolicyNames)
            {
                policyName.Should().BeOneOf(inputAccessPolicies.Select(policy => policy.PolicyName));
            }

            await this.azureBlobStorageProvider.DeleteContainerAsync(inputContainer);
        }
    }
}
