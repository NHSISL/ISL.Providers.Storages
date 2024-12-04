using ISL.Providers.Storages.Abstractions.Models;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ISL.Providers.Storage.Abstractions.Tests.Unit
{
    public partial class StorageAbstractionProviderTests
    {
        [Fact]
        public async Task ShouldCreateAndAssignAccessPoliciesAsync()
        {
            // given
            string randomString = GetRandomString();
            string inputContainer = randomString;
            List<Policy> inputPolicyNames = GetPolicies();

            // when
            await this.storageAbstractionProvider
                .CreateAndAssignAccessPoliciesAsync(inputContainer, inputPolicyNames);

            // then
            this.storageProviderMock.Verify(provider =>
                provider.CreateAndAssignAccessPoliciesAsync(inputContainer, inputPolicyNames),
                    Times.Once);

            this.storageProviderMock.VerifyNoOtherCalls();
        }
    }
}
