using FinancialStructures.Database;
using FinancialStructures.Database.Statistics;
using FinancialStructures.Tests.TestDatabaseConstructor;
using NUnit.Framework;

namespace FinancialStructures.Tests.Database.Statistics
{
    [TestFixture]
    public sealed class ProfitTests
    {
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
