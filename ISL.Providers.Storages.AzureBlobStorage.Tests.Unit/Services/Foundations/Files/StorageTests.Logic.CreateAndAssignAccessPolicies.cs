using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ISL.Providers.Storages.AzureBlobStorage.Tests.Unit.Services.Foundations.Files
{
    public partial class StorageTests
    {
        [Fact]
        public async Task ShouldCreateAndAssignAccessPoliciesAsync()
        {
            // given
            string randomString = GetRandomString();
            string inputContainer = randomString;
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            DateTimeOffset inputDateTimeOffset = randomDateTimeOffset;

            List<string> inputPolicyNames = new List<string>
            {
                "read",
                "write",
                "delete",
                "fullaccess"
            };

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffsetAsync())
                    .ReturnsAsync(randomDateTimeOffset);

            // when
            await this.storageService
                .CreateAndAssignAccessPoliciesToContainerAsync(inputContainer, inputPolicyNames);

            // then
            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffsetAsync(),
                    Times.Once);

            this.blobStorageBrokerMock.Verify(broker =>
                broker.CreateAndAssignAccessPoliciesToContainerAsync(
                    inputContainer, inputPolicyNames, inputDateTimeOffset),
                        Times.Once());

            this.blobStorageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
