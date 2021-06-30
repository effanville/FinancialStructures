using System;
using System.Linq;
using FinancialStructures.NamingStructures;
using NUnit.Framework;
using Common.Structure.DataStructures;
using FinancialStructures.Database;

namespace FinancialStructures.Tests.Database.DataEdit
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
            bool success = portfolio.TryAddOrEditDataToSecurity(new TwoName("Company", "Name"), new DateTime(2010, 1, 1), new DateTime(2010, 1, 1), 1, 1, 1);

            Assert.IsTrue(success);
            Assert.AreEqual(1, portfolio.Funds.Single().Count());
        }

        [Test]
        public void CanAddToSector()
        {
            var constructor = new DatabaseConstructor();
            constructor.WithSectorFromName("Company", "Name");

            var portfolio = constructor.database;
            var data = new DailyValuation(new DateTime(2010, 1, 1), 1);
            bool success = portfolio.TryAddOrEditData(Account.Benchmark, new TwoName("Company", "Name"), data, data);

            Assert.IsTrue(success);
            Assert.AreEqual(1, portfolio.BenchMarks.Single().Count());
        }

        [Test]
        public void CanEditToSecurity()
        {
            var constructor = new DatabaseConstructor();
            constructor.WithSecurityFromNameAndData("Company", "Name", dates: new DateTime[] { new DateTime(2010, 1, 1) }, sharePrice: new double[] { 2.0 }, numberUnits: new double[] { 100.0 }, investment: new double[] { 0.0 });

            var portfolio = constructor.database;

            bool success = portfolio.TryAddOrEditDataToSecurity(new TwoName("Company", "Name"), new DateTime(2010, 1, 1), new DateTime(2010, 1, 1), 1, 1, 1);

            Assert.IsTrue(success);
            Assert.AreEqual(1, portfolio.Funds.Single().Count());
        }

        [Test]
        public void CanEditToSector()
        {
            var constructor = new DatabaseConstructor();
            constructor.WithSectorFromNameAndData("Company", "Name", date: new DateTime[] { new DateTime(2010, 1, 1) }, value: new double[] { 2.0 });

            var portfolio = constructor.database;
            bool success = portfolio.TryAddOrEditData(Account.Benchmark, new TwoName("Company", "Name"), new DailyValuation(new DateTime(2010, 1, 1), 2), new DailyValuation(new DateTime(2010, 1, 1), 1));

            Assert.IsTrue(success);
            Assert.AreEqual(1, portfolio.BenchMarks.Single().Count());
            var values = portfolio.BenchMarks.Single().FirstValue();
            Assert.AreEqual(1, values.Value);
            Assert.AreEqual(new DateTime(2010, 1, 1), values.Day);
        }

        [Test]
        public void CanAddToSecurity2()
        {
            var constructor = new DatabaseConstructor();
            constructor.WithSecurityFromName("Company", "Name");

            var portfolio = constructor.database;

            bool success = portfolio.TryAddOrEditDataToSecurity(new TwoName("Company", "Name"), new DateTime(2010, 1, 1), new DateTime(2010, 1, 1), 1, 1, 1);

            Assert.IsTrue(success);
            Assert.AreEqual(1, portfolio.Funds.Single().Count());
        }

        [Test]
        public void CanAddToSector2()
        {
            var constructor = new DatabaseConstructor();
            constructor.WithSectorFromName("Company", "Name");

            var portfolio = constructor.database;

            bool success = portfolio.TryAddOrEditData(Account.Benchmark, new TwoName("Company", "Name"), new DailyValuation(new DateTime(2010, 1, 1), 1), new DailyValuation(new DateTime(2010, 1, 1), 1));

            Assert.IsTrue(success);
            Assert.AreEqual(1, portfolio.BenchMarks.Single().Count());
        }

        [Test]
        public void CanEditToSecurity2()
        {
            var constructor = new DatabaseConstructor();
            constructor.WithSecurityFromNameAndData("Company", "Name", dates: new DateTime[] { new DateTime(2010, 1, 1) }, sharePrice: new double[] { 2.0 }, numberUnits: new double[] { 100.0 }, investment: new double[] { 0.0 });

            var portfolio = constructor.database;

            bool success = portfolio.TryAddOrEditDataToSecurity(new TwoName("Company", "Name"), new DateTime(2010, 1, 1), new DateTime(2020, 1, 1), 1, 1, 1);

            Assert.IsTrue(success);
            Assert.AreEqual(1, portfolio.Funds.Single().Count());
        }

        [Test]
        public void CanEditToSector2()
        {
            var constructor = new DatabaseConstructor();
            constructor.WithSectorFromNameAndData("Company", "Name", date: new DateTime[] { new DateTime(2010, 1, 1) }, value: new double[] { 2.0 });

            var portfolio = constructor.database;

            bool success = portfolio.TryAddOrEditData(Account.Benchmark, new TwoName("Company", "Name"), new DailyValuation(new DateTime(2010, 1, 1), 2.0), new DailyValuation(new DateTime(2011, 1, 1), 1));

            Assert.IsTrue(success);
            Assert.AreEqual(1, portfolio.BenchMarks.Single().Count());
            var values = portfolio.BenchMarks.Single().FirstValue();
            Assert.AreEqual(1, values.Value);
            Assert.AreEqual(new DateTime(2011, 1, 1), values.Day);
        }
    }
}
