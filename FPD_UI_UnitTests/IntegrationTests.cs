using System;
using FinanceCommonViewModels;
using FinanceWindowsViewModels;
using FPD_UI_UnitTests.TestConstruction;
using NUnit.Framework;

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
            Moq.Mock<UICommon.Services.IFileInteractionService> fileMock = TestingGUICode.CreateFileMock(testFilePath);
            Moq.Mock<UICommon.Services.IDialogCreationService> dialogMock = TestingGUICode.CreateDialogMock();
            MainWindowViewModel viewModel = new MainWindowViewModel(fileMock.Object, dialogMock.Object);

            viewModel.OptionsToolbarCommands.LoadDatabaseCommand.Execute(1);

            Assert.AreEqual(1, viewModel.ProgramPortfolio.BankAccounts.Count);
            Assert.AreEqual(1, viewModel.ProgramPortfolio.Funds.Count);
            Assert.AreEqual(1, viewModel.ProgramPortfolio.BenchMarks.Count);

            BasicDataViewModel dataView = viewModel.Tabs[0] as BasicDataViewModel;
            Assert.AreEqual(1, dataView.AccountNames.Count);
            Assert.AreEqual(1, dataView.FundNames.Count);
            Assert.AreEqual(1, dataView.SectorNames.Count);

            SecurityEditWindowViewModel securityView = viewModel.Tabs[1] as SecurityEditWindowViewModel;
            DataNamesViewModel securityNamesView = securityView.Tabs[0] as DataNamesViewModel;
            Assert.AreEqual(1, securityNamesView.DataNames.Count);

            SingleValueEditWindowViewModel bankAccView = viewModel.Tabs[2] as SingleValueEditWindowViewModel;
            DataNamesViewModel bankAccNamesView = bankAccView.Tabs[0] as DataNamesViewModel;
            Assert.AreEqual(1, bankAccNamesView.DataNames.Count);
        }
    }
}
