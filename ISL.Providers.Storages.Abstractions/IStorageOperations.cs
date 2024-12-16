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
        /// Creates a SAS token scoped to the provided container and path, with the permissions of 
        /// the provided access policy.
        /// </summary>
        /// <param name="container">The name of the storage container where the SAS token will be created.</param>
        /// <param name="directoryPath">The path to which the SAS token will be scoped</param>
        /// <param name="accessPolicyIdentifier">The name of the stored access policy.</param>
        /// <param name="expiresOn">The <see cref="DateTimeOffset"/> indicating when the Sas 
        /// token will expire.</param>
        /// <returns>A <see cref="ValueTask{String}"/> containing the generated access token.</returns>
        ValueTask<string> CreateSasTokenAsync(
             string container,
             string directoryPath,
             string accessPolicyIdentifier,
             DateTimeOffset expiresOn);

        /// <summary>
        /// Creates a SAS token scoped to the provided container and path, with the permissions provided.
        /// </summary>
        /// <param name="container">The name of the storage container where the SAS token will be created.</param>
        /// <param name="path">The path to which the SAS token will be scoped</param>
        /// <param name="expiresOn">The <see cref="DateTimeOffset"/> indicating when the Sas 
        /// token will expire.</param>
        /// <param name="permissions">A <see cref="List{String}"/> containing the permissions of the token.
        /// The options are read, add, create, write, delete and list.</param>
        /// <returns>A <see cref="ValueTask{String}"/> containing the generated access token.</returns>
        ValueTask<string> CreateSasTokenAsync(
            string container,
            string path,
            DateTimeOffset expiresOn,
            List<string> permissions);

        /// <summary>
        /// Retrieves all stored access policies from the container.
        /// </summary>
        /// <param name="container">The name of the storage container.</param>
        /// <returns>A <see cref="ValueTask{List{String}}"/> containing the access policy names.</returns>
        ValueTask<List<string>> RetrieveListOfAllAccessPoliciesAsync(string container);

        /// <summary>
        /// Retrieves all stored access policies from the container.
        /// </summary>
        /// <param name="container">The name of the storage container.</param>
        /// <returns>A <see cref="ValueTask{List{Policy}}"/> containing policy objects corresponding to 
        /// the container access policies.</returns>
        ValueTask<List<Policy>> RetrieveAllAccessPoliciesAsync(string container);

        /// <summary>
        /// Retrieves the provided stored access policy from the container if it exists.
        /// </summary>
        /// <param name="container">The name of the storage container.</param>
        /// <param name="policyName">The name of the stored access policy.</param>
        /// <returns>A <see cref="ValueTask{Policy}"/> containing the access policy.</returns>
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
        ValueTask RemoveAllAccessPoliciesAsync(string container);

        /// <summary>
        /// Removes the provided stored access policy from the container.
        /// </summary>
        /// <param name="container">The name of the storage container.</param>
        /// <param name="policyName">The name of the stored access policy.</param>
        /// <returns>A <see cref="ValueTask"/> representing the asynchronous operation.</returns>
        ValueTask RemoveAccessPolicyByNameAsync(string container, string policyName);

        /// <summary>
        /// Creates a folder within the specified container.
        /// </summary>
        /// <param name="container">The name of the storage container to create the folder in.</param>
        /// <param name="folder">The name of the folder to create.</param>
        /// <returns>A <see cref="ValueTask"/> representing the asynchronous operation.</returns>
        ValueTask CreateFolderInContainerAsync(string container, string folder);
    }
}
