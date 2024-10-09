// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using Azure.Identity;
using ISL.Providers.Storages.AzureBlobStorage.Models.Foundations.Files.Exceptions;
using System;
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
                throw await CreateValidationExceptionAsync(invalidArgumentStorageException);
            }
            catch (ArgumentException argumentException)
            {
                throw await CreateDependencyValidationExceptionAsync(argumentException);
            }
            catch (AuthenticationFailedException authenticationFailedException)
            {
                throw await CreateDependencyValidationExceptionAsync(authenticationFailedException);
            }


        }

        private async ValueTask<StorageValidationException> CreateValidationExceptionAsync(
            Xeption exception)
        {
            var userAccessValidationException = new StorageValidationException(
                message: "Storage validation error occurred, please fix errors and try again.",
                innerException: exception);

            return userAccessValidationException;
        }

        private async ValueTask<StorageDependencyValidationException> CreateDependencyValidationExceptionAsync(
            Exception exception)
        {
            var storageDependencyValidationException = new StorageDependencyValidationException(
                message: "Storage dependency validation error occurred, please fix errors and try again.",
                innerException: exception);

            return storageDependencyValidationException;
        }
    }
}
