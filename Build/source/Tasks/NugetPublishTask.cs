using Cake.Common.Tools.DotNetCore;
using Cake.Common.Tools.DotNetCore.Pack;
using Cake.Frosting;
using Cake.Git;
using Cake.Common.Xml;
using System;
using Cake.Common.Diagnostics;

namespace Build.Tasks
{
    [TaskName("NugetPublish")]
    [IsDependentOn(typeof(TestTask))]
    public sealed class NugetPublishTask : FrostingTask<BuildContext>
    {
        public override void Run(BuildContext context)
        {
            var settings = new DotNetCorePackSettings()
            {
                OutputDirectory = context.NugetPublishLocation,
                Configuration = context.BuildConfiguration,
            };
            foreach (var (projectName, filePath) in context.NugetPackageProjectFilePaths())
            {
                context.DotNetCorePack(filePath.FullPath, settings);
                string readedVersion = context.XmlPeek(filePath.FullPath, "//Version");
                if (readedVersion == null)
                {
                    readedVersion = context.XmlPeek(filePath.FullPath, "//VersionPrefix");
                }
                var currentVersion = new Version(readedVersion);

                try
                {
                    context.GitTag(context.RepoDir, $"{projectName}/{currentVersion.Major}.{currentVersion.Minor}/{currentVersion.Major}.{currentVersion.Minor}.{currentVersion.Build}.{currentVersion.Revision}");
                }
                catch (Exception ex)
                {
                    context.Error($"Git tag failed with error: {ex.Message}");
                }
            }
        }
    }
}
