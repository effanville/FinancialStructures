using System;
using System.Collections.Generic;
using System.Linq;
using FinanceCommonViewModels;
using FinancialStructures.NamingStructures;
using FinancialStructures.PortfolioAPI;
using FPD_UI_UnitTests.TestConstruction;
using NUnit.Framework;
using StructureCommon.DataStructures;

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
            Moq.Mock<UICommon.Services.IFileInteractionService> fileMock = TestingGUICode.CreateFileMock("nothing");
            Moq.Mock<UICommon.Services.IDialogCreationService> dialogMock = TestingGUICode.CreateDialogMock();
            FinancialStructures.Database.Portfolio portfolio = TestingGUICode.CreateBasicDataBase();
            Action<Action<FinancialStructures.FinanceInterfaces.IPortfolio>> dataUpdater = TestingGUICode.CreateDataUpdater(portfolio);
            SingleValueEditWindowViewModel viewModel = new SingleValueEditWindowViewModel("Dummy", portfolio, dataUpdater, TestingGUICode.DummyReportLogger, fileMock.Object, dialogMock.Object, AccountType.BankAccount);

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
            SingleValueEditWindowViewModel viewModel = new SingleValueEditWindowViewModel("Dummy", portfolio, dataUpdater, TestingGUICode.DummyReportLogger, fileMock.Object, dialogMock.Object, AccountType.BankAccount);
            FinancialStructures.Database.Portfolio newData = TestingGUICode.CreateBasicDataBase();
            viewModel.UpdateData(newData);

            Assert.AreEqual("TestFilePath", viewModel.DataStore.FilePath);
            Assert.AreEqual(1, viewModel.DataStore.BankAccounts.Count);
        }

        [Test]
        public void CanUpdateDataAndRemoveOldTab()
        {
            Moq.Mock<UICommon.Services.IFileInteractionService> fileMock = TestingGUICode.CreateFileMock("nothing");
            Moq.Mock<UICommon.Services.IDialogCreationService> dialogMock = TestingGUICode.CreateDialogMock();
            FinancialStructures.Database.Portfolio portfolio = TestingGUICode.CreateEmptyDataBase();
            Action<Action<FinancialStructures.FinanceInterfaces.IPortfolio>> dataUpdater = TestingGUICode.CreateDataUpdater(portfolio);
            SingleValueEditWindowViewModel viewModel = new SingleValueEditWindowViewModel("Dummy", portfolio, dataUpdater, TestingGUICode.DummyReportLogger, fileMock.Object, dialogMock.Object, AccountType.BankAccount);

            NameData newNameData = new NameData("Fidelity", "Europe");
            viewModel.LoadTabFunc(newNameData);

            Assert.AreEqual(2, viewModel.Tabs.Count);

            FinancialStructures.Database.Portfolio newData = TestingGUICode.CreateBasicDataBase();
            viewModel.UpdateData(newData);
            Assert.AreEqual(1, viewModel.Tabs.Count);
            Assert.AreEqual("TestFilePath", viewModel.DataStore.FilePath);
            Assert.AreEqual(1, viewModel.DataStore.BankAccounts.Count);
        }

        [Test]
        public void CanAddTab()
        {
            Moq.Mock<UICommon.Services.IFileInteractionService> fileMock = TestingGUICode.CreateFileMock("nothing");
            Moq.Mock<UICommon.Services.IDialogCreationService> dialogMock = TestingGUICode.CreateDialogMock();
            FinancialStructures.Database.Portfolio portfolio = TestingGUICode.CreateBasicDataBase();
            Action<Action<FinancialStructures.FinanceInterfaces.IPortfolio>> dataUpdater = TestingGUICode.CreateDataUpdater(portfolio);
            SingleValueEditWindowViewModel viewModel = new SingleValueEditWindowViewModel("Dummy", portfolio, dataUpdater, TestingGUICode.DummyReportLogger, fileMock.Object, dialogMock.Object, AccountType.BankAccount);

            NameData newData = new NameData("Fidelity", "China");
            viewModel.LoadTabFunc(newData);

            Assert.AreEqual(2, viewModel.Tabs.Count);
        }
    }

    public class DataNamesTests
    {
        [Test]
        public void CanOpen()
        {
            FinancialStructures.Database.Portfolio portfolio = TestingGUICode.CreateBasicDataBase();
            Action<Action<FinancialStructures.FinanceInterfaces.IPortfolio>> dataUpdater = TestingGUICode.CreateDataUpdater(portfolio);
            DataNamesViewModel viewModel = new DataNamesViewModel(portfolio, dataUpdater, TestingGUICode.DummyReportLogger, TestingGUICode.DummyOpenTab, AccountType.BankAccount);
            Assert.AreEqual(1, viewModel.DataNames.Count);
        }

        [Test]
        public void CanUpdateData()
        {
            Moq.Mock<UICommon.Services.IFileInteractionService> fileMock = TestingGUICode.CreateFileMock("nothing");
            Moq.Mock<UICommon.Services.IDialogCreationService> dialogMock = TestingGUICode.CreateDialogMock();
            FinancialStructures.Database.Portfolio portfolio = TestingGUICode.CreateEmptyDataBase();
            Action<Action<FinancialStructures.FinanceInterfaces.IPortfolio>> dataUpdater = TestingGUICode.CreateDataUpdater(portfolio);

            DataNamesViewModel viewModel = new DataNamesViewModel(portfolio, dataUpdater, TestingGUICode.DummyReportLogger, TestingGUICode.DummyOpenTab, AccountType.BankAccount);

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

            DataNamesViewModel viewModel = new DataNamesViewModel(portfolio, dataUpdater, TestingGUICode.DummyReportLogger, TestingGUICode.DummyOpenTab, AccountType.BankAccount);
            NameCompDate newName = new NameCompDate("company", "name", "GBP", "someUrl", new HashSet<string>(), DateTime.Today)
            {
                Company = "Company"
            };
            viewModel.fPreEditSelectedName = newName;
            viewModel.DataNames.Add(newName);
            viewModel.CreateCommand.Execute(1);
            Assert.AreEqual(2, viewModel.DataNames.Count);
            Assert.AreEqual(2, portfolio.BankAccounts.Count);
        }

        [Test]
        public void CanEditName()
        {
            Moq.Mock<UICommon.Services.IFileInteractionService> fileMock = TestingGUICode.CreateFileMock("nothing");
            Moq.Mock<UICommon.Services.IDialogCreationService> dialogMock = TestingGUICode.CreateDialogMock();
            FinancialStructures.Database.Portfolio portfolio = TestingGUICode.CreateBasicDataBase();
            Action<Action<FinancialStructures.FinanceInterfaces.IPortfolio>> dataUpdater = TestingGUICode.CreateDataUpdater(portfolio);

            DataNamesViewModel viewModel = new DataNamesViewModel(portfolio, dataUpdater, TestingGUICode.DummyReportLogger, TestingGUICode.DummyOpenTab, AccountType.BankAccount);
            viewModel.fPreEditSelectedName = viewModel.DataNames[0];

            viewModel.fPreEditSelectedName.Company = "NewCompany";

            viewModel.CreateCommand.Execute(1);
            Assert.AreEqual(1, viewModel.DataNames.Count);
            Assert.AreEqual(1, portfolio.BankAccounts.Count);

            Assert.AreEqual("NewCompany", portfolio.BankAccounts.Single().Company);
        }

        [Test]
        [Ignore("IncompeteArchitecture - Downloader does not currently allow for use in test environment.")]
        public void CanDownload()
        {
            Moq.Mock<UICommon.Services.IFileInteractionService> fileMock = TestingGUICode.CreateFileMock("nothing");
            Moq.Mock<UICommon.Services.IDialogCreationService> dialogMock = TestingGUICode.CreateDialogMock();
            FinancialStructures.Database.Portfolio portfolio = TestingGUICode.CreateEmptyDataBase();
            Action<Action<FinancialStructures.FinanceInterfaces.IPortfolio>> dataUpdater = TestingGUICode.CreateDataUpdater(portfolio);

            DataNamesViewModel viewModel = new DataNamesViewModel(portfolio, dataUpdater, TestingGUICode.DummyReportLogger, TestingGUICode.DummyOpenTab, AccountType.BankAccount)
            {
                fPreEditSelectedName = new NameCompDate("Barclays", "currentAccount")
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

            DataNamesViewModel viewModel = new DataNamesViewModel(portfolio, dataUpdater, TestingGUICode.DummyReportLogger, TestingGUICode.DummyOpenTab, AccountType.BankAccount);

            Assert.AreEqual(1, viewModel.DataStore.BankAccounts.Count);
            Assert.AreEqual(1, portfolio.BankAccounts.Count);
            viewModel.fPreEditSelectedName = new NameCompDate("Barclays", "currentAccount");
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
            Moq.Mock<UICommon.Services.IFileInteractionService> fileMock = TestingGUICode.CreateFileMock("nothing");
            Moq.Mock<UICommon.Services.IDialogCreationService> dialogMock = TestingGUICode.CreateDialogMock();
            FinancialStructures.Database.Portfolio portfolio = TestingGUICode.CreateBasicDataBase();
            Action<Action<FinancialStructures.FinanceInterfaces.IPortfolio>> dataUpdater = TestingGUICode.CreateDataUpdater(portfolio);
            SelectedSingleDataViewModel viewModel = new SelectedSingleDataViewModel(portfolio, dataUpdater, TestingGUICode.DummyReportLogger, fileMock.Object, dialogMock.Object, new NameData("Barclays", "currentAccount"), AccountType.BankAccount);

            Assert.AreEqual(1, viewModel.SelectedData.Count);
        }

        [Test]
        public void CanAddValue()
        {
            Moq.Mock<UICommon.Services.IFileInteractionService> fileMock = TestingGUICode.CreateFileMock("nothing");
            Moq.Mock<UICommon.Services.IDialogCreationService> dialogMock = TestingGUICode.CreateDialogMock();
            FinancialStructures.Database.Portfolio portfolio = TestingGUICode.CreateBasicDataBase();
            Action<Action<FinancialStructures.FinanceInterfaces.IPortfolio>> dataUpdater = TestingGUICode.CreateDataUpdater(portfolio);
            SelectedSingleDataViewModel viewModel = new SelectedSingleDataViewModel(portfolio, dataUpdater, TestingGUICode.DummyReportLogger, fileMock.Object, dialogMock.Object, new NameData("Barclays", "currentAccount"), AccountType.BankAccount);

            Assert.AreEqual(1, viewModel.SelectedData.Count);
            DailyValuation newValue = new DailyValuation(new DateTime(2002, 1, 1), 1);
            viewModel.SelectedData.Add(newValue);
            viewModel.fOldSelectedValue = newValue;
            viewModel.EditDataCommand.Execute(1);
            Assert.AreEqual(2, viewModel.SelectedData.Count);
            Assert.AreEqual(2, portfolio.BankAccounts.Single().Count());
        }

        [Test]
        public void CanEditValue()
        {
            Moq.Mock<UICommon.Services.IFileInteractionService> fileMock = TestingGUICode.CreateFileMock("nothing");
            Moq.Mock<UICommon.Services.IDialogCreationService> dialogMock = TestingGUICode.CreateDialogMock();
            FinancialStructures.Database.Portfolio portfolio = TestingGUICode.CreateBasicDataBase();
            Action<Action<FinancialStructures.FinanceInterfaces.IPortfolio>> dataUpdater = TestingGUICode.CreateDataUpdater(portfolio);
            SelectedSingleDataViewModel viewModel = new SelectedSingleDataViewModel(portfolio, dataUpdater, TestingGUICode.DummyReportLogger, fileMock.Object, dialogMock.Object, new NameData("Barclays", "currentAccount"), AccountType.BankAccount);

            Assert.AreEqual(1, viewModel.SelectedData.Count);
            DailyValuation newValue = new DailyValuation(new DateTime(2000, 1, 1), 1);
            viewModel.SelectedData[0] = newValue;
            viewModel.fOldSelectedValue = newValue;
            viewModel.EditDataCommand.Execute(1);
            Assert.AreEqual(1, viewModel.SelectedData.Count);
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
            SelectedSingleDataViewModel viewModel = new SelectedSingleDataViewModel(portfolio, dataUpdater, TestingGUICode.DummyReportLogger, fileMock.Object, dialogMock.Object, new NameData("Barclays", "currentAccount"), AccountType.BankAccount);

            Assert.AreEqual(1, viewModel.SelectedData.Count);
        }

        [Test]
        [Ignore("IncompeteArchitecture - FileInteraction does not currently allow for use in test environment.")]
        public void CanWriteToCSV()
        {
            Moq.Mock<UICommon.Services.IFileInteractionService> fileMock = TestingGUICode.CreateFileMock("nothing");
            Moq.Mock<UICommon.Services.IDialogCreationService> dialogMock = TestingGUICode.CreateDialogMock();
            FinancialStructures.Database.Portfolio portfolio = TestingGUICode.CreateBasicDataBase();
            Action<Action<FinancialStructures.FinanceInterfaces.IPortfolio>> dataUpdater = TestingGUICode.CreateDataUpdater(portfolio);
            SelectedSingleDataViewModel viewModel = new SelectedSingleDataViewModel(portfolio, dataUpdater, TestingGUICode.DummyReportLogger, fileMock.Object, dialogMock.Object, new NameData("Barclays", "currentAccount"), AccountType.BankAccount);

            Assert.AreEqual(1, viewModel.SelectedData.Count);
        }

        [Test]
        public void CanDeleteValue()
        {
            Moq.Mock<UICommon.Services.IFileInteractionService> fileMock = TestingGUICode.CreateFileMock("nothing");
            Moq.Mock<UICommon.Services.IDialogCreationService> dialogMock = TestingGUICode.CreateDialogMock();
            FinancialStructures.Database.Portfolio portfolio = TestingGUICode.CreateBasicDataBase();
            Action<Action<FinancialStructures.FinanceInterfaces.IPortfolio>> dataUpdater = TestingGUICode.CreateDataUpdater(portfolio);
            SelectedSingleDataViewModel viewModel = new SelectedSingleDataViewModel(portfolio, dataUpdater, TestingGUICode.DummyReportLogger, fileMock.Object, dialogMock.Object, new NameData("Barclays", "currentAccount"), AccountType.BankAccount);
            viewModel.fOldSelectedValue = viewModel.SelectedData.Single();
            Assert.AreEqual(1, viewModel.SelectedData.Count);

            viewModel.DeleteValuationCommand.Execute(1);

            Assert.AreEqual(0, portfolio.BankAccounts.Single().Count());
        }
    }
}
