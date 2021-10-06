using System;
using System.IO;
using System.IO.Abstractions;
using System.IO.Abstractions.TestingHelpers;
using System.Windows;
using Common.UI.Services;
using FinancePortfolioDatabase.GUI.ViewModels;
using FinancePortfolioDatabase.Tests.TestHelpers;
using FinancialStructures.Database;
using Moq;
using NUnit.Framework;

namespace FinancePortfolioDatabase.Tests
{
    public class OptionsToolbarTests
    {
        [Test]
        public void CanOpenNewDatabase()
        {
            FileSystem fileSystem = new FileSystem();
            Mock<IFileInteractionService> fileMock = TestSetupHelper.CreateFileMock("notNeeded");
            Mock<IDialogCreationService> dialogMock = TestSetupHelper.CreateDialogMock(MessageBoxResult.Yes);
            IPortfolio portfolio = TestSetupHelper.CreateBasicDataBase();
            Action<Action<IPortfolio>> dataUpdater = TestSetupHelper.CreateDataUpdater(portfolio);
            OptionsToolbarViewModel viewModel = new OptionsToolbarViewModel(TestSetupHelper.CreateGlobalsMock(fileSystem, fileMock.Object, dialogMock.Object), null, portfolio, dataUpdater);
            viewModel.NewDatabaseCommand.Execute(1);
            //Check that data held is an empty database

            Assert.AreEqual(null, portfolio.FilePath);
            Assert.AreEqual(0, portfolio.FundsThreadSafe.Count);
            Assert.AreEqual(0, portfolio.BankAccountsThreadSafe.Count);
            Assert.AreEqual(0, portfolio.BenchMarksThreadSafe.Count);
        }

        [Test]
        public void CanOpenDatabase()
        {
            MockFileSystem tempFileSystem = new MockFileSystem();
            string file = File.ReadAllText(TestConstants.ExampleDatabaseLocation + "\\BasicTestDatabase.xml");
            string testPath = "c:/temp/saved.xml";

            tempFileSystem.AddFile(testPath, new MockFileData(file));

            Mock<IFileInteractionService> fileMock = TestSetupHelper.CreateFileMock(testPath);
            Mock<IDialogCreationService> dialogMock = TestSetupHelper.CreateDialogMock();
            IPortfolio portfolio = TestSetupHelper.CreateEmptyDataBase();
            Action<Action<IPortfolio>> dataUpdater = TestSetupHelper.CreateDataUpdater(portfolio);
            OptionsToolbarViewModel viewModel = new OptionsToolbarViewModel(TestSetupHelper.CreateGlobalsMock(tempFileSystem, fileMock.Object, dialogMock.Object), null, portfolio, dataUpdater);
            viewModel.LoadDatabaseCommand.Execute(1);
            //Input prespecified example database

            Assert.AreEqual(testPath, portfolio.FilePath);
            Assert.AreEqual(1, portfolio.FundsThreadSafe.Count);
            Assert.AreEqual(1, portfolio.BankAccountsThreadSafe.Count);
            Assert.AreEqual(1, portfolio.BenchMarksThreadSafe.Count);
        }

        [Test]
        public void CanOpenAndSaveDatabase()
        {
            MockFileSystem tempFileSystem = new MockFileSystem();
            string file = File.ReadAllText(TestConstants.ExampleDatabaseLocation + "\\BasicTestDatabase.xml");
            string testPath = "c:/temp/database.xml";
            string savePath = "c:/temp/saved.xml";

            tempFileSystem.AddFile(testPath, new MockFileData(file));

            Mock<IFileInteractionService> fileMock = TestSetupHelper.CreateFileMock(testPath, savePath);
            Mock<IDialogCreationService> dialogMock = TestSetupHelper.CreateDialogMock();
            IPortfolio portfolio = TestSetupHelper.CreateEmptyDataBase();
            Action<Action<IPortfolio>> dataUpdater = TestSetupHelper.CreateDataUpdater(portfolio);
            OptionsToolbarViewModel viewModel = new OptionsToolbarViewModel(TestSetupHelper.CreateGlobalsMock(tempFileSystem, fileMock.Object, dialogMock.Object), null, portfolio, dataUpdater);
            viewModel.LoadDatabaseCommand.Execute(1);
            //Input prespecified example database

            Assert.AreEqual(savePath, portfolio.FilePath);
            Assert.AreEqual(1, portfolio.FundsThreadSafe.Count);
            Assert.AreEqual(1, portfolio.BankAccountsThreadSafe.Count);
            Assert.AreEqual(1, portfolio.BenchMarksThreadSafe.Count);


            viewModel.SaveDatabaseCommand.Execute(1);

            MockFileData savedFile = tempFileSystem.GetFile(savePath);
            Assert.AreEqual(file, savedFile.TextContents);
        }

