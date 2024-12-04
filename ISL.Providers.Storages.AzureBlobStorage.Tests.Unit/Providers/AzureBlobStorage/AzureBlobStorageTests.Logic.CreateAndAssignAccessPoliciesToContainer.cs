using ISL.Providers.Storages.Abstractions.Models;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ISL.Providers.Storages.AzureBlobStorage.Tests.Unit.Providers.AzureBlobStorage
{
    public partial class AzureBlobStorageTests
    {
        [Fact]
        public async Task ShouldCreateAndAssignAccessPoliciesAsync()
        {
            // given
            string randomContainer = GetRandomString();
            string inputContainer = randomContainer;
            List<Policy> inputPolicies = GetPolicies();

            // when
            await this.azureBlobStorageProvider.CreateAndAssignAccessPoliciesAsync(
                inputContainer, inputPolicies);

            // then
            this.storageServiceMock.Verify(service =>
                service.CreateAndAssignAccessPoliciesAsync(inputContainer, inputPolicies),
                    Times.Once);

            this.storageServiceMock.VerifyNoOtherCalls();
        }
    }
}
