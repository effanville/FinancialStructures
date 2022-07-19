using FinancialStructures.Database;
using FinancialStructures.Database.Extensions.Statistics;
using NUnit.Framework;

namespace FinancialStructures.Tests.Database.Statistics
{
    [TestFixture]
    public sealed class RecentChangeTest
    {
        [TestCase(TestDatabaseName.OneBank, Totals.Security, 0.0)]
        [TestCase(TestDatabaseName.OneBank, Totals.BankAccount, 23.4)]
        [TestCase(TestDatabaseName.OneSec, Totals.Security, 113.16)]
        [TestCase(TestDatabaseName.OneSec, Totals.BankAccount, 0.0)]
        [TestCase(TestDatabaseName.TwoBank, Totals.Security, 0.0)]
        [TestCase(TestDatabaseName.TwoBank, Totals.BankAccount, 253.8)]
        [TestCase(TestDatabaseName.TwoSec, Totals.Security, -14553.68)]
        [TestCase(TestDatabaseName.TwoSec, Totals.BankAccount, 0.0)]
        [TestCase(TestDatabaseName.OneSecOneBank, Totals.Security, 113.16)]
        [TestCase(TestDatabaseName.OneSecOneBank, Totals.BankAccount, 23.4)]
        [TestCase(TestDatabaseName.TwoSecTwoBank, Totals.Security, -14553.68)]
        [TestCase(TestDatabaseName.TwoSecTwoBank, Totals.BankAccount, 253.8)]
        public void TotalRecentChangeTests(TestDatabaseName databaseName, Totals totals, decimal expectedValue)
        {
            IPortfolio portfolio = TestDatabase.Databases[databaseName];
            Assert.AreEqual(expectedValue, portfolio.RecentChange(totals));
        }

        [TestCase(TestDatabaseName.OneBank, Account.All, NameOrder.Default, 0)]
        [TestCase(TestDatabaseName.OneBank, Account.Security, NameOrder.Default, 0)]
        [TestCase(TestDatabaseName.OneBank, Account.BankAccount, NameOrder.Default, 23.4)]
        [TestCase(TestDatabaseName.TwoSecTwoBank, Account.All, NameOrder.Default, 0)]
        [TestCase(TestDatabaseName.TwoSecTwoBank, Account.Security, NameOrder.Default, 113.16)]
        [TestCase(TestDatabaseName.TwoSec, Account.Security, NameOrder.Default, 113.16)]
        [TestCase(TestDatabaseName.TwoBankCur, Account.Currency, NameOrder.Default, 0.0087)]
        public void RecentChangeTests(TestDatabaseName databaseName, Account totals, NameOrder order, decimal expectedValue)
        {
            IPortfolio portfolio = TestDatabase.Databases[databaseName];
            Assert.AreEqual(expectedValue, portfolio.RecentChange(totals, TestDatabase.Name(totals, order)));
        }
    }
}
