// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using ISL.Providers.Storages.Abstractions.Models;
using ISL.Providers.Storages.Abstractions.Models.Exceptions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace ISL.Providers.Storages.Abstractions
{
    public partial class StorageAbstractionProvider : IStorageAbstractionProvider
    {
        private readonly IStorageProvider storageProvider;

        public StorageAbstractionProvider(IStorageProvider storageProvider) =>
            this.storageProvider = storageProvider;

        /// <summary>
        /// Creates a file in the storage container.
        /// </summary>
        /// <param name="input">The <see cref="Stream"/> containing the file data to be uploaded.</param>
        /// <param name="fileName">The name of the file to create in the container.</param>
        /// <param name="container">The name of the storage container where the file will be stored.</param>
        /// <returns>A <see cref="ValueTask"/> representing the asynchronous operation.</returns>
        /// <exception cref="StorageProviderValidationException">
        /// Thrown when validation of input parameters fails.
        /// </exception>
        /// <exception cref="StorageProviderDependencyException">
        /// Thrown when there is an issue with the storage dependency.
        /// </exception>
        /// <exception cref="StorageProviderServiceException">
        /// Thrown when there is a general issue in the storage service layer.
        /// </exception>
        public ValueTask CreateFileAsync(Stream input, string fileName, string container) =>
        TryCatch(async () =>
        {
            await storageProvider.CreateFileAsync(input, fileName, container);
        });

        /// <summary>
        /// Retrieves a file from the storage container.
        /// </summary>
        /// <param name="output">The <see cref="Stream"/> containing the file data to be downloaded.</param>
        /// <param name="fileName">The name of the file to retrieve in the container.</param>
        /// <param name="container">The name of the storage container where the file is located.</param>
        /// <returns>A <see cref="ValueTask"/> representing the asynchronous operation.</returns>
        /// <exception cref="StorageProviderValidationException">
        /// Thrown when validation of input parameters fails.
        /// </exception>
        /// <exception cref="StorageProviderDependencyException">
        /// Thrown when there is an issue with the storage dependency.
        /// </exception>
        /// <exception cref="StorageProviderServiceException">
        /// Thrown when there is a general issue in the storage service layer.
        /// </exception>
        public ValueTask RetrieveFileAsync(Stream output, string fileName, string container) =>
        TryCatch(async () =>
        {
            await storageProvider.RetrieveFileAsync(output, fileName, container);
        });

        /// <summary>
        /// Asynchronously deletes a file from the specified storage container.
        /// </summary>
        /// <param name="fileName">The name of the file to delete.</param>
        /// <param name="container">The name of the storage container where the file is located.</param>
        /// <returns>A <see cref="ValueTask"/> representing the asynchronous operation.</returns>
        /// <exception cref="StorageProviderValidationException">
        /// Thrown when validation of input parameters fails.
        /// </exception>
        /// <exception cref="StorageProviderDependencyException">
        /// Thrown when there is an issue with the storage dependency.
        /// </exception>
        /// <exception cref="StorageProviderServiceException">
        /// Thrown when there is a general issue in the storage service layer.
        /// </exception>
        public ValueTask DeleteFileAsync(string fileName, string container) =>
        TryCatch(async () =>
        {
            await storageProvider.DeleteFileAsync(fileName, container);
        });

        /// <summary>
        /// Asynchronously generates a download link for a file in the specified storage container.
        /// </summary>
        /// <param name="fileName">The name of the file to generate a download link for.</param>
        /// <param name="container">The name of the storage container where the file is located.</param>
        /// <param name="expiresOn">The <see cref="DateTimeOffset"/> indicating when the download link will expire.</param>
        /// <returns>A <see cref="ValueTask{String}"/> containing the download link.</returns>
        /// <exception cref="StorageProviderValidationException">
        /// Thrown when validation of input parameters fails.
        /// </exception>
        /// <exception cref="StorageProviderDependencyException">
        /// Thrown when there is an issue with the storage dependency.
        /// </exception>
        /// <exception cref="StorageProviderServiceException">
        /// Thrown when there is a general issue in the storage service layer.
        /// </exception>
        public ValueTask<string> GetDownloadLinkAsync(string fileName, string container, DateTimeOffset expiresOn) =>
        TryCatch(async () =>
        {
            return await storageProvider.GetDownloadLinkAsync(fileName, container, expiresOn);
        });

        /// <summary>
        /// Creates a container in the storage account.
        /// </summary>
        /// <param name="container">The name of the created storage containe.</param>
        /// <returns>A <see cref="ValueTask"/> representing the asynchronous operation.</returns>
        /// <exception cref="StorageProviderValidationException">
        /// Thrown when validation of input parameters fails.
        /// </exception>
        /// <exception cref="StorageProviderDependencyException">
        /// Thrown when there is an issue with the storage dependency.
        /// </exception>
        /// <exception cref="StorageProviderServiceException">
        /// Thrown when there is a general issue in the storage service layer.
        /// </exception>
        public ValueTask CreateContainerAsync(string container) =>
        TryCatch(async () =>
        {
            await storageProvider.CreateContainerAsync(container);
        });

        /// <summary>
        /// Retrieves all container names in the storage account.
        /// </summary>
        /// <returns>A <see cref="ValueTask{List{String}}"/> representing container names.</returns>
        /// <exception cref="StorageProviderValidationException">
        /// Thrown when validation of input parameters fails.
        /// </exception>
        /// <exception cref="StorageProviderDependencyException">
        /// Thrown when there is an issue with the storage dependency.
        /// </exception>
        /// <exception cref="StorageProviderServiceException">
        /// Thrown when there is a general issue in the storage service layer.
        /// </exception>
        public ValueTask<List<string>> RetrieveAllContainersAsync() =>
            throw new NotImplementedException();

        /// <summary>
        /// Deletes a container in the storage account.
        /// </summary>
        /// <param name="container">The name of the deleted storage container.</param>
        /// <returns>A <see cref="ValueTask"/> representing the asynchronous operation.</returns>
        /// <exception cref="StorageProviderValidationException">
        /// Thrown when validation of input parameters fails.
        /// </exception>
        /// <exception cref="StorageProviderDependencyException">
        /// Thrown when there is an issue with the storage dependency.
        /// </exception>
        /// <exception cref="StorageProviderServiceException">
        /// Thrown when there is a general issue in the storage service layer.
        /// </exception>
        public ValueTask DeleteContainerAsync(string container) =>
            throw new NotImplementedException();

        /// <summary>
        /// Asynchronously lists all files in the specified storage container.
        /// </summary>
        /// <param name="container">The name of the storage container to list files from.</param>
        /// <returns>A <see cref="ValueTask{List{String}}"/> containing the list of file names.</returns>
        /// <exception cref="StorageProviderValidationException">
        /// Thrown when validation of input parameters fails.
        /// </exception>
        /// <exception cref="StorageProviderDependencyException">
        /// Thrown when there is an issue with the storage dependency.
        /// </exception>
        /// <exception cref="StorageProviderServiceException">
        /// Thrown when there is a general issue in the storage service layer.
        /// </exception>
        public ValueTask<List<string>> ListFilesInContainerAsync(string container) =>
        TryCatch(async () =>
        {
            return await storageProvider.ListFilesInContainerAsync(container);
        });

        /// <summary>
        /// Creates a SAS token scoped to the provided container and directory, with the permissions of 
        /// the provided access policy.
        /// </summary>
        /// <param name="container">The name of the storage container where the SAS token will be created.</param>
        /// <param name="directoryPath">The path to which the SAS token will be scoped</param>
        /// <param name="accessPolicyIdentifier">The name of the stored access policy.</param>
        /// <param name="expiresOn">The <see cref="DateTimeOffset"/> indicating when the SAS token will expire.</param>
        /// <returns>A <see cref="ValueTask{String}"/> containing the generated access token.</returns>
        /// <exception cref="StorageProviderValidationException">
        /// Thrown when validation of input parameters fails.
        /// </exception>
        /// <exception cref="StorageProviderDependencyException">
        /// Thrown when there is an issue with the storage dependency.
        /// </exception>
        /// <exception cref="StorageProviderServiceException">
        /// Thrown when there is a general issue in the storage service layer.
        /// </exception>
        public ValueTask<string> CreateDirectorySasTokenAsync(
             string container, string directoryPath, string accessPolicyIdentifier, DateTimeOffset expiresOn) =>
        TryCatch(async () =>
        {
            return await storageProvider.CreateDirectorySasTokenAsync(
                container, directoryPath, accessPolicyIdentifier, expiresOn);
        });

        /// <summary>
        /// Asynchronously generates an access token for a specified path in the storage container with a given access level.
        /// </summary>
        /// <param name="path">The path within the container for which the access token is generated.</param>
        /// <param name="container">The name of the storage container.</param>
        /// <param name="accessLevel">The access level for the token (e.g., "read" or "write").</param>
        /// <param name="expiresOn">The <see cref="DateTimeOffset"/> indicating when the access token will expire.</param>
        /// <returns>A <see cref="ValueTask{String}"/> containing the generated access token.</returns>
        /// <exception cref="StorageProviderValidationException">
        /// Thrown when validation of input parameters fails.
        /// </exception>
        /// <exception cref="StorageProviderDependencyException">
        /// Thrown when there is an issue with the storage dependency.
        /// </exception>
        /// <exception cref="StorageProviderServiceException">
        /// Thrown when there is a general issue in the storage service layer.
        /// </exception>
        public ValueTask<string> GetAccessTokenAsync(
            string path,
            string container,
            string accessLevel,
            DateTimeOffset expiresOn) =>
        TryCatch(async () =>
        {
            return await storageProvider.GetAccessTokenAsync(path, container, accessLevel, expiresOn);
        });

        /// <summary>
        /// Retrieves all stored access policies from the container.
        /// </summary>
        /// <param name="container">The name of the storage container.</param>
        /// <returns>A <see cref="ValueTask{List{String}}"/> containing the access policy names.</returns>
        /// <exception cref="StorageProviderValidationException">
        /// Thrown when validation of input parameters fails.
        /// </exception>
        /// <exception cref="StorageProviderDependencyException">
        /// Thrown when there is an issue with the storage dependency.
        /// </exception>
        /// <exception cref="StorageProviderServiceException">
        /// Thrown when there is a general issue in the storage service layer.
        public ValueTask<List<string>> RetrieveListOfAllAccessPoliciesAsync(string container) =>
        TryCatch(async () =>
        {
            return await storageProvider.RetrieveListOfAllAccessPoliciesAsync(container);
        });

        /// <summary>
        /// Retrieves all stored access policies from the container.
        /// </summary>
        /// <param name="container">The name of the storage container.</param>
        /// <returns>A <see cref="ValueTask{List{Policy}}"/> containing policy objects corresponding to 
        /// the container access policies.</returns>
        /// <exception cref="StorageProviderValidationException">
        /// Thrown when validation of input parameters fails.
        /// </exception>
        /// <exception cref="StorageProviderDependencyException">
        /// Thrown when there is an issue with the storage dependency.
        /// </exception>
        /// <exception cref="StorageProviderServiceException">
        /// Thrown when there is a general issue in the storage service layer.
        public ValueTask<List<Policy>> RetrieveAllAccessPoliciesAsync(string container) =>
        TryCatch(async () =>
        {
            return await this.storageProvider.RetrieveAllAccessPoliciesAsync(container);
        });

        /// <summary>
        /// Retrieves the provided stored access policy from the container if it exists.
        /// </summary>
        /// <param name="container">The name of the storage container.</param>
        /// <param name="policyName">The name of the stored access policy.</param>
        /// <returns>A <see cref="ValueTask{Policy}"/> containing the access policy.</returns>
        /// <exception cref="StorageProviderValidationException">
        /// Thrown when validation of input parameters fails.
        /// </exception>
        /// <exception cref="StorageProviderDependencyException">
        /// Thrown when there is an issue with the storage dependency.
        /// </exception>
        /// <exception cref="StorageProviderServiceException">
        /// Thrown when there is a general issue in the storage service layer.
        public ValueTask<Policy> RetrieveAccessPolicyByNameAsync(string container, string policyName) =>
        TryCatch(async () =>
        {
            return await this.storageProvider.RetrieveAccessPolicyByNameAsync(container, policyName);
        });

        /// <summary>
        /// Creates the provided stored access policies on the container.
        /// </summary>
        /// <param name="container">The name of the storage container where the access policies will be created.</param>
        /// <param name="policies"><see cref="List{Policy}"/>
        /// <returns>A <see cref="ValueTask"/> representing the asynchronous operation.</returns>
        /// <exception cref="StorageProviderValidationException">
        /// Thrown when validation of input parameters fails.
        /// </exception>
        /// <exception cref="StorageProviderDependencyException">
        /// Thrown when there is an issue with the storage dependency.
        /// </exception>
        /// <exception cref="StorageProviderServiceException">
        /// Thrown when there is a general issue in the storage service layer.
        /// </exception>
        public ValueTask CreateAndAssignAccessPoliciesAsync(string container, List<Policy> policies) =>
        TryCatch(async () =>
        {
            await this.storageProvider.CreateAndAssignAccessPoliciesAsync(container, policies);
        });

        /// <summary>
        /// Removes all stored access policies from the container.
        /// </summary>
        /// <param name="container">The name of the storage container.</param>
        /// <returns>A <see cref="ValueTask"/> representing the asynchronous operation.</returns>
        /// <exception cref="StorageProviderValidationException">
        /// Thrown when validation of input parameters fails.
        /// </exception>
        /// <exception cref="StorageProviderDependencyException">
        /// Thrown when there is an issue with the storage dependency.
        /// </exception>
        /// <exception cref="StorageProviderServiceException">
        /// Thrown when there is a general issue in the storage service layer.
        /// </exception>
        public ValueTask RemoveAccessPoliciesFromContainerAsync(string container) =>
        TryCatch(async () =>
        {
            await this.storageProvider.RemoveAccessPoliciesFromContainerAsync(container);
        });

        /// <summary>
        /// Creates a folder within the specified container.
        /// </summary>
        /// <param name="container">The name of the storage container to create the folder in.</param>
        /// <param name="folder">The name of the folder to create.</param>
        /// <returns>A <see cref="ValueTask"/> representing the asynchronous operation.</returns>
        /// <exception cref="StorageProviderValidationException">
        /// Thrown when validation of input parameters fails.
        /// </exception>
        /// <exception cref="StorageProviderDependencyException">
        /// Thrown when there is an issue with the storage dependency.
        /// </exception>
        /// <exception cref="StorageProviderServiceException">
        /// Thrown when there is a general issue in the storage service layer.
        /// </exception>
        public ValueTask CreateFolderInContainerAsync(string container, string folder) =>
        TryCatch(async () =>
        {
            await this.storageProvider.CreateFolderInContainerAsync(container, folder);
        });
    }
}
