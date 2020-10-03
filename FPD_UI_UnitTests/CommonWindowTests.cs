using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using FinanceCommonViewModels;
using FinancialStructures.Database;
using FinancialStructures.FinanceInterfaces;
using FinancialStructures.NamingStructures;
using FPD_UI_UnitTests.TestConstruction;
using Moq;
using NUnit.Framework;
using StructureCommon.DataStructures;
using UICommon.Services;

namespace FPD_UI_UnitTests.CommonWindowTests
{
    /// <summary>
    /// Tests for window displaying single data stream data.
    /// </summary>
    [TestFixture]
    [Apartment(ApartmentState.STA)]
    public class CommonWindowTests
    {
        [Test]
        public void CanLoadSuccessfully()
        {
            Mock<IFileInteractionService> fileMock = TestingGUICode.CreateFileMock("nothing");
            Mock<IDialogCreationService> dialogMock = TestingGUICode.CreateDialogMock();
            Portfolio portfolio = TestingGUICode.CreateBasicDataBase();
            Action<Action<IPortfolio>> dataUpdater = TestingGUICode.CreateDataUpdater(portfolio);
            SingleValueEditWindowViewModel viewModel = new SingleValueEditWindowViewModel("Dummy", portfolio, dataUpdater, TestingGUICode.DummyReportLogger, fileMock.Object, dialogMock.Object, Account.BankAccount);

            Assert.AreEqual(1, viewModel.Tabs.Count);
            object tab = viewModel.Tabs.Single();
            DataNamesViewModel nameModel = tab as DataNamesViewModel;
            Assert.AreEqual(1, nameModel.DataNames.Count);
        }

        [Test]
        public void CanUpdateData()
        {
            Mock<IFileInteractionService> fileMock = TestingGUICode.CreateFileMock("nothing");
            Mock<IDialogCreationService> dialogMock = TestingGUICode.CreateDialogMock();
            Portfolio portfolio = TestingGUICode.CreateEmptyDataBase();
            Action<Action<IPortfolio>> dataUpdater = TestingGUICode.CreateDataUpdater(portfolio);
            SingleValueEditWindowViewModel viewModel = new SingleValueEditWindowViewModel("Dummy", portfolio, dataUpdater, TestingGUICode.DummyReportLogger, fileMock.Object, dialogMock.Object, Account.BankAccount);
            Portfolio newData = TestingGUICode.CreateBasicDataBase();
            viewModel.UpdateData(newData);

            Assert.AreEqual("TestFilePath", viewModel.DataStore.FilePath);
            Assert.AreEqual(1, viewModel.DataStore.BankAccounts.Count);
        }

        [Test]
        public void CanUpdateDataAndRemoveOldTab()
        {
            Mock<IFileInteractionService> fileMock = TestingGUICode.CreateFileMock("nothing");
            Mock<IDialogCreationService> dialogMock = TestingGUICode.CreateDialogMock();
            Portfolio portfolio = TestingGUICode.CreateEmptyDataBase();
            Action<Action<IPortfolio>> dataUpdater = TestingGUICode.CreateDataUpdater(portfolio);
            SingleValueEditWindowViewModel viewModel = new SingleValueEditWindowViewModel("Dummy", portfolio, dataUpdater, TestingGUICode.DummyReportLogger, fileMock.Object, dialogMock.Object, Account.BankAccount);

            NameData newNameData = new NameData("Fidelity", "Europe");
            viewModel.LoadTabFunc(newNameData);

            Assert.AreEqual(2, viewModel.Tabs.Count);

            Portfolio newData = TestingGUICode.CreateBasicDataBase();
            viewModel.UpdateData(newData);
            Assert.AreEqual(1, viewModel.Tabs.Count);
            Assert.AreEqual("TestFilePath", viewModel.DataStore.FilePath);
            Assert.AreEqual(1, viewModel.DataStore.BankAccounts.Count);
        }

