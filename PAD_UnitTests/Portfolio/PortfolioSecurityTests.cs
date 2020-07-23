using System;
using FinancialStructures.Database;
using FinancialStructures.NamingStructures;
using FinancialStructures_UnitTests.TestDatabaseConstructor;
using NUnit.Framework;

namespace FinancialStructures_UnitTests.Database
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
            var sectings = database.GetSecuritiesSectors();

            Assert.AreEqual(sectors.Length, sectings.Count);
            for (int i = 0; i < sectors.Length; i++)
            {
                Assert.AreEqual(sectors[i], sectings[i]);
            }
        }

        [TestCase(SecurityDataStream.NumberOfShares, 12)]
        [TestCase(SecurityDataStream.SharePrice, 101)]
        public void TestSecurityPrices(SecurityDataStream stream, double expected)
        {
            var constructor = new DatabaseConstructor();
            constructor = constructor.WithSecurityFromNameAndDataPoint("company1", "name1", date: new DateTime(2000, 1, 1), sharePrice: 101, numberUnits: 12).WithSecurityFromNameAndDataPoint("company2", "name2", date: new DateTime(2000, 1, 1), sharePrice: 202, numberUnits: 52);
            var database = constructor.database;

            var value = database.SecurityPrices(new TwoName("company1", "name1"), new DateTime(2000, 2, 1), stream);

            Assert.AreEqual(expected, value);
        }
    }
}
