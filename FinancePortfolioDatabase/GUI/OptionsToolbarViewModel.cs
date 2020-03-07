using FinanceWindows;
using FinancialStructures.Database;
using FinancialStructures.FinanceStructures;
using GUISupport;
using SavingClasses;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Windows.Input;

namespace FinanceWindowsViewModels
{
    internal class OptionsToolbarViewModel : PropertyChangedBase
    {
        private Portfolio Portfolio;
        Action<Action<AllData>> UpdateData;
        Action<string, string, string> ReportLogger;

        public ICommand OpenHelpCommand { get; }
        public ICommand NewDatabaseCommand { get; }
        public ICommand SaveDatabaseCommand { get; }
        public ICommand LoadDatabaseCommand { get; }
        public ICommand UpdateDataCommand { get; }
        public ICommand RefreshCommand { get; }

        public OptionsToolbarViewModel(Portfolio portfolio, List<Sector> sectors, Action<Action<AllData>> updateData, Action<string, string, string> reportLogger)
        {
            Portfolio = portfolio;
            ReportLogger = reportLogger;
            UpdateData = updateData;
            OpenHelpCommand = new BasicCommand(OpenHelpDocsCommand);
            NewDatabaseCommand = new BasicCommand(ExecuteNewDatabase);
            SaveDatabaseCommand = new BasicCommand(ExecuteSaveDatabase);
            LoadDatabaseCommand = new BasicCommand(ExecuteLoadDatabase);
            UpdateDataCommand = new BasicCommand(ExecuteUpdateData);
        }

        private void OpenHelpDocsCommand(Object obj)
        {
            var helpwindow = new HelpWindow(ReportLogger);
            helpwindow.Show();
        }
        public void ExecuteNewDatabase(Object obj)
        {
            DialogResult result = MessageBox.Show("Do you want to load a new database?", "New Database?", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (result == DialogResult.Yes)
            {
                UpdateData(alldata => alldata.MyFunds.SetFilePath(""));
                UpdateData(alldata => alldata.myBenchMarks.AddRange(alldata.MyFunds.LoadPortfolio("", ReportLogger)));
            }
        }
        public void ExecuteSaveDatabase(Object obj)
        {
            SaveFileDialog saving = new SaveFileDialog() { DefaultExt = "xml", FileName = Portfolio.DatabaseName + Portfolio.Extension, InitialDirectory = Portfolio.Directory };
            saving.Filter = "XML Files|*.xml|All Files|*.*";
            if (saving.ShowDialog() == DialogResult.OK)
            {
                UpdateData(alldata => alldata.MyFunds.SetFilePath(saving.FileName));
                UpdateData(alldata => alldata.MyFunds.SavePortfolio(alldata.myBenchMarks, saving.FileName, ReportLogger));
            }

            saving.Dispose();
        }

        public void ExecuteLoadDatabase(Object obj)
        {
            OpenFileDialog openFile = new OpenFileDialog() { DefaultExt = "xml" };
            openFile.Filter = "XML Files|*.xml|All Files|*.*";
            if (openFile.ShowDialog() == DialogResult.OK)
            {
                UpdateData(alldata => alldata.MyFunds.SetFilePath(openFile.FileName));
                UpdateData(alldata => alldata.myBenchMarks.AddRange(alldata.MyFunds.LoadPortfolio(openFile.FileName, ReportLogger)));
                ReportLogger("Report", "Loading", $"Loaded new database from {openFile.FileName}");
            }
            openFile.Dispose();
        }

        private void ExecuteUpdateData(Object obj)
        {
            UpdateData(async alldata => await DataUpdater.Downloader(alldata.MyFunds, alldata.myBenchMarks, ReportLogger).ConfigureAwait(false));
        }
    }
}
