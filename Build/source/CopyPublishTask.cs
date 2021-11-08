using Cake.Common.Diagnostics;
using Cake.Common.IO;
using Cake.Frosting;

namespace Build
{
    [TaskName("CopyPublish")]
    [IsDependentOn(typeof(NugetPublishTask))]
    [IsDependentOn(typeof(PublishTask))]
    public sealed class CopyPublishTask : FrostingTask<BuildContext>
    {
        public override void Run(BuildContext context)
        {
            if (!string.IsNullOrEmpty(context.RemoteDir))
            {
                context.EnsureDirectoryExists(context.RemoteLocation);
                context.CopyDirectory(context.PublishLocation, context.RemoteLocation);
                context.Information($"Copied published files to directory {context.RemoteLocation}");
            }
            else
            {
                context.Information($"Could not locate directory {context.RemoteLocation}");
            }
        }
    }
}
