using FinancialStructures.Database;
using FinancePortfolioDatabase.Tests.TestHelpers;
using NUnit.Framework;
using FinancePortfolioDatabase.GUI.ViewModels.Common;
using FinancePortfolioDatabase.GUI.ViewModels;
using FinancePortfolioDatabase.GUI.ViewModels.Security;
using FinancialStructures.NamingStructures;
using StructureCommon.DisplayClasses;
using FinancePortfolioDatabase.Tests.ViewModelExtensions;

namespace FinancePortfolioDatabase.Tests
{
    /// <summary>
    /// Tests to ensure the entire ViewModel system integrates together.
    /// </summary>
    public class IntegrationTests : MainWindowViewModelTestHelper
    {
        [SetUp]
        public void SetUp()
        {
            PortfolioFilePath = $"{TestConstants.ExampleDatabaseLocation}\\BasicTestDatabase.xml";
        }

        /// <summary>
        /// The open database button propagates the new database to all tabs in the main view.
        /// </summary>
        [Test]
        public void OpenDatabaseUpdatesAllTabs()
        {
            ViewModel.OptionsToolbarCommands.LoadDatabaseCommand.Execute(1);

            Assert.AreEqual(1, ViewModel.ProgramPortfolio.BankAccountsThreadSafe.Count);
            Assert.AreEqual(1, ViewModel.ProgramPortfolio.FundsThreadSafe.Count);
            Assert.AreEqual(1, ViewModel.ProgramPortfolio.BenchMarksThreadSafe.Count);

            BasicDataViewModel dataView = ViewModel.Tabs[0] as BasicDataViewModel;
            Assert.AreEqual("Portfolio: BasicTestDatabase loaded.", dataView.PortfolioNameText);
            Assert.AreEqual("Total Securities: 1", dataView.SecurityTotalText);
            Assert.AreEqual("Total Bank Accounts: 1", dataView.BankAccountTotalText);

            SecurityEditWindowViewModel securityView = ViewModel.SecurityWindow();
            DataNamesViewModel securityNamesView = securityView.DataNames();
            Assert.AreEqual(1, securityNamesView.DataNames.Count);

            ValueListWindowViewModel bankAccView = ViewModel.Window(Account.BankAccount);
            DataNamesViewModel bankAccNamesView = bankAccView.DataNames();
            Assert.AreEqual(1, bankAccNamesView.DataNames.Count);
        }


        [Test]
        public void AddingSecurityUpdatesSuccessfully()
        {
            SecurityEditWindowViewModel securityViewModel = ViewModel.SecurityWindow();
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
