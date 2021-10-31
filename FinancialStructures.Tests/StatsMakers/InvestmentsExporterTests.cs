using System.IO.Abstractions.TestingHelpers;
using FinancialStructures.DataExporters.Investments;
using NUnit.Framework;

namespace FinancialStructures.Tests.StatsMakers
{
    [TestFixture]
    internal class InvestmentsExporterTests
    {
        [Test]
        public void CanGenerate()
        {
            var portfolio = TestDatabase.Databases[TestDatabaseName.OneSecOneBank];
            var investments = new PortfolioInvestments(portfolio, new PortfoliInvestmentsSettings());
            MockFileSystem tempFileSystem = new MockFileSystem();
            string savePath = "c:/temp/saved.csv";

            investments.ExportToFile(savePath, tempFileSystem);
            string file = tempFileSystem.File.ReadAllText(savePath);

            Assert.IsNotEmpty(file);
        }
    }
}
