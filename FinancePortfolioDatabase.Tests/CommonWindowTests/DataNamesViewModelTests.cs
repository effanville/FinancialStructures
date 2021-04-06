using System;
using System.Linq;
using System.Threading;
using FinancePortfolioDatabase.GUI.ViewModels.Common;
using FinancialStructures.Database;
using FinancialStructures.Database.Implementation;
using FinancialStructures.NamingStructures;
using FinancePortfolioDatabase.Tests.TestConstruction;
using Moq;
using NUnit.Framework;
using UICommon.Services;

namespace FinancePortfolioDatabase.Tests.CommonWindowTests
{
    [TestFixture]
    [Apartment(ApartmentState.STA)]
    public partial class DataNamesViewModelTests : DataNamesViewTestHelper
    {
        [Test]
        public void CanOpen()
        {
            Portfolio = TestingGUICode.CreateBasicDataBase();
            Assert.AreEqual(1, ViewModel.DataNames.Count);
        }

        [Test]
        public void CanUpdateData()
        {
            Portfolio newData = TestingGUICode.CreateBasicDataBase();

            ViewModel.UpdateData(newData);

            Assert.AreEqual(1, ViewModel.DataNames.Count);
        }

        [Test]
        public void CanOpenSecurity()
        {
            Portfolio output = TestingGUICode.CreateBasicDataBase();
            Portfolio portfolio = TestingGUICode.CreateBasicDataBase();
            Action<Action<IPortfolio>> dataUpdater = TestingGUICode.CreateDataUpdater(portfolio);
            DataNamesViewModel viewModel = new DataNamesViewModel(output, dataUpdater, TestingGUICode.DummyReportLogger, TestingGUICode.DummyOpenTab, Account.Security);
            Assert.AreEqual(1, viewModel.DataNames.Count);
        }

        [Test]
        public void CanUpdateSecurityData()
        {
            Mock<IFileInteractionService> fileMock = TestingGUICode.CreateFileMock("nothing");
            Mock<IDialogCreationService> dialogMock = TestingGUICode.CreateDialogMock();
            Portfolio portfolio = TestingGUICode.CreateEmptyDataBase();
            Action<Action<IPortfolio>> dataUpdater = TestingGUICode.CreateDataUpdater(portfolio);
            DataNamesViewModel viewModel = new DataNamesViewModel(portfolio, dataUpdater, TestingGUICode.DummyReportLogger, TestingGUICode.DummyOpenTab, Account.Security);
            Portfolio newData = TestingGUICode.CreateBasicDataBase();
            viewModel.UpdateData(newData);

            Assert.AreEqual(1, viewModel.DataNames.Count);
        }

        [Test]
        public void CanCreateNewSecurity()
        {
            Portfolio = TestingGUICode.CreateBasicDataBase();

            SelectItem(null);
            var newItem = AddNewItem();

            BeginEdit();
            newItem.Company = "company";
            newItem.Name = "name";
            newItem.Currency = "GBP";
            newItem.Url = "someUrl";
            CompleteEdit();
            Assert.AreEqual(2, ViewModel.DataNames.Count, "Bot enough in the view.");
            Assert.AreEqual(2, Portfolio.Funds.Count, "Not enough in portfolio");
        }

        [Test]
        public void CanEditSecurityName()
        {
            Portfolio = TestingGUICode.CreateBasicDataBase();
            var item = ViewModel.DataNames[0].Instance;
            SelectItem(item);
            BeginEdit();
            item.Company = "NewCompany";
            CompleteEdit();

            Assert.AreEqual(1, ViewModel.DataNames.Count);
            Assert.AreEqual(1, Portfolio.Funds.Count);

            Assert.AreEqual("NewCompany", Portfolio.Funds.Single().Names.Company);
        }

        [Test]
        public void CanEditSecurityNameAndUrl()
        {
            Portfolio = TestingGUICode.CreateBasicDataBase();
            var item = ViewModel.DataNames[0].Instance;
            SelectItem(item);
            BeginEdit();
            item.Company = "NewCompany";
            item.Url = "NewUrl";
            CompleteEdit();

            Assert.AreEqual(1, ViewModel.DataNames.Count);
            Assert.AreEqual(1, Portfolio.Funds.Count);

            Assert.AreEqual("NewCompany", Portfolio.Funds.Single().Names.Company);
            Assert.AreEqual("NewUrl", Portfolio.Funds.Single().Names.Url);
        }

        [Test]
        [Ignore("IncompeteArchitecture - Downloader does not currently allow for use in test environment.")]
        public void CanDownloadSecurity()
        {
            var item = new NameData("Fidelity", "China");
            SelectItem(item);
            DownloadSelected();

            Assert.AreEqual(1, ViewModel.DataNames.Count);
        }


        [Test]
        public void CanDeleteSecurity()
        {
            Portfolio = TestingGUICode.CreateBasicDataBase();
            Assert.AreEqual(1, ViewModel.DataStore.Funds.Count);
            Assert.AreEqual(1, Portfolio.Funds.Count);

            var item = new NameData("Fidelity", "China");
            SelectItem(item);
            DeleteSelected();
            Assert.AreEqual(0, ViewModel.DataStore.Funds.Count);
            Assert.AreEqual(0, Portfolio.Funds.Count);
        }
    }
}
