using System;
using Cake.Common.Diagnostics;
using Cake.Common.IO;
using Cake.Core.IO;
using Cake.Git;
using Cake.VersionReader;

namespace Build.Tasks
{
    internal static class Helpers
    {
        internal static Version GetAssemblyVersion(this BuildContext context, string projectName, bool isExe)
        {
            DirectoryPath binDir = context.RepoDir + context.Directory($"bin/{context.BuildConfiguration}/{context.Framework}/{context.Runtime}");
            string extension = isExe ? "exe" : "dll";
            FilePath thing = binDir.CombineWithFilePath($"{projectName}.{extension}");
            string assemblyVersionString = context.GetFullVersionNumber(thing);
            var assemblyVersion = new Version(assemblyVersionString);
            return context.IsProductionBuild
                ? new Version(assemblyVersion.Major, assemblyVersion.Minor)
                : assemblyVersion;
        }

        internal static void TryGitTag(this BuildContext context, string projectName, Version version)
        {
            try
            {
                context.GitTag(context.RepoDir, $"{projectName}/{version.Major}.{version.Minor}/{version}");
            }
            catch (Exception ex)
            {
                context.Error($"Git tag failed with error: {ex.Message}");
            }
        }
    }
}