        [Test]
        public void CanAddTab()
        {
            Mock<IFileInteractionService> fileMock = TestingGUICode.CreateFileMock("nothing");
            Mock<IDialogCreationService> dialogMock = TestingGUICode.CreateDialogMock();
            Portfolio portfolio = TestingGUICode.CreateBasicDataBase();
            Action<Action<IPortfolio>> dataUpdater = TestingGUICode.CreateDataUpdater(portfolio);
            SingleValueEditWindowViewModel viewModel = new SingleValueEditWindowViewModel("Dummy", portfolio, dataUpdater, TestingGUICode.DummyReportLogger, fileMock.Object, dialogMock.Object, Account.BankAccount);

            NameData newData = new NameData("Fidelity", "China");
            viewModel.LoadTabFunc(newData);

            Assert.AreEqual(2, viewModel.Tabs.Count);
        }
    }

    [TestFixture]
    [Apartment(ApartmentState.STA)]
    public class DataNamesTests
    {
        [Test]
        public void CanOpen()
        {
            Portfolio portfolio = TestingGUICode.CreateBasicDataBase();
            Action<Action<IPortfolio>> dataUpdater = TestingGUICode.CreateDataUpdater(portfolio);
            DataNamesViewModel viewModel = new DataNamesViewModel(portfolio, dataUpdater, TestingGUICode.DummyReportLogger, TestingGUICode.DummyOpenTab, Account.BankAccount);
            Assert.AreEqual(1, viewModel.DataNames.Count);
        }

        [Test]
        public void CanUpdateData()
        {
            Mock<IFileInteractionService> fileMock = TestingGUICode.CreateFileMock("nothing");
            Mock<IDialogCreationService> dialogMock = TestingGUICode.CreateDialogMock();
            Portfolio portfolio = TestingGUICode.CreateEmptyDataBase();
            Action<Action<IPortfolio>> dataUpdater = TestingGUICode.CreateDataUpdater(portfolio);

            DataNamesViewModel viewModel = new DataNamesViewModel(portfolio, dataUpdater, TestingGUICode.DummyReportLogger, TestingGUICode.DummyOpenTab, Account.BankAccount);

            Portfolio newData = TestingGUICode.CreateBasicDataBase();

            viewModel.UpdateData(newData);

            Assert.AreEqual(1, viewModel.DataNames.Count);
        }

        [Test]
        public void CanCreateNew()
        {
            Mock<IFileInteractionService> fileMock = TestingGUICode.CreateFileMock("nothing");
            Mock<IDialogCreationService> dialogMock = TestingGUICode.CreateDialogMock();
            Portfolio portfolio = TestingGUICode.CreateBasicDataBase();
            Action<Action<IPortfolio>> dataUpdater = TestingGUICode.CreateDataUpdater(portfolio);

            DataNamesViewModel viewModel = new DataNamesViewModel(portfolio, dataUpdater, TestingGUICode.DummyReportLogger, TestingGUICode.DummyOpenTab, Account.BankAccount);
            NameCompDate newName = new NameCompDate("company", "name", "GBP", "someUrl", new HashSet<string>(), DateTime.Today)
            {
                Company = "Company"
            };
            viewModel.fPreEditSelectedName = newName;
            viewModel.DataNames.Add(newName);
            var dataGridArgs = TestingGUICode.CreateRowArgs(viewModel.DataNames.Last());
            viewModel.CreateCommand.Execute(dataGridArgs);
            Assert.AreEqual(2, viewModel.DataNames.Count);
            Assert.AreEqual(2, portfolio.BankAccounts.Count);
        }

        [Test]
        [STAThread]
        public void CanEditName()
        {
            Mock<IFileInteractionService> fileMock = TestingGUICode.CreateFileMock("nothing");
            Mock<IDialogCreationService> dialogMock = TestingGUICode.CreateDialogMock();
            Portfolio portfolio = TestingGUICode.CreateBasicDataBase();
            Action<Action<IPortfolio>> dataUpdater = TestingGUICode.CreateDataUpdater(portfolio);

            DataNamesViewModel viewModel = new DataNamesViewModel(portfolio, dataUpdater, TestingGUICode.DummyReportLogger, TestingGUICode.DummyOpenTab, Account.BankAccount);
            viewModel.fPreEditSelectedName = viewModel.DataNames[0].Copy();
            viewModel.DataNames[0].Company = "NewCompany";
            var dataGridArgs = TestingGUICode.CreateRowArgs(viewModel.DataNames[0]);
            viewModel.CreateCommand.Execute(dataGridArgs);
            Assert.AreEqual(1, viewModel.DataNames.Count);
            Assert.AreEqual(1, portfolio.BankAccounts.Count);

            Assert.AreEqual("NewCompany", portfolio.BankAccounts.Single().Company);
        }

