using FluentAssertions;
using ISL.Providers.Storages.Abstractions.Models;
using Moq;
using System.Threading.Tasks;

namespace ISL.Providers.Storage.Abstractions.Tests.Unit
{
    public partial class StorageAbstractionProviderTests
    {
        [Fact]
        public async Task ShouldRetrieveAccessPolicyByNameAsync()
        {
            // given
            string randomContainer = GetRandomString();
            string randomPolicyName = GetRandomString();
            string inputContainer = randomContainer;
            string inputPolicyName = randomPolicyName;
            Policy outputPolicy = GetPolicy(inputPolicyName);
            Policy expectedPolicy = outputPolicy;

            this.storageProviderMock.Setup(provider =>
                provider.RetrieveAccessPolicyByNameAsync(inputContainer, inputPolicyName))
                    .ReturnsAsync(outputPolicy);

            // when
            Policy actualAccessPolicy = await this.storageAbstractionProvider
                .RetrieveAccessPolicyByNameAsync(inputContainer, inputPolicyName);

            // then
            actualAccessPolicy.Should().BeEquivalentTo(expectedPolicy);

            this.storageProviderMock.Verify(provider =>
                provider.RetrieveAccessPolicyByNameAsync(inputContainer, inputPolicyName),
                    Times.Once);

            this.storageProviderMock.VerifyNoOtherCalls();
        }
    }
}
