using FluentAssertions;
using ISL.Providers.Storages.Abstractions.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ISL.Providers.Storages.AzureBlobStorage.Tests.Acceptance
{
    public partial class AzureBlobStorageProviderTests
    {
        [Fact]
        public async Task ShouldRemoveAccessPolicyByNameAsync()
        {
            // given
            string randomContainer = GetRandomString();
            string randomPolicyName = GetRandomString();
            string randomOtherPolicyName = GetRandomString();
            string inputContainer = randomContainer.ToLower();
            string inputPolicyName = randomPolicyName;
            string otherPolicyName = randomOtherPolicyName;

            List<Policy> inputAccessPolicies = new List<Policy>
            {
                new Policy
                {
                    PolicyName = inputPolicyName,
                    Permissions = new List<string>
                    {
                        "read",
                        "list"
                    }
                },
                new Policy
                {
                    PolicyName = otherPolicyName,
                    Permissions = new List<string>
                    {
                        "write",
                        "add",
                        "create"
                    }
                },
            };

            await this.azureBlobStorageProvider.CreateContainerAsync(inputContainer);

            await this.azureBlobStorageProvider
                .CreateAndAssignAccessPoliciesAsync(inputContainer, inputAccessPolicies);

            // when
            await this.azureBlobStorageProvider
                .RemoveAccessPolicyByNameAsync(inputContainer, inputPolicyName);

            // then
            List<string> actualAccessPolicyNames = await this.azureBlobStorageProvider
                .RetrieveListOfAllAccessPoliciesAsync(inputContainer);

            actualAccessPolicyNames.Should().NotContain(inputPolicyName);
            await this.azureBlobStorageProvider.DeleteContainerAsync(inputContainer);
        }
    }
}
