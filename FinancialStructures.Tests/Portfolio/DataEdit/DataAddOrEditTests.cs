using System;
using System.Linq;
using Common.Structure.DataStructures;
using FinancialStructures.Database;
using FinancialStructures.Database.Implementation;
using FinancialStructures.NamingStructures;
using FinancialStructures.Tests.TestDatabaseConstructor;
using NUnit.Framework;

namespace FinancialStructures.Tests.Database.DataEdit
{
    [TestFixture]
    public sealed class DataAddorEditTests
    {
        [Test]
        public void CanAddToSecurity()
        {
            DatabaseConstructor constructor = new DatabaseConstructor();
            _ = constructor.WithSecurity("Company", "Name");

            Portfolio portfolio = constructor.Database;
            bool success = portfolio.TryAddOrEditDataToSecurity(new TwoName("Company", "Name"), new DateTime(2010, 1, 1), new DateTime(2010, 1, 1), 1, 1, 1, null);

            Assert.IsTrue(success);
            Assert.AreEqual(1, portfolio.Funds.Single().Count());
        }

        [Test]
        public void CanAddToSector()
        {
            DatabaseConstructor constructor = new DatabaseConstructor();
            _ = constructor.WithSectorFromName("Company", "Name");

            Portfolio portfolio = constructor.Database;
            DailyValuation data = new DailyValuation(new DateTime(2010, 1, 1), 1);
            bool success = portfolio.TryAddOrEditData(Account.Benchmark, new TwoName("Company", "Name"), data, data);

            Assert.IsTrue(success);
            Assert.AreEqual(1, portfolio.BenchMarks.Single().Count());
        }

        [Test]
        public void CanEditToSecurity()
        {
            DatabaseConstructor constructor = new DatabaseConstructor();
            _ = constructor.WithSecurity("Company", "Name", dates: new DateTime[] { new DateTime(2010, 1, 1) }, sharePrice: new double[] { 2.0 }, numberUnits: new double[] { 100.0 }, investment: new double[] { 0.0 });

            Portfolio portfolio = constructor.Database;

            bool success = portfolio.TryAddOrEditDataToSecurity(new TwoName("Company", "Name"), new DateTime(2010, 1, 1), new DateTime(2010, 1, 1), 1, 1, 1, null);

            Assert.IsTrue(success);
            Assert.AreEqual(1, portfolio.Funds.Single().Count());
        }

        [Test]
        public void CanEditToSector()
        {
            DatabaseConstructor constructor = new DatabaseConstructor();
            _ = constructor.WithSectorFromNameAndData("Company", "Name", date: new DateTime[] { new DateTime(2010, 1, 1) }, value: new double[] { 2.0 });

            Portfolio portfolio = constructor.Database;
            bool success = portfolio.TryAddOrEditData(Account.Benchmark, new TwoName("Company", "Name"), new DailyValuation(new DateTime(2010, 1, 1), 2), new DailyValuation(new DateTime(2010, 1, 1), 1));

            Assert.IsTrue(success);
            Assert.AreEqual(1, portfolio.BenchMarks.Single().Count());
            DailyValuation values = portfolio.BenchMarks.Single().FirstValue();
            Assert.AreEqual(1, values.Value);
            Assert.AreEqual(new DateTime(2010, 1, 1), values.Day);
        }

        [Test]
        public void CanAddToSecurity2()
        {
            DatabaseConstructor constructor = new DatabaseConstructor();
            _ = constructor.WithSecurity("Company", "Name");

            Portfolio portfolio = constructor.Database;

            bool success = portfolio.TryAddOrEditDataToSecurity(new TwoName("Company", "Name"), new DateTime(2010, 1, 1), new DateTime(2010, 1, 1), 1, 1, 1, null);

            Assert.IsTrue(success);
            Assert.AreEqual(1, portfolio.Funds.Single().Count());
        }

        [Test]
        public void CanAddToSector2()
        {
            DatabaseConstructor constructor = new DatabaseConstructor();
            _ = constructor.WithSectorFromName("Company", "Name");

            Portfolio portfolio = constructor.Database;

            bool success = portfolio.TryAddOrEditData(Account.Benchmark, new TwoName("Company", "Name"), new DailyValuation(new DateTime(2010, 1, 1), 1), new DailyValuation(new DateTime(2010, 1, 1), 1));

            Assert.IsTrue(success);
            Assert.AreEqual(1, portfolio.BenchMarks.Single().Count());
        }

        [Test]
        public void CanEditToSecurity2()
        {
            DatabaseConstructor constructor = new DatabaseConstructor();
            _ = constructor.WithSecurity("Company", "Name", dates: new DateTime[] { new DateTime(2010, 1, 1) }, sharePrice: new double[] { 2.0 }, numberUnits: new double[] { 100.0 }, investment: new double[] { 0.0 });

            Portfolio portfolio = constructor.Database;

            bool success = portfolio.TryAddOrEditDataToSecurity(new TwoName("Company", "Name"), new DateTime(2010, 1, 1), new DateTime(2020, 1, 1), 1, 1, 1, null);

            Assert.IsTrue(success);
            Assert.AreEqual(1, portfolio.Funds.Single().Count());
        }

        [Test]
        public void CanEditToSector2()
        {
            DatabaseConstructor constructor = new DatabaseConstructor();
            _ = constructor.WithSectorFromNameAndData("Company", "Name", date: new DateTime[] { new DateTime(2010, 1, 1) }, value: new double[] { 2.0 });

            Portfolio portfolio = constructor.Database;

            bool success = portfolio.TryAddOrEditData(Account.Benchmark, new TwoName("Company", "Name"), new DailyValuation(new DateTime(2010, 1, 1), 2.0), new DailyValuation(new DateTime(2011, 1, 1), 1));

            Assert.IsTrue(success);
            Assert.AreEqual(1, portfolio.BenchMarks.Single().Count());
            DailyValuation values = portfolio.BenchMarks.Single().FirstValue();
            Assert.AreEqual(1, values.Value);
            Assert.AreEqual(new DateTime(2011, 1, 1), values.Day);
        }
    }
}
