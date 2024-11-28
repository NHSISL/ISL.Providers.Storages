using FluentAssertions;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ISL.Providers.Storage.Abstractions.Tests.Unit
{
    public partial class StorageAbstractionProviderTests
    {
        [Fact]
        public async Task ShouldRetrieveAllAccessPoliciesFromContainerAsync()
        {
            // given
            string randomString = GetRandomString();
            string inputContainer = randomString;

            List<string> outputPolicyNames = new List<string>
            {
                "read",
                "write",
                "delete",
                "fullaccess"
            };

            List<string> expectedAccessPolicies = outputPolicyNames;

            this.storageProviderMock.Setup(provider =>
                provider.RetrieveAllAccessPoliciesFromContainerAsync(inputContainer))
                    .ReturnsAsync(outputPolicyNames);

            // when
            List<string> actualAccessPolicies = await this.storageAbstractionProvider
                .RetrieveAllAccessPoliciesFromContainerAsync(inputContainer);

            // then
            actualAccessPolicies.Should().BeEquivalentTo(expectedAccessPolicies);

            this.storageProviderMock.Verify(provider =>
                provider.RetrieveAllAccessPoliciesFromContainerAsync(inputContainer),
                    Times.Once);

            this.storageProviderMock.VerifyNoOtherCalls();
        }
    }
}
