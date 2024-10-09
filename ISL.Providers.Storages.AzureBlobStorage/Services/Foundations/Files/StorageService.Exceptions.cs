// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using Azure;
using Azure.Identity;
using ISL.Providers.Storages.AzureBlobStorage.Models.Foundations.Files.Exceptions;
using Microsoft.WindowsAzure.Storage;
using System;
using System.IO;
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
            catch (RequestFailedException requestFailedException)
            {
                throw await CreateDependencyExceptionAsync(requestFailedException);
            }
            catch (StorageException storageException)
            {
                throw await CreateDependencyExceptionAsync(storageException);
            }
            catch (OperationCanceledException operationCanceledException)
            {
                throw await CreateDependencyExceptionAsync(operationCanceledException);
            }
            catch (TimeoutException timeoutException)
            {
                throw await CreateDependencyExceptionAsync(timeoutException);
            }
            catch (IOException iOException)
            {
                throw await CreateDependencyExceptionAsync(iOException);
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

        private async ValueTask<StorageDependencyException> CreateDependencyExceptionAsync(
            Exception exception)
        {
            var storageDependencyException = new StorageDependencyException(
                message: "Storage dependency error occurred, please fix errors and try again.",
                innerException: exception);

            return storageDependencyException;
        }
    }
}
