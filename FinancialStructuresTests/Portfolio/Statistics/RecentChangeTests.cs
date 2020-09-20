using FinancialStructures.Database.Statistics;
using FinancialStructures.Tests.TestDatabaseConstructor;
using NUnit.Framework;

namespace FinancialStructuresTests.Portfolio.Statistics
{
    public class RecentChangeTest
    {
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
    }
}
