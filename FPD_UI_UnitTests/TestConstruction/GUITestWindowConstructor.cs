using FinanceWindowsViewModels;
using FinancialStructures.Database;
using FinancialStructures.FinanceInterfaces;
using FinancialStructures.NamingStructures;
using FinancialStructures.PortfolioAPI;
using FinancialStructures.Reporting;
using System;
using System.Collections.Generic;

namespace FPD_UI_UnitTests.TestConstruction
{
    internal static class TestingGUICode
    {
        public static void GenerateDummyActions()
        { }

        internal static LogReporter DummyReportLogger = new LogReporter(dummyFunction);

        private static void dummyFunction(ReportSeverity a, ReportType b, ReportLocation c, string d)
        {
            return;
        }

        internal static Action<Action<Portfolio>> DummyDataUpdater => action => UpdateData(action);

        private static void UpdateData(object obj)
        {
            return;
        }

        internal static Action<NameData> DummyOpenTab => action => OpenTab(action);

        private static void OpenTab(object obj)
        {
            return;
        }

        internal static EditMethods DummyEditMethods;

        public static Portfolio CreateBasicDataBase()
        {
            var portfolio = new Portfolio();
            UpdatePortfolio(portfolio);
            return portfolio;
        }

        public static void UpdatePortfolio(Portfolio portfolio)
        {
            portfolio.TryAdd(AccountType.Security, new NameData("Fidelity", "China", "GBP", "http://www.fidelity.co.uk", new HashSet<string>() { "Bonds", "UK" }), TestingGUICode.DummyReportLogger);
            portfolio.TryAdd(AccountType.BankAccount, new NameData("Barclays", "currentAccount"), TestingGUICode.DummyReportLogger);
            portfolio.TryAdd(AccountType.Currency, new NameData(string.Empty, "GBP"), TestingGUICode.DummyReportLogger);

            portfolio.TryAdd(AccountType.Sector, new NameData(string.Empty, "UK", string.Empty, "http://www.hi.com"), TestingGUICode.DummyReportLogger);
        }

        public static Portfolio CreateEmptyDataBase()
        {
            return new Portfolio();
        }

        internal static MainWindowViewModel SetupWindow(Portfolio portfolio)
        {
            var viewModel = new MainWindowViewModel();
            viewModel.ProgramPortfolio = portfolio;
            return viewModel;
        }
    }
}
