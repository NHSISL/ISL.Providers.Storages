using Moq;
using System.Threading.Tasks;

namespace ISL.Providers.Storage.Abstractions.Tests.Unit
{
    public partial class StorageAbstractionProviderTests
    {
        [Fact]
        public async Task ShouldDeleteFileAsync()
        {
            // given
            string randomFileName = GetRandomString();
            string randomContainer = GetRandomString();
            string inputFileName = randomFileName;
            string inputContainer = randomContainer;

            // when
            await this.storageAbstractionProvider
                .DeleteFileAsync(inputFileName, inputContainer);

            // then
            this.storageProviderMock.Verify(service =>
                service.DeleteFileAsync(inputFileName, inputContainer),
                    Times.Once);

            this.storageProviderMock.VerifyNoOtherCalls();
        }
    }
}
