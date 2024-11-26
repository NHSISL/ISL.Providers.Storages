using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ISL.Providers.Storage.Abstractions.Tests.Unit
{
    public partial class StorageAbstractionProviderTests
    {
        [Fact]
        public async Task ShouldCreateAndAssignAccessPoliciesToContainerAsync()
        {
            // given
            string randomString = GetRandomString();
            string inputContainer = randomString;

            List<string> inputPolicyNames = new List<string>
            {
                "read",
                "write",
                "delete",
                "fullaccess"
            };

            // when
            await this.storageAbstractionProvider
                .CreateAndAssignAccessPoliciesToContainerAsync(inputPolicyNames, inputContainer);

            // then
            this.storageProviderMock.Verify(provider =>
                provider.CreateAndAssignAccessPoliciesToContainerAsync(inputPolicyNames, inputContainer),
                    Times.Once);

            this.storageProviderMock.VerifyNoOtherCalls();
        }
    }
}
