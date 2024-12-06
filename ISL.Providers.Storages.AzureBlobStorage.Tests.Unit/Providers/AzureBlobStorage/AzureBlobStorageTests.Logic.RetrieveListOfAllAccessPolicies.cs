using FluentAssertions;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ISL.Providers.Storages.AzureBlobStorage.Tests.Unit.Providers.AzureBlobStorage
{
    public partial class AzureBlobStorageTests
    {
        [Fact]
        public async Task ShouldRetrieveListOfAllAccessPoliciesAsync()
        {
            // given
            string randomContainer = GetRandomString();
            List<string> randomStringList = GetRandomStringList();
            string inputContainer = randomContainer;
            List<string> outputAccessPolicies = randomStringList;
            List<string> expectedAccessPolicies = outputAccessPolicies;

            this.storageServiceMock.Setup(service =>
                service.RetrieveListOfAllAccessPoliciesAsync(inputContainer))
                    .ReturnsAsync(outputAccessPolicies);

            // when
            List<string> actualAccessPolicies = await this.azureBlobStorageProvider
                .RetrieveListOfAllAccessPoliciesAsync(inputContainer);

            // then
            actualAccessPolicies.Should().BeEquivalentTo(expectedAccessPolicies);

            this.storageServiceMock.Verify(service =>
                service.RetrieveListOfAllAccessPoliciesAsync(inputContainer),
                    Times.Once);

            this.storageServiceMock.VerifyNoOtherCalls();
        }
    }
}
