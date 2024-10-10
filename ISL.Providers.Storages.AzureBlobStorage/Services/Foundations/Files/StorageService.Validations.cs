// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using ISL.Providers.Storages.AzureBlobStorage.Models.Foundations.Files.Exceptions;
using System;
using System.IO;
using System.Threading.Tasks;

namespace ISL.Providers.Storages.AzureBlobStorage.Services.Foundations.Storages
{
    internal partial class StorageService : IStorageService
    {
        private async ValueTask ValidateStorageArgumentsOnCreateAsync(Stream inputStream, string fileName, string container)
        {
            Validate(
                (Rule: await IsInvalidAsync(fileName), Parameter: "FileName"),
                (Rule: await IsInvalidAsync(container), Parameter: "Container"),
                (Rule: await IsInvalidInputStreamAsync(inputStream), Parameter: "Input"));
        }

        private async ValueTask ValidateStorageArgumentsOnRetrieveAsync(Stream outputStream, string fileName, string container)
        {
            Validate(
                (Rule: await IsInvalidAsync(fileName), Parameter: "FileName"),
                (Rule: await IsInvalidAsync(container), Parameter: "Container"),
                (Rule: await IsInvalidOutputStreamAsync(outputStream), Parameter: "Output"));
        }

        private async ValueTask ValidateStorageArgumentsOnDeleteAsync(string fileName, string container)
        {
            Validate(
                (Rule: await IsInvalidAsync(fileName), Parameter: "FileName"),
                (Rule: await IsInvalidAsync(container), Parameter: "Container"));
        }

        private async ValueTask ValidateStorageArgumentsOnListAsync(string container)
        {
            Validate(
                (Rule: await IsInvalidAsync(container), Parameter: "Container"));
        }

        private static async ValueTask<dynamic> IsInvalidAsync(string text) => new
        {
            Condition = String.IsNullOrWhiteSpace(text),
            Message = "Text is invalid"
        };

        private static async ValueTask<dynamic> IsInvalidInputStreamAsync(Stream inputStream) => new
        {
            Condition = inputStream is null || inputStream.Length == 0,
            Message = "Stream is invalid"
        };

        private static async ValueTask<dynamic> IsInvalidOutputStreamAsync(Stream outputStream) => new
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
