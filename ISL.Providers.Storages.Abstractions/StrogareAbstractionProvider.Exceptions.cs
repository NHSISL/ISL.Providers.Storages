// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using ISL.Providers.Storages.Abstractions.Models.Exceptions;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xeptions;

namespace ISL.Providers.Storages.Abstractions
{
    public partial class StorageAbstractionProvider
    {
        private delegate ValueTask ReturningNothingFunction();
        private delegate ValueTask<string> ReturningStringFunction();
        private delegate ValueTask<List<string>> ReturningStringListFunction();

        private async ValueTask TryCatch(
            ReturningNothingFunction returningNothingFunction)
        {
            try
            {
                await returningNothingFunction();
            }
            catch (Xeption ex) when (ex is IStorageProviderValidationException)
            {
                throw CreateValidationException(ex);
            }
            catch (Xeption ex) when (ex is IStorageProviderDependencyValidationException)
            {
                throw CreateDependencyValidationException(ex);
            }
            catch (Xeption ex) when (ex is IStorageProviderDependencyException)
            {
                throw CreateDependencyException(ex);
            }
            catch (Xeption ex) when (ex is IStorageProviderServiceException)
            {
                throw CreateServiceException(ex);
            }
            catch (Exception ex)
            {
                var uncatagorizedStroageProviderException =
                    new UncatagorizedStorageProviderException(
                        message: "Storage provider not properly implemented. Uncatagorized errors found, " +
                            "contact the storage provider owner for support.",
                        innerException: ex,
                        data: ex.Data);

                throw CreateUncatagorizedServiceException(uncatagorizedStroageProviderException);
            }
        }

        private async ValueTask<string> TryCatch(ReturningStringFunction returningStringFunction)
        {
            try
            {
                return await returningStringFunction();
            }
            catch (Xeption ex) when (ex is IStorageProviderValidationException)
            {
                throw CreateValidationException(ex);
            }
            catch (Xeption ex) when (ex is IStorageProviderDependencyValidationException)
            {
                throw CreateDependencyValidationException(ex);
            }
            catch (Xeption ex) when (ex is IStorageProviderDependencyException)
            {
                throw CreateDependencyException(ex);
            }
            catch (Xeption ex) when (ex is IStorageProviderServiceException)
            {
                throw CreateServiceException(ex);
            }
            catch (Exception ex)
            {
                var uncatagorizedStroageProviderException =
                    new UncatagorizedStorageProviderException(
                        message: "Storage provider not properly implemented. Uncatagorized errors found, " +
                            "contact the storage provider owner for support.",
                        innerException: ex,
                        data: ex.Data);

                throw CreateUncatagorizedServiceException(uncatagorizedStroageProviderException);
            }
        }

        private async ValueTask<List<string>> TryCatch(ReturningStringListFunction returningStringListFunction)
        {
            try
            {
                return await returningStringListFunction();
            }
            catch (Xeption ex) when (ex is IStorageProviderValidationException)
            {
                throw CreateValidationException(ex);
            }
            catch (Xeption ex) when (ex is IStorageProviderDependencyValidationException)
            {
                throw CreateDependencyValidationException(ex);
            }
            catch (Xeption ex) when (ex is IStorageProviderDependencyException)
            {
                throw CreateDependencyException(ex);
            }
            catch (Xeption ex) when (ex is IStorageProviderServiceException)
            {
                throw CreateServiceException(ex);
            }
            catch (Exception ex)
            {
                var uncatagorizedStroageProviderException =
                    new UncatagorizedStorageProviderException(
                        message: "Storage provider not properly implemented. Uncatagorized errors found, " +
                            "contact the storage provider owner for support.",
                        innerException: ex,
                        data: ex.Data);

                throw CreateUncatagorizedServiceException(uncatagorizedStroageProviderException);
            }
        }

        private StorageProviderValidationException CreateValidationException(
            Xeption exception)
        {
            var storageValidationProviderException =
                new StorageProviderValidationException(
                    message: "Storage provider validation errors occurred, please try again.",
                    innerException: exception,
                    data: exception.Data);

            return storageValidationProviderException;
        }

        private StorageProviderDependencyValidationException CreateDependencyValidationException(
            Xeption exception)
        {
            var storageDependencyValidationProviderException =
                new StorageProviderDependencyValidationException(
                    message: "Storage provider dependency validation errors occurred, please try again.",
                    innerException: exception,
                    data: exception.Data);

            return storageDependencyValidationProviderException;
        }

        private StorageProviderDependencyException CreateDependencyException(
            Xeption exception)
        {
            var storageDependencyProviderException = new StorageProviderDependencyException(
                message: "Storage provider dependency error occurred, contact support.",
                innerException: exception,
                data: exception.Data);

            return storageDependencyProviderException;
        }

        private StorageProviderServiceException CreateServiceException(
            Xeption exception)
        {
            var storageServiceProviderException = new StorageProviderServiceException(
                message: "Storage provider service error occurred, contact support.",
                innerException: exception,
                data: exception.Data);

            return storageServiceProviderException;
        }

        private StorageProviderServiceException CreateUncatagorizedServiceException(
            Exception exception)
        {
            var storageServiceProviderException = new StorageProviderServiceException(
                message: "Uncatagorized storage provider service error occurred, contact support.",
                innerException: exception as Xeption,
                data: exception.Data);

            return storageServiceProviderException;
        }
    }
}
