using System;
using Cake.Common.IO;
using Cake.Common.Tools.DotNetCore;
using Cake.Common.Tools.DotNetCore.Publish;
using Cake.Core.IO;
using Cake.Frosting;
using Cake.Git;
using Cake.VersionReader;

namespace Build
{
    [TaskName("Publish")]
    [IsDependentOn(typeof(TestTask))]
    public sealed class PublishTask : FrostingTask<BuildContext>
    {
        public override void Run(BuildContext context)
        {
            DirectoryPath binDir = context.RepoDir + context.Directory($"/bin/{context.BuildConfiguration}/{context.Framework}");
            FilePath thing = binDir.CombineWithFilePath("FinancePortfolioDatabase.exe");
            string assemblyVersion = context.GetFullVersionNumber(thing);
            context.PublishLocation = context.PublishLocation + context.Directory($"/{assemblyVersion}");
            DirectoryPath outputDir = context.PublishLocation;
            var settings = new DotNetCorePublishSettings()
            {
                Framework = context.Framework,
                Configuration = context.BuildConfiguration,
                Runtime = context.Runtime,
                OutputDirectory = outputDir,
                SelfContained = true,
                PublishSingleFile = true,
                PublishReadyToRun = true,
                NoBuild = true
            };
            FilePath file = ((DirectoryPath)(context.RepoDir + context.Directory("/FinancePortfolioDatabase/"))).CombineWithFilePath("FinancePortfolioDatabase.csproj");
            if (context.DirectoryExists(outputDir))
            {
                context.CleanDirectory(outputDir);
            }

            context.DotNetCorePublish(file.FullPath, settings);
            Version version = new Version(assemblyVersion);
            context.GitTag(context.RepoDir, $"FPD/{version.Major}.{version.Minor}/{assemblyVersion}");
        }
    }
}
