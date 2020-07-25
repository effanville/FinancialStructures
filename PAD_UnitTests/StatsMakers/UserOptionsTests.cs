using FinancialStructures.StatisticStructures;
using FinancialStructures.StatsMakers;
using NUnit.Framework;

namespace FinancialStructures_UnitTests.StatsMakers
{
    [TestFixture]
    public sealed class UserOptionsTests
    {
        [Test]
        public void EnsureDefaults()
        {
            var options = new UserOptions();

            Assert.AreEqual(SortDirection.Descending, options.SecuritySortDirection);
            Assert.AreEqual(SortDirection.Descending, options.BankSortDirection);
            Assert.AreEqual(SortDirection.Descending, options.SectorSortDirection);
            Assert.IsFalse(options.DisplayValueFunds);
            Assert.IsFalse(options.Spacing);
            Assert.IsFalse(options.Colours);
            Assert.IsTrue(options.ShowSecurites);
            Assert.IsTrue(options.ShowSectors);
            Assert.IsTrue(options.ShowBankAccounts);
        }
    }
}
