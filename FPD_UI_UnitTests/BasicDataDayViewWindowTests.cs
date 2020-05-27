using System.Linq;
using FinanceWindowsViewModels;
using FinancialStructures.Database;
using FPD_UI_UnitTests.TestConstruction;
using NUnit.Framework;

namespace FPD_UI_UnitTests
{
    /// <summary>
    /// Tests for the default data window.
    /// </summary>
    public class BasicDataViewWindowTests
    {
        /// <summary>
        /// Ensures that the window displays data on loading.
        /// </summary>
        [Test]
        public void CanViewData()
        {
            BasicDataViewModel viewModel = new BasicDataViewModel(TestingGUICode.CreateBasicDataBase());

            Assert.AreEqual(1, viewModel.FundNames.Count);
            Assert.AreEqual("China", viewModel.FundNames.Single().Name);
            Assert.AreEqual("Fidelity", viewModel.FundNames.Single().Company);
            Assert.AreEqual(1, viewModel.AccountNames.Count);
            Assert.AreEqual("currentAccount", viewModel.AccountNames.Single().Name);
            Assert.AreEqual("Barclays", viewModel.AccountNames.Single().Company);
            Assert.AreEqual(1, viewModel.CurrencyNames.Count);
            Assert.AreEqual("GBP", viewModel.CurrencyNames.Single().Name);
            Assert.AreEqual(1, viewModel.SectorNames.Count);
            Assert.AreEqual("UK", viewModel.SectorNames.Single().Name);
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

            // Now update that data.
            TestingGUICode.UpdatePortfolio(portfolio);
            viewModel.UpdateData(portfolio);

            // Ensure new data has been displayed correctly.
            Assert.AreEqual(1, viewModel.FundNames.Count);
            Assert.AreEqual("China", viewModel.FundNames.Single().Name);
            Assert.AreEqual("Fidelity", viewModel.FundNames.Single().Company);
            Assert.AreEqual(1, viewModel.AccountNames.Count);
            Assert.AreEqual("currentAccount", viewModel.AccountNames.Single().Name);
            Assert.AreEqual("Barclays", viewModel.AccountNames.Single().Company);
            Assert.AreEqual(1, viewModel.CurrencyNames.Count);
            Assert.AreEqual("GBP", viewModel.CurrencyNames.Single().Name);
            Assert.AreEqual(1, viewModel.SectorNames.Count);
            Assert.AreEqual("UK", viewModel.SectorNames.Single().Name);
        }
    }
}
