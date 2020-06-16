using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using FinanceWindowsViewModels;
using FinancialStructures.Database;
using FinancialStructures.FinanceInterfaces;
using FinancialStructures.NamingStructures;
using FinancialStructures.PortfolioAPI;
using Moq;
using StructureCommon.DataStructures;
using StructureCommon.Reporting;
using UICommon.Services;

namespace FPD_UI_UnitTests.TestConstruction
{
    internal static class TestingGUICode
    {
        public static string ExampleDatabaseFolder = "ExampleDatabases";
        internal static IReportLogger DummyReportLogger = new NothingReportLogger();

        public static Mock<IFileInteractionService> CreateFileMock(string expectedFilePath)
        {
            Mock<IFileInteractionService> mockfileinteraction = new Mock<IFileInteractionService>();
            mockfileinteraction.Setup(x => x.OpenFile(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Returns(new FileInteractionResult(true, expectedFilePath));
            mockfileinteraction.Setup(x => x.SaveFile(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Returns(new FileInteractionResult(true, expectedFilePath));
            return mockfileinteraction;
        }

        public static Mock<IDialogCreationService> CreateDialogMock(MessageBoxResult result = MessageBoxResult.OK)
        {
            Mock<IDialogCreationService> mockfileinteraction = new Mock<IDialogCreationService>();
            mockfileinteraction.Setup(x => x.ShowMessageBox(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<MessageBoxButton>(), It.IsAny<MessageBoxImage>())).Returns(result);
            return mockfileinteraction;
        }

        public static Action<Action<IPortfolio>> CreateDataUpdater(IPortfolio portfolio)
        {
            Action<Action<IPortfolio>> DummyDataUpdater = action => UpdateData(action);
            void UpdateData(object obj)
            {
                if (obj is Action<IPortfolio> updateAction)
                {
                    updateAction(portfolio);
                }
            }

            return DummyDataUpdater;
        }

        internal static Action<object> DummyOpenTab
        {
            get
            {
                return action => OpenTab(action);
            }
        }

        private static void OpenTab(object obj)
        {
            return;
        }

        [STAThread]
        public static DataGridRowEditEndingEventArgs CreateRowArgs<T>(T obj) where T : class
        {
            var dataGridrow = new DataGridRow() { DataContext = obj };
            return new DataGridRowEditEndingEventArgs(dataGridrow, DataGridEditAction.Commit);
        }

        public static Portfolio CreateBasicDataBase()
        {
            Portfolio portfolio = new Portfolio();
            UpdatePortfolio(portfolio);
            return portfolio;
        }

        public static void UpdatePortfolio(Portfolio portfolio)
        {
            portfolio.SetFilePath("TestFilePath");
            portfolio.TryAdd(AccountType.Security, new NameData("Fidelity", "China", "GBP", "http://www.fidelity.co.uk", new HashSet<string>() { "Bonds", "UK" }), TestingGUICode.DummyReportLogger);
            portfolio.TryAddDataToSecurity(new TwoName("Fidelity", "China"), new DateTime(2000, 1, 1), 1, 1, 1);
            portfolio.TryAdd(AccountType.BankAccount, new NameData("Barclays", "currentAccount"), TestingGUICode.DummyReportLogger);
            portfolio.TryAddData(AccountType.BankAccount, new NameData("Barclays", "currentAccount"), new DailyValuation(new DateTime(2000, 1, 1), 1));
            portfolio.TryAdd(AccountType.Currency, new NameData(string.Empty, "GBP"), TestingGUICode.DummyReportLogger);

            portfolio.TryAdd(AccountType.Sector, new NameData(string.Empty, "UK", string.Empty, "http://www.hi.com"), TestingGUICode.DummyReportLogger);
        }

        public static Portfolio CreateEmptyDataBase()
        {
            return new Portfolio();
        }

        internal static MainWindowViewModel SetupWindow(Portfolio portfolio)
        {
            Mock<IFileInteractionService> fileMock = CreateFileMock("filepath");
            Mock<IDialogCreationService> dialogMock = CreateDialogMock(MessageBoxResult.OK);
            MainWindowViewModel viewModel = new MainWindowViewModel(fileMock.Object, dialogMock.Object)
            {
                ProgramPortfolio = portfolio
            };
            return viewModel;
        }
    }
}
