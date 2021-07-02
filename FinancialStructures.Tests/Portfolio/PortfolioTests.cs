using System.IO.Abstractions;
using FinancialStructures.Database;
using NUnit.Framework;

namespace FinancialStructures.Tests.Database
{
    [TestFixture]
    public class PortfolioTests
    {
        [TestCase("fish.txt", "fish")]
        [TestCase("c:/dev/fish.txt", "fish")]
        [TestCase("c:/dev/super.txt", "super")]
        [TestCase("c:/dev/.txt", "")]
        public void FilePathAttributes(string filePath, string databaseName)
        {
            IPortfolio portfolio = PortfolioFactory.GenerateEmpty();
            portfolio.FilePath = filePath;
            Assert.AreEqual(filePath, portfolio.FilePath);
            Assert.AreEqual(databaseName, portfolio.DatabaseName(new FileSystem()));
        }
    }
}
