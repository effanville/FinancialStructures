using FinancialStructures.Database.Implementation;
using NUnit.Framework;

namespace FinancialStructures.Tests.Database
{
    [TestFixture]
    public class PortfolioTests
    {
        [TestCase("fish.txt", ".txt", "", "fish")]
        [TestCase("c:/dev/fish.txt", ".txt", "c:\\dev", "fish")]
        [TestCase("c:/dev/super.txt", ".txt", "c:\\dev", "super")]
        [TestCase("c:/dev/.txt", ".txt", "c:\\dev", "")]
        public void FilePathAttributes(string filePath, string extension, string directory, string databaseName)
        {
            var portfolio = new Portfolio();
            portfolio.SetFilePath(filePath);
            Assert.AreEqual(filePath, portfolio.FilePath);
            Assert.AreEqual(extension, portfolio.Extension);
            Assert.AreEqual(directory, portfolio.Directory);
            Assert.AreEqual(databaseName, portfolio.DatabaseName);
        }
    }
}
