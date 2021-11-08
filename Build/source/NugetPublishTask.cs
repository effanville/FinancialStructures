using Cake.Common.IO;
using Cake.Common.Tools.DotNetCore;
using Cake.Common.Tools.DotNetCore.Pack;
using Cake.Core.IO;
using Cake.Frosting;

namespace Build
{
    [TaskName("NugetPublish")]
    [IsDependentOn(typeof(TestTask))]
    public sealed class NugetPublishTask : FrostingTask<BuildContext>
    {
        public override void Run(BuildContext context)
        {
            var settings = new DotNetCorePackSettings()
            {
                OutputDirectory = context.PublishLocation
            };
            FilePath file = ((DirectoryPath)(context.RepoDir + context.Directory("/FinancialStructures"))).CombineWithFilePath("FinancialStructures.csproj");
            context.DotNetCorePack(file.FullPath, settings);
        }
    }
}
