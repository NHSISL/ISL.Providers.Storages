// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using Azure;
using Azure.Identity;
using ISL.Providers.Storages.AzureBlobStorage.Models.Foundations.Files.Exceptions;
using Microsoft.WindowsAzure.Storage;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Xeptions;

namespace ISL.Providers.Storages.AzureBlobStorage.Services.Foundations.Storages
{
    internal partial class StorageService : IStorageService
    {
        private delegate ValueTask ReturningNothingFunction();
        private delegate ValueTask<List<string>> ReturningListFunction();

        private async ValueTask TryCatch(ReturningNothingFunction returningNothingFunction)
        {
            try
            {
                await returningNothingFunction();
            }
            catch (InvalidArgumentStorageException invalidArgumentStorageException)
            {
                throw CreateValidationException(invalidArgumentStorageException);
            }
            catch (ArgumentException argumentException)
            {
                var failedStorageDependencyValidationException =
                    new FailedStorageDependencyValidationException(
                        message: "Failed storage dependency validation error occurred, please contact support.",
                        innerException: argumentException);

                throw CreateDependencyValidationException(failedStorageDependencyValidationException);
            }
            catch (AuthenticationFailedException authenticationFailedException)
            {
                var failedStorageDependencyValidationException =
                    new FailedStorageDependencyValidationException(
                        message: "Failed storage dependency validation error occurred, please contact support.",
                        innerException: authenticationFailedException);

                throw CreateDependencyValidationException(failedStorageDependencyValidationException);
            }
            catch (RequestFailedException requestFailedException)
            {
                var failedStorageDependencyException =
                    new FailedStorageDependencyException(
                        message: "Failed storage dependency error occurred, please contact support.",
                        innerException: requestFailedException);

                throw CreateDependencyException(failedStorageDependencyException);
            }
            catch (StorageException storageException)
            {
                var failedStorageDependencyException =
                    new FailedStorageDependencyException(
                        message: "Failed storage dependency error occurred, please contact support.",
                        innerException: storageException);

                throw CreateDependencyException(failedStorageDependencyException);
            }
            catch (OperationCanceledException operationCanceledException)
            {
                var failedStorageDependencyException =
                    new FailedStorageDependencyException(
                        message: "Failed storage dependency error occurred, please contact support.",
                        innerException: operationCanceledException);

                throw CreateDependencyException(failedStorageDependencyException);
            }
            catch (TimeoutException timeoutException)
            {
                var failedStorageDependencyException =
                    new FailedStorageDependencyException(
                        message: "Failed storage dependency error occurred, please contact support.",
                        innerException: timeoutException);

                throw CreateDependencyException(failedStorageDependencyException);
            }
            catch (IOException iOException)
            {
                var failedStorageDependencyException =
                    new FailedStorageDependencyException(
                        message: "Failed storage dependency error occurred, please contact support.",
                        innerException: iOException);

                throw CreateDependencyException(failedStorageDependencyException);
            }
            catch (Exception exception)
            {
                var failedStorageServiceException =
                    new FailedStorageServiceException(
                        message: "Failed storage service error occurred, please contact support.",
                        innerException: exception);

                throw CreateServiceException(failedStorageServiceException);
            }
        }

        private async ValueTask<List<string>> TryCatch(ReturningListFunction returningListFunction)
        {
            try
            {
                return await returningListFunction();
            }
            catch (InvalidArgumentStorageException invalidArgumentStorageException)
            {
                throw CreateValidationException(invalidArgumentStorageException);
            }
            catch (ArgumentException argumentException)
            {
                var failedStorageDependencyValidationException =
                    new FailedStorageDependencyValidationException(
                        message: "Failed storage dependency validation error occurred, please contact support.",
                        innerException: argumentException);

                throw CreateDependencyValidationException(failedStorageDependencyValidationException);
            }
            catch (AuthenticationFailedException authenticationFailedException)
            {
                var failedStorageDependencyValidationException =
                    new FailedStorageDependencyValidationException(
                        message: "Failed storage dependency validation error occurred, please contact support.",
                        innerException: authenticationFailedException);

                throw CreateDependencyValidationException(failedStorageDependencyValidationException);
            }
            catch (RequestFailedException requestFailedException)
            {
                var failedStorageDependencyException =
                    new FailedStorageDependencyException(
                        message: "Failed storage dependency error occurred, please contact support.",
                        innerException: requestFailedException);

                throw CreateDependencyException(failedStorageDependencyException);
            }
            catch (StorageException storageException)
            {
                var failedStorageDependencyException =
                    new FailedStorageDependencyException(
                        message: "Failed storage dependency error occurred, please contact support.",
                        innerException: storageException);

                throw CreateDependencyException(failedStorageDependencyException);
            }
            catch (OperationCanceledException operationCanceledException)
            {
                var failedStorageDependencyException =
                    new FailedStorageDependencyException(
                        message: "Failed storage dependency error occurred, please contact support.",
                        innerException: operationCanceledException);

                throw CreateDependencyException(failedStorageDependencyException);
            }
            catch (TimeoutException timeoutException)
            {
                var failedStorageDependencyException =
                    new FailedStorageDependencyException(
                        message: "Failed storage dependency error occurred, please contact support.",
                        innerException: timeoutException);

                throw CreateDependencyException(failedStorageDependencyException);
            }
            catch (IOException iOException)
            {
                var failedStorageDependencyException =
                    new FailedStorageDependencyException(
                        message: "Failed storage dependency error occurred, please contact support.",
                        innerException: iOException);

                throw CreateDependencyException(failedStorageDependencyException);
            }
        }

        private static StorageValidationException CreateValidationException(
            Xeption exception)
        {
            var storageValidationException = new StorageValidationException(
                message: "Storage validation error occurred, please fix errors and try again.",
                innerException: exception);

            return storageValidationException;
        }

        private static StorageDependencyValidationException CreateDependencyValidationException(
            Xeption exception)
        {
            var storageDependencyValidationException = new StorageDependencyValidationException(
                message: "Storage dependency validation error occurred, please fix errors and try again.",
                innerException: exception);

            return storageDependencyValidationException;
        }

        private static StorageDependencyException CreateDependencyException(
            Xeption exception)
        {
            var storageDependencyException = new StorageDependencyException(
                message: "Storage dependency error occurred, please fix errors and try again.",
                innerException: exception);

            return storageDependencyException;
        }

        private static StorageServiceException CreateServiceException(
            Xeption exception)
        {
            var storageServiceException = new StorageServiceException(
                message: "Storage service error occurred, please fix errors and try again.",
                innerException: exception);

            return storageServiceException;
        }
    }
}
