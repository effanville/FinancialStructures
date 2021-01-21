using FinancialStructures.Database.Statistics;
using FinancialStructures.Tests.TestDatabaseConstructor;
using NUnit.Framework;

namespace FinancialStructures.Tests.Database.Statistics
{
    public class RecentChangeTest
    {
        [TestCase(TestDatabaseName.OneBank, 0.0)]
        [TestCase(TestDatabaseName.OneSec, 113.15999999999991)]
        [TestCase(TestDatabaseName.TwoBank, 0.0)]
        [TestCase(TestDatabaseName.TwoSec, -14553.68)]
        [TestCase(TestDatabaseName.OneSecOneBank, 113.15999999999991)]
        [TestCase(TestDatabaseName.TwoSecTwoBank, -14553.68)]
        public void RecentChangeTests(TestDatabaseName databaseName, double expectedValue)
        {
            var portfolio = TestDatabase.Databases[databaseName];
            Assert.AreEqual(expectedValue, portfolio.RecentChange());
        }
    }
}
