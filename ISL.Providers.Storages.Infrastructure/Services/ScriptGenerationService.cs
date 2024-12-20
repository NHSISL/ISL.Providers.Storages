﻿// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Collections.Generic;
using System.IO;
using ADotNet.Clients;
using ADotNet.Models.Pipelines.GithubPipelines.DotNets;
using ADotNet.Models.Pipelines.GithubPipelines.DotNets.Tasks;
using ADotNet.Models.Pipelines.GithubPipelines.DotNets.Tasks.SetupDotNetTaskV3s;

namespace ISL.Providers.Storages.Infrastructure.Services
{
    internal class ScriptGenerationService
    {
        private readonly ADotNetClient adotNetClient;

        public ScriptGenerationService() =>
            adotNetClient = new ADotNetClient();

        public void GenerateBuildScript(string branchName, string projectName, string dotNetVersion)
        {
            var githubPipeline = new GithubPipeline
            {
                Name = "Build",

                OnEvents = new Events
                {
                    Push = new PushEvent
                    {
                        Branches = [branchName]
                    },

                    PullRequest = new PullRequestEvent
                    {
                        Types = ["opened", "synchronize", "reopened", "closed"],
                        Branches = [branchName]
                    }
                },

                EnvironmentVariables = new Dictionary<string, string>
                {
                    { "IS_RELEASE_CANDIDATE", EnvironmentVariables.IsGitHubReleaseCandidate() }
                },


                Jobs = new Dictionary<string, Job>
                {
                    {
                        "Label",
                        new LabelJobV2(runsOn: BuildMachines.UbuntuLatest)
                    },
                    {
                        "Build",
                        new Job
                        {
                            RunsOn = BuildMachines.UbuntuLatest,

                            EnvironmentVariables = new Dictionary<string, string>
                            {
                                { "AZUREBLOBSTORECONFIGURATIONS__STORAGEACCOUNTACCESSKEY",
                                    "${{ secrets.AZUREBLOBSTORECONFIGURATIONS__STORAGEACCOUNTACCESSKEY }}" },
                                { "AZUREBLOBSTORECONFIGURATIONS__STORAGEACCOUNTNAME",
                                    "${{ secrets.AZUREBLOBSTORECONFIGURATIONS__STORAGEACCOUNTNAME }}"},
                                { "AZUREBLOBSTORECONFIGURATIONS__SERVICEURI",
                                    "${{ secrets.AZUREBLOBSTORECONFIGURATIONS__SERVICEURI }}"},
                            },

                            Steps = new List<GithubTask>
                            {
                                new CheckoutTaskV3
                                {
                                    Name = "Check Out"
                                },

                                new SetupDotNetTaskV3
                                {
                                    Name = "Setup Dot Net Version",

                                    With = new TargetDotNetVersionV3
                                    {
                                        DotNetVersion = dotNetVersion
                                    }
                                },

                                new RestoreTask
                                {
                                    Name = "Restore"
                                },

                                new DotNetBuildTask
                                {
                                    Name = "Build",
                                },

                                new TestTask
                                {
                                    Name = "Test"
                                }
                            }
                        }
                    },
                    {
                        "add_tag",
                        new TagJob(
                            runsOn: BuildMachines.UbuntuLatest,
                            dependsOn: "build",
                            projectRelativePath: $"{projectName}/{projectName}.csproj",
                            githubToken: "${{ secrets.PAT_FOR_TAGGING }}",
                            branchName: branchName)
                        {
                            Name = "Tag and Release"
                        }
                    },
                    {
                        "publish",
                        new PublishJobV2(
                            runsOn: BuildMachines.UbuntuLatest,
                            dependsOn: "add_tag",
                            dotNetVersion: dotNetVersion,
                            nugetApiKey: "${{ secrets.NUGET_ACCESS }}")
                        {
                            Name = "Publish to NuGet"
                        }
                    }
                }
            };

            string buildScriptPath = "../../../../.github/workflows/build.yml";
            string directoryPath = Path.GetDirectoryName(buildScriptPath);

            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            adotNetClient.SerializeAndWriteToFile(
                adoPipeline: githubPipeline,
                path: buildScriptPath);
        }
    }
}
