using FinancialStructures.DataExporters.ExportOptions;
using FinancialStructures.Statistics;
using NUnit.Framework;

namespace FinancialStructures.Tests.StatsMakers
{
    [TestFixture]
    public sealed class UserOptionsTests
    {
        [Test]
        public void EnsureDefaults()
        {
            var options = new UserDisplayOptions();

            Assert.AreEqual(SortDirection.Descending, options.SecurityDisplayOptions.SortingDirection);
            Assert.AreEqual(SortDirection.Descending, options.BankAccountDisplayOptions.SortingDirection);
            Assert.AreEqual(SortDirection.Descending, options.SectorDisplayOptions.SortingDirection);
            Assert.IsFalse(options.DisplayValueFunds);
            Assert.IsFalse(options.Spacing);
            Assert.IsFalse(options.Colours);
            Assert.IsTrue(options.SecurityDisplayOptions.ShouldDisplay);
            Assert.IsTrue(options.SectorDisplayOptions.ShouldDisplay);
            Assert.IsTrue(options.BankAccountDisplayOptions.ShouldDisplay);
        }
    }
}
