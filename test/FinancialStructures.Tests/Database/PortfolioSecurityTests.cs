using System.Collections.Generic;

using Effanville.FinancialStructures.Database;
using Effanville.FinancialStructures.Database.Implementation;

using NUnit.Framework;

namespace Effanville.FinancialStructures.Tests.Database
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
            DatabaseConstructor constructor = new DatabaseConstructor();
            constructor = constructor.WithSecurity("company1", "name1", sectors: firstSecuritySectors).WithSecurity("company2", "name2", sectors: secondSecuritySectors);
            Portfolio database = constructor.Database;
            IReadOnlyList<string> sectings = database.Sectors(Account.Security);

            Assert.AreEqual(sectors.Length, sectings.Count);
            for (int i = 0; i < sectors.Length; i++)
            {
                Assert.AreEqual(sectors[i], sectings[i]);
            }
        }
    }
}
