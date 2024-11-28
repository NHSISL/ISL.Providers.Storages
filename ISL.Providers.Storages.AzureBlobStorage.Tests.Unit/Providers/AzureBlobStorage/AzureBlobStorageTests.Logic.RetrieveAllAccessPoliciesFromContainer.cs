using FluentAssertions;
using Moq;
using System.Collections.Generic;
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
            List<string> randomStringList = GetRandomStringList();
            string inputContainer = randomContainer;
            List<string> outputAccessPolicies = randomStringList;
            List<string> expectedAccessPolicies = outputAccessPolicies;

            this.storageServiceMock.Setup(service =>
                service.RetrieveAllAccessPoliciesFromContainerAsync(inputContainer))
                    .ReturnsAsync(outputAccessPolicies);

            // when
            List<string> actualAccessPolicies = await this.azureBlobStorageProvider
                .RetrieveAllAccessPoliciesFromContainerAsync(inputContainer);

            // then
            actualAccessPolicies.Should().BeEquivalentTo(expectedAccessPolicies);

            this.storageServiceMock.Verify(service =>
                service.RetrieveAllAccessPoliciesFromContainerAsync(inputContainer),
                    Times.Once);

            this.storageServiceMock.VerifyNoOtherCalls();
        }
    }
}
