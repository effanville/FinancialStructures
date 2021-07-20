using FinancialStructures.Database;
using NUnit.Framework;

namespace FinancialStructures.Tests.Database
{
    [TestFixture]
    public class PortfolioSecurityTests
    {
        [TestCase("cat,hat", "fish,dog", "cat,dog,fish,hat")]
        [TestCase("cat,hat,fish", "fish,dog", "cat,dog,fish,hat")]
        [TestCase("cat,hat", "", "cat,hat")]
        [TestCase("", "", "")]
        public void GetSecuritySectorList(string firstSecuritySectors, string secondSecuritySectors, string sectorsFlat)
        {
            string[] sectors = sectorsFlat.Split(',');
            if (sectors.Length == 1 && sectors[0] == "")
            {
                sectors = new string[0];
            }
            var constructor = new DatabaseConstructor();
            constructor = constructor.WithSecurityFromName("company1", "name1", sectors: firstSecuritySectors).WithSecurityFromName("company2", "name2", sectors: secondSecuritySectors);
            var database = constructor.database;
            var sectings = database.Sectors(Account.Security);

            Assert.AreEqual(sectors.Length, sectings.Count);
            for (int i = 0; i < sectors.Length; i++)
            {
                Assert.AreEqual(sectors[i], sectings[i]);
            }
        }
    }
}
