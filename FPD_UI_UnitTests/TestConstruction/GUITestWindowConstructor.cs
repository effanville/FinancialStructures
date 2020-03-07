using System;
using System.Collections.Generic;
using FinanceWindowsViewModels;
using FinancialStructures.Database;
using FinancialStructures.FinanceStructures;
using FinancialStructures.GUIFinanceStructures;
using SavingClasses;

namespace FPD_UI_UnitTests.TestConstruction
{
    internal static class TestingGUICode
    {
        public static void GenerateDummyActions()
        { }

        internal static Action<string, string, string> DummyReportLogger => (a, b, c) => dummyFunction(a, b, c);

        private static void dummyFunction(string a, string b, string c)
        {
            return;
        }

        internal static Action<Action<AllData>> DummyDataUpdater => action => UpdateData(action);

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

        public static Tuple<Portfolio, List<Sector>> CreateBasicDataBase()
        {
            var portfolio = new Portfolio();
            portfolio.TryAddSecurity(TestingGUICode.DummyReportLogger, "Fidelity", "China", "GBP", "http://www.fidelity.co.uk", "Bonds, UK");
            portfolio.TryAddBankAccount("currentAccount", "Barclays", string.Empty, string.Empty, TestingGUICode.DummyReportLogger);
            portfolio.TryAddCurrency("GBP", string.Empty, TestingGUICode.DummyReportLogger);

            var sectors = new List<Sector>();
            sectors.Add(new Sector("UK", "http://www.hi.com"));

            return new Tuple<Portfolio, List<Sector>>(portfolio, sectors);
        }

        public static Tuple<Portfolio, List<Sector>> CreateEmptyDataBase()
        {
            var portfolio = new Portfolio();
            var sectors = new List<Sector>();
            return new Tuple<Portfolio, List<Sector>>(portfolio, sectors);
        }

        internal static MainWindowViewModel SetupWindow(Portfolio portfolio, List<Sector> sectors)
        {
            var viewModel = new MainWindowViewModel();
            var allData = new AllData(portfolio, sectors);
            viewModel.allData = allData;
            return viewModel;
        }
    }
}
