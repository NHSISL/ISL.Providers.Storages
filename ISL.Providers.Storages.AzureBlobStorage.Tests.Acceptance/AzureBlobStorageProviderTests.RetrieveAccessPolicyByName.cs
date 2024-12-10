using FluentAssertions;
using ISL.Providers.Storages.Abstractions.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ISL.Providers.Storages.AzureBlobStorage.Tests.Acceptance
{
    public partial class AzureBlobStorageProviderTests
    {
        [Fact]
        public async Task ShouldRetrieveAccessPolicyByNameAsync()
        {
            // given
            string randomContainer = GetRandomString();
            string randomPolicyName = GetRandomString();
            string inputContainer = randomContainer.ToLower();
            string inputPolicyName = randomPolicyName;


            List<Policy> inputAccessPolicyList = new List<Policy>
            {
                new Policy
                {
                    PolicyName = inputPolicyName,
                    Permissions = new List<string>
                    {
                        "read",
                        "list"
                    }
                }
            };

            Policy expectedPolicy = inputAccessPolicyList[0];

            await this.azureBlobStorageProvider.CreateContainerAsync(inputContainer);

            await this.azureBlobStorageProvider.CreateAndAssignAccessPoliciesAsync(
                inputContainer, inputAccessPolicyList);

            // when
            Policy actualPolicy = await this.azureBlobStorageProvider
                .RetrieveAccessPolicyByNameAsync(inputContainer, inputPolicyName);

            // then
            actualPolicy.Should().BeEquivalentTo(expectedPolicy);
            await this.azureBlobStorageProvider.DeleteContainerAsync(inputContainer);
        }
    }
}
