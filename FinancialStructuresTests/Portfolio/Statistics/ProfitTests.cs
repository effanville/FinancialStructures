using FinancialStructures.Database;
using FinancialStructures.Database.Statistics;
using FinancialStructures.Tests.TestDatabaseConstructor;
using NUnit.Framework;

namespace FinancialStructures.Tests.Database.Statistics
{
    [TestFixture]
    public sealed class ProfitTests
    {
        // some of these values dont seem what they should be
        [TestCase(TestDatabaseName.OneBank, Account.All, NameOrder.Default, double.NaN)]
        [TestCase(TestDatabaseName.OneBank, Account.Security, NameOrder.Default, double.NaN)]
        [TestCase(TestDatabaseName.OneBank, Account.BankAccount, NameOrder.Default, 1.0999999999999943)]
        [TestCase(TestDatabaseName.TwoSecTwoBank, Account.All, NameOrder.Default, double.NaN)]
        [TestCase(TestDatabaseName.TwoSecTwoBank, Account.Security, NameOrder.Default, 555.04999999999995)]
        [TestCase(TestDatabaseName.TwoSec, Account.Security, NameOrder.Default, 555.04999999999995)]
        public void ProfitTest(TestDatabaseName databaseName, Account totals, NameOrder order, double expectedValue)
        {
            var portfolio = TestDatabase.Databases[databaseName];
            Assert.AreEqual(expectedValue, portfolio.Profit(totals, TestDatabase.Name(totals, order)));
        }

        [TestCase(TestDatabaseName.OneBank, Totals.All, 1.0999999999999943)]
        [TestCase(TestDatabaseName.OneBank, Totals.Security, 0.0)]
        [TestCase(TestDatabaseName.OneBank, Totals.BankAccount, 1.0999999999999943)]
        [TestCase(TestDatabaseName.TwoSecTwoBank, Totals.All, 25983.299999999999)]
        [TestCase(TestDatabaseName.TwoSecTwoBank, Totals.Security, 26081.099999999999)]
        [TestCase(TestDatabaseName.TwoSec, Totals.Security, 26081.099999999999)]
        public void TotalProfitTests(TestDatabaseName databaseName, Totals totals, double expectedValue)
        {
            var portfolio = TestDatabase.Databases[databaseName];
            Assert.AreEqual(expectedValue, portfolio.TotalProfit(totals));
        }
    }
}
