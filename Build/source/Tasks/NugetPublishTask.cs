using Cake.Common.Tools.DotNetCore;
using Cake.Common.Tools.DotNetCore.MSBuild;
using Cake.Common.Tools.DotNetCore.Pack;
using Cake.Frosting;
using System;

namespace Build.Tasks
{
    [TaskName("NugetPublish")]
    [IsDependentOn(typeof(TestTask))]
    public sealed class NugetPublishTask : FrostingTask<BuildContext>
    {
        public override void Run(BuildContext context)
        {
            foreach (var (projectName, filePath) in context.NugetPackageProjectFilePaths())
            {
                Version currentVersion = context.GetAssemblyVersion(projectName, false);

                var settings = new DotNetCorePackSettings()
                {
                    OutputDirectory = context.NugetPublishLocation,
                    Configuration = context.BuildConfiguration,
                };

                if (context.IsProductionBuild)
                {
                    settings.MSBuildSettings = new DotNetCoreMSBuildSettings();
                    _ = settings.MSBuildSettings.SetVersion(currentVersion.ToString());
                }

                context.DotNetCorePack(filePath.FullPath, settings);


                context.TryGitTag(projectName, currentVersion);
            }
        }
    }
}
