using Cake.Common.Tools.DotNetCore;
using Cake.Common.Tools.DotNetCore.Pack;
using Cake.Frosting;
using Cake.Git;
using System;
using Cake.Common.Diagnostics;
using Cake.Core.IO;
using Cake.VersionReader;
using Cake.Common.IO;

namespace Build.Tasks
{
    [TaskName("NugetPublish")]
    [IsDependentOn(typeof(TestTask))]
    public sealed class NugetPublishTask : FrostingTask<BuildContext>
    {
        public override void Run(BuildContext context)
        {
            var settings = new DotNetCorePackSettings()
            {
                OutputDirectory = context.NugetPublishLocation,
                Configuration = context.BuildConfiguration,
            };
            foreach (var (projectName, filePath) in context.NugetPackageProjectFilePaths())
            {
                context.DotNetCorePack(filePath.FullPath, settings);

                DirectoryPath binDir = context.RepoDir + context.Directory($"bin/{context.BuildConfiguration}/{context.Framework}");
                FilePath thing = binDir.CombineWithFilePath("FinancePortfolioDatabase.exe");
                string assemblyVersion = context.GetFullVersionNumber(thing);

                var currentVersion = new Version(assemblyVersion);

                try
                {
                    context.GitTag(context.RepoDir, $"{projectName}/{currentVersion.Major}.{currentVersion.Minor}/{currentVersion.Major}.{currentVersion.Minor}.{currentVersion.Build}.{currentVersion.Revision}");
                }
                catch (Exception ex)
                {
                    context.Error($"Git tag failed with error: {ex.Message}");
                }
            }
        }
    }
}
