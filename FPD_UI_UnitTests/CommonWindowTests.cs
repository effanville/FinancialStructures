using FinanceCommonViewModels;
using FinancialStructures.DataStructures;
using FinancialStructures.NamingStructures;
using FinancialStructures.PortfolioAPI;
using FPD_UI_UnitTests.TestConstruction;
using NUnit.Framework;
using StructureCommon.DataStructures;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FPD_UI_UnitTests.CommonWindowTests
{
    /// <summary>
    /// Tests for window displaying single data stream data.
    /// </summary>
    [TestFixture]
    public class CommonWindowTests
    {
        [Test]
        public void CanLoadSuccessfully()
        {
            var fileMock = TestingGUICode.CreateFileMock("nothing");
            var dialogMock = TestingGUICode.CreateDialogMock();
            var portfolio = TestingGUICode.CreateBasicDataBase();
            var dataUpdater = TestingGUICode.CreateDataUpdater(portfolio);
            var viewModel = new SingleValueEditWindowViewModel("Dummy", portfolio, dataUpdater, TestingGUICode.DummyReportLogger, fileMock.Object, dialogMock.Object, TestingGUICode.DummyEditMethods, AccountType.BankAccount);

            Assert.AreEqual(1, viewModel.Tabs.Count);
            var tab = viewModel.Tabs.Single();
            var nameModel = tab as DataNamesViewModel;
            Assert.AreEqual(1, nameModel.DataNames.Count);
        }
        [Test]
        public void CanUpdateData()
        {
            var fileMock = TestingGUICode.CreateFileMock("nothing");
            var dialogMock = TestingGUICode.CreateDialogMock();
            var portfolio = TestingGUICode.CreateEmptyDataBase();
            var dataUpdater = TestingGUICode.CreateDataUpdater(portfolio);
            var viewModel = new SingleValueEditWindowViewModel("Dummy", portfolio, dataUpdater, TestingGUICode.DummyReportLogger, fileMock.Object, dialogMock.Object, TestingGUICode.DummyEditMethods, AccountType.BankAccount);
            var newData = TestingGUICode.CreateBasicDataBase();
            viewModel.UpdateData(newData);

            Assert.AreEqual("TestFilePath", viewModel.DataStore.FilePath);
            Assert.AreEqual(1, viewModel.DataStore.BankAccounts.Count);
        }

        [Test]
        public void CanUpdateDataAndRemoveOldTab()
        {
            var fileMock = TestingGUICode.CreateFileMock("nothing");
            var dialogMock = TestingGUICode.CreateDialogMock();
            var portfolio = TestingGUICode.CreateEmptyDataBase();
            var dataUpdater = TestingGUICode.CreateDataUpdater(portfolio);
            var viewModel = new SingleValueEditWindowViewModel("Dummy", portfolio, dataUpdater, TestingGUICode.DummyReportLogger, fileMock.Object, dialogMock.Object, TestingGUICode.DummyEditMethods, AccountType.BankAccount);

            var newNameData = new NameData_ChangeLogged("Fidelity", "Europe");
            viewModel.LoadSelectedTab(newNameData);

            Assert.AreEqual(2, viewModel.Tabs.Count);

            var newData = TestingGUICode.CreateBasicDataBase();
            viewModel.UpdateData(newData);
            Assert.AreEqual(1, viewModel.Tabs.Count);
            Assert.AreEqual("TestFilePath", viewModel.DataStore.FilePath);
            Assert.AreEqual(1, viewModel.DataStore.BankAccounts.Count);
        }

        [Test]
        public void CanAddTab()
        {
            var fileMock = TestingGUICode.CreateFileMock("nothing");
            var dialogMock = TestingGUICode.CreateDialogMock();
            var portfolio = TestingGUICode.CreateBasicDataBase();
            var dataUpdater = TestingGUICode.CreateDataUpdater(portfolio);
            var viewModel = new SingleValueEditWindowViewModel("Dummy", portfolio, dataUpdater, TestingGUICode.DummyReportLogger, fileMock.Object, dialogMock.Object, TestingGUICode.DummyEditMethods, AccountType.BankAccount);

            var newData = new NameData_ChangeLogged("Fidelity", "China");
            viewModel.LoadSelectedTab(newData);

            Assert.AreEqual(2, viewModel.Tabs.Count);
        }
    }

    public class DataNamesTests
    {
        [Test]
        public void CanOpen()
        {
            var portfolio = TestingGUICode.CreateBasicDataBase();
            var dataUpdater = TestingGUICode.CreateDataUpdater(portfolio);
            var viewModel = new DataNamesViewModel(portfolio, dataUpdater, TestingGUICode.DummyReportLogger, TestingGUICode.DummyOpenTab, TestingGUICode.DummyEditMethods);
            Assert.AreEqual(1, viewModel.DataNames.Count);
        }

        [Test]
        public void CanUpdateData()
        {
            var fileMock = TestingGUICode.CreateFileMock("nothing");
            var dialogMock = TestingGUICode.CreateDialogMock();
            var portfolio = TestingGUICode.CreateEmptyDataBase();
            var dataUpdater = TestingGUICode.CreateDataUpdater(portfolio);

            var viewModel = new DataNamesViewModel(portfolio, dataUpdater, TestingGUICode.DummyReportLogger, TestingGUICode.DummyOpenTab, TestingGUICode.DummyEditMethods);

            var newData = TestingGUICode.CreateBasicDataBase();

            viewModel.UpdateData(newData);

            Assert.AreEqual(1, viewModel.DataNames.Count);
        }

        [Test]
        public void CanCreateNew()
        {
            var fileMock = TestingGUICode.CreateFileMock("nothing");
            var dialogMock = TestingGUICode.CreateDialogMock();
            var portfolio = TestingGUICode.CreateBasicDataBase();
            var dataUpdater = TestingGUICode.CreateDataUpdater(portfolio);

            var viewModel = new DataNamesViewModel(portfolio, dataUpdater, TestingGUICode.DummyReportLogger, TestingGUICode.DummyOpenTab, TestingGUICode.DummyEditMethods);
            var newName = new NameCompDate("company", "name", "GBP", "someUrl", new HashSet<string>(), DateTime.Today)
            {
                Company = "Company"
            };
            viewModel.SelectedName = newName;
            viewModel.DataNames.Add(newName);
            viewModel.CreateCommand.Execute(1);
            Assert.AreEqual(2, viewModel.DataNames.Count);
            Assert.AreEqual(2, portfolio.BankAccounts.Count);
        }

        [Test]
        public void CanEditName()
        {
            var fileMock = TestingGUICode.CreateFileMock("nothing");
            var dialogMock = TestingGUICode.CreateDialogMock();
            var portfolio = TestingGUICode.CreateBasicDataBase();
            var dataUpdater = TestingGUICode.CreateDataUpdater(portfolio);

            var viewModel = new DataNamesViewModel(portfolio, dataUpdater, TestingGUICode.DummyReportLogger, TestingGUICode.DummyOpenTab, TestingGUICode.DummyEditMethods);
            viewModel.SelectedName = viewModel.DataNames[0];

            viewModel.SelectedName.Company = "NewCompany";

            viewModel.CreateCommand.Execute(1);
            Assert.AreEqual(1, viewModel.DataNames.Count);
            Assert.AreEqual(1, portfolio.BankAccounts.Count);

            Assert.AreEqual("NewCompany", portfolio.BankAccounts.Single().Company);
        }

        [Test]
        [Ignore("IncompeteArchitecture - Downloader does not currently allow for use in test environment.")]
        public void CanDownload()
        {
            var fileMock = TestingGUICode.CreateFileMock("nothing");
            var dialogMock = TestingGUICode.CreateDialogMock();
            var portfolio = TestingGUICode.CreateEmptyDataBase();
            var dataUpdater = TestingGUICode.CreateDataUpdater(portfolio);

            var viewModel = new DataNamesViewModel(portfolio, dataUpdater, TestingGUICode.DummyReportLogger, TestingGUICode.DummyOpenTab, TestingGUICode.DummyEditMethods)
            {
                SelectedName = new NameData_ChangeLogged("Barclays", "currentAccount")
            };
            viewModel.DownloadCommand.Execute(1);

            Assert.AreEqual(1, viewModel.DataNames.Count);
        }

        [Test]
        public void CanDelete()
        {
            var fileMock = TestingGUICode.CreateFileMock("nothing");
            var dialogMock = TestingGUICode.CreateDialogMock();
            var portfolio = TestingGUICode.CreateBasicDataBase();
            var dataUpdater = TestingGUICode.CreateDataUpdater(portfolio);

            var viewModel = new DataNamesViewModel(portfolio, dataUpdater, TestingGUICode.DummyReportLogger, TestingGUICode.DummyOpenTab, TestingGUICode.DummyEditMethods);

            Assert.AreEqual(1, viewModel.DataStore.BankAccounts.Count);
            Assert.AreEqual(1, portfolio.BankAccounts.Count);
            viewModel.SelectedName = new NameData_ChangeLogged("Barclays", "currentAccount");
            viewModel.DeleteCommand.Execute(1);
            Assert.AreEqual(0, viewModel.DataStore.BankAccounts.Count);
            Assert.AreEqual(0, portfolio.BankAccounts.Count);
        }
    }

    public class SelectedAccountDataTests
    {

        [Test]
        public void CanOpenWindow()
        {
            var fileMock = TestingGUICode.CreateFileMock("nothing");
            var dialogMock = TestingGUICode.CreateDialogMock();
            var portfolio = TestingGUICode.CreateBasicDataBase();
            var dataUpdater = TestingGUICode.CreateDataUpdater(portfolio);
            var viewModel = new SelectedSingleDataViewModel(portfolio, dataUpdater, TestingGUICode.DummyReportLogger, fileMock.Object, dialogMock.Object, TestingGUICode.DummyEditMethods, new NameData_ChangeLogged("Barclays", "currentAccount"), AccountType.BankAccount);

            Assert.AreEqual(1, viewModel.SelectedData.Count);
        }

        [Test]
        public void CanAddValue()
        {
            var fileMock = TestingGUICode.CreateFileMock("nothing");
            var dialogMock = TestingGUICode.CreateDialogMock();
            var portfolio = TestingGUICode.CreateBasicDataBase();
            var dataUpdater = TestingGUICode.CreateDataUpdater(portfolio);
            var viewModel = new SelectedSingleDataViewModel(portfolio, dataUpdater, TestingGUICode.DummyReportLogger, fileMock.Object, dialogMock.Object, TestingGUICode.DummyEditMethods, new NameData_ChangeLogged("Barclays", "currentAccount"), AccountType.BankAccount);

            Assert.AreEqual(1, viewModel.SelectedData.Count);
            var newValue = new DayValue_ChangeLogged(new DateTime(2002, 1, 1), 1);
            viewModel.SelectedData.Add(newValue);
            viewModel.SelectedValue = newValue;
            viewModel.EditDataCommand.Execute(1);
            Assert.AreEqual(2, viewModel.SelectedData.Count);
            Assert.AreEqual(2, portfolio.BankAccounts.Single().Count());
        }

        [Test]
        public void CanEditValue()
        {
            var fileMock = TestingGUICode.CreateFileMock("nothing");
            var dialogMock = TestingGUICode.CreateDialogMock();
            var portfolio = TestingGUICode.CreateBasicDataBase();
            var dataUpdater = TestingGUICode.CreateDataUpdater(portfolio);
            var viewModel = new SelectedSingleDataViewModel(portfolio, dataUpdater, TestingGUICode.DummyReportLogger, fileMock.Object, dialogMock.Object, TestingGUICode.DummyEditMethods, new NameData_ChangeLogged("Barclays", "currentAccount"), AccountType.BankAccount);

            Assert.AreEqual(1, viewModel.SelectedData.Count);
            var newValue = new DayValue_ChangeLogged(new DateTime(2000, 1, 1), 1);
            viewModel.SelectedData[0] = newValue;
            viewModel.SelectedValue = newValue;
            viewModel.EditDataCommand.Execute(1);
            Assert.AreEqual(1, viewModel.SelectedData.Count);
            Assert.AreEqual(1, portfolio.Funds.Single().Count());
            Assert.AreEqual(new DateTime(2000, 1, 1), portfolio.Funds.Single().FirstValue().Day);
        }


        [Test]
        [Ignore("IncompeteArchitecture - FileInteraction does not currently allow for use in test environment.")]
        public void CanAddFromCSV()
        {
            var fileMock = TestingGUICode.CreateFileMock("nothing");
            var dialogMock = TestingGUICode.CreateDialogMock();
            var portfolio = TestingGUICode.CreateBasicDataBase();
            var dataUpdater = TestingGUICode.CreateDataUpdater(portfolio);
            var viewModel = new SelectedSingleDataViewModel(portfolio, dataUpdater, TestingGUICode.DummyReportLogger, fileMock.Object, dialogMock.Object, TestingGUICode.DummyEditMethods, new NameData_ChangeLogged("Barclays", "currentAccount"), AccountType.BankAccount);

            Assert.AreEqual(1, viewModel.SelectedData.Count);
        }

        [Test]
        [Ignore("IncompeteArchitecture - FileInteraction does not currently allow for use in test environment.")]
        public void CanWriteToCSV()
        {
            var fileMock = TestingGUICode.CreateFileMock("nothing");
            var dialogMock = TestingGUICode.CreateDialogMock();
            var portfolio = TestingGUICode.CreateBasicDataBase();
            var dataUpdater = TestingGUICode.CreateDataUpdater(portfolio);
            var viewModel = new SelectedSingleDataViewModel(portfolio, dataUpdater, TestingGUICode.DummyReportLogger, fileMock.Object, dialogMock.Object, TestingGUICode.DummyEditMethods, new NameData_ChangeLogged("Barclays", "currentAccount"), AccountType.BankAccount);

            Assert.AreEqual(1, viewModel.SelectedData.Count);
        }

        [Test]
        public void CanDeleteValue()
        {
            var fileMock = TestingGUICode.CreateFileMock("nothing");
            var dialogMock = TestingGUICode.CreateDialogMock();
            var portfolio = TestingGUICode.CreateBasicDataBase();
            var dataUpdater = TestingGUICode.CreateDataUpdater(portfolio);
            var viewModel = new SelectedSingleDataViewModel(portfolio, dataUpdater, TestingGUICode.DummyReportLogger, fileMock.Object, dialogMock.Object, TestingGUICode.DummyEditMethods, new NameData_ChangeLogged("Barclays", "currentAccount"), AccountType.BankAccount);
            viewModel.SelectedValue = viewModel.SelectedData.Single();
            Assert.AreEqual(1, viewModel.SelectedData.Count);

            viewModel.DeleteValuationCommand.Execute(1);

            Assert.AreEqual(0, portfolio.BankAccounts.Single().Count());
        }
    }
}
