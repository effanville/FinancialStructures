using NUnit.Framework;
using FinanceWindowsViewModels;
using FinancialStructures.Database;
using System.Linq;
using FPD_UI_UnitTests.TestConstruction;

namespace FPD_UI_UnitTests
{
    public class BasicDataViewWindowTests
    {
        [Test]
        public void CanViewData()
        {
            var viewModel = new BasicDataViewModel(TestingGUICode.CreateBasicDataBase());

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

        [Test]
        public void CanUpdateData()
        {
            var portfolio = new Portfolio();
            var viewModel = new BasicDataViewModel(portfolio);

            TestingGUICode.UpdatePortfolio(portfolio);

            viewModel.UpdateData(portfolio);
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
