using Moq;
using System.Threading.Tasks;

namespace ISL.Providers.Storage.Abstractions.Tests.Unit
{
    public partial class StorageAbstractionProviderTests
    {
        [Fact]
        public async Task ShouldRemoveAllAccessPoliciesAsync()
        {
            // given
            string randomString = GetRandomString();
            string inputContainer = randomString;

            // when
            await this.storageAbstractionProvider
                .RemoveAllAccessPoliciesAsync(inputContainer);

            // then
            this.storageProviderMock.Verify(provider =>
                provider.RemoveAllAccessPoliciesAsync(inputContainer),
                    Times.Once);

            this.storageProviderMock.VerifyNoOtherCalls();
        }
    }
}
