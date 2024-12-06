using FluentAssertions;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ISL.Providers.Storage.Abstractions.Tests.Unit
{
    public partial class StorageAbstractionProviderTests
    {
        [Fact]
        public async Task ShouldRetrieveListOfAllAccessPoliciesAsync()
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
                provider.RetrieveListOfAllAccessPoliciesAsync(inputContainer))
                    .ReturnsAsync(outputPolicyNames);

            // when
            List<string> actualAccessPolicies = await this.storageAbstractionProvider
                .RetrieveListOfAllAccessPoliciesAsync(inputContainer);

            // then
            actualAccessPolicies.Should().BeEquivalentTo(expectedAccessPolicies);

            this.storageProviderMock.Verify(provider =>
                provider.RetrieveListOfAllAccessPoliciesAsync(inputContainer),
                    Times.Once);

            this.storageProviderMock.VerifyNoOtherCalls();
        }
    }
}
