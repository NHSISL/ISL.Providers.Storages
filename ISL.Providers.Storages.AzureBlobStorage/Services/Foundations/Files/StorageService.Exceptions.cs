﻿// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using ISL.Providers.Storages.AzureBlobStorage.Models.Foundations.Files.Exceptions;
using System.Threading.Tasks;
using Xeptions;

namespace ISL.Providers.Storages.AzureBlobStorage.Services.Foundations.Storages
{
    internal partial class StorageService : IStorageService
    {
        private delegate ValueTask ReturningNothingFunction();

        private async ValueTask TryCatch(ReturningNothingFunction returningNothingFunction)
        {
            try
            {
                await returningNothingFunction();
            }
            catch (InvalidArgumentStorageException invalidArgumentStorageException)
            {
                throw await CreateAndLogValidationExceptionAsync(invalidArgumentStorageException);
            }
        }

        private async ValueTask<StorageValidationException> CreateAndLogValidationExceptionAsync(
            Xeption exception)
        {
            var userAccessValidationException = new StorageValidationException(
                message: "Storage validation error occurred, please fix errors and try again.",
                innerException: exception);

            //await this.loggingBroker.LogErrorAsync(userAccessValidationException);

            return userAccessValidationException;
        }
    }
}
