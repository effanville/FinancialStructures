using System;
using System.Windows;
using FinancialStructures.Database;
using Moq;
using NUnit.Framework;
using UICommon.Services;
using FinancialStructures.Database.Implementation;
using FinancePortfolioDatabase.GUI.ViewModels;
using System.IO.Abstractions;
using FinancePortfolioDatabase.Tests.TestHelpers;
using System.IO.Abstractions.TestingHelpers;
using System.IO;

namespace FinancePortfolioDatabase.Tests
{
    public class OptionsToolbarTests
    {
        [Test]
        public void CanOpenNewDatabase()
        {
            var fileSystem = new FileSystem();
            Mock<IFileInteractionService> fileMock = TestSetupHelper.CreateFileMock("notNeeded");
            Mock<IDialogCreationService> dialogMock = TestSetupHelper.CreateDialogMock(MessageBoxResult.Yes);
            Portfolio portfolio = TestSetupHelper.CreateBasicDataBase();
            Action<Action<IPortfolio>> dataUpdater = TestSetupHelper.CreateDataUpdater(portfolio);
            OptionsToolbarViewModel viewModel = new OptionsToolbarViewModel(portfolio, dataUpdater, TestSetupHelper.DummyReportLogger, TestSetupHelper.CreateGlobalsMock(fileSystem, fileMock.Object, dialogMock.Object));
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
            var tempFileSystem = new MockFileSystem();
            string file = File.ReadAllText(TestConstants.ExampleDatabaseLocation + "\\BasicTestDatabase.xml");
            string testPath = "c:/data/database.xml";

            tempFileSystem.AddFile(testPath, new MockFileData(file));

            Mock<IFileInteractionService> fileMock = TestSetupHelper.CreateFileMock(testPath);
            Mock<IDialogCreationService> dialogMock = TestSetupHelper.CreateDialogMock();
            Portfolio portfolio = TestSetupHelper.CreateEmptyDataBase();
            Action<Action<IPortfolio>> dataUpdater = TestSetupHelper.CreateDataUpdater(portfolio);
            OptionsToolbarViewModel viewModel = new OptionsToolbarViewModel(portfolio, dataUpdater, TestSetupHelper.DummyReportLogger, TestSetupHelper.CreateGlobalsMock(tempFileSystem, fileMock.Object, dialogMock.Object));
            viewModel.LoadDatabaseCommand.Execute(1);
            //Input prespecified example database

            Assert.AreEqual(testPath, portfolio.FilePath);
            Assert.AreEqual(1, portfolio.Funds.Count);
            Assert.AreEqual(1, portfolio.BankAccounts.Count);
            Assert.AreEqual(1, portfolio.BenchMarks.Count);
        }

        [Test]
        public void CanOpenAndSaveDatabase()
        {
            var tempFileSystem = new MockFileSystem();
            string file = File.ReadAllText(TestConstants.ExampleDatabaseLocation + "\\BasicTestDatabase.xml");
            string testPath = "c:/temp/database.xml";
            string savePath = "c:/temp/saved.xml";

            tempFileSystem.AddFile(testPath, new MockFileData(file));

            Mock<IFileInteractionService> fileMock = TestSetupHelper.CreateFileMock(testPath, savePath);
            Mock<IDialogCreationService> dialogMock = TestSetupHelper.CreateDialogMock();
            Portfolio portfolio = TestSetupHelper.CreateEmptyDataBase();
            Action<Action<IPortfolio>> dataUpdater = TestSetupHelper.CreateDataUpdater(portfolio);
            OptionsToolbarViewModel viewModel = new OptionsToolbarViewModel(portfolio, dataUpdater, TestSetupHelper.DummyReportLogger, TestSetupHelper.CreateGlobalsMock(tempFileSystem, fileMock.Object, dialogMock.Object));
            viewModel.LoadDatabaseCommand.Execute(1);
            //Input prespecified example database

            Assert.AreEqual(testPath, portfolio.FilePath);
            Assert.AreEqual(1, portfolio.Funds.Count);
            Assert.AreEqual(1, portfolio.BankAccounts.Count);
            Assert.AreEqual(1, portfolio.BenchMarks.Count);


            viewModel.SaveDatabaseCommand.Execute(1);

            var savedFile = tempFileSystem.GetFile(savePath);
            Assert.AreEqual(file, savedFile.TextContents);
        }