        [Test]
        [Ignore("IncompeteArchitecture - Downloader does not currently allow for use in test environment.")]
        public void CanDownload()
        {
            Mock<IFileInteractionService> fileMock = TestingGUICode.CreateFileMock("nothing");
            Mock<IDialogCreationService> dialogMock = TestingGUICode.CreateDialogMock();
            Portfolio portfolio = TestingGUICode.CreateEmptyDataBase();
            Action<Action<IPortfolio>> dataUpdater = TestingGUICode.CreateDataUpdater(portfolio);

            DataNamesViewModel viewModel = new DataNamesViewModel(portfolio, dataUpdater, TestingGUICode.DummyReportLogger, TestingGUICode.DummyOpenTab, Account.BankAccount)
            {
                fPreEditSelectedName = new NameCompDate("Barclays", "currentAccount")
            };
            viewModel.DownloadCommand.Execute(1);

            Assert.AreEqual(1, viewModel.DataNames.Count);
        }

        [Test]
        public void CanDelete()
        {
            Mock<IFileInteractionService> fileMock = TestingGUICode.CreateFileMock("nothing");
            Mock<IDialogCreationService> dialogMock = TestingGUICode.CreateDialogMock();
            Portfolio portfolio = TestingGUICode.CreateBasicDataBase();
            Action<Action<IPortfolio>> dataUpdater = TestingGUICode.CreateDataUpdater(portfolio);

            DataNamesViewModel viewModel = new DataNamesViewModel(portfolio, dataUpdater, TestingGUICode.DummyReportLogger, TestingGUICode.DummyOpenTab, Account.BankAccount);

            Assert.AreEqual(1, viewModel.DataStore.BankAccounts.Count);
            Assert.AreEqual(1, portfolio.BankAccounts.Count);
            viewModel.fPreEditSelectedName = new NameCompDate("Barclays", "currentAccount");
            viewModel.DeleteCommand.Execute(1);
            Assert.AreEqual(0, viewModel.DataStore.BankAccounts.Count);
            Assert.AreEqual(0, portfolio.BankAccounts.Count);
        }
    }

    [TestFixture]
    [Apartment(ApartmentState.STA)]
    public class SelectedAccountDataTests
    {

        [Test]
        public void CanOpenWindow()
        {
            Mock<IFileInteractionService> fileMock = TestingGUICode.CreateFileMock("nothing");
            Mock<IDialogCreationService> dialogMock = TestingGUICode.CreateDialogMock();
            Portfolio portfolio = TestingGUICode.CreateBasicDataBase();
            Action<Action<IPortfolio>> dataUpdater = TestingGUICode.CreateDataUpdater(portfolio);
            SelectedSingleDataViewModel viewModel = new SelectedSingleDataViewModel(portfolio, dataUpdater, TestingGUICode.DummyReportLogger, fileMock.Object, dialogMock.Object, new NameData("Barclays", "currentAccount"), Account.BankAccount);

            Assert.AreEqual(1, viewModel.SelectedData.Count);
        }

        [Test]
        public void CanAddValue()
        {
            Mock<IFileInteractionService> fileMock = TestingGUICode.CreateFileMock("nothing");
            Mock<IDialogCreationService> dialogMock = TestingGUICode.CreateDialogMock();
            Portfolio portfolio = TestingGUICode.CreateBasicDataBase();
            Action<Action<IPortfolio>> dataUpdater = TestingGUICode.CreateDataUpdater(portfolio);
            SelectedSingleDataViewModel viewModel = new SelectedSingleDataViewModel(portfolio, dataUpdater, TestingGUICode.DummyReportLogger, fileMock.Object, dialogMock.Object, new NameData("Barclays", "currentAccount"), Account.BankAccount);

            Assert.AreEqual(1, viewModel.SelectedData.Count);
            DailyValuation newValue = new DailyValuation(new DateTime(2002, 1, 1), 1);
            viewModel.SelectedData.Add(newValue);
            viewModel.fOldSelectedValue = newValue.Copy();

            var dataGridArgs = TestingGUICode.CreateRowArgs(viewModel.SelectedData.Last());
            viewModel.EditDataCommand.Execute(dataGridArgs);
            Assert.AreEqual(2, viewModel.SelectedData.Count);
            Assert.AreEqual(2, portfolio.BankAccounts.Single().Count());
        }

