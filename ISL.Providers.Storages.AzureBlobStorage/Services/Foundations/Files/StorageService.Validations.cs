// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using ISL.Providers.Storages.AzureBlobStorage.Models.Foundations.Files.Exceptions;
using System;
using System.IO;

namespace ISL.Providers.Storages.AzureBlobStorage.Services.Foundations.Storages
{
    internal partial class StorageService : IStorageService
    {
        private static void ValidateStorageArgumentsOnCreate(Stream inputStream, string fileName, string container)
        {
            Validate(
                (Rule: IsInvalid(fileName), Parameter: "FileName"),
                (Rule: IsInvalid(container), Parameter: "Container"),
                (Rule: IsInvalidInputStream(inputStream), Parameter: "Input"));
        }

        private static void ValidateStorageArgumentsOnRetrieve(Stream outputStream, string fileName, string container)
        {
            Validate(
                (Rule: IsInvalid(fileName), Parameter: "FileName"),
                (Rule: IsInvalid(container), Parameter: "Container"),
                (Rule: IsInvalidOutputStream(outputStream), Parameter: "Output"));
        }

        private static void ValidateStorageArgumentsOnDelete(string fileName, string container)
        {
            Validate(
                (Rule: IsInvalid(fileName), Parameter: "FileName"),
                (Rule: IsInvalid(container), Parameter: "Container"));
        }

        private static void ValidateStorageArgumentsOnList(string container)
        {
            Validate(
                (Rule: IsInvalid(container), Parameter: "Container"));
        }

        private static void ValidateStorageArgumentsOnGetDownloadLink(
            string fileName, string container, DateTimeOffset expiresOn)
        {
            Validate(
                (Rule: IsInvalid(fileName), Parameter: "FileName"),
                (Rule: IsInvalid(container), Parameter: "Container"),
                (Rule: IsInvalid(expiresOn), Parameter: "ExpiresOn"));
        }

        private static dynamic IsInvalid(string text) => new
        {
            Condition = String.IsNullOrWhiteSpace(text),
            Message = "Text is invalid"
        };

        private static dynamic IsInvalid(DateTimeOffset dateTimeOffset) => new
        {
            Condition = dateTimeOffset == default || dateTimeOffset <= DateTimeOffset.UtcNow,
            Message = "Date is invalid"
        };

        private static dynamic IsInvalidInputStream(Stream inputStream) => new
        {
            Condition = inputStream is null || inputStream.Length == 0,
            Message = "Stream is invalid"
        };

        private static dynamic IsInvalidOutputStream(Stream outputStream) => new
        {
            Condition = outputStream is null || outputStream.Length > 0,
            Message = "Stream is invalid"
        };

        private static void Validate(params (dynamic Rule, string Parameter)[] validations)
        {
            var invalidArgumentStorageException =
                new InvalidArgumentStorageException(
                    message: "Invalid storage service argument(s), please fix the errors and try again.");

            foreach ((dynamic rule, string parameter) in validations)
            {
                if (rule.Condition)
                {
                    invalidArgumentStorageException.UpsertDataList(
                        key: parameter,
                        value: rule.Message);
                }
            }

            invalidArgumentStorageException.ThrowIfContainsErrors();
        }
    }
}
