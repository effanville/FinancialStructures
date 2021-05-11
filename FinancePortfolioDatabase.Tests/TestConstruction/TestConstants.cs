using System;

namespace FinancePortfolioDatabase.Tests.TestConstruction
{
    public static class TestConstants
    {
        public static readonly string CurrentPath = AppDomain.CurrentDomain.SetupInformation.ApplicationBase;

        public const string ExampleDatabaseFolder = "ExampleDatabases";

        public static readonly string ExampleDatabaseLocation = $"{CurrentPath}\\{ExampleDatabaseFolder}";
    }
}
