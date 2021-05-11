using System;
using System.Windows;
using FinancialStructures.Database;
using FinancePortfolioDatabase.Tests.TestConstruction;
using Moq;
using NUnit.Framework;
using UICommon.Services;
using FinancialStructures.Database.Implementation;
using FinancePortfolioDatabase.GUI.ViewModels;
using System.IO.Abstractions;

namespace FinancePortfolioDatabase.Tests
{
    public class OptionsToolbarTests
    {
        [Test]
        public void CanOpenNewDatabase()
        {
            var fileSystem = new FileSystem();
            Mock<IFileInteractionService> fileMock = TestingGUICode.CreateFileMock("notNeeded");
            Mock<IDialogCreationService> dialogMock = TestingGUICode.CreateDialogMock(MessageBoxResult.Yes);
            Portfolio portfolio = TestingGUICode.CreateBasicDataBase();
            Action<Action<IPortfolio>> dataUpdater = TestingGUICode.CreateDataUpdater(portfolio);
            OptionsToolbarViewModel viewModel = new OptionsToolbarViewModel(portfolio, dataUpdater, TestingGUICode.DummyReportLogger, TestingGUICode.CreateGlobalsMock(fileSystem, fileMock.Object, dialogMock.Object));
            viewModel.NewDatabaseCommand.Execute(1);
            //Check that data held is an empty database

            Assert.AreEqual(null, portfolio.FilePath);
            Assert.AreEqual(0, portfolio.Funds.Count);
            Assert.AreEqual(0, portfolio.BankAccounts.Count);
            Assert.AreEqual(0, portfolio.BenchMarks.Count);
        }

        [Test]
        public void CanOpenDatabase()
        {
            var fileSystem = new FileSystem();
            string testFilePath = TestConstants.ExampleDatabaseLocation + "\\BasicTestDatabase.xml";
            Mock<IFileInteractionService> fileMock = TestingGUICode.CreateFileMock(testFilePath);
            Mock<IDialogCreationService> dialogMock = TestingGUICode.CreateDialogMock();
            Portfolio portfolio = TestingGUICode.CreateEmptyDataBase();
            Action<Action<IPortfolio>> dataUpdater = TestingGUICode.CreateDataUpdater(portfolio);
            OptionsToolbarViewModel viewModel = new OptionsToolbarViewModel(portfolio, dataUpdater, TestingGUICode.DummyReportLogger, TestingGUICode.CreateGlobalsMock(fileSystem, fileMock.Object, dialogMock.Object));
            viewModel.LoadDatabaseCommand.Execute(1);
            //Input prespecified example database

            Assert.AreEqual(testFilePath, portfolio.FilePath);
            Assert.AreEqual(1, portfolio.Funds.Count);
            Assert.AreEqual(1, portfolio.BankAccounts.Count);
            Assert.AreEqual(1, portfolio.BenchMarks.Count);
        }

        [Test]
        [Ignore("IncompeteArchitecture - Requires improvements in dialog creation to be able to run.")]
        public void CanOpenHelpDocsPage()
        {
            var fileSystem = new FileSystem();
            string testFilePath = TestConstants.ExampleDatabaseLocation + "\\BasicTestDatabase.xml";
            Mock<IFileInteractionService> fileMock = TestingGUICode.CreateFileMock(testFilePath);
            Mock<IDialogCreationService> dialogMock = TestingGUICode.CreateDialogMock();
            Portfolio portfolio = TestingGUICode.CreateEmptyDataBase();
            Action<Action<IPortfolio>> dataUpdater = TestingGUICode.CreateDataUpdater(portfolio);
            OptionsToolbarViewModel viewModel = new OptionsToolbarViewModel(portfolio, dataUpdater, TestingGUICode.DummyReportLogger, TestingGUICode.CreateGlobalsMock(fileSystem, fileMock.Object, dialogMock.Object));
            viewModel.OpenHelpCommand.Execute(1);
            //Input prespecified example database

            Assert.AreEqual(testFilePath, portfolio.FilePath);
            Assert.AreEqual(1, portfolio.Funds.Count);
            Assert.AreEqual(1, portfolio.BankAccounts.Count);
            Assert.AreEqual(1, portfolio.BenchMarks.Count);
        }

