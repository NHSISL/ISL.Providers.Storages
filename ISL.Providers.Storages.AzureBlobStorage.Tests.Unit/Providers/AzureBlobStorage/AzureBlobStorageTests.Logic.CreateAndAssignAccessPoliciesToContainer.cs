using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ISL.Providers.Storages.AzureBlobStorage.Tests.Unit.Providers.AzureBlobStorage
{
    public partial class AzureBlobStorageTests
    {
        [Fact]
        public async Task ShouldCreateAndAssignAccessPoliciesToContainerAsync()
        {
            // given
            string randomContainer = GetRandomString();
            List<string> randomPolicyNames = GetRandomStringList();
            string inputContainer = randomContainer;
            List<string> inputPolicyNames = randomPolicyNames;

            // when
            await this.azureBlobStorageProvider.CreateAndAssignAccessPoliciesToContainerAsync(
                inputContainer, inputPolicyNames);

            // then
            this.storageServiceMock.Verify(service =>
                service.CreateAndAssignAccessPoliciesToContainerAsync(inputContainer, inputPolicyNames),
                    Times.Once);

            this.storageServiceMock.VerifyNoOtherCalls();
        }
    }
}
