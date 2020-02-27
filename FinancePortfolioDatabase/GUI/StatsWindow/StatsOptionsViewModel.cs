using FinancialStructures.Database;
using FinancialStructures.DataStructures;
using FinancialStructures.FinanceStructures;
using FinancialStructures.GUIFinanceStructures;
using FinancialStructures.ReportingStructures;
using FinancialStructures.StatsMakers;
using GUISupport;
using PortfolioStatsCreatorHelper;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Windows.Input;

namespace FinanceWindowsViewModels
{
    internal class StatsOptionsViewModel : PropertyChangedBase
    {
        private Portfolio Portfolio;
        private List<Sector> Sectors;
        private bool fDisplayValueFunds = true;
        public bool displayValueFunds
        {
            get { return fDisplayValueFunds; }
            set { fDisplayValueFunds = value; OnPropertyChanged(); }
        }

        private bool fSpacing = true;
        public bool spacing
        {
            get { return fSpacing; }
            set { fSpacing = value; OnPropertyChanged(); }
        }

        private bool fSecurities = true;
        public bool securities
        {
            get { return fSecurities; }
            set { fSecurities = value; OnPropertyChanged(); }
        }

        private bool fSectors = true;
        public bool sectors
        {
            get { return fSectors; }
            set { fSectors = value; OnPropertyChanged(); }
        }

        private bool fBankAccs = true;
        public bool bankAccs
        {
            get { return fBankAccs; }
            set { fBankAccs = value; OnPropertyChanged(); }
        }

        private bool fColours = true;
        public bool colours
        {
            get { return fColours; }
            set { fColours = value; OnPropertyChanged(); }
        }

        private List<VisibleName> fColumnNames;

        public List<VisibleName> ColumnNames
        {
            get { return fColumnNames; }
            set { fColumnNames = value; OnPropertyChanged(); }
        }

        private List<VisibleName> fBankColumnNames;

        public List<VisibleName> BankColumnNames
        {
            get { return fBankColumnNames; }
            set { fBankColumnNames = value; OnPropertyChanged(); }
        }

        public ICommand ExportCommand { get; }

        private void ExecuteExportCommand(Object obj)
        {
            var reports = new ErrorReports();
            SaveFileDialog saving = new SaveFileDialog() { DefaultExt = fFileExtension, FileName = Portfolio.DatabaseName + "-" + fExtension + "HTMLStats" + fFileExtension, InitialDirectory = Portfolio.Directory };
            saving.Filter = fExtension + " file|*" + fFileExtension + "|All files|*.*";
            string path = null;
            if (saving.ShowDialog() == DialogResult.OK)
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
                var options = new UserOptions() { DisplayValueFunds = displayValueFunds, Spacing = spacing, Colours = colours, SecurityDataToExport = selected, BankAccDataToExport = BankSelected, ShowSecurites = securities, ShowBankAccounts = bankAccs, ShowSectors = sectors };
                if (windowType == ExportType.HTML)
                {
                    PortfolioStatsCreators.CreateHTMLPageCustom(Portfolio, Sectors, saving.FileName, options);
                }
                else 
                {
                    CSVStatsCreator.CreateCSVPageCustom(Portfolio, Sectors, saving.FileName, options);
                }
                reports.AddReport("Created statistics page", Location.StatisticsPage);
            }
            else
            {
                reports.AddError("Was not able to create " + fExtension + " page in place specified.", Location.StatisticsPage);
            }
            saving.Dispose();
            if (reports.Any())
            {
                UpdateReports(reports);
            }

            CloseWindowAction(path);
        }
       
        Action<ErrorReports> UpdateReports;
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
        public StatsOptionsViewModel(Portfolio portfolio, List<Sector> sectors, ExportType exportType, Action<ErrorReports> updateReports, Action<string> CloseWindow)
        {
            windowType = exportType;
            Portfolio = portfolio;
            Sectors = sectors;
            UpdateReports = updateReports;
            CloseWindowAction = CloseWindow;
            
            ExportCommand = new BasicCommand(ExecuteExportCommand);

            var totals = new SecurityStatsHolder();
            var properties = totals.GetType().GetProperties();
            ColumnNames = new List<VisibleName>();
            foreach (var info in properties)
            {
                ColumnNames.Add(new VisibleName(info.Name, true));
            }

            var BankNames = new DayValue_Named();
            var props = BankNames.GetType().GetProperties();
            BankColumnNames = new List<VisibleName>();
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
        }
    }

    internal class VisibleName
    {
        public string Name { get; set; }
        public bool Visible { get; set; }
        public VisibleName()
        { }

        public VisibleName(string name, bool vis)
        {
            Visible = vis;
            Name = name;
        }
    }
}