        [Test]
        [Ignore("IncompeteArchitecture - Test needs to have a dummy file structure to do this in.")]
        public void CanSaveDatabase()
        {
            var fileSystem = new FileSystem();
            string testFilePath = TestConstants.ExampleDatabaseLocation + "\\BasicTestDatabase.xml";
            Mock<IFileInteractionService> fileMock = TestingGUICode.CreateFileMock(testFilePath);
            Mock<IDialogCreationService> dialogMock = TestingGUICode.CreateDialogMock();
            Portfolio portfolio = TestingGUICode.CreateEmptyDataBase();
            Action<Action<IPortfolio>> dataUpdater = TestingGUICode.CreateDataUpdater(portfolio);
            OptionsToolbarViewModel viewModel = new OptionsToolbarViewModel(portfolio, dataUpdater, TestingGUICode.DummyReportLogger, TestingGUICode.CreateGlobalsMock(fileSystem, fileMock.Object, dialogMock.Object));
            viewModel.SaveDatabaseCommand.Execute(1);
            //Input prespecified example database

            Assert.AreEqual(testFilePath, portfolio.FilePath);
            Assert.AreEqual(1, portfolio.Funds.Count);
            Assert.AreEqual(1, portfolio.BankAccounts.Count);
            Assert.AreEqual(1, portfolio.BenchMarks.Count);
        }

        [Test]
        [Ignore("IncompeteArchitecture - Downloader does not currently allow for use in test environment.")]
        public void CanUpdateDatabase()
        {
            var fileSystem = new FileSystem();
            string testFilePath = TestConstants.ExampleDatabaseLocation + "\\BasicTestDatabase.xml";
            Mock<IFileInteractionService> fileMock = TestingGUICode.CreateFileMock(testFilePath);
            Mock<IDialogCreationService> dialogMock = TestingGUICode.CreateDialogMock();
            Portfolio portfolio = TestingGUICode.CreateEmptyDataBase();
            Action<Action<IPortfolio>> dataUpdater = TestingGUICode.CreateDataUpdater(portfolio);
            OptionsToolbarViewModel viewModel = new OptionsToolbarViewModel(portfolio, dataUpdater, TestingGUICode.DummyReportLogger, TestingGUICode.CreateGlobalsMock(fileSystem, fileMock.Object, dialogMock.Object));
            viewModel.UpdateDataCommand.Execute(1);
            //Input prespecified example database

            Assert.AreEqual(testFilePath, portfolio.FilePath);
            Assert.AreEqual(1, portfolio.Funds.Count);
            Assert.AreEqual(1, portfolio.BankAccounts.Count);
            Assert.AreEqual(1, portfolio.BenchMarks.Count);
        }

        [Test]
        public void CanRefreshDatabase()
        {
            var fileSystem = new FileSystem();
            Mock<IFileInteractionService> fileMock = TestingGUICode.CreateFileMock("notNeeded");
            Mock<IDialogCreationService> dialogMock = TestingGUICode.CreateDialogMock(MessageBoxResult.Yes);
            Portfolio portfolio = TestingGUICode.CreateBasicDataBase();
            Action<Action<IPortfolio>> dataUpdater = TestingGUICode.CreateDataUpdater(portfolio);
            OptionsToolbarViewModel viewModel = new OptionsToolbarViewModel(portfolio, dataUpdater, TestingGUICode.DummyReportLogger, TestingGUICode.CreateGlobalsMock(fileSystem, fileMock.Object, dialogMock.Object));
            viewModel.RefreshCommand.Execute(1);
            //Check that data held is an empty database

            Assert.AreEqual("TestFilePath", portfolio.FilePath);
            Assert.AreEqual(1, portfolio.Funds.Count);
            Assert.AreEqual(1, portfolio.BankAccounts.Count);
            Assert.AreEqual(1, portfolio.BenchMarks.Count);
        }
    }
}
