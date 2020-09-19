using System;
using System.Linq;
using FinancialStructures.NamingStructures;
using FinancialStructures.PortfolioAPI;
using FinancialStructures.Tests.TestDatabaseConstructor;
using NUnit.Framework;
using StructureCommon.DataStructures;

namespace FinancialStructures.Tests.PortfolioAPI.DataEdit
{
    [TestFixture]
    public sealed class DataAddTests
    {
        [Test]
        public void CanAddToSecurity()
        {
            var constructor = new DatabaseConstructor();
            constructor.WithSecurityFromName("Company", "Name");

            var portfolio = constructor.database;

            bool success = portfolio.TryAddDataToSecurity(new TwoName("Company", "Name"), new DateTime(2010, 1, 1), 1, 1, 1);

            Assert.IsTrue(success);
            Assert.AreEqual(1, portfolio.Funds.Single().Count());
        }

        [Test]
        public void CanAddToSector()
        {
            var constructor = new DatabaseConstructor();
            constructor.WithSectorFromName("Company", "Name");

            var portfolio = constructor.database;

            bool success = portfolio.TryAddData(AccountType.Sector, new NameData("Company", "Name"), new DailyValuation(new DateTime(2010, 1, 1), 1));

            Assert.IsTrue(success);
            Assert.AreEqual(1, portfolio.BenchMarks.Single().Count());
        }
    }
}
