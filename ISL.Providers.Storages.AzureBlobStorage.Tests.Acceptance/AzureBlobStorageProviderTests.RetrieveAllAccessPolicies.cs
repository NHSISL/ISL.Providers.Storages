using FluentAssertions;
using ISL.Providers.Storages.Abstractions.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ISL.Providers.Storages.AzureBlobStorage.Tests.Acceptance
{
    public partial class AzureBlobStorageProviderTests
    {
        [Fact]
        public async Task ShouldRetrieveAllAccessPoliciesAsync()
        {
            // given
            string randomContainer = GetRandomString();
            string randomPolicyName = GetRandomString();
            string inputContainer = randomContainer.ToLower();
            string inputPolicyName = randomPolicyName;
            List<Policy> inputPolicyList = GetPolicies();
            List<Policy> expectedPolicyList = inputPolicyList;
            await this.azureBlobStorageProvider.CreateContainerAsync(inputContainer);

            await this.azureBlobStorageProvider
                .CreateAndAssignAccessPoliciesAsync(inputContainer, inputPolicyList);

            // when
            List<Policy> actualPolicyList = await this.azureBlobStorageProvider
                .RetrieveAllAccessPoliciesAsync(inputContainer);

            // then
            actualPolicyList.Should().BeEquivalentTo(expectedPolicyList);
            await this.azureBlobStorageProvider.DeleteContainerAsync(inputContainer);
        }
    }
}
