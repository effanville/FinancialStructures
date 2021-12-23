using System;
using Cake.Common.IO;
using Cake.Common.Tools.DotNetCore;
using Cake.Common.Tools.DotNetCore.MSBuild;
using Cake.Common.Tools.DotNetCore.Publish;
using Cake.Core.IO;
using Cake.Frosting;

namespace Build.Tasks
{
    [TaskName("Publish")]
    [IsDependentOn(typeof(TestTask))]
    public sealed class PublishTask : FrostingTask<BuildContext>
    {
        public override void Run(BuildContext context)
        {
            foreach (var project in context.ExecutableProjectFilePaths())
            {
                Version currentVersion = context.GetAssemblyVersion(project.ProjectName, true);
                DirectoryPath outputDir = context.PublishLocation + context.Directory($"{currentVersion}");

                var settings = new DotNetCorePublishSettings()
                {
                    Framework = context.Framework,
                    Configuration = context.BuildConfiguration,
                    Runtime = context.Runtime,
                    OutputDirectory = outputDir,
                    SelfContained = true,
                    PublishSingleFile = true,
                    PublishReadyToRun = true,
                    NoBuild = false,
                    NoRestore = true
                };

                if (context.IsProductionBuild)
                {
                    settings.MSBuildSettings = new DotNetCoreMSBuildSettings();
                    _ = settings.MSBuildSettings.SetVersion(currentVersion.ToString());
                }

                context.DotNetCorePublish(project.FilePath.FullPath, settings);

                context.TryGitTag(project.ProjectName, currentVersion);
            }
        }
    }
}
