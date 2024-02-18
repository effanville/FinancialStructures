using Effanville.FinancialStructures.Database.Export.Statistics;
using Effanville.FinancialStructures.Database.Statistics;

using NUnit.Framework;

namespace Effanville.FinancialStructures.Tests.StatsMakers
{
    [TestFixture]
    public sealed class UserOptionsTests
    {
        [Test]
        public void EnsureDefaults()
        {
            PortfolioStatisticsSettings options = PortfolioStatisticsSettings.DefaultSettings();

            Assert.IsTrue(options.DisplayValueFunds);
            Assert.IsTrue(options.SecurityGenerateOptions.ShouldGenerate);
            Assert.IsTrue(options.SectorGenerateOptions.ShouldGenerate);
            Assert.IsTrue(options.BankAccountGenerateOptions.ShouldGenerate);
            Assert.IsTrue(options.AssetGenerateOptions.ShouldGenerate);
        }

        [Test]
        public void EnsureExportDefaults()
        {
            PortfolioStatisticsExportSettings options = PortfolioStatisticsExportSettings.DefaultSettings();

            Assert.IsFalse(options.Spacing);
            Assert.IsFalse(options.Colours);
            Assert.IsTrue(options.SecurityDisplayOptions.ShouldDisplay);
            Assert.IsTrue(options.SectorDisplayOptions.ShouldDisplay);
            Assert.IsTrue(options.BankAccountDisplayOptions.ShouldDisplay);
            Assert.AreEqual(SortDirection.Descending, options.SecurityDisplayOptions.SortingDirection);
            Assert.AreEqual(SortDirection.Descending, options.BankAccountDisplayOptions.SortingDirection);
            Assert.AreEqual(SortDirection.Descending, options.SectorDisplayOptions.SortingDirection);
        }
    }
}
