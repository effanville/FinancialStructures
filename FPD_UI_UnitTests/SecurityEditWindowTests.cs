using System;
using System.Collections.Generic;
using System.Linq;
using FinanceCommonViewModels;
using FinanceWindowsViewModels;
using FinancialStructures.DataStructures;
using FinancialStructures.NamingStructures;
using FinancialStructures.PortfolioAPI;
using FPD_UI_UnitTests.TestConstruction;
using NUnit.Framework;

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
            Moq.Mock<UICommon.Services.IFileInteractionService> fileMock = TestingGUICode.CreateFileMock("nothing");
            Moq.Mock<UICommon.Services.IDialogCreationService> dialogMock = TestingGUICode.CreateDialogMock();
            FinancialStructures.Database.Portfolio portfolio = TestingGUICode.CreateBasicDataBase();
            Action<Action<FinancialStructures.FinanceInterfaces.IPortfolio>> dataUpdater = TestingGUICode.CreateDataUpdater(portfolio);
            SecurityEditWindowViewModel viewModel = new SecurityEditWindowViewModel(portfolio, dataUpdater, TestingGUICode.DummyReportLogger, fileMock.Object, dialogMock.Object);

            Assert.AreEqual(1, viewModel.Tabs.Count);
            object tab = viewModel.Tabs.Single();
            DataNamesViewModel nameModel = tab as DataNamesViewModel;
            Assert.AreEqual(1, nameModel.DataNames.Count);
        }

        [Test]
        public void CanUpdateData()
        {
            Moq.Mock<UICommon.Services.IFileInteractionService> fileMock = TestingGUICode.CreateFileMock("nothing");
            Moq.Mock<UICommon.Services.IDialogCreationService> dialogMock = TestingGUICode.CreateDialogMock();
            FinancialStructures.Database.Portfolio portfolio = TestingGUICode.CreateEmptyDataBase();
            Action<Action<FinancialStructures.FinanceInterfaces.IPortfolio>> dataUpdater = TestingGUICode.CreateDataUpdater(portfolio);
            SecurityEditWindowViewModel viewModel = new SecurityEditWindowViewModel(portfolio, dataUpdater, TestingGUICode.DummyReportLogger, fileMock.Object, dialogMock.Object);
            FinancialStructures.Database.Portfolio newData = TestingGUICode.CreateBasicDataBase();
            viewModel.UpdateData(newData);

            Assert.AreEqual("TestFilePath", viewModel.DataStore.FilePath);
            Assert.AreEqual(1, viewModel.DataStore.Funds.Count);
        }

        [Test]
        public void CanUpdateDataAndRemoveOldTab()
        {
            Moq.Mock<UICommon.Services.IFileInteractionService> fileMock = TestingGUICode.CreateFileMock("nothing");
            Moq.Mock<UICommon.Services.IDialogCreationService> dialogMock = TestingGUICode.CreateDialogMock();
            FinancialStructures.Database.Portfolio portfolio = TestingGUICode.CreateEmptyDataBase();
            Action<Action<FinancialStructures.FinanceInterfaces.IPortfolio>> dataUpdater = TestingGUICode.CreateDataUpdater(portfolio);
            SecurityEditWindowViewModel viewModel = new SecurityEditWindowViewModel(portfolio, dataUpdater, TestingGUICode.DummyReportLogger, fileMock.Object, dialogMock.Object);

            NameData newNameData = new NameData("Fidelity", "Europe");
            viewModel.LoadTabFunc(newNameData);

            Assert.AreEqual(2, viewModel.Tabs.Count);

            FinancialStructures.Database.Portfolio newData = TestingGUICode.CreateBasicDataBase();
            viewModel.UpdateData(newData);
            Assert.AreEqual(1, viewModel.Tabs.Count);
            Assert.AreEqual("TestFilePath", viewModel.DataStore.FilePath);
            Assert.AreEqual(1, viewModel.DataStore.Funds.Count);
        }

        [Test]
        public void CanAddTab()
        {
            Moq.Mock<UICommon.Services.IFileInteractionService> fileMock = TestingGUICode.CreateFileMock("nothing");
            Moq.Mock<UICommon.Services.IDialogCreationService> dialogMock = TestingGUICode.CreateDialogMock();
            FinancialStructures.Database.Portfolio portfolio = TestingGUICode.CreateBasicDataBase();
            Action<Action<FinancialStructures.FinanceInterfaces.IPortfolio>> dataUpdater = TestingGUICode.CreateDataUpdater(portfolio);
            SecurityEditWindowViewModel viewModel = new SecurityEditWindowViewModel(portfolio, dataUpdater, TestingGUICode.DummyReportLogger, fileMock.Object, dialogMock.Object);

            NameData newData = new NameData("Fidelity", "China");
            viewModel.LoadTabFunc(newData);

            Assert.AreEqual(2, viewModel.Tabs.Count);
        }
    }

    public class SecurityNamesTests
    {
        [Test]
        public void CanOpen()
        {
            FinancialStructures.Database.Portfolio output = TestingGUICode.CreateBasicDataBase();
            FinancialStructures.Database.Portfolio portfolio = TestingGUICode.CreateBasicDataBase();
            Action<Action<FinancialStructures.FinanceInterfaces.IPortfolio>> dataUpdater = TestingGUICode.CreateDataUpdater(portfolio);
            DataNamesViewModel viewModel = new DataNamesViewModel(output, dataUpdater, TestingGUICode.DummyReportLogger, TestingGUICode.DummyOpenTab, AccountType.Security);
            Assert.AreEqual(1, viewModel.DataNames.Count);
        }

        [Test]
        public void CanUpdateData()
        {
            Moq.Mock<UICommon.Services.IFileInteractionService> fileMock = TestingGUICode.CreateFileMock("nothing");
            Moq.Mock<UICommon.Services.IDialogCreationService> dialogMock = TestingGUICode.CreateDialogMock();
            FinancialStructures.Database.Portfolio portfolio = TestingGUICode.CreateEmptyDataBase();
            Action<Action<FinancialStructures.FinanceInterfaces.IPortfolio>> dataUpdater = TestingGUICode.CreateDataUpdater(portfolio);
            DataNamesViewModel viewModel = new DataNamesViewModel(portfolio, dataUpdater, TestingGUICode.DummyReportLogger, TestingGUICode.DummyOpenTab, AccountType.Security);
            FinancialStructures.Database.Portfolio newData = TestingGUICode.CreateBasicDataBase();
            viewModel.UpdateData(newData);

            Assert.AreEqual(1, viewModel.DataNames.Count);
        }

        [Test]
        public void CanCreateNew()
        {
            Moq.Mock<UICommon.Services.IFileInteractionService> fileMock = TestingGUICode.CreateFileMock("nothing");
            Moq.Mock<UICommon.Services.IDialogCreationService> dialogMock = TestingGUICode.CreateDialogMock();
            FinancialStructures.Database.Portfolio portfolio = TestingGUICode.CreateBasicDataBase();
            Action<Action<FinancialStructures.FinanceInterfaces.IPortfolio>> dataUpdater = TestingGUICode.CreateDataUpdater(portfolio);
            DataNamesViewModel viewModel = new DataNamesViewModel(portfolio, dataUpdater, TestingGUICode.DummyReportLogger, TestingGUICode.DummyOpenTab, AccountType.Security);
            NameCompDate newName = new NameCompDate("company", "name", "GBP", "someUrl", new HashSet<string>(), DateTime.Today)
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
            Moq.Mock<UICommon.Services.IFileInteractionService> fileMock = TestingGUICode.CreateFileMock("nothing");
            Moq.Mock<UICommon.Services.IDialogCreationService> dialogMock = TestingGUICode.CreateDialogMock();
            FinancialStructures.Database.Portfolio portfolio = TestingGUICode.CreateBasicDataBase();
            Action<Action<FinancialStructures.FinanceInterfaces.IPortfolio>> dataUpdater = TestingGUICode.CreateDataUpdater(portfolio);
            DataNamesViewModel viewModel = new DataNamesViewModel(portfolio, dataUpdater, TestingGUICode.DummyReportLogger, TestingGUICode.DummyOpenTab, AccountType.Security);
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
            Moq.Mock<UICommon.Services.IFileInteractionService> fileMock = TestingGUICode.CreateFileMock("nothing");
            Moq.Mock<UICommon.Services.IDialogCreationService> dialogMock = TestingGUICode.CreateDialogMock();
            FinancialStructures.Database.Portfolio portfolio = TestingGUICode.CreateEmptyDataBase();
            Action<Action<FinancialStructures.FinanceInterfaces.IPortfolio>> dataUpdater = TestingGUICode.CreateDataUpdater(portfolio);
            DataNamesViewModel viewModel = new DataNamesViewModel(portfolio, dataUpdater, TestingGUICode.DummyReportLogger, TestingGUICode.DummyOpenTab, AccountType.Security)
            {
                SelectedName = new NameCompDate("Fidelity", "China")
            };
            viewModel.DownloadCommand.Execute(1);

            Assert.AreEqual(1, viewModel.DataNames.Count);
        }

        [Test]
        public void CanDelete()
        {
            Moq.Mock<UICommon.Services.IFileInteractionService> fileMock = TestingGUICode.CreateFileMock("nothing");
            Moq.Mock<UICommon.Services.IDialogCreationService> dialogMock = TestingGUICode.CreateDialogMock();
            FinancialStructures.Database.Portfolio portfolio = TestingGUICode.CreateBasicDataBase();
            Action<Action<FinancialStructures.FinanceInterfaces.IPortfolio>> dataUpdater = TestingGUICode.CreateDataUpdater(portfolio);
            DataNamesViewModel viewModel = new DataNamesViewModel(portfolio, dataUpdater, TestingGUICode.DummyReportLogger, TestingGUICode.DummyOpenTab, AccountType.Security);

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
            Moq.Mock<UICommon.Services.IFileInteractionService> fileMock = TestingGUICode.CreateFileMock("nothing");
            Moq.Mock<UICommon.Services.IDialogCreationService> dialogMock = TestingGUICode.CreateDialogMock();
            FinancialStructures.Database.Portfolio portfolio = TestingGUICode.CreateBasicDataBase();
            Action<Action<FinancialStructures.FinanceInterfaces.IPortfolio>> dataUpdater = TestingGUICode.CreateDataUpdater(portfolio);
            SelectedSecurityViewModel viewModel = new SelectedSecurityViewModel(portfolio, dataUpdater, TestingGUICode.DummyReportLogger, fileMock.Object, dialogMock.Object, new NameData("Fidelity", "China"));

            Assert.AreEqual(1, viewModel.SelectedSecurityData.Count);
        }

        [Test]
        public void CanAddValue()
        {
            Moq.Mock<UICommon.Services.IFileInteractionService> fileMock = TestingGUICode.CreateFileMock("nothing");
            Moq.Mock<UICommon.Services.IDialogCreationService> dialogMock = TestingGUICode.CreateDialogMock();
            FinancialStructures.Database.Portfolio portfolio = TestingGUICode.CreateBasicDataBase();
            Action<Action<FinancialStructures.FinanceInterfaces.IPortfolio>> dataUpdater = TestingGUICode.CreateDataUpdater(portfolio);
            SelectedSecurityViewModel viewModel = new SelectedSecurityViewModel(portfolio, dataUpdater, TestingGUICode.DummyReportLogger, fileMock.Object, dialogMock.Object, new NameData("Fidelity", "China"));

            Assert.AreEqual(1, viewModel.SelectedSecurityData.Count);
            SecurityDayData newValue = new SecurityDayData(new DateTime(2002, 1, 1), 1, 1, 1);
            viewModel.SelectedSecurityData.Add(newValue);
            viewModel.selectedValues = newValue;
            viewModel.AddEditSecurityDataCommand.Execute(1);
            Assert.AreEqual(2, viewModel.SelectedSecurityData.Count);
            Assert.AreEqual(2, portfolio.Funds.Single().Count());
        }

        [Test]
        public void CanEditValue()
        {
            Moq.Mock<UICommon.Services.IFileInteractionService> fileMock = TestingGUICode.CreateFileMock("nothing");
            Moq.Mock<UICommon.Services.IDialogCreationService> dialogMock = TestingGUICode.CreateDialogMock();
            FinancialStructures.Database.Portfolio portfolio = TestingGUICode.CreateBasicDataBase();
            Action<Action<FinancialStructures.FinanceInterfaces.IPortfolio>> dataUpdater = TestingGUICode.CreateDataUpdater(portfolio);
            SelectedSecurityViewModel viewModel = new SelectedSecurityViewModel(portfolio, dataUpdater, TestingGUICode.DummyReportLogger, fileMock.Object, dialogMock.Object, new NameData("Fidelity", "China"));

            Assert.AreEqual(1, viewModel.SelectedSecurityData.Count);
            SecurityDayData newValue = new SecurityDayData(new DateTime(2000, 1, 1), 1, 1, 1);
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
            Moq.Mock<UICommon.Services.IFileInteractionService> fileMock = TestingGUICode.CreateFileMock("nothing");
            Moq.Mock<UICommon.Services.IDialogCreationService> dialogMock = TestingGUICode.CreateDialogMock();
            FinancialStructures.Database.Portfolio portfolio = TestingGUICode.CreateBasicDataBase();
            Action<Action<FinancialStructures.FinanceInterfaces.IPortfolio>> dataUpdater = TestingGUICode.CreateDataUpdater(portfolio);
            SelectedSecurityViewModel viewModel = new SelectedSecurityViewModel(portfolio, dataUpdater, TestingGUICode.DummyReportLogger, fileMock.Object, dialogMock.Object, new NameData("Fidelity", "China"));

            Assert.AreEqual(1, viewModel.SelectedSecurityData.Count);
        }

        [Test]
        [Ignore("IncompeteArchitecture - FileInteraction does not currently allow for use in test environment.")]
        public void CanWriteToCSV()
        {
            Moq.Mock<UICommon.Services.IFileInteractionService> fileMock = TestingGUICode.CreateFileMock("nothing");
            Moq.Mock<UICommon.Services.IDialogCreationService> dialogMock = TestingGUICode.CreateDialogMock();
            FinancialStructures.Database.Portfolio portfolio = TestingGUICode.CreateBasicDataBase();
            Action<Action<FinancialStructures.FinanceInterfaces.IPortfolio>> dataUpdater = TestingGUICode.CreateDataUpdater(portfolio);
            SelectedSecurityViewModel viewModel = new SelectedSecurityViewModel(portfolio, dataUpdater, TestingGUICode.DummyReportLogger, fileMock.Object, dialogMock.Object, new NameData("Fidelity", "China"));

            Assert.AreEqual(1, viewModel.SelectedSecurityData.Count);
        }

        [Test]
        public void CanDeleteValue()
        {
            Moq.Mock<UICommon.Services.IFileInteractionService> fileMock = TestingGUICode.CreateFileMock("nothing");
            Moq.Mock<UICommon.Services.IDialogCreationService> dialogMock = TestingGUICode.CreateDialogMock();
            FinancialStructures.Database.Portfolio portfolio = TestingGUICode.CreateBasicDataBase();
            Action<Action<FinancialStructures.FinanceInterfaces.IPortfolio>> dataUpdater = TestingGUICode.CreateDataUpdater(portfolio);
            SelectedSecurityViewModel viewModel = new SelectedSecurityViewModel(portfolio, dataUpdater, TestingGUICode.DummyReportLogger, fileMock.Object, dialogMock.Object, new NameData("Fidelity", "China"));
            viewModel.selectedValues = viewModel.SelectedSecurityData.Single();
            Assert.AreEqual(1, viewModel.SelectedSecurityData.Count);

            viewModel.DeleteValuationCommand.Execute(1);

            Assert.AreEqual(0, portfolio.Funds.Single().Count());
        }
    }
}
