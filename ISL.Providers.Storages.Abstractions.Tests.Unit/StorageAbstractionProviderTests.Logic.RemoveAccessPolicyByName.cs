using Moq;
using System.Threading.Tasks;

namespace ISL.Providers.Storage.Abstractions.Tests.Unit
{
    public partial class StorageAbstractionProviderTests
    {
        [Fact]
        public async Task ShouldRemoveAccessPolicyByNameAsync()
        {
            // given
            string randomContainer = GetRandomString();
            string randomPolicyName = GetRandomString();
            string inputContainer = randomContainer;
            string inputPolicyName = randomPolicyName;

            // when
            await this.storageAbstractionProvider
                .RemoveAccessPolicyByNameAsync(inputContainer, inputPolicyName);

            // then
            this.storageProviderMock.Verify(provider =>
                provider.RemoveAccessPolicyByNameAsync(inputContainer, inputPolicyName),
                    Times.Once);

            this.storageProviderMock.VerifyNoOtherCalls();
        }
    }
}
