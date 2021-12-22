using Cake.Common.IO;
using Cake.Common.Tools.DotNetCore;
using Cake.Common.Tools.DotNetCore.Build;
using Cake.Common.Tools.NuGet;
using Cake.Common.Tools.NuGet.Restore;
using Cake.Core.IO;
using Cake.Frosting;

namespace Build.Tasks
{
    [TaskName("Build")]
    [IsDependentOn(typeof(CleanTask))]
    public sealed class BuildTask : FrostingTask<BuildContext>
    {
        public override void Run(BuildContext context)
        {
            FilePath solutionFile = context.SolutionFilePath();
            FilePath nugetConfigFilePath = context.NugetConfigFilePath();

            // First perform a nuget restore with the specific config file (if it exists).
            var nugetSettings = new NuGetRestoreSettings();
            if (context.FileExists(nugetConfigFilePath))
            {
                nugetSettings.ConfigFile = nugetConfigFilePath;
            }

            context.NuGetRestore(solutionFile, nugetSettings);

            // now build the solution with the relevant configuration.
            var settings = new DotNetCoreBuildSettings()
            {
                Configuration = context.BuildConfiguration,
                NoRestore = false
            };
            context.DotNetCoreBuild(solutionFile.FullPath, settings);
        }
    }
}
