using Cake.Common.Tools.DotNetCore;
using Cake.Common.Tools.DotNetCore.Test;
using Cake.Core.IO;
using Cake.Frosting;

namespace Build
{
    [TaskName("Test")]
    [IsDependentOn(typeof(BuildTask))]
    public sealed class TestTask : FrostingTask<BuildContext>
    {
        public override void Run(BuildContext context)
        {
            var settings = new DotNetCoreTestSettings()
            {
                Configuration = context.BuildConfiguration,
                NoBuild = true
            };
            FilePath file = context.RepoDir.CombineWithFilePath("FinancePortfolioDatabase.sln");
            context.DotNetCoreTest(file.FullPath, settings);
        }
    }
}
