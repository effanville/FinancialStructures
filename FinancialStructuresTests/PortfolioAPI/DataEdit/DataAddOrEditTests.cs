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
    public sealed class DataAddorEditTests
    {
        [Test]
        public void CanAddToSecurity()
        {
            var constructor = new DatabaseConstructor();
            constructor.WithSecurityFromName("Company", "Name");

            var portfolio = constructor.database;

            bool success = portfolio.TryAddOrEditDataToSecurity(new TwoName("Company", "Name"), new DateTime(2010, 1, 1), 1, 1, 1);

            Assert.IsTrue(success);
            Assert.AreEqual(1, portfolio.Funds.Single().Count());
        }

        [Test]
        public void CanAddToSector()
        {
            var constructor = new DatabaseConstructor();
            constructor.WithSectorFromName("Company", "Name");

            var portfolio = constructor.database;

            bool success = portfolio.TryAddOrEditData(AccountType.Sector, new NameData("Company", "Name"), new DailyValuation(new DateTime(2010, 1, 1), 1));

            Assert.IsTrue(success);
            Assert.AreEqual(1, portfolio.BenchMarks.Single().Count());
        }

        [Test]
        public void CanEditToSecurity()
        {
            var constructor = new DatabaseConstructor();
            constructor.WithSecurityFromNameAndData("Company", "Name", dates: new DateTime[] { new DateTime(2010, 1, 1) }, sharePrice: new double[] { 2.0 }, numberUnits: new double[] { 100.0 }, investment: new double[] { 0.0 });

            var portfolio = constructor.database;

            bool success = portfolio.TryAddOrEditDataToSecurity(new TwoName("Company", "Name"), new DateTime(2010, 1, 1), 1, 1, 1);

            Assert.IsTrue(success);
            Assert.AreEqual(1, portfolio.Funds.Single().Count());
        }

        [Test]
        public void CanEditToSector()
        {
            var constructor = new DatabaseConstructor();
            constructor.WithSectorFromNameAndData("Company", "Name", date: new DateTime[] { new DateTime(2010, 1, 1) }, value: new double[] { 2.0 });

            var portfolio = constructor.database;

            bool success = portfolio.TryAddOrEditData(AccountType.Sector, new NameData("Company", "Name"), new DailyValuation(new DateTime(2010, 1, 1), 1));

            Assert.IsTrue(success);
            Assert.AreEqual(1, portfolio.BenchMarks.Single().Count());
            var values = portfolio.BenchMarks.Single().FirstValue();
            Assert.AreEqual(1, values.Value);
            Assert.AreEqual(new DateTime(2010, 1, 1), values.Day);
        }
    }
}
