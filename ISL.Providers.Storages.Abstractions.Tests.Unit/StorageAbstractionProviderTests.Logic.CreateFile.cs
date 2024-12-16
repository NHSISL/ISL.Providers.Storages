using Moq;
using System.IO;
using System.Threading.Tasks;

namespace ISL.Providers.Storage.Abstractions.Tests.Unit
{
    public partial class StorageAbstractionProviderTests
    {
        [Fact]
        public async Task ShouldCreateFileAsync()
        {
            // given
            string randomFileName = GetRandomString();
            string randomContainer = GetRandomString();
            Stream randomStream = new HasLengthStream();
            string inputFileName = randomFileName;
            string inputContainer = randomContainer;
            Stream inputStream = randomStream;

            // when
            await this.storageAbstractionProvider
                .CreateFileAsync(inputStream, inputFileName, inputContainer);

            // then
            this.storageProviderMock.Verify(service =>
                service.CreateFileAsync(inputStream, inputFileName, inputContainer),
                    Times.Once);

            this.storageProviderMock.VerifyNoOtherCalls();
        }
    }
}
