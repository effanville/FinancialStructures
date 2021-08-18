using Cake.Common.Tools.DotNetCore;
using Cake.Common.Tools.DotNetCore.Build;
using Cake.Common.Tools.NuGet;
using Cake.Common.Tools.NuGet.Restore;
using Cake.Core.IO;
using Cake.Frosting;

namespace Build
{
    [TaskName("Build")]
    [IsDependentOn(typeof(CleanTask))]
    public sealed class BuildTask : FrostingTask<BuildContext>
    {
        public override void Run(BuildContext context)
        {
            FilePath file = context.RepoDir.CombineWithFilePath("FinancePortfolioDatabase.sln");
            FilePath nugetConfigFile = context.RepoDir.CombineWithFilePath("nuget.config");
            var nugetSettings = new NuGetRestoreSettings()
            {
                ConfigFile = nugetConfigFile
            };
            context.NuGetRestore(file, nugetSettings);
            var settings = new DotNetCoreBuildSettings()
            {
                Configuration = context.BuildConfiguration,
                NoRestore = false
            };
            context.DotNetCoreBuild(file.FullPath, settings);
        }
    }
}
