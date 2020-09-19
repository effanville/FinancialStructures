using System;
using FinancialStructures.Database.Statistics;
using FinancialStructures.Tests.TestDatabaseConstructor;
using FinancialStructures.FinanceInterfaces;
using NUnit.Framework;

namespace FinancialStructures.Tests.PortfolioAPI.Statistics
{
    [TestFixture]
    public sealed class StatisticsTests
    {
        [Test]
        public void FirstValueDateTests()
        {
            var constructor = new DatabaseConstructor();
            constructor.WithDefaultBankAccount();
            constructor.WithDefaultSecurity();
            constructor.WithSecondaryBankAccount();
            constructor.WithSecondarySecurity();
            var portfolio = constructor.database;
            Assert.AreEqual(new DateTime(2010, 1, 1), portfolio.FirstValueDate(AccountType.All));
        }

        [Test]
        public void TotalProfitTests()
        {
            var constructor = new DatabaseConstructor();
            constructor.WithDefaultBankAccount();
            constructor.WithDefaultSecurity();
            constructor.WithSecondaryBankAccount();
            constructor.WithSecondarySecurity();
            var portfolio = constructor.database;
            Assert.AreEqual(26081.099999999999, portfolio.TotalProfit(AccountType.All));
        }

        [Test]
        public void RecentChangeTests()
        {
            var constructor = new DatabaseConstructor();
            constructor.WithDefaultBankAccount();
            constructor.WithDefaultSecurity();
            constructor.WithSecondaryBankAccount();
            constructor.WithSecondarySecurity();
            var portfolio = constructor.database;
            Assert.AreEqual(-14553.68, portfolio.RecentChange());
        }

        [TestCase("2010/1/1", "2019/1/1", 0.79150390625)]
        [TestCase("2011/1/1", "2019/1/1", 0.41650390625)]
        [TestCase("2012/1/1", "2019/1/1", 0.42822265625)]
        [TestCase("2013/1/1", "2019/1/1", 0.095018679818698937)]
        [TestCase("2014/1/1", "2019/1/1", 0.13186306269750059)]
        public void IRRPortfolioTests(DateTime earlier, DateTime later, double expected)
        {
            var constructor = new DatabaseConstructor();
            constructor.WithDefaultBankAccount();
            constructor.WithDefaultSecurity();
            constructor.WithSecondaryBankAccount();
            constructor.WithSecondarySecurity();
            var portfolio = constructor.database;
            Assert.AreEqual(expected, portfolio.IRRPortfolio(earlier, later));
        }
    }
}
