using System;
using FinancialStructures.Database;
using FinancialStructures.Database.Extensions.Values;
using NUnit.Framework;

namespace FinancialStructures.Tests.Database.Statistics
{
    [TestFixture]
    public sealed class LastDateTests
    {
        [TestCase(TestDatabaseName.OneBank, Totals.BankAccount, "2020/01/01")]
        [TestCase(TestDatabaseName.OneBank, Totals.Security, "0001/01/01")]
        [TestCase(TestDatabaseName.OneSec, Totals.BankAccount, "0001/01/01")]
        [TestCase(TestDatabaseName.OneSec, Totals.Security, "2020/01/01")]
        [TestCase(TestDatabaseName.TwoSec, Totals.BankAccount, "0001/01/01")]
        [TestCase(TestDatabaseName.TwoSec, Totals.Security, "2020/01/01")]
        [TestCase(TestDatabaseName.TwoBank, Totals.BankAccount, "2020/01/01")]
        [TestCase(TestDatabaseName.TwoBank, Totals.Security, "0001/01/01")]
        public void LastDateTest(TestDatabaseName databaseName, Totals totals, DateTime expected)
        {
            IPortfolio portfolio = TestDatabase.Databases[databaseName];
            Assert.AreEqual(expected, portfolio.LatestDate(totals));
        }
    }
}
