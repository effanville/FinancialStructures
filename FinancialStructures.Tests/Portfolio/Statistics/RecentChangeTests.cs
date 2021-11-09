using FinancialStructures.Database;
using FinancialStructures.Database.Extensions.Statistics;
using NUnit.Framework;

namespace FinancialStructures.Tests.Database.Statistics
{
    public class RecentChangeTest
    {
        [TestCase(TestDatabaseName.OneBank, Totals.Security, 0.0)]
        [TestCase(TestDatabaseName.OneBank, Totals.BankAccount, 23.399999999999991)]
        [TestCase(TestDatabaseName.OneSec, Totals.Security, 128.69999999999993)]
        [TestCase(TestDatabaseName.OneSec, Totals.BankAccount, 0.0)]
        [TestCase(TestDatabaseName.TwoBank, Totals.Security, 0.0)]
        [TestCase(TestDatabaseName.TwoBank, Totals.BankAccount, 253.79999999999995)]
        [TestCase(TestDatabaseName.TwoSec, Totals.Security, -19496.099999999999)]
        [TestCase(TestDatabaseName.TwoSec, Totals.BankAccount, 0.0)]
        [TestCase(TestDatabaseName.OneSecOneBank, Totals.Security, 128.69999999999993)]
        [TestCase(TestDatabaseName.OneSecOneBank, Totals.BankAccount, 23.399999999999991)]
        [TestCase(TestDatabaseName.TwoSecTwoBank, Totals.Security, -19496.099999999999)]
        [TestCase(TestDatabaseName.TwoSecTwoBank, Totals.BankAccount, 253.79999999999995)]
        public void TotalRecentChangeTests(TestDatabaseName databaseName, Totals totals, double expectedValue)
        {
            IPortfolio portfolio = TestDatabase.Databases[databaseName];
            Assert.AreEqual(expectedValue, portfolio.RecentChange(totals));
        }

        [TestCase(TestDatabaseName.OneBank, Account.All, NameOrder.Default, double.NaN)]
        [TestCase(TestDatabaseName.OneBank, Account.Security, NameOrder.Default, double.NaN)]
        [TestCase(TestDatabaseName.OneBank, Account.BankAccount, NameOrder.Default, 23.399999999999991)]
        [TestCase(TestDatabaseName.TwoSecTwoBank, Account.All, NameOrder.Default, double.NaN)]
        [TestCase(TestDatabaseName.TwoSecTwoBank, Account.Security, NameOrder.Default, 128.69999999999993)]
        [TestCase(TestDatabaseName.TwoSec, Account.Security, NameOrder.Default, 128.69999999999993)]
        [TestCase(TestDatabaseName.TwoBankCur, Account.Currency, NameOrder.Default, 0.0086999999999999994)]
        public void RecentChangeTests(TestDatabaseName databaseName, Account totals, NameOrder order, double expectedValue)
        {
            IPortfolio portfolio = TestDatabase.Databases[databaseName];
            Assert.AreEqual(expectedValue, portfolio.RecentChange(totals, TestDatabase.Name(totals, order)));
        }
    }
}
