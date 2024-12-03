using FluentAssertions;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ISL.Providers.Storages.AzureBlobStorage.Tests.Acceptance
{
    public partial class AzureBlobStorageProviderTests
    {
        [Fact]
        public async Task ShouldRetrieveAllAccessPoliciesFromContainerAsync()
        {
            // given
            string randomContainer = GetRandomString();
            string inputContainer = randomContainer.ToLower();

            List<string> inputAccessPolicyNames = new List<string>
            {
                "read",
                "write",
            };

            await this.azureBlobStorageProvider.CreateContainerAsync(inputContainer);

            await this.azureBlobStorageProvider.CreateAndAssignAccessPoliciesToContainerAsync(
                inputContainer, inputAccessPolicyNames);

            // when
            List<string> actualAccessPolicyNames =
                await this.azureBlobStorageProvider.RetrieveAllAccessPoliciesFromContainerAsync(
                    inputContainer);

            // then
            actualAccessPolicyNames.Count.Should().Be(inputAccessPolicyNames.Count);

            foreach (string policyName in actualAccessPolicyNames)
            {
                string[] policyNameParts = policyName.Split('_');
                policyNameParts[0].Should().BeOneOf(inputAccessPolicyNames);
            }

            await this.azureBlobStorageProvider.DeleteContainerAsync(inputContainer);
        }
    }
}
