﻿using FinanceCommonViewModels;
using FinancialStructures.Database;
using FinancialStructures.FinanceInterfaces;
using FinancialStructures.PortfolioAPI;
using FinancialStructures.Reporting;
using GUISupport;
using System;
using System.Collections.Generic;

namespace FinanceWindowsViewModels
{
    internal class MainWindowViewModel : PropertyChangedBase
    {
        public EditMethods bankAccEditMethods = EditMethods.GenerateEditMethods(AccountType.BankAccount);

        public EditMethods sectorEditMethods = EditMethods.GenerateEditMethods(AccountType.Sector);

        public EditMethods currencyEditMethods = EditMethods.GenerateEditMethods(AccountType.Currency);

        internal IPortfolio ProgramPortfolio = new Portfolio();

        private OptionsToolbarViewModel fOptionsToolbarCommands;

        public OptionsToolbarViewModel OptionsToolbarCommands
        {
            get { return fOptionsToolbarCommands; }
            set { fOptionsToolbarCommands = value; OnPropertyChanged(); }
        }

        private ReportingWindowViewModel fReports;

        public ReportingWindowViewModel ReportsViewModel
        {
            get { return fReports; }
            set { fReports = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// The collection of tabs to hold the data and interactions for the various subwindows.
        /// </summary>
        public List<object> Tabs { get; } = new List<object>(6);

        public MainWindowViewModel()
        {
            ReportsViewModel = new ReportingWindowViewModel();
            ReportLogger = new LogReporter(ReportsViewModel.UpdateReport);

            OptionsToolbarCommands = new OptionsToolbarViewModel(ProgramPortfolio, UpdateDataCallback, ReportLogger);
            Tabs.Add(new BasicDataViewModel(ProgramPortfolio));
            Tabs.Add(new SecurityEditWindowViewModel(ProgramPortfolio, UpdateDataCallback, ReportLogger));
            Tabs.Add(new SingleValueEditWindowViewModel("Bank Account Edit", ProgramPortfolio, UpdateDataCallback, ReportLogger, bankAccEditMethods, AccountType.BankAccount));
            Tabs.Add(new SingleValueEditWindowViewModel("Sector Edit", ProgramPortfolio, UpdateDataCallback, ReportLogger, sectorEditMethods, AccountType.Sector));
            Tabs.Add(new SingleValueEditWindowViewModel("Currency Edit", ProgramPortfolio, UpdateDataCallback, ReportLogger, currencyEditMethods, AccountType.Currency));
            Tabs.Add(new StatsCreatorWindowViewModel(ProgramPortfolio, ReportLogger));


        }

        private void AllData_portfolioChanged(object sender, EventArgs e)
        {
            foreach (var tab in Tabs)
            {
                if (tab is ViewModelBase vm)
                {
                    vm.UpdateData(ProgramPortfolio);
                }
            }

            OptionsToolbarCommands.UpdateData(ProgramPortfolio);
        }

        /// <summary>
        /// 
        /// </summary>
        internal readonly IReportLogger ReportLogger;

        /// <summary>
        /// The mechanism by which the data in <see cref="Portfolio"/> is updated. This includes a GUI update action.
        /// </summary>
        private Action<Action<IPortfolio>> UpdateDataCallback => action => UpdateData(action);

        private void UpdateData(object obj)
        {
            if (obj is Action<IPortfolio> updateAction)
            {
                updateAction(ProgramPortfolio);
                AllData_portfolioChanged(obj, null);
            }
        }
    }
}
