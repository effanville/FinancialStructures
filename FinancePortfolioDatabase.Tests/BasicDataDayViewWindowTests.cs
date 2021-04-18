using FinancePortfolioDatabase.GUI.ViewModels;
using FinancePortfolioDatabase.Tests.TestHelpers;
using FinancialStructures.Database.Implementation;
using NUnit.Framework;

namespace FinancePortfolioDatabase.Tests
{
    /// <summary>
    /// Tests for the default data window.
    /// </summary>
    public class BasicDataViewWindowTests
    {
        /// <summary>
        /// Ensures the window displays data if the underlying database is modified.
        /// </summary>
        [Test]
        public void EmptyPortfolioHasEmptyData()
        {
            // Setup basic data in the display.
            Portfolio portfolio = new Portfolio();
            BasicDataViewModel viewModel = new BasicDataViewModel(portfolio);

            Assert.Multiple(() =>
            {
                Assert.IsFalse(viewModel.HasValues);
                Assert.AreEqual(viewModel.PortfolioNameText, "Unsaved database loaded");
            });
        }

        /// <summary>
        /// Ensures that the window displays data on loading.
        /// </summary>
        [Test]
        public void CanViewData()
        {
            BasicDataViewModel viewModel = new BasicDataViewModel(TestSetupHelper.CreateBasicDataBase());
            Assert.Multiple(() =>
            {
                Assert.IsTrue(viewModel.HasValues);
                Assert.AreEqual("Portfolio: TestFilePath loaded.", viewModel.PortfolioNameText);
                Assert.AreEqual("Total Securities: 1", viewModel.SecurityTotalText);
                Assert.AreEqual("Total Value: 1 ", viewModel.SecurityAmountText);

                Assert.AreEqual("Total Bank Accounts: 1", viewModel.BankAccountTotalText);
                Assert.AreEqual("Total Value: 1 ", viewModel.BankAccountAmountText);
            });
        }

        /// <summary>
        /// Ensures the window displays data if the underlying database is modified.
        /// </summary>
        [Test]
        public void CanUpdateData()
        {
            // Setup basic data in the display.
            Portfolio portfolio = new Portfolio();
            BasicDataViewModel viewModel = new BasicDataViewModel(portfolio);

            Assert.IsFalse(viewModel.HasValues);
            // Now update that data.
            TestSetupHelper.UpdatePortfolio(portfolio);
            viewModel.UpdateData(portfolio);

            // Ensure new data has been displayed correctly.
            Assert.Multiple(() =>
            {

                Assert.IsTrue(viewModel.HasValues);
                Assert.AreEqual("Total Securities: 1", viewModel.SecurityTotalText);
                Assert.AreEqual("Total Value: 1 ", viewModel.SecurityAmountText);

                Assert.AreEqual("Total Bank Accounts: 1", viewModel.BankAccountTotalText);
                Assert.AreEqual("Total Value: 1 ", viewModel.BankAccountAmountText);
            });
        }
    }
}
