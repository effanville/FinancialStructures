using FinanceCommonViewModels;
using FinanceWindowsViewModels;
using FPD_UI_UnitTests.TestConstruction;
using NUnit.Framework;
using System;

namespace FPD_UI_UnitTests
{
    public class IntegrationTests
    {
        [Test]
        public void OpenDatabaseUpdatesAllTabs()
        {
            string databaseToLoad = TestingGUICode.ExampleDatabaseFolder + "\\BasicTestDatabase.xml";
            var testFilePath = AppDomain.CurrentDomain.SetupInformation.ApplicationBase + databaseToLoad;
            var fileMock = TestingGUICode.CreateFileMock(testFilePath);
            var dialogMock = TestingGUICode.CreateDialogMock();
            var viewModel = new MainWindowViewModel(fileMock.Object, dialogMock.Object);

            viewModel.OptionsToolbarCommands.LoadDatabaseCommand.Execute(1);

            Assert.AreEqual(1, viewModel.ProgramPortfolio.BankAccounts.Count);
            Assert.AreEqual(1, viewModel.ProgramPortfolio.Funds.Count);
            Assert.AreEqual(1, viewModel.ProgramPortfolio.BenchMarks.Count);

            var dataView = viewModel.Tabs[0] as BasicDataViewModel;
            Assert.AreEqual(1, dataView.AccountNames.Count);
            Assert.AreEqual(1, dataView.FundNames.Count);
            Assert.AreEqual(1, dataView.SectorNames.Count);

            var securityView = viewModel.Tabs[1] as SecurityEditWindowViewModel;
            var securityNamesView = securityView.Tabs[0] as SecurityNamesViewModel;
            Assert.AreEqual(1, securityNamesView.FundNames.Count);

            var bankAccView = viewModel.Tabs[2] as SingleValueEditWindowViewModel;
            var bankAccNamesView = bankAccView.Tabs[0] as DataNamesViewModel;
            Assert.AreEqual(1, bankAccNamesView.DataNames.Count);
        }
    }
}
