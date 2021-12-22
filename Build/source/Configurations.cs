namespace Build
{
    internal static class Configurations
    {
        public const string SolutionName = "FinancePortfolioDatabase";
        public static readonly (string FolderName, string ProjectName)[] ExecutablePublishProjects =
            new (string FolderName, string ProjectName)[]
            {
                ("FinancePortfolioDatabase", "FinancePortfolioDatabase"),
                ("FPDConsole", "FPDConsole"),
            };
        public static readonly (string FolderName, string ProjectName)[] NugetPackageProjects =
            new (string FolderName, string ProjectName)[]
            {
                ("FinancialStructures", "FinancialStructures"),
            };

        public static readonly string SolutionFileName = $"{SolutionName}.sln";

        public const string DefaultBuildConfiguration = "Release";
        public const string DefaultFramework = "net5.0-windows";
        public const string DefaultRuntime = "win-x64";
        public const string DefaultPublishDir = "../Publish";
    }
}
