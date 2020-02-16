using FinanceWindows;
using FinancialStructures.Database;
using FinancialStructures.FinanceStructures;
using FinancialStructures.ReportingStructures;
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
        Action<ErrorReports> UpdateReports;

        public ICommand OpenHelpCommand { get; }
        public ICommand NewDatabaseCommand { get; }
        public ICommand SaveDatabaseCommand { get; }
        public ICommand LoadDatabaseCommand { get; }
        public ICommand UpdateDataCommand { get; }
        public ICommand RefreshCommand { get; }

        public OptionsToolbarViewModel(Portfolio portfolio, List<Sector> sectors, Action<Action<AllData>> updateData, Action<ErrorReports> updateReports)
        {
            Portfolio = portfolio;
            UpdateReports = updateReports;
            UpdateData = updateData;
            OpenHelpCommand = new BasicCommand(OpenHelpDocsCommand);

            NewDatabaseCommand = new BasicCommand(ExecuteNewDatabase);
            SaveDatabaseCommand = new BasicCommand(ExecuteSaveDatabase);
            LoadDatabaseCommand = new BasicCommand(ExecuteLoadDatabase);
            UpdateDataCommand = new BasicCommand(ExecuteUpdateData);
        }

        private void OpenHelpDocsCommand(Object obj)
        {
            var helpwindow = new HelpWindow(UpdateReports);
            helpwindow.Show();
        }
        public void ExecuteNewDatabase(Object obj)
        {
            DialogResult result = MessageBox.Show("Do you want to load a new database?", "New Database?", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (result == DialogResult.Yes)
            {
                var reports = new ErrorReports();
                UpdateData(alldata => alldata.MyFunds.SetFilePath(""));
                UpdateData(alldata => alldata.myBenchMarks.AddRange(alldata.MyFunds.LoadPortfolio("", reports)));
                if (reports.Any())
                {
                    UpdateReports(reports);
                }
            }
        }
        public void ExecuteSaveDatabase(Object obj)
        {
            var reports = new ErrorReports();
            SaveFileDialog saving = new SaveFileDialog() { DefaultExt = "xml", FileName = Portfolio.DatabaseName + Portfolio.Extension, InitialDirectory = Portfolio.Directory };
            saving.Filter = "XML Files|*.xml|All Files|*.*";
            if (saving.ShowDialog() == DialogResult.OK)
            {
                UpdateData(alldata => alldata.MyFunds.SetFilePath(saving.FileName));
                UpdateData(alldata => alldata.MyFunds.SavePortfolio(alldata.myBenchMarks, saving.FileName, reports));
            }


            saving.Dispose();
            if (reports.Any())
            {
                UpdateReports(reports);
            }
        }

        public void ExecuteLoadDatabase(Object obj)
        {
            var reports = new ErrorReports();
            OpenFileDialog openFile = new OpenFileDialog() { DefaultExt = "xml" };
            openFile.Filter = "XML Files|*.xml|All Files|*.*";
            if (openFile.ShowDialog() == DialogResult.OK)
            {
                UpdateData(alldata => alldata.MyFunds.SetFilePath(openFile.FileName));
                UpdateData(alldata => alldata.myBenchMarks.AddRange(alldata.MyFunds.LoadPortfolio(openFile.FileName, reports)));
                reports.AddReport($"Loaded new database from {openFile.FileName}", Location.Loading);
            }
            openFile.Dispose();

            if (reports.Any())
            {
                UpdateReports(reports);
            }
        }

        private void ExecuteUpdateData(Object obj)
        {
            var reports = new ErrorReports();
            UpdateData(async alldata => await DataUpdater.Downloader(alldata.MyFunds, alldata.myBenchMarks, UpdateReports, reports).ConfigureAwait(false));

            if (reports.Any())
            {
                UpdateReports(reports);
            }
        }
    }
}
