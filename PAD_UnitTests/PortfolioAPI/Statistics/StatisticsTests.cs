using System;
using FinancialStructures.PortfolioAPI;
using FinancialStructures_UnitTests.TestDatabaseConstructor;
using NUnit.Framework;

namespace FinancialStructures_UnitTests.PortfolioAPI.Statistics
{
    [TestFixture]
    public sealed class StatisticsTests
    {
        [Test]
        public void LongestNameTests()
        {
            var constructor = new DatabaseConstructor();
            constructor.WithDefaultBankAccount();
            constructor.WithSecondaryBankAccount();
            constructor.WithSecondarySecurity();
            constructor.WithDefaultSecurity();
            var portfolio = constructor.database;
            Assert.AreEqual(8, portfolio.LongestName());
        }

        [Test]
        public void LongestCompanyTests()
        {
            var constructor = new DatabaseConstructor();
            constructor.WithDefaultBankAccount();
            constructor.WithDefaultSecurity();
            constructor.WithSecondaryBankAccount();
            constructor.WithSecondarySecurity();
            var portfolio = constructor.database;
            Assert.AreEqual(10, portfolio.LongestCompany());
        }

        [Test]
        public void FirstValueDateTests()
        {
            var constructor = new DatabaseConstructor();
            constructor.WithDefaultBankAccount();
            constructor.WithDefaultSecurity();
            constructor.WithSecondaryBankAccount();
            constructor.WithSecondarySecurity();
            var portfolio = constructor.database;
            Assert.AreEqual(new DateTime(2010, 1, 1), portfolio.FirstValueDate());
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
            Assert.AreEqual(26081.099999999999, portfolio.TotalProfit());
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

        [TestCase("2010/1/1", 1400)]
        [TestCase("2015/5/1", 18220.028512967812)]
        [TestCase("2010/5/1", 3479.1836734693879)]
        [TestCase("2020/1/1", 27186.299999999999)]
        [TestCase("2018/10/23", 37785.153651090215)]
        public void ValueTests(DateTime date, double expected)
        {
            var constructor = new DatabaseConstructor();
            constructor.WithDefaultBankAccount();
            constructor.WithDefaultSecurity();
            constructor.WithSecondaryBankAccount();
            constructor.WithSecondarySecurity();
            var portfolio = constructor.database;
            Assert.AreEqual(expected, portfolio.Value(date));
        }
    }
}
