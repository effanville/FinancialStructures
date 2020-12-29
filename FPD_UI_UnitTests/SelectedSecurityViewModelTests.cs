using System;
using System.Linq;
using System.Threading;
using FinanceWindowsViewModels;
using FinancialStructures.Database;
using FinancialStructures.Database.Implementation;
using FinancialStructures.DataStructures;
using FinancialStructures.NamingStructures;
using FPD_UI_UnitTests.TestConstruction;
using Moq;
using NUnit.Framework;
using UICommon.Services;

namespace FPD_UI_UnitTests.SecurityWindowTests
{
    [TestFixture]
    [Apartment(ApartmentState.STA)]
    public class SelectedSecurityViewModelTests
    {
        [Test]
        public void CanOpenWindow()
        {
            Mock<IFileInteractionService> fileMock = TestingGUICode.CreateFileMock("nothing");
            Mock<IDialogCreationService> dialogMock = TestingGUICode.CreateDialogMock();
            Portfolio portfolio = TestingGUICode.CreateBasicDataBase();
            Action<Action<IPortfolio>> dataUpdater = TestingGUICode.CreateDataUpdater(portfolio);
            SelectedSecurityViewModel viewModel = new SelectedSecurityViewModel(portfolio, dataUpdater, TestingGUICode.DummyReportLogger, fileMock.Object, dialogMock.Object, new NameData("Fidelity", "China"));

            Assert.AreEqual(1, viewModel.SelectedSecurityData.Count);
        }

        [Test]
        public void CanAddValue()
        {
            Mock<IFileInteractionService> fileMock = TestingGUICode.CreateFileMock("nothing");
            Mock<IDialogCreationService> dialogMock = TestingGUICode.CreateDialogMock();
            Portfolio portfolio = TestingGUICode.CreateBasicDataBase();
            Action<Action<IPortfolio>> dataUpdater = TestingGUICode.CreateDataUpdater(portfolio);
            SelectedSecurityViewModel viewModel = new SelectedSecurityViewModel(portfolio, dataUpdater, TestingGUICode.DummyReportLogger, fileMock.Object, dialogMock.Object, new NameData("Fidelity", "China"));

            Assert.AreEqual(1, viewModel.SelectedSecurityData.Count);
            SecurityDayData newValue = new SecurityDayData(new DateTime(2002, 1, 1), 1, 1, 1);
            viewModel.SelectedSecurityData.Add(newValue);

            viewModel.fOldSelectedValues = newValue.Copy();

            var dataGridArgs = TestingGUICode.CreateRowArgs(viewModel.SelectedSecurityData.Last());
            viewModel.AddEditSecurityDataCommand.Execute(dataGridArgs);
            Assert.AreEqual(2, viewModel.SelectedSecurityData.Count);
            Assert.AreEqual(2, portfolio.Funds.Single().Count());
        }

        [Test]
        public void CanEditValue()
        {
            Mock<IFileInteractionService> fileMock = TestingGUICode.CreateFileMock("nothing");
            Mock<IDialogCreationService> dialogMock = TestingGUICode.CreateDialogMock();
            Portfolio portfolio = TestingGUICode.CreateBasicDataBase();
            Action<Action<IPortfolio>> dataUpdater = TestingGUICode.CreateDataUpdater(portfolio);
            SelectedSecurityViewModel viewModel = new SelectedSecurityViewModel(portfolio, dataUpdater, TestingGUICode.DummyReportLogger, fileMock.Object, dialogMock.Object, new NameData("Fidelity", "China"));

            Assert.AreEqual(1, viewModel.SelectedSecurityData.Count);
            viewModel.fOldSelectedValues = viewModel.SelectedSecurityData[0].Copy();
            SecurityDayData newValue = new SecurityDayData(new DateTime(2000, 1, 1), 1, 1, 1);
            viewModel.SelectedSecurityData[0] = newValue;

            var dataGridArgs = TestingGUICode.CreateRowArgs(viewModel.SelectedSecurityData.Last());
            viewModel.AddEditSecurityDataCommand.Execute(dataGridArgs);
            Assert.AreEqual(1, viewModel.SelectedSecurityData.Count);
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
            SelectedSecurityViewModel viewModel = new SelectedSecurityViewModel(portfolio, dataUpdater, TestingGUICode.DummyReportLogger, fileMock.Object, dialogMock.Object, new NameData("Fidelity", "China"));

            Assert.AreEqual(1, viewModel.SelectedSecurityData.Count);
        }

        [Test]
        [Ignore("IncompeteArchitecture - FileInteraction does not currently allow for use in test environment.")]
        public void CanWriteToCSV()
        {
            Mock<IFileInteractionService> fileMock = TestingGUICode.CreateFileMock("nothing");
            Mock<IDialogCreationService> dialogMock = TestingGUICode.CreateDialogMock();
            Portfolio portfolio = TestingGUICode.CreateBasicDataBase();
            Action<Action<IPortfolio>> dataUpdater = TestingGUICode.CreateDataUpdater(portfolio);
            SelectedSecurityViewModel viewModel = new SelectedSecurityViewModel(portfolio, dataUpdater, TestingGUICode.DummyReportLogger, fileMock.Object, dialogMock.Object, new NameData("Fidelity", "China"));

            Assert.AreEqual(1, viewModel.SelectedSecurityData.Count);
        }

        [Test]
        public void CanDeleteValue()
        {
            Mock<IFileInteractionService> fileMock = TestingGUICode.CreateFileMock("nothing");
            Mock<IDialogCreationService> dialogMock = TestingGUICode.CreateDialogMock();
            Portfolio portfolio = TestingGUICode.CreateBasicDataBase();
            Action<Action<IPortfolio>> dataUpdater = TestingGUICode.CreateDataUpdater(portfolio);
            SelectedSecurityViewModel viewModel = new SelectedSecurityViewModel(portfolio, dataUpdater, TestingGUICode.DummyReportLogger, fileMock.Object, dialogMock.Object, new NameData("Fidelity", "China"));
            viewModel.fOldSelectedValues = viewModel.SelectedSecurityData.Single();
            Assert.AreEqual(1, viewModel.SelectedSecurityData.Count);

            viewModel.DeleteValuationCommand.Execute(1);

            Assert.AreEqual(0, portfolio.Funds.Single().Count());
        }
    }
}
