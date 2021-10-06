using FinancialStructures.DataExporters.Statistics;
using FinancialStructures.Statistics;
using NUnit.Framework;

namespace FinancialStructures.Tests.DataExporters.Statistics
{
    [TestFixture]
    public sealed class UserOptionsTests
    {
        [Test]
        public void EnsureDefaults()
        {
            PortfolioStatisticsSettings options = PortfolioStatisticsSettings.DefaultSettings();

            Assert.AreEqual(SortDirection.Descending, options.SecurityDisplayOptions.SortingDirection);
            Assert.AreEqual(SortDirection.Descending, options.BankAccountDisplayOptions.SortingDirection);
            Assert.AreEqual(SortDirection.Descending, options.SectorDisplayOptions.SortingDirection);
            Assert.IsFalse(options.DisplayValueFunds);
            Assert.IsTrue(options.SecurityDisplayOptions.ShouldDisplay);
            Assert.IsTrue(options.SectorDisplayOptions.ShouldDisplay);
            Assert.IsTrue(options.BankAccountDisplayOptions.ShouldDisplay);
        }

        [Test]
        public void EnsureExportDefaults()
        {
            PortfolioStatisticsExportSettings options = PortfolioStatisticsExportSettings.DefaultSettings();

            Assert.IsFalse(options.Spacing);
            Assert.IsFalse(options.Colours);
        }
    }
}
