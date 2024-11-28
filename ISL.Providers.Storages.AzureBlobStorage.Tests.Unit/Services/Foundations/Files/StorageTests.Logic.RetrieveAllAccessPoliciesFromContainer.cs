using FluentAssertions;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ISL.Providers.Storages.AzureBlobStorage.Tests.Unit.Services.Foundations.Files
{
    public partial class StorageTests
    {
        [Fact]
        public async Task ShouldRetrieveAllAccessPoliciesFromContainerAsync()
        {
            // given
            string randomString = GetRandomString();
            List<string> randomStringList = GetRandomStringList();
            string inputContainer = randomString;
            List<string> outputAccessPolicies = randomStringList;
            List<string> expectedAccessPolicies = outputAccessPolicies;

            this.blobStorageBrokerMock.Setup(broker =>
                broker.RetrieveAllAccessPoliciesFromContainerAsync(inputContainer))
                    .ReturnsAsync(outputAccessPolicies);

            // when
            List<string> actualAccessPolicies = await this.storageService
                .RetrieveAllAccessPoliciesFromContainerAsync(inputContainer);

            // then
            actualAccessPolicies.Should().BeEquivalentTo(expectedAccessPolicies);

            this.blobStorageBrokerMock.Verify(broker =>
                broker.RetrieveAllAccessPoliciesFromContainerAsync(inputContainer),
                    Times.Once);

            this.blobStorageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
