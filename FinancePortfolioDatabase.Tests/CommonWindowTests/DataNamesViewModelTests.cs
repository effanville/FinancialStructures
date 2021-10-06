using System;
using System.Linq;
using System.Threading;
using FinancePortfolioDatabase.GUI.ViewModels.Common;
using FinancePortfolioDatabase.Tests.TestHelpers;
using FinancePortfolioDatabase.Tests.ViewModelExtensions;
using FinancialStructures.Database;
using FinancialStructures.NamingStructures;
using NUnit.Framework;

namespace FinancePortfolioDatabase.Tests.CommonWindowTests
{
    [TestFixture]
    [Apartment(ApartmentState.STA)]
    public partial class DataNamesViewModelTests : DataNamesViewTestHelper
    {
        [Test]
        public void CanOpen()
        {
            Portfolio = TestSetupHelper.CreateBasicDataBase();
            Assert.AreEqual(1, ViewModel.DataNames.Count);
        }

        [Test]
        public void CanUpdateData()
        {
            IPortfolio newData = TestSetupHelper.CreateBasicDataBase();

            ViewModel.UpdateData(newData);

            Assert.AreEqual(1, ViewModel.DataNames.Count);
        }

        [Test]
        public void CanOpenSecurity()
        {
            IPortfolio output = TestSetupHelper.CreateBasicDataBase();
            IPortfolio portfolio = TestSetupHelper.CreateBasicDataBase();
            Action<Action<IPortfolio>> dataUpdater = TestSetupHelper.CreateDataUpdater(portfolio);
            DataNamesViewModel viewModel = new DataNamesViewModel(output, dataUpdater, TestSetupHelper.DummyReportLogger, null, TestSetupHelper.DummyOpenTab, Account.Security);
            Assert.AreEqual(1, viewModel.DataNames.Count);
        }

        [Test]
        public void CanUpdateSecurityData()
        {
            IPortfolio newData = TestSetupHelper.CreateBasicDataBase();
            ViewModel.UpdateData(newData);

            Assert.AreEqual(1, ViewModel.DataNames.Count);
        }

        [Test]
        public void CanCreateNewSecurity()
        {
            Portfolio = TestSetupHelper.CreateBasicDataBase();

            ViewModel.SelectItem(null);
            NameData newItem = ViewModel.AddNewItem();

            ViewModel.BeginEdit();
            newItem.Company = "company";
            newItem.Name = "name";
            newItem.Currency = "GBP";
            newItem.Url = "someUrl";
            ViewModel.CompleteEdit();
            Assert.AreEqual(2, ViewModel.DataNames.Count, "Bot enough in the view.");
            Assert.AreEqual(2, Portfolio.FundsThreadSafe.Count, "Not enough in portfolio");
        }

        [Test]
        public void CanEditSecurityName()
        {
            Portfolio = TestSetupHelper.CreateBasicDataBase();
            NameData item = ViewModel.DataNames[0].Instance;
            ViewModel.SelectItem(item);
            ViewModel.BeginEdit();
            item.Company = "NewCompany";
            ViewModel.CompleteEdit();

            Assert.AreEqual(1, ViewModel.DataNames.Count);
            Assert.AreEqual(1, Portfolio.FundsThreadSafe.Count);

            Assert.AreEqual("NewCompany", Portfolio.FundsThreadSafe.Single().Names.Company);
        }

        [Test]
        public void CanEditSecurityNameAndUrl()
        {
            Portfolio = TestSetupHelper.CreateBasicDataBase();
            NameData item = ViewModel.DataNames[0].Instance;
            ViewModel.SelectItem(item);
            ViewModel.BeginEdit();
            item.Company = "NewCompany";
            item.Url = "NewUrl";
            ViewModel.CompleteEdit();

            Assert.AreEqual(1, ViewModel.DataNames.Count);
            Assert.AreEqual(1, Portfolio.FundsThreadSafe.Count);

            Assert.AreEqual("NewCompany", Portfolio.FundsThreadSafe.Single().Names.Company);
            Assert.AreEqual("NewUrl", Portfolio.FundsThreadSafe.Single().Names.Url);
        }

        [Test]
        [Ignore("IncompeteArchitecture - Downloader does not currently allow for use in test environment.")]
        public void CanDownloadSecurity()
        {
            NameData item = new NameData("Fidelity", "China");
            ViewModel.SelectItem(item);
            ViewModel.DownloadSelected();

            Assert.AreEqual(1, ViewModel.DataNames.Count);
        }


        [Test]
        public void CanDeleteSecurity()
        {
            Portfolio = TestSetupHelper.CreateBasicDataBase();
            Assert.AreEqual(1, ViewModel.DataStore.FundsThreadSafe.Count);
            Assert.AreEqual(1, Portfolio.FundsThreadSafe.Count);

            NameData item = new NameData("Fidelity", "China");
            ViewModel.SelectItem(item);
            ViewModel.DeleteSelected();
            Assert.AreEqual(0, ViewModel.DataStore.FundsThreadSafe.Count);
            Assert.AreEqual(0, Portfolio.FundsThreadSafe.Count);
        }
    }
}