        [Test]
        [Ignore("IncompeteArchitecture - Requires improvements in dialog creation to be able to run.")]
        public void CanOpenHelpDocsPage()
        {
            FileSystem fileSystem = new FileSystem();
            string testFilePath = TestConstants.ExampleDatabaseLocation + "\\BasicTestDatabase.xml";
            Mock<IFileInteractionService> fileMock = TestSetupHelper.CreateFileMock(testFilePath);
            Mock<IDialogCreationService> dialogMock = TestSetupHelper.CreateDialogMock();
            IPortfolio portfolio = TestSetupHelper.CreateEmptyDataBase();
            Action<Action<IPortfolio>> dataUpdater = TestSetupHelper.CreateDataUpdater(portfolio);
            OptionsToolbarViewModel viewModel = new OptionsToolbarViewModel(TestSetupHelper.CreateGlobalsMock(fileSystem, fileMock.Object, dialogMock.Object), null, portfolio, dataUpdater);
            viewModel.OpenHelpCommand.Execute(1);
            //Input prespecified example database

            Assert.AreEqual(testFilePath, portfolio.FilePath);
            Assert.AreEqual(1, portfolio.FundsThreadSafe.Count);
            Assert.AreEqual(1, portfolio.BankAccountsThreadSafe.Count);
            Assert.AreEqual(1, portfolio.BenchMarksThreadSafe.Count);
        }

        [Test]
        public void CanSaveDatabase()
        {
            MockFileSystem tempFileSystem = new MockFileSystem();
            string file = File.ReadAllText(TestConstants.ExampleDatabaseLocation + "\\BasicTestDatabase.xml");
            string testPath = "c:/temp/database.xml";

            Mock<IFileInteractionService> fileMock = TestSetupHelper.CreateFileMock(testPath);
            Mock<IDialogCreationService> dialogMock = TestSetupHelper.CreateDialogMock();
            IPortfolio portfolio = TestSetupHelper.CreateBasicDataBase();
            Action<Action<IPortfolio>> dataUpdater = TestSetupHelper.CreateDataUpdater(portfolio);
            OptionsToolbarViewModel viewModel = new OptionsToolbarViewModel(TestSetupHelper.CreateGlobalsMock(tempFileSystem, fileMock.Object, dialogMock.Object), null, portfolio, dataUpdater);
            viewModel.SaveDatabaseCommand.Execute(1);
            //Input prespecified example database

            Assert.AreEqual(testPath, portfolio.FilePath);
            Assert.AreEqual(1, portfolio.FundsThreadSafe.Count);
            Assert.AreEqual(1, portfolio.BankAccountsThreadSafe.Count);
            Assert.AreEqual(1, portfolio.BenchMarksThreadSafe.Count);
        }

        [Test]
        [Ignore("IncompeteArchitecture - Downloader does not currently allow for use in test environment.")]
        public void CanUpdateDatabase()
        {
            FileSystem fileSystem = new FileSystem();
            string testFilePath = TestConstants.ExampleDatabaseLocation + "\\BasicTestDatabase.xml";
            Mock<IFileInteractionService> fileMock = TestSetupHelper.CreateFileMock(testFilePath);
            Mock<IDialogCreationService> dialogMock = TestSetupHelper.CreateDialogMock();
            IPortfolio portfolio = TestSetupHelper.CreateEmptyDataBase();
            Action<Action<IPortfolio>> dataUpdater = TestSetupHelper.CreateDataUpdater(portfolio);
            OptionsToolbarViewModel viewModel = new OptionsToolbarViewModel(TestSetupHelper.CreateGlobalsMock(fileSystem, fileMock.Object, dialogMock.Object), null, portfolio, dataUpdater);
            viewModel.UpdateDataCommand.Execute(1);
            //Input prespecified example database

            Assert.AreEqual(testFilePath, portfolio.FilePath);
            Assert.AreEqual(1, portfolio.FundsThreadSafe.Count);
            Assert.AreEqual(1, portfolio.BankAccountsThreadSafe.Count);
            Assert.AreEqual(1, portfolio.BenchMarksThreadSafe.Count);
        }

        [Test]
        public void CanRefreshDatabase()
        {
            FileSystem fileSystem = new FileSystem();
            Mock<IFileInteractionService> fileMock = TestSetupHelper.CreateFileMock("notNeeded");
            Mock<IDialogCreationService> dialogMock = TestSetupHelper.CreateDialogMock(MessageBoxResult.Yes);
            IPortfolio portfolio = TestSetupHelper.CreateBasicDataBase();
            Action<Action<IPortfolio>> dataUpdater = TestSetupHelper.CreateDataUpdater(portfolio);
            OptionsToolbarViewModel viewModel = new OptionsToolbarViewModel(TestSetupHelper.CreateGlobalsMock(fileSystem, fileMock.Object, dialogMock.Object), null, portfolio, dataUpdater);
            viewModel.RefreshCommand.Execute(1);
            //Check that data held is an empty database

            Assert.AreEqual("TestFilePath", portfolio.FilePath);
            Assert.AreEqual(1, portfolio.FundsThreadSafe.Count);
            Assert.AreEqual(1, portfolio.BankAccountsThreadSafe.Count);
            Assert.AreEqual(1, portfolio.BenchMarksThreadSafe.Count);
        }
    }
}
