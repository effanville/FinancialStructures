using System.IO;
using Cake.Core.IO;
using Newtonsoft.Json;

namespace Build
{
    /// <summary>
    /// Class for all configuration information 
    /// </summary>
    public sealed class UserConfiguration
    {
        public string SolutionName
        {
            get; set;
        }

        public ProjectInfo[] ExecutablePublishProjects
        {
            get; set;
        }

        public ProjectInfo[] NugetPackageProjects
        {
            get; set;
        }

        public string SolutionFileName
        {
            get => $"{SolutionName}.sln";
        }

        public string DefaultBuildConfiguration
        {
            get; set;
        }

        public string DefaultFramework
        {
            get; set;
        }

        public string DefaultRuntime
        {
            get; set;
        }

        public string DefaultPublishDir
        {
            get; set;
        }

        public UserConfiguration()
        {
        }

        public static UserConfiguration ReadConfig(DirectoryPath repoDir)
        {
            string configFilePath = repoDir.CombineWithFilePath("build.config").ToString();
            // deserialize JSON directly from a file
            using (StreamReader file = File.OpenText(configFilePath))
            {
                JsonSerializer serializer = new JsonSerializer();
                return (UserConfiguration)serializer.Deserialize(file, typeof(UserConfiguration));
            }
        }
    }
}
