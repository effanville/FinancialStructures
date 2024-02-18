using System.IO.Abstractions.TestingHelpers;
using FinancialStructures.Database.Export.History;
using NUnit.Framework;

namespace FinancialStructures.Tests.StatsMakers
{
    [TestFixture]
    internal class CSVHIstoryWriterTests
    {
        [Test]
        public void CanGenerate()
        {
            var portfolio = TestDatabase.Databases[TestDatabaseName.OneSecOneBank];
            var history = new PortfolioHistory(portfolio, new PortfolioHistory.Settings());
            MockFileSystem tempFileSystem = new MockFileSystem();
            string savePath = "c:/temp/saved.csv";

            history.ExportToFile(savePath, tempFileSystem);
            string file = tempFileSystem.File.ReadAllText(savePath);

            Assert.IsNotEmpty(file);
        }
    }
}
