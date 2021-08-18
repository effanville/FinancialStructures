using Cake.Frosting;

namespace Build
{
    [TaskName("Default")]
    [IsDependentOn(typeof(CopyPublishTask))]
    public sealed class DefaultTask : FrostingTask<BuildContext>
    {
    }
}
