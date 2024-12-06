// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using ISL.Providers.Storages.Abstractions.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace ISL.Providers.Storages.Abstractions
{
    public interface IStorageOperations
    {
        /// <summary>
        /// Creates a file in the storage container.
        /// </summary>
        /// <param name="input">The <see cref="Stream"/> containing the file data to be uploaded.</param>
        /// <param name="fileName">The name of the file to create in the container.</param>
        /// <param name="container">The name of the storage container where the file will be stored.</param>
        /// <returns>A <see cref="ValueTask"/> representing the asynchronous operation.</returns>
        ValueTask CreateFileAsync(Stream input, string fileName, string container);

        /// <summary>
        /// Retrieves a file from the storage container.
        /// </summary>
        /// <param name="output">The <see cref="Stream"/> containing the file data to be downloaded.</param>
        /// <param name="fileName">The name of the file to retrieve in the container.</param>
        /// <param name="container">The name of the storage container where the file is located.</param>
        /// <returns>A <see cref="ValueTask"/> representing the asynchronous operation.</returns>
        ValueTask RetrieveFileAsync(Stream output, string fileName, string container);

        /// <summary>
        /// Asynchronously deletes a file from the specified storage container.
        /// </summary>
        /// <param name="fileName">The name of the file to delete.</param>
        /// <param name="container">The name of the storage container where the file is located.</param>
        /// <returns>A <see cref="ValueTask"/> representing the asynchronous operation.</returns>
        ValueTask DeleteFileAsync(string fileName, string container);

        /// <summary>
        /// Asynchronously generates a download link for a file in the specified storage container.
        /// </summary>
        /// <param name="fileName">The name of the file to generate a download link for.</param>
        /// <param name="container">The name of the storage container where the file is located.</param>
        /// <param name="expiresOn">The <see cref="DateTimeOffset"/> indicating when the download 
        /// link will expire.</param>
        /// <returns>A <see cref="ValueTask{String}"/> containing the download link.</returns>
        ValueTask<string> GetDownloadLinkAsync(string fileName, string container, DateTimeOffset expiresOn);

        /// <summary>
        /// Creates a container in the storage account.
        /// </summary>
        /// <param name="container">The name of the created storage container.</param>
        /// <returns>A <see cref="ValueTask"/> representing the asynchronous operation.</returns>
        ValueTask CreateContainerAsync(string container);

        /// <summary>
        /// Retrieves all container names in the storage account.
        /// </summary>
        /// <returns>A <see cref="ValueTask{List{String}}"/> representing container names.</returns>
        ValueTask<List<string>> RetrieveAllContainersAsync();

        /// <summary>
        /// Deletes a container in the storage account.
        /// </summary>
        /// <param name="container">The name of the deleted storage container.</param>
        /// <returns>A <see cref="ValueTask"/> representing the asynchronous operation.</returns>
        ValueTask DeleteContainerAsync(string container);

        /// <summary>
        /// Asynchronously lists all files in the specified storage container.
        /// </summary>
        /// <param name="container">The name of the storage container to list files from.</param>
        /// <returns>A <see cref="ValueTask{List{String}}"/> containing the list of file names.</returns>
        ValueTask<List<string>> ListFilesInContainerAsync(string container);

        /// <summary>
        /// Creates a SAS token scoped to the provided container and directory, with the permissions of 
        /// the provided access policy.
        /// </summary>
        /// <param name="container">The name of the storage container where the SAS token will be created.</param>
        /// <param name="directoryPath">The path to which the SAS token will be scoped</param>
        /// <param name="accessPolicyIdentifier">The name of the stored access policy.</param>
        /// <param name="expiresOn">The <see cref="DateTimeOffset"/> indicating when the SAS token will expire.</param>
        /// <returns>A <see cref="ValueTask{String}"/> containing the generated access token.</returns>
        public ValueTask<string> CreateDirectorySasTokenAsync(
             string container, string directoryPath, string accessPolicyIdentifier, DateTimeOffset expiresOn);

        /// <summary>
        /// Asynchronously generates an access token for a specified path in the storage container with a given 
        /// access level.
        /// </summary>
        /// <param name="path">The path within the container for which the access token is generated.</param>
        /// <param name="container">The name of the storage container.</param>
        /// <param name="accessLevel">The access level for the token (e.g., "read" or "write").</param>
        /// <param name="expiresOn">The <see cref="DateTimeOffset"/> indicating when the access token will 
        /// expire.</param>
        /// <returns>A <see cref="ValueTask{String}"/> containing the generated access token.</returns>
        ValueTask<string> GetAccessTokenAsync(
            string path, string container, string accessLevel, DateTimeOffset expiresOn);

        /// <summary>
        /// Retrieves all stored access policies from the container.
        /// </summary>
        /// <param name="container">The name of the storage container.</param>
        /// <returns>A <see cref="ValueTask{List{String}}"/> containing the access policy names.</returns>
        /// <exception cref="StorageValidationProviderException">
        /// Thrown when validation of input parameters fails.
        /// </exception>
        /// <exception cref="StorageDependencyProviderException">
        /// Thrown when there is an issue with the storage dependency.
        /// </exception>
        /// <exception cref="StorageServiceProviderException">
        /// Thrown when there is a general issue in the storage service layer.
        ValueTask<List<string>> RetrieveListOfAllAccessPoliciesAsync(string container);

        /// <summary>
        /// Retrieves the provided stored access policy from the container if it exists.
        /// </summary>
        /// <param name="container">The name of the storage container.</param>
        /// <param name="policyName">The name of the stored access policy.</param>
        /// <returns>A <see cref="ValueTask{Policy}"/> containing the access policy.</returns>
        /// <exception cref="StorageValidationProviderException">
        /// Thrown when validation of input parameters fails.
        /// </exception>
        /// <exception cref="StorageDependencyProviderException">
        /// Thrown when there is an issue with the storage dependency.
        /// </exception>
        /// <exception cref="StorageServiceProviderException">
        /// Thrown when there is a general issue in the storage service layer.
        ValueTask<Policy> RetrieveAccessPolicyByNameAsync(string container, string policyName);

        /// <summary>
        /// Creates the provided stored access policies on the container.
        /// </summary>
        /// <param name="container">The name of the storage container where the access policies will be created.</param>
        /// <param name="policies"><see cref="List{Policy}"/>
        /// A list of Policy objects representing the access policies to be created</param>
        /// <returns>A <see cref="ValueTask"/> representing the asynchronous operation.</returns>
        ValueTask CreateAndAssignAccessPoliciesAsync(string container, List<Policy> policies);

        /// <summary>
        /// Removes all stored access policies from the container.
        /// </summary>
        /// <param name="container">The name of the storage container.</param>
        /// <returns>A <see cref="ValueTask"/> representing the asynchronous operation.</returns>
        ValueTask RemoveAccessPoliciesFromContainerAsync(string container);

        /// <summary>
        /// Creates a folder within the specified container.
        /// </summary>
        /// <param name="container">The name of the storage container to create the folder in.</param>
        /// <param name="folder">The name of the folder to create.</param>
        /// <returns>A <see cref="ValueTask"/> representing the asynchronous operation.</returns>
        ValueTask CreateFolderInContainerAsync(string container, string folder);
    }
}
