using System;
using FinancialStructures.Database.Statistics;
using FinancialStructures.FinanceInterfaces;
using FinancialStructures.Tests.TestDatabaseConstructor;
using NUnit.Framework;

namespace FinancialStructuresTests.Portfolio.Statistics
{
    public sealed class IRRTests
    {
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
            Assert.AreEqual(expected, portfolio.IRRTotal(Totals.All, earlier, later));
        }
    }
}