        [Test]
        public void CanEditValue()
        {
            Mock<IFileInteractionService> fileMock = TestingGUICode.CreateFileMock("nothing");
            Mock<IDialogCreationService> dialogMock = TestingGUICode.CreateDialogMock();
            Portfolio portfolio = TestingGUICode.CreateBasicDataBase();
            Action<Action<IPortfolio>> dataUpdater = TestingGUICode.CreateDataUpdater(portfolio);
            SelectedSingleDataViewModel viewModel = new SelectedSingleDataViewModel(portfolio, dataUpdater, TestingGUICode.DummyReportLogger, fileMock.Object, dialogMock.Object, new NameData("Barclays", "currentAccount"), Account.BankAccount);

            Assert.AreEqual(1, viewModel.SelectedData.Count);
            viewModel.fOldSelectedValue = viewModel.SelectedData[0].Copy();
            DailyValuation newValue = new DailyValuation(new DateTime(2000, 1, 1), 1);
            viewModel.SelectedData[0] = newValue;

            var dataGridArgs = TestingGUICode.CreateRowArgs(viewModel.SelectedData.Last());
            viewModel.EditDataCommand.Execute(dataGridArgs);
            Assert.AreEqual(1, viewModel.SelectedData.Count);
            Assert.AreEqual(1, portfolio.Funds.Single().Count());
            Assert.AreEqual(new DateTime(2000, 1, 1), portfolio.Funds.Single().FirstValue().Day);
        }


        [Test]
        [Ignore("IncompeteArchitecture - FileInteraction does not currently allow for use in test environment.")]
        public void CanAddFromCSV()
        {
            Mock<IFileInteractionService> fileMock = TestingGUICode.CreateFileMock("nothing");
            Mock<IDialogCreationService> dialogMock = TestingGUICode.CreateDialogMock();
            Portfolio portfolio = TestingGUICode.CreateBasicDataBase();
            Action<Action<IPortfolio>> dataUpdater = TestingGUICode.CreateDataUpdater(portfolio);
            SelectedSingleDataViewModel viewModel = new SelectedSingleDataViewModel(portfolio, dataUpdater, TestingGUICode.DummyReportLogger, fileMock.Object, dialogMock.Object, new NameData("Barclays", "currentAccount"), Account.BankAccount);

            Assert.AreEqual(1, viewModel.SelectedData.Count);
        }

        [Test]
        [Ignore("IncompeteArchitecture - FileInteraction does not currently allow for use in test environment.")]
        public void CanWriteToCSV()
        {
            Mock<IFileInteractionService> fileMock = TestingGUICode.CreateFileMock("nothing");
            Mock<IDialogCreationService> dialogMock = TestingGUICode.CreateDialogMock();
            Portfolio portfolio = TestingGUICode.CreateBasicDataBase();
            Action<Action<IPortfolio>> dataUpdater = TestingGUICode.CreateDataUpdater(portfolio);
            SelectedSingleDataViewModel viewModel = new SelectedSingleDataViewModel(portfolio, dataUpdater, TestingGUICode.DummyReportLogger, fileMock.Object, dialogMock.Object, new NameData("Barclays", "currentAccount"), Account.BankAccount);

            Assert.AreEqual(1, viewModel.SelectedData.Count);
        }

        [Test]
        public void CanDeleteValue()
        {
            Mock<IFileInteractionService> fileMock = TestingGUICode.CreateFileMock("nothing");
            Mock<IDialogCreationService> dialogMock = TestingGUICode.CreateDialogMock();
            Portfolio portfolio = TestingGUICode.CreateBasicDataBase();
            Action<Action<IPortfolio>> dataUpdater = TestingGUICode.CreateDataUpdater(portfolio);
            SelectedSingleDataViewModel viewModel = new SelectedSingleDataViewModel(portfolio, dataUpdater, TestingGUICode.DummyReportLogger, fileMock.Object, dialogMock.Object, new NameData("Barclays", "currentAccount"), Account.BankAccount);
            viewModel.fOldSelectedValue = viewModel.SelectedData.Single();
            Assert.AreEqual(1, viewModel.SelectedData.Count);

            viewModel.DeleteValuationCommand.Execute(1);

            Assert.AreEqual(0, portfolio.BankAccounts.Single().Count());
        }
    }
}
