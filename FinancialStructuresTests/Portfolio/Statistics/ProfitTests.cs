using FinancialStructures.Database;
using FinancialStructures.Database.Statistics;
using FinancialStructures.Tests.TestDatabaseConstructor;
using NUnit.Framework;

namespace FinancialStructuresTests.Portfolio.Statistics
{
    [TestFixture]
    public sealed class ProfitTests
    {
        [Test]
        public void TotalProfitTests()
        {
            var constructor = new DatabaseConstructor();
            constructor.WithDefaultBankAccount();
            constructor.WithDefaultSecurity();
            constructor.WithSecondaryBankAccount();
            constructor.WithSecondarySecurity();
            var portfolio = constructor.database;
            Assert.AreEqual(25983.299999999999, portfolio.TotalProfit(Totals.All));
        }
    }
}
