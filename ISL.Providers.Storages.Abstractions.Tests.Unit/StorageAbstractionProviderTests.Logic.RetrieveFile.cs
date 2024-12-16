using Moq;
using System.IO;
using System.Threading.Tasks;

namespace ISL.Providers.Storage.Abstractions.Tests.Unit
{
    public partial class StorageAbstractionProviderTests
    {
        [Fact]
        public async Task ShouldRetrieveFileAsync()
        {
            // given
            string randomFileName = GetRandomString();
            string randomContainer = GetRandomString();
            Stream randomStream = new ZeroLengthStream();
            string inputFileName = randomFileName;
            string inputContainer = randomContainer;
            Stream outputStream = randomStream;

            // when
            await this.storageAbstractionProvider
                .RetrieveFileAsync(outputStream, inputFileName, inputContainer);

            // then
            this.storageProviderMock.Verify(service =>
                service.RetrieveFileAsync(outputStream, inputFileName, inputContainer),
                    Times.Once);

            this.storageProviderMock.VerifyNoOtherCalls();
        }
    }
}
