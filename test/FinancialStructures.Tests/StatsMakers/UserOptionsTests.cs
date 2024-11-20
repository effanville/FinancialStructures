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

            Assert.That(options.DisplayValueFunds, Is.True);
            Assert.That(options.SecurityGenerateOptions.ShouldGenerate, Is.True);
            Assert.That(options.SectorGenerateOptions.ShouldGenerate, Is.True);
            Assert.That(options.BankAccountGenerateOptions.ShouldGenerate, Is.True);
            Assert.That(options.AssetGenerateOptions.ShouldGenerate, Is.True);
        }

        [Test]
        public void EnsureExportDefaults()
        {
            PortfolioStatisticsExportSettings options = PortfolioStatisticsExportSettings.DefaultSettings();

            Assert.That(options.Spacing, Is.False);
            Assert.That(options.Colours, Is.False);
            Assert.That(options.SecurityDisplayOptions.ShouldDisplay, Is.True);
            Assert.That(options.SectorDisplayOptions.ShouldDisplay, Is.True);
            Assert.That(options.BankAccountDisplayOptions.ShouldDisplay, Is.True);
            Assert.That(options.SecurityDisplayOptions.SortingDirection, Is.EqualTo(SortDirection.Ascending));
            Assert.That(options.BankAccountDisplayOptions.SortingDirection, Is.EqualTo(SortDirection.Ascending));
            Assert.That(options.SectorDisplayOptions.SortingDirection, Is.EqualTo(SortDirection.Ascending));
        }
    }
}
