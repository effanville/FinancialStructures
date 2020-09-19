using System;
using System.Linq;
using FinancialStructures.NamingStructures;
using FinancialStructures.PortfolioAPI;
using FinancialStructures.Tests.TestDatabaseConstructor;
using NUnit.Framework;
using StructureCommon.DataStructures;

namespace FinancialStructures.Tests.PortfolioAPI.DataEdit
{
    public sealed class DataDeleteTests
    {
        [Test]
        public void CanDeleteFromSecurity()
        {
            var constructor = new DatabaseConstructor();
            constructor.WithSecurityFromNameAndData("Company", "Name", dates: new DateTime[] { new DateTime(2010, 1, 1) }, sharePrice: new double[] { 2.0 }, numberUnits: new double[] { 100.0 }, investment: new double[] { 0.0 });

            var portfolio = constructor.database;

            bool success = portfolio.TryDeleteData(AccountType.Security, new NameData("Company", "Name"), new DailyValuation(new DateTime(2010, 1, 1), 5), null);

            Assert.AreEqual(0, portfolio.Funds.Single().Count());

            // This is false as there is no investment value as value 0 is not stored
            Assert.IsFalse(success);
        }

        [Test]
        public void CanDeleteFromSector()
        {
            var constructor = new DatabaseConstructor();
            constructor.WithSectorFromNameAndData("Company", "Name", date: new DateTime[] { new DateTime(2010, 1, 1) }, value: new double[] { 2.0 });

            var portfolio = constructor.database;

            bool success = portfolio.TryDeleteData(AccountType.Sector, new NameData("Company", "Name"), new DailyValuation(new DateTime(2010, 1, 1), 2.0), null);

            Assert.IsTrue(success);
            Assert.AreEqual(0, portfolio.BenchMarks.Single().Count());
        }
    }
}
