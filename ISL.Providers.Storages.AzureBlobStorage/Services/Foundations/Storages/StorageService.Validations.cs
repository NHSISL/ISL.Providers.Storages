// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using Azure.Storage.Blobs.Models;
using ISL.Providers.Storages.Abstractions.Models;
using ISL.Providers.Storages.AzureBlobStorage.Models.Foundations.Files.Exceptions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

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

        private static void ValidateContainerName(string container)
        {
            Validate(
                (Rule: IsInvalid(container), Parameter: "Container"));
        }

        private static void ValidateStorageArgumentsOnCreateDirectory(string container, string directory)
        {
            Validate(
                (Rule: IsInvalid(container), Parameter: "Container"),
                (Rule: IsInvalid(directory), Parameter: "Directory"));
        }

        private static void ValidateStorageArgumentsOnGetDownloadLink(
            string fileName,
            string container,
            DateTimeOffset expiresOn)
        {
            Validate(
                (Rule: IsInvalid(fileName), Parameter: "FileName"),
                (Rule: IsInvalid(container), Parameter: "Container"),
                (Rule: IsInvalid(expiresOn), Parameter: "ExpiresOn"));
        }

        private static void ValidateStorageArgumentsOnCreateAccessPolicy(string container, List<Policy> policies)
        {
            Validate(
                (Rule: IsInvalid(container), Parameter: "Container"),
                (Rule: IsInvalidList(policies), Parameter: "Policies"));

            foreach (Policy policy in policies)
            {
                Validate(
                    (Rule: IsInvalid(policy.PolicyName),
                    Parameter: $"{nameof(Policy)}.{nameof(Policy.PolicyName)}"),

                    (Rule: IsInvalidList(policy.Permissions),
                    Parameter: $"{nameof(Policy)}.{nameof(Policy.Permissions)}"));
            }
        }

        private static void ValidateStorageArgumentsOnCreateSasToken(
            string container,
            string path,
            string accessPolicyIdentifier,
            DateTimeOffset expiresOn)
        {
            Validate(
                (Rule: IsInvalid(container), Parameter: "Container"),
                (Rule: IsInvalid(path), Parameter: "Path"),
                (Rule: IsInvalid(accessPolicyIdentifier), Parameter: "AccessPolicyIdentifier"),
                (Rule: IsInvalid(expiresOn), Parameter: "ExpiresOn"));
        }

        private static void ValidateStorageArgumentsOnCreateSasToken(
            string container,
            string path,
            DateTimeOffset expiresOn,
            List<string> permissions)
        {
            Validate(
                (Rule: IsInvalid(container), Parameter: "Container"),
                (Rule: IsInvalid(path), Parameter: "Path"),
                (Rule: IsInvalid(expiresOn), Parameter: "ExpiresOn"));

            Validate(
                (Rule: IsInvalidList(permissions), Parameter: "Permissions"));
        }

        private static void ValidateStorageArgumentsOnRemoveAccessPolicies(string container)
        {
            Validate(
                (Rule: IsInvalid(container), Parameter: "Container"));
        }

        private static void ValidateStorageArgumentsOnRetrieveAllAccessPolicies(string container)
        {
            Validate(
                (Rule: IsInvalid(container), Parameter: "Container"));

        }

        private static void ValidateStorageArgumentsOnRetrieveAccessPolicyByName(string container, string policyName)
        {
            Validate(
                (Rule: IsInvalid(container), Parameter: "Container"),
                (Rule: IsInvalid(policyName), Parameter: "PolicyName"));
        }

        private static void ValidateStorageArgumentsOnRemoveAccessPolicyByName(string container, string policyName)
        {
            Validate(
                (Rule: IsInvalid(container), Parameter: "Container"),
                (Rule: IsInvalid(policyName), Parameter: "PolicyName"));
        }

        private static void ValidateAccessPolicyExists(string policyName, List<BlobSignedIdentifier> signedIdentifiers)
        {
            if (!(signedIdentifiers.Any(signedIdentifier => signedIdentifier.Id == policyName)))
            {
                throw new AccessPolicyNotFoundStorageException(
                    message: "Access policy with the provided name was not found on this container.");
            }
        }

        private static dynamic IsInvalid(string text) => new
        {
            Condition = String.IsNullOrWhiteSpace(text),
            Message = "Text is invalid"
        };

        private static dynamic IsInvalidList(List<string> textList) => new
        {
            Condition = textList is null || textList.Count == 0 || textList.Any(string.IsNullOrWhiteSpace),
            Message = "List is invalid"
        };

        private static dynamic IsInvalidList(List<Policy> policyList) => new
        {
            Condition = policyList is null || policyList.Count == 0,
            Message = "List is invalid"
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

        private static void ValidatePermissions(List<string> permissions)
        {
            foreach (var permission in permissions)
            {
                if (permission.ToLower() != "read" &&
                    permission.ToLower() != "write" &&
                    permission.ToLower() != "delete" &&
                    permission.ToLower() != "create" &&
                    permission.ToLower() != "add" &&
                    permission.ToLower() != "list")
                {
                    throw new InvalidPolicyPermissionStorageException(
                        message: "Invalid permission. Read, write, delete, create, add and list" +
                        "permissions are supported at this time.");
                }
            }
        }

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
