using FluentAssertions;
using ISL.Providers.Storages.Abstractions.Models;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ISL.Providers.Storages.AzureBlobStorage.Tests.Unit.Providers.AzureBlobStorage
{
    public partial class AzureBlobStorageTests
    {
        [Fact]
        public async Task ShouldRetrieveAllAccessPoliciesAsync()
        {
            // given
            string randomContainer = GetRandomString();
            string randomPolicyName = GetRandomString();
            string inputContainer = randomContainer;
            string inputPolicyName = randomPolicyName;
            List<Policy> outputPolicies = GetPolicies();
            List<Policy> expectedPolicies = outputPolicies;

            this.storageServiceMock.Setup(service =>
                service.RetrieveAllAccessPoliciesAsync(inputContainer))
                    .ReturnsAsync(outputPolicies);

            // when
            List<Policy> actualPolicies = await this.azureBlobStorageProvider
                .RetrieveAllAccessPoliciesAsync(inputContainer);

            // then
            actualPolicies.Should().BeEquivalentTo(expectedPolicies);

            this.storageServiceMock.Verify(service =>
                service.RetrieveAllAccessPoliciesAsync(inputContainer),
                    Times.Once);

            this.storageServiceMock.VerifyNoOtherCalls();
        }
    }
}
