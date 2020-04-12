using FinanceWindowsViewModels;
using FinancialStructures.DataStructures;
using FinancialStructures.NamingStructures;
using FPD_UI_UnitTests.TestConstruction;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FPD_UI_UnitTests.SecurityWindowTests
{
    [TestFixture]
    public class SecurityEditWindowTests
    {
        [Test]
        public void CanLoadSuccessfully()
        {
            var output = TestingGUICode.CreateBasicDataBase();
            var fileMock = TestingGUICode.CreateFileMock("nothing");
            var dialogMock = TestingGUICode.CreateDialogMock();
            var portfolio = TestingGUICode.CreateEmptyDataBase();
            var dataUpdater = TestingGUICode.CreateDataUpdater(portfolio);
            var viewModel = new SecurityEditWindowViewModel(output, dataUpdater, TestingGUICode.DummyReportLogger, fileMock.Object, dialogMock.Object);

            Assert.AreEqual(1, viewModel.Tabs.Count);
            var tab = viewModel.Tabs.Single();
            var nameModel = tab as SecurityNamesViewModel;
            Assert.AreEqual(1, nameModel.FundNames.Count);
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

            Assert.AreEqual("TestFilePath", viewModel.Portfolio.FilePath);
            Assert.AreEqual(1, viewModel.Portfolio.Funds.Count);
        }

        [Test]
        public void CanUpdateDataAndRemoveOldTab()
        {
            var fileMock = TestingGUICode.CreateFileMock("nothing");
            var dialogMock = TestingGUICode.CreateDialogMock();
            var portfolio = TestingGUICode.CreateEmptyDataBase();
            var dataUpdater = TestingGUICode.CreateDataUpdater(portfolio);
            var viewModel = new SecurityEditWindowViewModel(portfolio, dataUpdater, TestingGUICode.DummyReportLogger, fileMock.Object, dialogMock.Object);

            var newNameData = new NameData_ChangeLogged("Fidelity", "Europe");
            viewModel.LoadSelectedTab(newNameData);

            Assert.AreEqual(2, viewModel.Tabs.Count);

            var newData = TestingGUICode.CreateBasicDataBase();
            viewModel.UpdateData(newData);
            Assert.AreEqual(1, viewModel.Tabs.Count);
            Assert.AreEqual("TestFilePath", viewModel.Portfolio.FilePath);
            Assert.AreEqual(1, viewModel.Portfolio.Funds.Count);
        }

        [Test]
        public void CanAddTab()
        {
            var fileMock = TestingGUICode.CreateFileMock("nothing");
            var dialogMock = TestingGUICode.CreateDialogMock();
            var portfolio = TestingGUICode.CreateBasicDataBase();
            var dataUpdater = TestingGUICode.CreateDataUpdater(portfolio);
            var viewModel = new SecurityEditWindowViewModel(portfolio, dataUpdater, TestingGUICode.DummyReportLogger, fileMock.Object, dialogMock.Object);

            var newData = new NameData_ChangeLogged("Fidelity", "China");
            viewModel.LoadSelectedTab(newData);

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
            var viewModel = new SecurityNamesViewModel(output, dataUpdater, TestingGUICode.DummyReportLogger, TestingGUICode.DummyOpenTab);
            Assert.AreEqual(1, viewModel.FundNames.Count);
        }

        [Test]
        public void CanUpdateData()
        {
            var fileMock = TestingGUICode.CreateFileMock("nothing");
            var dialogMock = TestingGUICode.CreateDialogMock();
            var portfolio = TestingGUICode.CreateEmptyDataBase();
            var dataUpdater = TestingGUICode.CreateDataUpdater(portfolio);

            var viewModel = new SecurityNamesViewModel(portfolio, dataUpdater, TestingGUICode.DummyReportLogger, TestingGUICode.DummyOpenTab);
            var newData = TestingGUICode.CreateBasicDataBase();
            viewModel.UpdateData(newData);

            Assert.AreEqual(1, viewModel.FundNames.Count);
        }

        [Test]
        public void CanCreateNew()
        {
            var fileMock = TestingGUICode.CreateFileMock("nothing");
            var dialogMock = TestingGUICode.CreateDialogMock();
            var portfolio = TestingGUICode.CreateBasicDataBase();
            var dataUpdater = TestingGUICode.CreateDataUpdater(portfolio);

            var viewModel = new SecurityNamesViewModel(portfolio, dataUpdater, TestingGUICode.DummyReportLogger, TestingGUICode.DummyOpenTab);
            var newName = new NameCompDate("company", "name", "GBP", "someUrl", new HashSet<string>(), DateTime.Today);
            newName.Company = "Company";
            viewModel.selectedName = newName;
            viewModel.FundNames.Add(newName);
            viewModel.CreateSecurityCommand.Execute(1);
            Assert.AreEqual(2, viewModel.FundNames.Count);
            Assert.AreEqual(2, portfolio.Funds.Count);
        }

        [Test]
        public void CanEditSecurityName()
        {
            var fileMock = TestingGUICode.CreateFileMock("nothing");
            var dialogMock = TestingGUICode.CreateDialogMock();
            var portfolio = TestingGUICode.CreateBasicDataBase();
            var dataUpdater = TestingGUICode.CreateDataUpdater(portfolio);

            var viewModel = new SecurityNamesViewModel(portfolio, dataUpdater, TestingGUICode.DummyReportLogger, TestingGUICode.DummyOpenTab);
            viewModel.selectedName = viewModel.FundNames[0];

            viewModel.selectedName.Company = "NewCompany";

            viewModel.CreateSecurityCommand.Execute(1);
            Assert.AreEqual(1, viewModel.FundNames.Count);
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

            var viewModel = new SecurityNamesViewModel(portfolio, dataUpdater, TestingGUICode.DummyReportLogger, TestingGUICode.DummyOpenTab);

            viewModel.selectedName = new NameData_ChangeLogged("Fidelity", "China");
            viewModel.DownloadCommand.Execute(1);

            Assert.AreEqual(1, viewModel.FundNames.Count);
        }

        [Test]
        public void CanDelete()
        {
            var fileMock = TestingGUICode.CreateFileMock("nothing");
            var dialogMock = TestingGUICode.CreateDialogMock();
            var portfolio = TestingGUICode.CreateBasicDataBase();
            var dataUpdater = TestingGUICode.CreateDataUpdater(portfolio);

            var viewModel = new SecurityNamesViewModel(portfolio, dataUpdater, TestingGUICode.DummyReportLogger, TestingGUICode.DummyOpenTab);

            Assert.AreEqual(1, viewModel.Portfolio.Funds.Count);
            Assert.AreEqual(1, portfolio.Funds.Count);
            viewModel.selectedName = new NameData_ChangeLogged("Fidelity", "China");
            viewModel.DeleteSecurityCommand.Execute(1);
            Assert.AreEqual(0, viewModel.Portfolio.Funds.Count);
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
            var viewModel = new SelectedSecurityViewModel(portfolio, dataUpdater, TestingGUICode.DummyReportLogger, fileMock.Object, dialogMock.Object, new NameData_ChangeLogged("Fidelity", "China"));

            Assert.AreEqual(1, viewModel.SelectedSecurityData.Count);
        }

        [Test]
        public void CanAddValue()
        {
            var fileMock = TestingGUICode.CreateFileMock("nothing");
            var dialogMock = TestingGUICode.CreateDialogMock();
            var portfolio = TestingGUICode.CreateBasicDataBase();
            var dataUpdater = TestingGUICode.CreateDataUpdater(portfolio);
            var viewModel = new SelectedSecurityViewModel(portfolio, dataUpdater, TestingGUICode.DummyReportLogger, fileMock.Object, dialogMock.Object, new NameData_ChangeLogged("Fidelity", "China"));

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
            var viewModel = new SelectedSecurityViewModel(portfolio, dataUpdater, TestingGUICode.DummyReportLogger, fileMock.Object, dialogMock.Object, new NameData_ChangeLogged("Fidelity", "China"));

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
            var viewModel = new SelectedSecurityViewModel(portfolio, dataUpdater, TestingGUICode.DummyReportLogger, fileMock.Object, dialogMock.Object, new NameData_ChangeLogged("Fidelity", "China"));

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
            var viewModel = new SelectedSecurityViewModel(portfolio, dataUpdater, TestingGUICode.DummyReportLogger, fileMock.Object, dialogMock.Object, new NameData_ChangeLogged("Fidelity", "China"));

            Assert.AreEqual(1, viewModel.SelectedSecurityData.Count);
        }

        [Test]
        public void CanDeleteValue()
        {
            var fileMock = TestingGUICode.CreateFileMock("nothing");
            var dialogMock = TestingGUICode.CreateDialogMock();
            var portfolio = TestingGUICode.CreateBasicDataBase();
            var dataUpdater = TestingGUICode.CreateDataUpdater(portfolio);
            var viewModel = new SelectedSecurityViewModel(portfolio, dataUpdater, TestingGUICode.DummyReportLogger, fileMock.Object, dialogMock.Object, new NameData_ChangeLogged("Fidelity", "China"));
            viewModel.selectedValues = viewModel.SelectedSecurityData.Single();
            Assert.AreEqual(1, viewModel.SelectedSecurityData.Count);

            viewModel.DeleteValuationCommand.Execute(1);

            Assert.AreEqual(0, portfolio.Funds.Single().Count());
        }
    }
}
