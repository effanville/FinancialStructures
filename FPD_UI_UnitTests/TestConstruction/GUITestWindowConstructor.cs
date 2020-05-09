using FinanceWindowsViewModels;
using FinancialStructures.Database;
using FinancialStructures.FinanceInterfaces;
using FinancialStructures.NamingStructures;
using FinancialStructures.PortfolioAPI;
using FinancialStructures.Reporting;
using UICommon.Services;
using Moq;
using System;
using System.Windows;
using System.Collections.Generic;
using FinancialStructures.DataStructures;

namespace FPD_UI_UnitTests.TestConstruction
{
    internal static class TestingGUICode
    {
        public static string ExampleDatabaseFolder = "ExampleDatabases";
        internal static IReportLogger DummyReportLogger = new NothingReportLogger();

        public static Mock<IFileInteractionService> CreateFileMock(string expectedFilePath)
        {
            var mockfileinteraction = new Mock<IFileInteractionService>();
            mockfileinteraction.Setup(x => x.OpenFile(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Returns(new FileInteractionResult(true, expectedFilePath));
            mockfileinteraction.Setup(x => x.SaveFile(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Returns(new FileInteractionResult(true, expectedFilePath));
            return mockfileinteraction;
        }

        public static Mock<IDialogCreationService> CreateDialogMock(MessageBoxResult result = MessageBoxResult.OK)
        {
            var mockfileinteraction = new Mock<IDialogCreationService>();
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

        internal static Action<object> DummyOpenTab => action => OpenTab(action);

        private static void OpenTab(object obj)
        {
            return;
        }

        public static EditMethods GetMethodsForTesting(AccountType accountType)
        {
            return EditMethods.GenerateEditMethods(accountType);
        }

        internal static EditMethods DummyEditMethods = EditMethods.GenerateEditMethods(AccountType.BankAccount);

        public static Portfolio CreateBasicDataBase()
        {
            var portfolio = new Portfolio();
            UpdatePortfolio(portfolio);
            return portfolio;
        }

        public static void UpdatePortfolio(Portfolio portfolio)
        {
            portfolio.SetFilePath("TestFilePath");
            portfolio.TryAdd(AccountType.Security, new NameData("Fidelity", "China", "GBP", "http://www.fidelity.co.uk", new HashSet<string>() { "Bonds", "UK" }), TestingGUICode.DummyReportLogger);
            portfolio.TryAddDataToSecurity(new TwoName("Fidelity", "China"), new DateTime(2000, 1, 1), 1, 1, 1);
            portfolio.TryAdd(AccountType.BankAccount, new NameData("Barclays", "currentAccount"), TestingGUICode.DummyReportLogger);
            portfolio.TryAddData(AccountType.BankAccount, new NameData("Barclays", "currentAccount"), new DayValue_ChangeLogged(new DateTime(2000, 1, 1), 1));
            portfolio.TryAdd(AccountType.Currency, new NameData(string.Empty, "GBP"), TestingGUICode.DummyReportLogger);

            portfolio.TryAdd(AccountType.Sector, new NameData(string.Empty, "UK", string.Empty, "http://www.hi.com"), TestingGUICode.DummyReportLogger);
        }

        public static Portfolio CreateEmptyDataBase()
        {
            return new Portfolio();
        }

        internal static MainWindowViewModel SetupWindow(Portfolio portfolio)
        {
            var fileMock = CreateFileMock("filepath");
            var dialogMock = CreateDialogMock(MessageBoxResult.OK);
            var viewModel = new MainWindowViewModel(fileMock.Object, dialogMock.Object);
            viewModel.ProgramPortfolio = portfolio;
            return viewModel;
        }
    }
}