        [Test]
        [Ignore("IncompeteArchitecture - Requires improvements in dialog creation to be able to run.")]
        public void CanOpenHelpDocsPage()
        {
            var fileSystem = new FileSystem();
            string testFilePath = TestConstants.ExampleDatabaseLocation + "\\BasicTestDatabase.xml";
            Mock<IFileInteractionService> fileMock = TestSetupHelper.CreateFileMock(testFilePath);
            Mock<IDialogCreationService> dialogMock = TestSetupHelper.CreateDialogMock();
            Portfolio portfolio = TestSetupHelper.CreateEmptyDataBase();
            Action<Action<IPortfolio>> dataUpdater = TestSetupHelper.CreateDataUpdater(portfolio);
            OptionsToolbarViewModel viewModel = new OptionsToolbarViewModel(portfolio, dataUpdater, TestSetupHelper.DummyReportLogger, TestSetupHelper.CreateGlobalsMock(fileSystem, fileMock.Object, dialogMock.Object));
            viewModel.OpenHelpCommand.Execute(1);
            //Input prespecified example database

            Assert.AreEqual(testFilePath, portfolio.FilePath);
            Assert.AreEqual(1, portfolio.Funds.Count);
            Assert.AreEqual(1, portfolio.BankAccounts.Count);
            Assert.AreEqual(1, portfolio.BenchMarks.Count);
        }

        [Test]
        public void CanSaveDatabase()
        {
            var tempFileSystem = new MockFileSystem();
            string file = File.ReadAllText(TestConstants.ExampleDatabaseLocation + "\\BasicTestDatabase.xml");
            string testPath = "c:/temp/database.xml";

            Mock<IFileInteractionService> fileMock = TestSetupHelper.CreateFileMock(testPath);
            Mock<IDialogCreationService> dialogMock = TestSetupHelper.CreateDialogMock();
            Portfolio portfolio = TestSetupHelper.CreateBasicDataBase();
            Action<Action<IPortfolio>> dataUpdater = TestSetupHelper.CreateDataUpdater(portfolio);
            OptionsToolbarViewModel viewModel = new OptionsToolbarViewModel(portfolio, dataUpdater, TestSetupHelper.DummyReportLogger, TestSetupHelper.CreateGlobalsMock(tempFileSystem, fileMock.Object, dialogMock.Object));
            viewModel.SaveDatabaseCommand.Execute(1);
            //Input prespecified example database

            Assert.AreEqual(testPath, portfolio.FilePath);
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
            Mock<IFileInteractionService> fileMock = TestSetupHelper.CreateFileMock(testFilePath);
            Mock<IDialogCreationService> dialogMock = TestSetupHelper.CreateDialogMock();
            Portfolio portfolio = TestSetupHelper.CreateEmptyDataBase();
            Action<Action<IPortfolio>> dataUpdater = TestSetupHelper.CreateDataUpdater(portfolio);
            OptionsToolbarViewModel viewModel = new OptionsToolbarViewModel(portfolio, dataUpdater, TestSetupHelper.DummyReportLogger, TestSetupHelper.CreateGlobalsMock(fileSystem, fileMock.Object, dialogMock.Object));
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
            Mock<IFileInteractionService> fileMock = TestSetupHelper.CreateFileMock("notNeeded");
            Mock<IDialogCreationService> dialogMock = TestSetupHelper.CreateDialogMock(MessageBoxResult.Yes);
            Portfolio portfolio = TestSetupHelper.CreateBasicDataBase();
            Action<Action<IPortfolio>> dataUpdater = TestSetupHelper.CreateDataUpdater(portfolio);
            OptionsToolbarViewModel viewModel = new OptionsToolbarViewModel(portfolio, dataUpdater, TestSetupHelper.DummyReportLogger, TestSetupHelper.CreateGlobalsMock(fileSystem, fileMock.Object, dialogMock.Object));
            viewModel.RefreshCommand.Execute(1);
            //Check that data held is an empty database

            Assert.AreEqual("TestFilePath", portfolio.FilePath);
            Assert.AreEqual(1, portfolio.Funds.Count);
            Assert.AreEqual(1, portfolio.BankAccounts.Count);
            Assert.AreEqual(1, portfolio.BenchMarks.Count);
        }
    }
}
