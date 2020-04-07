using FinancialStructures.DataStructures;
using FinancialStructures.FinanceInterfaces;
using FinancialStructures.ReportLogging;
using FinancialStructures.StatisticStructures;
using FinancialStructures.StatsMakers;
using GUISupport;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Windows.Input;

namespace FinanceWindowsViewModels
{
    internal class StatsOptionsViewModel : PropertyChangedBase
    {
        private IPortfolio Portfolio;
        private bool fDisplayValueFunds = true;
        public bool displayValueFunds
        {
            get { return fDisplayValueFunds; }
            set { fDisplayValueFunds = value; OnPropertyChanged(); }
        }
        private UserOptions fSelectOptions;

        public UserOptions SelectOptions
        {
            get { return fSelectOptions; }
            set { fSelectOptions = value; OnPropertyChanged(); }
        }

        private List<VisibleName> fDisplayConditions = new List<VisibleName>();

        public List<VisibleName> DisplayConditions
        {
            get { return fDisplayConditions; }
            set { fDisplayConditions = value; OnPropertyChanged(); }
        }

        private List<VisibleName> fColumnNames = new List<VisibleName>();

        public List<VisibleName> ColumnNames
        {
            get { return fColumnNames; }
            set { fColumnNames = value; OnPropertyChanged(); }
        }

        private List<VisibleName> fBankColumnNames = new List<VisibleName>();

        public List<VisibleName> BankColumnNames
        {
            get { return fBankColumnNames; }
            set { fBankColumnNames = value; OnPropertyChanged(); }
        }

        public ICommand ExportCommand { get; }

        private void ExecuteExportCommand(Object obj)
        {
            SaveFileDialog saving = new SaveFileDialog() { DefaultExt = fFileExtension, FileName = Portfolio.DatabaseName + "-" + fExtension + "HTMLStats" + fFileExtension, InitialDirectory = Portfolio.Directory };
            saving.Filter = fExtension + " file|*" + fFileExtension + "|All files|*.*";
            string path = null;

            bool? saved = saving.ShowDialog();
            if (saved != null && (bool)saved)
            {
                path = saving.FileName;
                var selected = new List<string>();
                foreach (var column in ColumnNames)
                {
                    if (column.Visible || column.Name == "Name" || column.Name == "Company")
                    {
                        selected.Add(column.Name);
                    }
                }
                var BankSelected = new List<string>();
                foreach (var column in BankColumnNames)
                {
                    if (column.Visible || column.Name == "Name" || column.Name == "Company")
                    {
                        BankSelected.Add(column.Name);
                    }
                }
                var options = new UserOptions(selected, BankSelected, selected, DisplayConditions);
                if (windowType == ExportType.HTML)
                {
                    PortfolioStatsCreators.CreateHTMLPageCustom(Portfolio, saving.FileName, options);
                }
                else
                {
                    CSVStatsCreator.CreateCSVPageCustom(Portfolio, saving.FileName, options);
                }
                ReportLogger.Log("Report", "StatisticsPage", "Created statistics page");
            }
            else
            {
                ReportLogger.LogDetailed("Critical", "Error", "StatisticsPage", "Was not able to create " + fExtension + " page in place specified.");
            }

            CloseWindowAction(path);
        }

        private readonly LogReporter ReportLogger;
        private Action<string> CloseWindowAction;
        private ExportType windowType;
        private string fExtension
        {
            get { return windowType == ExportType.HTML ? "html" : "csv"; }
        }
        private string fFileExtension
        {
            get { return "." + fExtension; }
        }
        public StatsOptionsViewModel(IPortfolio portfolio, ExportType exportType, LogReporter reportLogger, Action<string> CloseWindow)
        {
            windowType = exportType;
            Portfolio = portfolio;
            ReportLogger = reportLogger;
            CloseWindowAction = CloseWindow;
            ExportCommand = new BasicCommand(ExecuteExportCommand);

            var totals = new SecurityStatistics();
            var properties = totals.GetType().GetProperties();
            foreach (var info in properties)
            {
                ColumnNames.Add(new VisibleName(info.Name, true));
            }

            var BankNames = new DayValue_Named();
            var props = BankNames.GetType().GetProperties();
            foreach (var info in props)
            {
                if (info.Name == "Day")
                {
                    BankColumnNames.Add(new VisibleName(info.Name, false));
                }
                else
                {
                    BankColumnNames.Add(new VisibleName(info.Name, true));
                }
            }

            var fish = new UserOptions();
            var optionsInfo = fish.GetType().GetProperties();
            foreach (var info in optionsInfo)
            {
                if (info.PropertyType == typeof(bool))
                {
                    DisplayConditions.Add(new VisibleName(info.Name, true));
                }
            }
        }
    }
}
