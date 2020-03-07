using NUnit.Framework;
using FinanceWindowsViewModels;
using FinancialStructures.Database;
using FinancialStructures.FinanceStructures;
using System.Linq;
using System.Collections.Generic;
using FPD_UI_UnitTests.TestConstruction;

namespace FPD_UI_UnitTests
{
    public class BasicDataViewWindowTests
    {
        [Test]
        public void CanViewData()
        {
            var output = TestingGUICode.CreateBasicDataBase();

            var viewModel = new BasicDataViewModel(output.Item1, output.Item2);

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
            var sectors = new List<Sector>();
            var viewModel = new BasicDataViewModel(portfolio, sectors);

            portfolio.TryAddSecurity(TestingGUICode.DummyReportLogger, "Fidelity", "China", "GBP", "http://www.fidelity.co.uk", "Bonds, UK");
            portfolio.TryAddBankAccount("currentAccount", "Barclays", string.Empty, string.Empty, TestingGUICode.DummyReportLogger);
            portfolio.TryAddCurrency("GBP", string.Empty, TestingGUICode.DummyReportLogger);
            sectors.Add(new Sector("UK", "http://www.hi.com"));

            viewModel.UpdateData(portfolio, sectors);
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
