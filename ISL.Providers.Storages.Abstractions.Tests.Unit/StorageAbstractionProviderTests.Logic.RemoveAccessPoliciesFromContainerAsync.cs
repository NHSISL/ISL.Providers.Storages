using Moq;
using System.Threading.Tasks;

namespace ISL.Providers.Storage.Abstractions.Tests.Unit
{
    public partial class StorageAbstractionProviderTests
    {
        [Fact]
        public async Task ShouldRemoveAccessPoliciesAsync()
        {
            // given
            string randomString = GetRandomString();
            string inputContainer = randomString;

            // when
            await this.storageAbstractionProvider
                .RemoveAccessPoliciesAsync(inputContainer);

            // then
            this.storageProviderMock.Verify(provider =>
                provider.RemoveAccessPoliciesAsync(inputContainer),
                    Times.Once);

            this.storageProviderMock.VerifyNoOtherCalls();
        }
    }
}
