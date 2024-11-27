using Moq;
using System.Threading.Tasks;

namespace ISL.Providers.Storage.Abstractions.Tests.Unit
{
    public partial class StorageAbstractionProviderTests
    {
        [Fact]
        public async Task ShouldRemoveAccessPoliciesFromContainerAsync()
        {
            // given
            string randomString = GetRandomString();
            string inputContainer = randomString;

            // when
            await this.storageAbstractionProvider
                .RemoveAccessPoliciesFromContainerAsync(inputContainer);

            // then
            this.storageProviderMock.Verify(provider =>
                provider.RemoveAccessPoliciesFromContainerAsync(inputContainer),
                    Times.Once);

            this.storageProviderMock.VerifyNoOtherCalls();
        }
    }
}
