using System;
using FinancialStructures.Database;
using FinancialStructures.Database.Extensions.Rates;
using NUnit.Framework;

namespace FinancialStructures.Tests.Database.Statistics
{
    [TestFixture]
    public sealed class IRRTests
    {
        [TestCase(TestDatabaseName.TwoSecTwoBank, Totals.All, "2010 /1/1", "2019/1/1", 0.074487209320068359)]
        [TestCase(TestDatabaseName.TwoSecTwoBank, Totals.All, "2011/1/1", "2019/1/1", 0.074704647064208984)]
        [TestCase(TestDatabaseName.TwoSecTwoBank, Totals.All, "2012/1/1", "2019/1/1", 0.070493221282958984)]
        [TestCase(TestDatabaseName.TwoSecTwoBank, Totals.All, "2013/1/1", "2019/1/1", 0.072749286956620862)]
        [TestCase(TestDatabaseName.TwoSecTwoBank, Totals.All, "2014/1/1", "2019/1/1", 0.10421917492311406)]
        [TestCase(TestDatabaseName.TwoSecTwoBank, Totals.Security, "2010 /1/1", "2019/1/1", 0.074487209320068359)]
        [TestCase(TestDatabaseName.TwoSecTwoBank, Totals.Security, "2011/1/1", "2019/1/1", 0.074704647064208984)]
        [TestCase(TestDatabaseName.TwoSecTwoBank, Totals.Security, "2012/1/1", "2019/1/1", 0.070493221282958984)]
        [TestCase(TestDatabaseName.TwoSecTwoBank, Totals.Security, "2013/1/1", "2019/1/1", 0.072749286956620862)]
        [TestCase(TestDatabaseName.TwoSecTwoBank, Totals.Security, "2014/1/1", "2019/1/1", 0.10421917492311406)]
        [TestCase(TestDatabaseName.TwoSecTwoBank, Totals.BankAccount, "2010 /1/1", "2019/1/1", -0.025712539370376319)]
        [TestCase(TestDatabaseName.TwoSecTwoBank, Totals.BankAccount, "2011/1/1", "2019/1/1", -0.09969157728161171)]
        [TestCase(TestDatabaseName.TwoSecTwoBank, Totals.BankAccount, "2012/1/1", "2019/1/1", -0.062068923361617845)]
        [TestCase(TestDatabaseName.TwoSecTwoBank, Totals.BankAccount, "2013/1/1", "2019/1/1", -0.037103019820752703)]
        [TestCase(TestDatabaseName.TwoSecTwoBank, Totals.BankAccount, "2014/1/1", "2019/1/1", -0.029457843771104053)]
        [TestCase(TestDatabaseName.TwoSec, Totals.BankAccount, "2010 /1/1", "2019/1/1", 0.0)]
        [TestCase(TestDatabaseName.TwoSec, Totals.BankAccount, "2011/1/1", "2019/1/1", 0.0)]
        public void IRRPortfolioTests(TestDatabaseName databaseName, Totals totals, DateTime earlier, DateTime later, double expected)
        {
            IPortfolio portfolio = TestDatabase.Databases[databaseName];
            Assert.AreEqual(expected, portfolio.TotalIRR(totals, earlier, later, numIterations: 20));
        }
    }
}
