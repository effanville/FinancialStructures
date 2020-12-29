using System;
using System.Linq;
using System.Threading;
using FinanceCommonViewModels;
using FinancialStructures.Database;
using FinancialStructures.Database.Implementation;
using FinancialStructures.NamingStructures;
using FPD_UI_UnitTests.TestConstruction;
using Moq;
using NUnit.Framework;
using StructureCommon.DataStructures;
using UICommon.Services;

namespace FPD_UI_UnitTests.CommonWindowTests
{
    [TestFixture]
    [Apartment(ApartmentState.STA)]
    public class SelectedSingleDataViewModelTests
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
