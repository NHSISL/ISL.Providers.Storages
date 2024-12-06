using FluentAssertions;
using ISL.Providers.Storages.Abstractions.Models;
using Moq;
using System.Threading.Tasks;

namespace ISL.Providers.Storages.AzureBlobStorage.Tests.Unit.Providers.AzureBlobStorage
{
    public partial class AzureBlobStorageTests
    {
        [Fact]
        public async Task ShouldRetrieveAccessPolicyByNameAsync()
        {
            // given
            string randomContainer = GetRandomString();
            string randomPolicyName = GetRandomString();
            string inputContainer = randomContainer;
            string inputPolicyName = randomPolicyName;
            Policy outputPolicy = GetPolicy(inputPolicyName);
            Policy expectedPolicy = outputPolicy;

            this.storageServiceMock.Setup(service =>
                service.RetrieveAccessPolicyByNameAsync(inputContainer, inputPolicyName))
                    .ReturnsAsync(outputPolicy);

            // when
            Policy actualPolicy = await this.azureBlobStorageProvider
                .RetrieveAccessPolicyByName(inputContainer, inputPolicyName);

            // then
            actualPolicy.Should().BeEquivalentTo(expectedPolicy);

            this.storageServiceMock.Verify(service =>
                service.RetrieveAccessPolicyByNameAsync(inputContainer, inputPolicyName),
                    Times.Once);

            this.storageServiceMock.VerifyNoOtherCalls();
        }
    }
}
