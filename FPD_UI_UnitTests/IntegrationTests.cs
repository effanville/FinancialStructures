using System;
using System.Linq;
using FinancialStructures.Database;
using FinanceCommonViewModels;
using FinanceWindowsViewModels;
using FPD_UI_UnitTests.TestConstruction;
using Moq;
using NUnit.Framework;
using UICommon.Services;

namespace FPD_UI_UnitTests
{
    /// <summary>
    /// Tests to ensure the entire ViewModel system integrates together.
    /// </summary>
    public class IntegrationTests
    {
        /// <summary>
        /// The open database button propagates the new database to all tabs in the main view.
        /// </summary>
        [Test]
        public void OpenDatabaseUpdatesAllTabs()
        {
            string databaseToLoad = TestingGUICode.ExampleDatabaseFolder + "\\BasicTestDatabase.xml";
            string testFilePath = AppDomain.CurrentDomain.SetupInformation.ApplicationBase + databaseToLoad;
            Mock<IFileInteractionService> fileMock = TestingGUICode.CreateFileMock(testFilePath);
            Mock<IDialogCreationService> dialogMock = TestingGUICode.CreateDialogMock();
            MainWindowViewModel viewModel = new MainWindowViewModel(fileMock.Object, dialogMock.Object);

            viewModel.OptionsToolbarCommands.LoadDatabaseCommand.Execute(1);

            Assert.AreEqual(1, viewModel.ProgramPortfolio.BankAccounts.Count);
            Assert.AreEqual(1, viewModel.ProgramPortfolio.Funds.Count);
            Assert.AreEqual(1, viewModel.ProgramPortfolio.BenchMarks.Count);

            BasicDataViewModel dataView = viewModel.Tabs[0] as BasicDataViewModel;
            Assert.AreEqual("Portfolio: BasicTestDatabase loaded.", dataView.PortfolioNameText);
            Assert.AreEqual("Total Securities: 1", dataView.SecurityTotalText);
            Assert.AreEqual("Total Bank Accounts: 1", dataView.BankAccountTotalText);

            SecurityEditWindowViewModel securityView = viewModel.Tabs.First(view => view is SecurityEditWindowViewModel) as SecurityEditWindowViewModel;
            DataNamesViewModel securityNamesView = securityView.Tabs[0] as DataNamesViewModel;
            Assert.AreEqual(1, securityNamesView.DataNames.Count);

            SingleValueEditWindowViewModel bankAccView = viewModel.Tabs.First(view => view is SingleValueEditWindowViewModel vm && vm.DataType == Account.BankAccount) as SingleValueEditWindowViewModel;
            DataNamesViewModel bankAccNamesView = bankAccView.Tabs[0] as DataNamesViewModel;
            Assert.AreEqual(1, bankAccNamesView.DataNames.Count);
        }
    }
}
