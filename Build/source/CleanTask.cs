using Cake.Common.IO;
using Cake.Core.Diagnostics;
using Cake.Core.IO;
using Cake.Frosting;

namespace Build
{
    [TaskName("Clean")]
    public sealed class CleanTask : FrostingTask<BuildContext>
    {
        public override void Run(BuildContext context)
        {
            context.Log.Information(context.RepoDir);
            DirectoryPath binDir = context.RepoDir + context.Directory($"/bin/{context.BuildConfiguration}");
            context.CleanDirectory(binDir);
        }
    }
}
