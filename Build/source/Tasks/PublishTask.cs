using System;
using Cake.Common.Diagnostics;
using Cake.Common.IO;
using Cake.Common.Tools.DotNetCore;
using Cake.Common.Tools.DotNetCore.Publish;
using Cake.Common.Xml;
using Cake.Core.IO;
using Cake.Frosting;
using Cake.Git;

namespace Build.Tasks
{
    [TaskName("Publish")]
    [IsDependentOn(typeof(TestTask))]
    public sealed class PublishTask : FrostingTask<BuildContext>
    {
        public override void Run(BuildContext context)
        {
            foreach (var project in context.ExecutableProjectFilePaths())
            {
                string readedVersion = context.XmlPeek(project.FilePath.FullPath, "//Version");
                if (readedVersion == null)
                {
                    readedVersion = context.XmlPeek(project.FilePath.FullPath, "//VersionPrefix");
                }

                var currentVersion = new Version(readedVersion);
                DirectoryPath outputDir = context.PublishLocation + context.Directory($"{currentVersion.Major}.{currentVersion.Minor}.{currentVersion.Build}.{currentVersion.Revision}");
                if (context.DirectoryExists(outputDir))
                {
                    context.CleanDirectory(outputDir);
                }
                var settings = new DotNetCorePublishSettings()
                {
                    Framework = context.Framework,
                    Configuration = context.BuildConfiguration,
                    Runtime = context.Runtime,
                    OutputDirectory = outputDir,
                    SelfContained = true,
                    PublishSingleFile = true,
                    PublishReadyToRun = true,
                    NoBuild = true
                };

                context.DotNetCorePublish(project.FilePath.FullPath, settings);

                try
                {
                    context.GitTag(context.RepoDir, $"{project.ProjectName}/{currentVersion.Major}.{currentVersion.Minor}/{currentVersion.Major}.{currentVersion.Minor}.{currentVersion.Build}.{currentVersion.Revision}");
                }
                catch (Exception ex)
                {
                    context.Error($"Git tag failed with error: {ex.Message}");
                }
            }
        }
    }
}
