using FinanceCommonViewModels;
using FinanceWindowsViewModels;
using FinancialStructures.DataStructures;
using FinancialStructures.NamingStructures;
using FinancialStructures.PortfolioAPI;
using FPD_UI_UnitTests.TestConstruction;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FPD_UI_UnitTests.SecurityWindowTests
{
    /// <summary>
    /// Tests for window displaying security data.
    /// </summary>
    [TestFixture]
    public class SecurityEditWindowTests
    {
        [Test]
        public void CanLoadSuccessfully()
        {
            var fileMock = TestingGUICode.CreateFileMock("nothing");
            var dialogMock = TestingGUICode.CreateDialogMock();
            var portfolio = TestingGUICode.CreateBasicDataBase();
            var dataUpdater = TestingGUICode.CreateDataUpdater(portfolio);
            var viewModel = new SecurityEditWindowViewModel(portfolio, dataUpdater, TestingGUICode.DummyReportLogger, fileMock.Object, dialogMock.Object);

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
            var viewModel = new SecurityEditWindowViewModel(portfolio, dataUpdater, TestingGUICode.DummyReportLogger, fileMock.Object, dialogMock.Object);
            var newData = TestingGUICode.CreateBasicDataBase();
            viewModel.UpdateData(newData);

            Assert.AreEqual("TestFilePath", viewModel.DataStore.FilePath);
            Assert.AreEqual(1, viewModel.DataStore.Funds.Count);
        }

        [Test]
        public void CanUpdateDataAndRemoveOldTab()
        {
            var fileMock = TestingGUICode.CreateFileMock("nothing");
            var dialogMock = TestingGUICode.CreateDialogMock();
            var portfolio = TestingGUICode.CreateEmptyDataBase();
            var dataUpdater = TestingGUICode.CreateDataUpdater(portfolio);
            var viewModel = new SecurityEditWindowViewModel(portfolio, dataUpdater, TestingGUICode.DummyReportLogger, fileMock.Object, dialogMock.Object);

            var newNameData = new NameData("Fidelity", "Europe");
            viewModel.LoadTabFunc(newNameData);

            Assert.AreEqual(2, viewModel.Tabs.Count);

            var newData = TestingGUICode.CreateBasicDataBase();
            viewModel.UpdateData(newData);
            Assert.AreEqual(1, viewModel.Tabs.Count);
            Assert.AreEqual("TestFilePath", viewModel.DataStore.FilePath);
            Assert.AreEqual(1, viewModel.DataStore.Funds.Count);
        }

        [Test]
        public void CanAddTab()
        {
            var fileMock = TestingGUICode.CreateFileMock("nothing");
            var dialogMock = TestingGUICode.CreateDialogMock();
            var portfolio = TestingGUICode.CreateBasicDataBase();
            var dataUpdater = TestingGUICode.CreateDataUpdater(portfolio);
            var viewModel = new SecurityEditWindowViewModel(portfolio, dataUpdater, TestingGUICode.DummyReportLogger, fileMock.Object, dialogMock.Object);

            var newData = new NameData("Fidelity", "China");
            viewModel.LoadTabFunc(newData);

            Assert.AreEqual(2, viewModel.Tabs.Count);
        }
    }

    public class SecurityNamesTests
    {
        [Test]
        public void CanOpen()
        {
            var output = TestingGUICode.CreateBasicDataBase();
            var portfolio = TestingGUICode.CreateBasicDataBase();
            var dataUpdater = TestingGUICode.CreateDataUpdater(portfolio);
            var viewModel = new DataNamesViewModel(output, dataUpdater, TestingGUICode.DummyReportLogger, TestingGUICode.DummyOpenTab, AccountType.Security);
            Assert.AreEqual(1, viewModel.DataNames.Count);
        }

        [Test]
        public void CanUpdateData()
        {
            var fileMock = TestingGUICode.CreateFileMock("nothing");
            var dialogMock = TestingGUICode.CreateDialogMock();
            var portfolio = TestingGUICode.CreateEmptyDataBase();
            var dataUpdater = TestingGUICode.CreateDataUpdater(portfolio);
            var viewModel = new DataNamesViewModel(portfolio, dataUpdater, TestingGUICode.DummyReportLogger, TestingGUICode.DummyOpenTab, AccountType.Security);
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
            var viewModel = new DataNamesViewModel(portfolio, dataUpdater, TestingGUICode.DummyReportLogger, TestingGUICode.DummyOpenTab, AccountType.Security);
            var newName = new NameCompDate("company", "name", "GBP", "someUrl", new HashSet<string>(), DateTime.Today)
            {
                Company = "Company"
            };
            viewModel.SelectedName = newName;
            viewModel.DataNames.Add(newName);
            viewModel.CreateCommand.Execute(1);
            Assert.AreEqual(2, viewModel.DataNames.Count);
            Assert.AreEqual(2, portfolio.Funds.Count);
        }

        [Test]
        public void CanEditSecurityName()
        {
            var fileMock = TestingGUICode.CreateFileMock("nothing");
            var dialogMock = TestingGUICode.CreateDialogMock();
            var portfolio = TestingGUICode.CreateBasicDataBase();
            var dataUpdater = TestingGUICode.CreateDataUpdater(portfolio);
            var viewModel = new DataNamesViewModel(portfolio, dataUpdater, TestingGUICode.DummyReportLogger, TestingGUICode.DummyOpenTab, AccountType.Security);
            viewModel.SelectedName = viewModel.DataNames[0];

            viewModel.SelectedName.Company = "NewCompany";

            viewModel.CreateCommand.Execute(1);
            Assert.AreEqual(1, viewModel.DataNames.Count);
            Assert.AreEqual(1, portfolio.Funds.Count);

            Assert.AreEqual("NewCompany", portfolio.Funds.Single().Company);
        }

        [Test]
        [Ignore("IncompeteArchitecture - Downloader does not currently allow for use in test environment.")]
        public void CanDownload()
        {
            var fileMock = TestingGUICode.CreateFileMock("nothing");
            var dialogMock = TestingGUICode.CreateDialogMock();
            var portfolio = TestingGUICode.CreateEmptyDataBase();
            var dataUpdater = TestingGUICode.CreateDataUpdater(portfolio);
            var viewModel = new DataNamesViewModel(portfolio, dataUpdater, TestingGUICode.DummyReportLogger, TestingGUICode.DummyOpenTab, AccountType.Security)
            {
                SelectedName = new NameCompDate("Fidelity", "China")
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
            var viewModel = new DataNamesViewModel(portfolio, dataUpdater, TestingGUICode.DummyReportLogger, TestingGUICode.DummyOpenTab, AccountType.Security);

            Assert.AreEqual(1, viewModel.DataStore.Funds.Count);
            Assert.AreEqual(1, portfolio.Funds.Count);
            viewModel.SelectedName = new NameCompDate("Fidelity", "China");
            viewModel.DeleteCommand.Execute(1);
            Assert.AreEqual(0, viewModel.DataStore.Funds.Count);
            Assert.AreEqual(0, portfolio.Funds.Count);
        }
    }

    public class SelectedSecurityDataTests
    {
        [Test]
        public void CanOpenWindow()
        {
            var fileMock = TestingGUICode.CreateFileMock("nothing");
            var dialogMock = TestingGUICode.CreateDialogMock();
            var portfolio = TestingGUICode.CreateBasicDataBase();
            var dataUpdater = TestingGUICode.CreateDataUpdater(portfolio);
            var viewModel = new SelectedSecurityViewModel(portfolio, dataUpdater, TestingGUICode.DummyReportLogger, fileMock.Object, dialogMock.Object, new NameData("Fidelity", "China"));

            Assert.AreEqual(1, viewModel.SelectedSecurityData.Count);
        }

        [Test]
        public void CanAddValue()
        {
            var fileMock = TestingGUICode.CreateFileMock("nothing");
            var dialogMock = TestingGUICode.CreateDialogMock();
            var portfolio = TestingGUICode.CreateBasicDataBase();
            var dataUpdater = TestingGUICode.CreateDataUpdater(portfolio);
            var viewModel = new SelectedSecurityViewModel(portfolio, dataUpdater, TestingGUICode.DummyReportLogger, fileMock.Object, dialogMock.Object, new NameData("Fidelity", "China"));

            Assert.AreEqual(1, viewModel.SelectedSecurityData.Count);
            var newValue = new SecurityDayData(new DateTime(2002, 1, 1), 1, 1, 1);
            viewModel.SelectedSecurityData.Add(newValue);
            viewModel.selectedValues = newValue;
            viewModel.AddEditSecurityDataCommand.Execute(1);
            Assert.AreEqual(2, viewModel.SelectedSecurityData.Count);
            Assert.AreEqual(2, portfolio.Funds.Single().Count());
        }

        [Test]
        public void CanEditValue()
        {
            var fileMock = TestingGUICode.CreateFileMock("nothing");
            var dialogMock = TestingGUICode.CreateDialogMock();
            var portfolio = TestingGUICode.CreateBasicDataBase();
            var dataUpdater = TestingGUICode.CreateDataUpdater(portfolio);
            var viewModel = new SelectedSecurityViewModel(portfolio, dataUpdater, TestingGUICode.DummyReportLogger, fileMock.Object, dialogMock.Object, new NameData("Fidelity", "China"));

            Assert.AreEqual(1, viewModel.SelectedSecurityData.Count);
            var newValue = new SecurityDayData(new DateTime(2000, 1, 1), 1, 1, 1);
            viewModel.SelectedSecurityData[0] = newValue;
            viewModel.selectedValues = newValue;
            viewModel.AddEditSecurityDataCommand.Execute(1);
            Assert.AreEqual(1, viewModel.SelectedSecurityData.Count);
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
            var viewModel = new SelectedSecurityViewModel(portfolio, dataUpdater, TestingGUICode.DummyReportLogger, fileMock.Object, dialogMock.Object, new NameData("Fidelity", "China"));

            Assert.AreEqual(1, viewModel.SelectedSecurityData.Count);
        }

        [Test]
        [Ignore("IncompeteArchitecture - FileInteraction does not currently allow for use in test environment.")]
        public void CanWriteToCSV()
        {
            var fileMock = TestingGUICode.CreateFileMock("nothing");
            var dialogMock = TestingGUICode.CreateDialogMock();
            var portfolio = TestingGUICode.CreateBasicDataBase();
            var dataUpdater = TestingGUICode.CreateDataUpdater(portfolio);
            var viewModel = new SelectedSecurityViewModel(portfolio, dataUpdater, TestingGUICode.DummyReportLogger, fileMock.Object, dialogMock.Object, new NameData("Fidelity", "China"));

            Assert.AreEqual(1, viewModel.SelectedSecurityData.Count);
        }

        [Test]
        public void CanDeleteValue()
        {
            var fileMock = TestingGUICode.CreateFileMock("nothing");
            var dialogMock = TestingGUICode.CreateDialogMock();
            var portfolio = TestingGUICode.CreateBasicDataBase();
            var dataUpdater = TestingGUICode.CreateDataUpdater(portfolio);
            var viewModel = new SelectedSecurityViewModel(portfolio, dataUpdater, TestingGUICode.DummyReportLogger, fileMock.Object, dialogMock.Object, new NameData("Fidelity", "China"));
            viewModel.selectedValues = viewModel.SelectedSecurityData.Single();
            Assert.AreEqual(1, viewModel.SelectedSecurityData.Count);

            viewModel.DeleteValuationCommand.Execute(1);

            Assert.AreEqual(0, portfolio.Funds.Single().Count());
        }
    }
}
