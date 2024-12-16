// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using FluentAssertions;
using ISL.Providers.Storages.AzureBlobStorage.Services.Foundations.Storages;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ISL.Providers.Storages.AzureBlobStorage.Tests.Unit.Services.Foundations.Storages
{
    public partial class StorageTests
    {
        [Theory]
        [MemberData(nameof(PathRelatedInputs))]
        public async Task ShouldCreateSasTokenWithAccessPolicyAsync(
            string path,
            bool isDirectory,
            string resource)
        {
            // given
            string randomContainer = GetRandomString();
            string randomAccessPolicyIdentifier = GetRandomString();
            DateTimeOffset randomDateTimeOffset = GetRandomFutureDateTimeOffset();
            string randomSasToken = GetRandomString();
            string inputContainer = randomContainer;
            string inputPath = path;
            string inputAccessPolicyIdentifier = randomAccessPolicyIdentifier;
            DateTimeOffset inputExpiresOn = randomDateTimeOffset;
            bool inputIsDirectory = isDirectory;
            string inputResource = resource;
            string outputSasToken = randomSasToken;
            string expectedSasToken = outputSasToken;

            this.blobStorageBrokerMock.Setup(broker =>
                broker.CreateSasTokenAsync(
                    inputContainer,
                    inputPath,
                    inputAccessPolicyIdentifier,
                    inputExpiresOn,
                    inputIsDirectory,
                    inputResource))
                        .ReturnsAsync(outputSasToken);

            // when
            var actualSasToken = await this.storageService.CreateSasTokenAsync(
                inputContainer,
                inputPath,
                inputAccessPolicyIdentifier,
                inputExpiresOn);

            // then
            actualSasToken.Should().BeEquivalentTo(expectedSasToken);

            this.blobStorageBrokerMock.Verify(broker =>
                broker.CreateSasTokenAsync(
                    inputContainer,
                    inputPath,
                    inputAccessPolicyIdentifier,
                    inputExpiresOn,
                    inputIsDirectory,
                    inputResource),
                        Times.Once);

            this.blobStorageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(PathRelatedInputs))]
        public async Task ShouldCreateSasTokenWithPermissionsListAsync(
            string path,
            bool isDirectory,
            string resource)
        {
            // given
            var storageServiceMock = new Mock<StorageService>(
                this.blobStorageBrokerMock.Object,
                this.dateTimeBrokerMock.Object)
            { CallBase = true };

            string randomContainer = GetRandomString();
            List<string> randomPermissionsList = GetRandomPermissionsList();
            DateTimeOffset randomDateTimeOffset = GetRandomFutureDateTimeOffset();
            string randomSasToken = GetRandomString();
            string inputContainer = randomContainer;
            string inputPath = path;
            List<string> inputPermissionsList = randomPermissionsList;
            DateTimeOffset inputExpiresOn = randomDateTimeOffset;
            bool inputIsDirectory = isDirectory;
            string inputResource = resource;
            string outputPermissionsString = GetRandomPermissionsString();
            string outputSasToken = randomSasToken;
            string expectedSasToken = outputSasToken;

            storageServiceMock.Setup(service =>
                service.ConvertToPermissionsString(inputPermissionsList))
                    .Returns(outputPermissionsString);

            this.blobStorageBrokerMock.Setup(broker =>
                broker.CreateSasTokenAsync(
                    inputContainer,
                    inputPath,
                    inputExpiresOn,
                    outputPermissionsString,
                    inputIsDirectory,
                    inputResource))
                        .ReturnsAsync(outputSasToken);

            StorageService storageService = storageServiceMock.Object;

            // when
            var actualSasToken = await storageService.CreateSasTokenAsync(
                inputContainer,
                inputPath,
                inputExpiresOn,
                inputPermissionsList);

            // then
            actualSasToken.Should().BeEquivalentTo(expectedSasToken);

            this.blobStorageBrokerMock.Verify(broker =>
                broker.CreateSasTokenAsync(
                    inputContainer,
                    inputPath,
                    inputExpiresOn,
                    outputPermissionsString,
                    inputIsDirectory,
                    inputResource),
                        Times.Once);

            this.blobStorageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
