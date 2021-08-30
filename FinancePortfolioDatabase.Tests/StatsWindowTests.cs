using FinancePortfolioDatabase.GUI.ViewModels.Stats;
using FinancePortfolioDatabase.Tests.TestHelpers;
using NUnit.Framework;

namespace FinancePortfolioDatabase.Tests
{

    /// <summary>
    /// Tests to ensure that the stats window displays what it should do.
    /// </summary>
    public class StatsWindowTests : StatsWindowTestHelper
    {
        /// <summary>
        /// The defaults are loaded correctly.
        /// </summary>
        [Test]
        public void CanLoadWithNames()
        {
            Assert.AreEqual(2, ViewModel.StatsTabs.Count);
            Assert.AreEqual(true, ViewModel.DisplayValueFunds);
        }

        /// <summary>
        /// The defaults are loaded correctly.
        /// </summary>
        [TestCase(false)]
        [TestCase(true)]
        public void CanStoreConfig(bool valueFunds)
        {
            Assert.AreEqual(2, ViewModel.StatsTabs.Count);
            Assert.AreEqual(true, ViewModel.DisplayValueFunds);

            ViewModel.DisplayValueFunds = valueFunds;
            Assert.AreEqual(valueFunds, ViewModel.DisplayValueFunds);

            ViewModel = new StatsCreatorWindowViewModel(Portfolio, TestSetupHelper.DummyReportLogger, Globals, VMConfiguration);

            Assert.AreEqual(valueFunds, ViewModel.DisplayValueFunds);
            Assert.AreEqual(2, ViewModel.StatsTabs.Count);
        }
    }
}
