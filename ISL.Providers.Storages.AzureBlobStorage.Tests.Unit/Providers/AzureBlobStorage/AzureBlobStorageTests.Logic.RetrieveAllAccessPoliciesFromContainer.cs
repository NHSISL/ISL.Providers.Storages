using Moq;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace ISL.Providers.Storages.AzureBlobStorage.Tests.Unit.Providers.AzureBlobStorage
{
    public partial class AzureBlobStorageTests
    {
        [Fact]
        public async Task ShouldRetrieveAllAccessPoliciesFromContainerAsync()
        {
            // given
            string randomContainer = GetRandomString();
            string inputContainer = randomContainer;

            // when
            await this.azureBlobStorageProvider.RetrieveAllAccessPoliciesFromContainerAsync(
                inputContainer);

            // then
            this.storageServiceMock.Verify(service =>
                service.RetrieveAllAccessPoliciesFromContainerAsync(inputContainer),
                    Times.Once);

            this.storageServiceMock.VerifyNoOtherCalls();
        }
    }
}
