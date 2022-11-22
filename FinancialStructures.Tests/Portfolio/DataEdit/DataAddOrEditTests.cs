using System;
using System.Linq;
using Common.Structure.DataStructures;
using FinancialStructures.Database;
using FinancialStructures.Database.Extensions;
using FinancialStructures.Database.Implementation;
using FinancialStructures.DataStructures;
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
            _ = portfolio.TryAddOrEditData(Account.Security, new TwoName("Company", "Name"), new DailyValuation(new DateTime(2010, 1, 1), 1), new DailyValuation(new DateTime(2010, 1, 1), 1));
            bool success = portfolio.TryAddOrEditTradeData(Account.Security, new TwoName("Company", "Name"), new SecurityTrade(new DateTime(2010, 1, 1)), new SecurityTrade(TradeType.Buy, new TwoName("Company", "Name"), new DateTime(2010, 1, 1), 1, 1, 0));

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
            _ = constructor.WithSecurity("Company", "Name", dates: new DateTime[] { new DateTime(2010, 1, 1) }, sharePrice: new decimal[] { 2.0m }, numberUnits: new decimal[] { 100.0m }, investment: new decimal[] { 0.0m });

            Portfolio portfolio = constructor.Database;

            _ = portfolio.TryAddOrEditData(Account.Security, new TwoName("Company", "Name"), new DailyValuation(new DateTime(2010, 1, 1), 1), new DailyValuation(new DateTime(2010, 1, 1), 1));
            bool success = portfolio.TryAddOrEditTradeData(Account.Security, new TwoName("Company", "Name"), new SecurityTrade(new DateTime(2010, 1, 1)), new SecurityTrade(TradeType.Buy, new TwoName("Company", "Name"), new DateTime(2010, 1, 1), 1, 1, 0));

            Assert.IsTrue(success);
            Assert.AreEqual(1, portfolio.Funds.Single().Count());
        }

        [Test]
        public void CanEditToSector()
        {
            DatabaseConstructor constructor = new DatabaseConstructor();
            _ = constructor.WithSectorFromNameAndData("Company", "Name", date: new DateTime[] { new DateTime(2010, 1, 1) }, value: new decimal[] { 2.0m });

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

            _ = portfolio.TryAddOrEditData(Account.Security, new TwoName("Company", "Name"), new DailyValuation(new DateTime(2010, 1, 1), 1), new DailyValuation(new DateTime(2010, 1, 1), 1));
            bool success = portfolio.TryAddOrEditTradeData(Account.Security, new TwoName("Company", "Name"), new SecurityTrade(new DateTime(2010, 1, 1)), new SecurityTrade(TradeType.Buy, new TwoName("Company", "Name"), new DateTime(2010, 1, 1), 1, 1, 0));

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
            _ = constructor.WithSecurity("Company", "Name", dates: new DateTime[] { new DateTime(2010, 1, 1) }, sharePrice: new decimal[] { 2.0m }, numberUnits: new decimal[] { 100.0m }, investment: new decimal[] { 0.0m });

            Portfolio portfolio = constructor.Database;
            _ = portfolio.TryAddOrEditData(Account.Security, new TwoName("Company", "Name"), new DailyValuation(new DateTime(2010, 1, 1), 1), new DailyValuation(new DateTime(2010, 1, 1), 1));
            bool success = portfolio.TryAddOrEditTradeData(Account.Security, new TwoName("Company", "Name"), new SecurityTrade(new DateTime(2010, 1, 1)), new SecurityTrade(TradeType.Buy, new TwoName("Company", "Name"), new DateTime(2010, 1, 1), 1, 1, 0));

            Assert.IsTrue(success);
            Assert.AreEqual(1, portfolio.Funds.Single().Count());
        }

        [Test]
        public void CanEditToSector2()
        {
            DatabaseConstructor constructor = new DatabaseConstructor();
            _ = constructor.WithSectorFromNameAndData("Company", "Name", date: new DateTime[] { new DateTime(2010, 1, 1) }, value: new decimal[] { 2.0m });

            Portfolio portfolio = constructor.Database;

            bool success = portfolio.TryAddOrEditData(Account.Benchmark, new TwoName("Company", "Name"), new DailyValuation(new DateTime(2010, 1, 1), 2.0m), new DailyValuation(new DateTime(2011, 1, 1), 1));

            Assert.IsTrue(success);
            Assert.AreEqual(1, portfolio.BenchMarks.Single().Count());
            DailyValuation values = portfolio.BenchMarks.Single().FirstValue();
            Assert.AreEqual(1, values.Value);
            Assert.AreEqual(new DateTime(2011, 1, 1), values.Day);
        }
    }
}
