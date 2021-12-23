using FinancePortfolioDatabase.Tests.TestHelpers;
using NUnit.Framework;

namespace FinancePortfolioDatabase.Tests
{
    /// <summary>
    /// Tests for the MainWindowViewModel.
    /// </summary>
    public class MainWindowTests : MainWindowViewModelTestHelper
    {
        /// <summary>
        /// Checks whether the MainWindowViewModel can be loaded successfully.
        /// </summary>
        [Test]
        public void CanSuccessfullyCreateMainViewModel()
        {
            Assert.AreEqual(8, ViewModel.Tabs.Count);
        }
    }
}
