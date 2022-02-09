using System.Collections.Generic;

using Cake.Common.IO;
using Cake.Core;
using Cake.Core.IO;
using Cake.Frosting;

namespace Build
{
    public class BuildContext : FrostingContext
    {
        #region InputParameters
        public string BuildConfiguration
        {
            get;
        }

        public string Framework
        {
            get;
        }

        public string Runtime
        {
            get;
        }

        private string SolutionFileName
        {
            get;
        }

        public ProjectInfo[] NugetPublishProjects
        {
            get;
        }

        public ProjectInfo[] ExeProjects
        {
            get;
        }


        #endregion

        public DirectoryPath RepoDir
        {
            get;
        }

        public DirectoryPath PublishLocation
        {
            get;
        }

        public DirectoryPath NugetPublishLocation
        {
            get;
        }

        public bool IsProductionBuild
        {
            get;
        }

        public BuildContext(ICakeContext context)
            : base(context)
        {
            RepoDir = context.MakeAbsolute(context.Directory("../../"));
            var config = UserConfiguration.ReadConfig(RepoDir);

            SolutionFileName = config.SolutionFileName;

            BuildConfiguration = context.Arguments.GetArgument("configuration") ?? config.DefaultBuildConfiguration;
            Framework = context.Arguments.GetArgument("framework") ?? config.DefaultFramework;
            string publishDir = context.Arguments.GetArgument("publishDir") ?? $"{config.DefaultPublishDir}/{config.SolutionName}/{Framework}";
            string nugetPublishDir = context.Arguments.GetArgument("publishDir") ?? $"{config.DefaultPublishDir}/{config.SolutionName}";
            Runtime = context.Arguments.GetArgument("runtime") ?? config.DefaultRuntime;
            IsProductionBuild = context.Arguments.HasArgument("isProd");

            PublishLocation = RepoDir + context.Directory($"{publishDir}");
            NugetPublishLocation = RepoDir + context.Directory($"{nugetPublishDir}");

            string nugetPackages = context.Arguments.GetArgument("nuget");
            if (!string.IsNullOrWhiteSpace(nugetPackages))
            {
                string[] projects = nugetPackages.Split(',');
                NugetPublishProjects = new ProjectInfo[projects.Length];
                for (int i = 0; i < projects.Length; i++)
                {
                    NugetPublishProjects[i] = new ProjectInfo(projects[i], projects[i]);
                }
            }
            else
            {
                NugetPublishProjects = config.NugetPackageProjects;
            }

            string exeProjects = context.Arguments.GetArgument("exe");
            if (!string.IsNullOrWhiteSpace(exeProjects))
            {
                string[] projects = exeProjects.Split(',');
                ExeProjects = new ProjectInfo[projects.Length];
                for (int i = 0; i < projects.Length; i++)
                {
                    ExeProjects[i] = new ProjectInfo(projects[i], projects[i]);
                }
            }
            else
            {
                ExeProjects = config.ExecutablePublishProjects;
            }
        }

        public FilePath SolutionFilePath()
        {
            return RepoDir.CombineWithFilePath(SolutionFileName);
        }

        public FilePath NugetConfigFilePath()
        {
            return RepoDir.CombineWithFilePath("nuget.config");
        }

        public (string ProjectName, FilePath FilePath)[] ExecutableProjectFilePaths()
        {
            var filePaths = new List<(string ProjectName, FilePath)>();
            foreach (var info in ExeProjects)
            {
                filePaths.Add((info.ProjectName, ((DirectoryPath)(RepoDir + this.Directory($"{info.FolderName}"))).CombineWithFilePath($"{info.ProjectName}.csproj")));
            }

            return filePaths.ToArray();
        }

        public (string ProjectName, FilePath FilePath)[] NugetPackageProjectFilePaths()
        {
            var filePaths = new List<(string, FilePath)>();
            foreach (var info in NugetPublishProjects)
            {
                filePaths.Add((info.ProjectName, ((DirectoryPath)(RepoDir + this.Directory($"{info.FolderName}"))).CombineWithFilePath($"{info.ProjectName}.csproj")));
            }

            return filePaths.ToArray();
        }
    }
}
