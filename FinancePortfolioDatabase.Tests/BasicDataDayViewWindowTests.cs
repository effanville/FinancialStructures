using FinancePortfolioDatabase.Tests.TestHelpers;
using FinancialStructures.Database;
using NUnit.Framework;

namespace FinancePortfolioDatabase.Tests
{
    /// <summary>
    /// Tests for the default data window.
    /// </summary>
    [TestFixture]
    internal class BasicDataViewWindowTests : BasicDataWindowTestHelper
    {
        /// <summary>
        /// Ensures the window displays data if the underlying database is modified.
        /// </summary>
        [Test]
        public void EmptyPortfolioHasEmptyData()
        {
            Portfolio = PortfolioFactory.GenerateEmpty();
            Assert.Multiple(() =>
            {
                Assert.IsFalse(ViewModel.HasValues);
                Assert.AreEqual(ViewModel.PortfolioNameText, "Unsaved database loaded");
            });
        }

        /// <summary>
        /// Ensures that the window displays data on loading.
        /// </summary>
        [Test]
        public void CanViewData()
        {
            Portfolio = TestSetupHelper.CreateBasicDataBase();
            Assert.Multiple(() =>
            {
                Assert.IsTrue(ViewModel.HasValues);
                Assert.AreEqual("Portfolio: TestFilePath loaded.", ViewModel.PortfolioNameText);
                Assert.AreEqual("Total Securities: 1", ViewModel.SecurityTotalText);
                Assert.AreEqual("Total Value: 1 ", ViewModel.SecurityAmountText);

                Assert.AreEqual("Total Bank Accounts: 1", ViewModel.BankAccountTotalText);
                Assert.AreEqual("Total Value: 1 ", ViewModel.BankAccountAmountText);
            });
        }

        /// <summary>
        /// Ensures the window displays data if the underlying database is modified.
        /// </summary>
        [Test]
        public void CanUpdateData()
        {
            Assert.IsFalse(ViewModel.HasValues);

            // Now update that data.
            TestSetupHelper.UpdatePortfolio(Portfolio);
            ViewModel.UpdateData(Portfolio);

            // Ensure new data has been displayed correctly.
            Assert.Multiple(() =>
            {

                Assert.IsTrue(ViewModel.HasValues);
                Assert.AreEqual("Total Securities: 1", ViewModel.SecurityTotalText);
                Assert.AreEqual("Total Value: 1 ", ViewModel.SecurityAmountText);

                Assert.AreEqual("Total Bank Accounts: 1", ViewModel.BankAccountTotalText);
                Assert.AreEqual("Total Value: 1 ", ViewModel.BankAccountAmountText);
            });
        }
    }
}
