using FluentAssertions;
using ISL.Providers.Storages.Abstractions.Models;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ISL.Providers.Storage.Abstractions.Tests.Unit
{
    public partial class StorageAbstractionProviderTests
    {
        [Fact]
        public async Task ShouldRetrieveAllAccessPoliciesAsync()
        {
            // given
            string randomString = GetRandomString();
            string inputContainer = randomString;
            List<Policy> outputPolicies = GetPolicies();
            List<Policy> expectedPolicies = outputPolicies;

            this.storageProviderMock.Setup(provider =>
                provider.RetrieveAllAccessPoliciesAsync(inputContainer))
                    .ReturnsAsync(outputPolicies);

            // when
            List<Policy> actualPolicies = await this.storageAbstractionProvider
                .RetrieveAllAccessPoliciesAsync(inputContainer);

            // then
            actualPolicies.Should().BeEquivalentTo(expectedPolicies);

            this.storageProviderMock.Verify(provider =>
                provider.RetrieveAllAccessPoliciesAsync(inputContainer),
                    Times.Once);

            this.storageProviderMock.VerifyNoOtherCalls();
        }
    }
}
