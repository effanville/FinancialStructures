using FinanceCommonViewModels;
using FinanceWindows;
using FinancialStructures.Database;
using FinancialStructures.FinanceStructures;
using FinancialStructures.PortfolioAPI;
using GUISupport;
using SavingClasses;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Windows.Input;

namespace FinanceWindowsViewModels
{
    internal class OptionsToolbarViewModel : ViewModelBase
    {
        private string fFileName;
        private string fDirectory;
        private readonly Action<Action<AllData>> DataUpdateCallback;
        private readonly Action<string, string, string> ReportLogger;

        public OptionsToolbarViewModel(Portfolio portfolio, Action<Action<AllData>> updateData, Action<string, string, string> reportLogger)
            : base("Options")
        {
            ReportLogger = reportLogger;
            DataUpdateCallback = updateData;
            UpdateData(portfolio, null);

            OpenHelpCommand = new BasicCommand(OpenHelpDocsCommand);
            NewDatabaseCommand = new BasicCommand(ExecuteNewDatabase);
            SaveDatabaseCommand = new BasicCommand(ExecuteSaveDatabase);
            LoadDatabaseCommand = new BasicCommand(ExecuteLoadDatabase);
            UpdateDataCommand = new BasicCommand(ExecuteUpdateData);
            RefreshCommand = new BasicCommand(ExecuteRefresh);
        }

        public override void UpdateData(Portfolio portfolio, List<Sector> sectors)
        {
            fFileName = portfolio.DatabaseName + portfolio.Extension;
            fDirectory = portfolio.Directory;
        }

        public ICommand OpenHelpCommand { get; }
        private void OpenHelpDocsCommand(Object obj)
        {
            var helpwindow = new HelpWindow(ReportLogger);
            helpwindow.Show();
        }

        public ICommand NewDatabaseCommand { get; }
        private void ExecuteNewDatabase(Object obj)
        {
            DialogResult result = MessageBox.Show("Do you want to load a new database?", "New Database?", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (result == DialogResult.Yes)
            {
                DataUpdateCallback(alldata => alldata.MyFunds.SetFilePath(""));
                DataUpdateCallback(alldata => alldata.myBenchMarks.AddRange(alldata.MyFunds.LoadPortfolio("", ReportLogger)));
            }
        }

        public ICommand SaveDatabaseCommand { get; }
        private void ExecuteSaveDatabase(Object obj)
        {
            SaveFileDialog saving = new SaveFileDialog() { DefaultExt = "xml", FileName = fFileName, InitialDirectory = fDirectory };
            saving.Filter = "XML Files|*.xml|All Files|*.*";
            if (saving.ShowDialog() == DialogResult.OK)
            {
                DataUpdateCallback(alldata => alldata.MyFunds.SetFilePath(saving.FileName));
                DataUpdateCallback(alldata => alldata.MyFunds.SavePortfolio(alldata.myBenchMarks, saving.FileName, ReportLogger));
            }

            saving.Dispose();
        }

        public ICommand LoadDatabaseCommand { get; }
        private void ExecuteLoadDatabase(Object obj)
        {
            OpenFileDialog openFile = new OpenFileDialog() { DefaultExt = "xml" };
            openFile.Filter = "XML Files|*.xml|All Files|*.*";
            if (openFile.ShowDialog() == DialogResult.OK)
            {
                DataUpdateCallback(alldata => alldata.MyFunds.SetFilePath(openFile.FileName));
                DataUpdateCallback(alldata => alldata.myBenchMarks.AddRange(alldata.MyFunds.LoadPortfolio(openFile.FileName, ReportLogger)));
                ReportLogger("Report", "Loading", $"Loaded new database from {openFile.FileName}");
            }
            openFile.Dispose();
        }

        public ICommand UpdateDataCommand { get; }
        private void ExecuteUpdateData(Object obj)
        {
            DataUpdateCallback(async alldata => await PortfolioDataUpdater.Downloader(alldata.MyFunds, alldata.myBenchMarks, ReportLogger).ConfigureAwait(false));
        }

        public ICommand RefreshCommand { get; }
        private void ExecuteRefresh(Object obj)
        {
            DataUpdateCallback(alldata => alldata.MyFunds.SetFilePath(fFileName));
        }
    }
}
