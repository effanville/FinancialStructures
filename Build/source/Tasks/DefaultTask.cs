using Cake.Frosting;

namespace Build.Tasks
{
    [TaskName("Default")]
    [IsDependentOn(typeof(PublishTask))]
    [IsDependentOn(typeof(NugetPublishTask))]
    public sealed class DefaultTask : FrostingTask<BuildContext>
    {
    }
}
