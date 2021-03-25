using System;
using System.Linq;
using FinancialStructures.Database;
using FinancePortfolioDatabase.Tests.TestConstruction;
using Moq;
using NUnit.Framework;
using UICommon.Services;
using FinancePortfolioDatabase.GUI.ViewModels.Common;
using FinancePortfolioDatabase.GUI.ViewModels;
using FinancePortfolioDatabase.GUI.ViewModels.Security;
using System.IO.Abstractions;
using FinancialStructures.NamingStructures;
using StructureCommon.DisplayClasses;

namespace FinancePortfolioDatabase.Tests
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
        [STAThread]
        public void OpenDatabaseUpdatesAllTabs()
        {
            var fileSystem = new FileSystem();
            string testFilePath = $"{TestConstants.ExampleDatabaseLocation}\\BasicTestDatabase.xml";
            Mock<IFileInteractionService> fileMock = TestingGUICode.CreateFileMock(testFilePath);
            Mock<IDialogCreationService> dialogMock = TestingGUICode.CreateDialogMock();
            MainWindowViewModel viewModel = new MainWindowViewModel(TestingGUICode.CreateGlobalsMock(fileSystem, fileMock.Object, dialogMock.Object));

            viewModel.OptionsToolbarCommands.LoadDatabaseCommand.Execute(1);

            Assert.AreEqual(1, viewModel.ProgramPortfolio.BankAccountsThreadSafe.Count);
            Assert.AreEqual(1, viewModel.ProgramPortfolio.FundsThreadSafe.Count);
            Assert.AreEqual(1, viewModel.ProgramPortfolio.BenchMarksThreadSafe.Count);

            BasicDataViewModel dataView = viewModel.Tabs[0] as BasicDataViewModel;
            Assert.AreEqual("Portfolio: BasicTestDatabase loaded.", dataView.PortfolioNameText);
            Assert.AreEqual("Total Securities: 1", dataView.SecurityTotalText);
            Assert.AreEqual("Total Bank Accounts: 1", dataView.BankAccountTotalText);

            SecurityEditWindowViewModel securityView = viewModel.Tabs.First(view => view is SecurityEditWindowViewModel) as SecurityEditWindowViewModel;
            DataNamesViewModel securityNamesView = securityView.Tabs[0] as DataNamesViewModel;
            Assert.AreEqual(1, securityNamesView.DataNames.Count);

            ValueListWindowViewModel bankAccView = viewModel.Tabs.First(view => view is ValueListWindowViewModel vm && vm.DataType == Account.BankAccount) as ValueListWindowViewModel;
            DataNamesViewModel bankAccNamesView = bankAccView.Tabs[0] as DataNamesViewModel;
            Assert.AreEqual(1, bankAccNamesView.DataNames.Count);
        }


        [Test]
        [STAThread]
        public void AddingSecurityUpdatesSuccessfully()
        {
            var fileSystem = new FileSystem();
            string testFilePath = $"{TestConstants.ExampleDatabaseLocation}\\BasicTestDatabase.xml";
            Mock<IFileInteractionService> fileMock = TestingGUICode.CreateFileMock(testFilePath);
            Mock<IDialogCreationService> dialogMock = TestingGUICode.CreateDialogMock();
            MainWindowViewModel viewModel = new MainWindowViewModel(TestingGUICode.CreateGlobalsMock(fileSystem, fileMock.Object, dialogMock.Object));

            var securityTab = viewModel.Tabs.First(tab => tab is SecurityEditWindowViewModel);
            SecurityEditWindowViewModel securityViewModel = securityTab as SecurityEditWindowViewModel;
            var securityNames = securityViewModel.Tabs[0] as DataNamesViewModel;
            var selectedInitialName = new SelectableEquatable<NameData>(new NameData(), false);
            securityNames.DataNames.Add(selectedInitialName);
            securityNames.SelectionChangedCommand.Execute(selectedInitialName);

            var selectedEditedName = new Selectable<NameData>(new NameData("Forgotton", "New"), false);
            securityNames.CreateCommand.Execute(selectedEditedName);

            Assert.AreEqual(1, securityNames.DataNames.Count);
        }
    }
}
