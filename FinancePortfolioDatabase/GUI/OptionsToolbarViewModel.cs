using FinanceWindows;
using FinancialStructures.Database;
using FinancialStructures.FinanceStructures;
using FinancialStructures.ReportingStructures;
using GUISupport;
using PADGlobals;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using System.Windows.Input;

namespace FinanceWindowsViewModels
{
    public class OptionsToolbarViewModel : PropertyChangedBase
    {
        private Portfolio Portfolio;
        private List<Sector> Sectors;
        Action UpdateMainWindow;
        Action<ErrorReports> UpdateReports;

        public ICommand OpenHelpCommand { get; }
        public ICommand NewDatabaseCommand { get; }
        public ICommand SaveDatabaseCommand { get; }
        public ICommand LoadDatabaseCommand { get; }
        public ICommand UpdateDataCommand { get; }
        public ICommand RefreshCommand { get; }

        public OptionsToolbarViewModel(Portfolio portfolio, List<Sector> sectors, Action updateWindow, Action<ErrorReports> updateReports)
        {
            Portfolio = portfolio;
            Sectors = sectors;
            UpdateMainWindow = updateWindow;
            UpdateReports = updateReports;
            OpenHelpCommand = new BasicCommand(OpenHelpDocsCommand);

            NewDatabaseCommand = new BasicCommand(ExecuteNewDatabase);
            SaveDatabaseCommand = new BasicCommand(ExecuteSaveDatabase);
            LoadDatabaseCommand = new BasicCommand(ExecuteLoadDatabase);
            UpdateDataCommand = new BasicCommand(ExecuteUpdateData);
            RefreshCommand = new BasicCommand(RefreshData);
        }

        private void OpenHelpDocsCommand(Object obj)
        {
            var helpwindow = new HelpWindow();
            helpwindow.Show();
        }
        public void ExecuteNewDatabase(Object obj)
        {
            DialogResult result = MessageBox.Show("Do you want to load a new database?", "New Database?", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (result == DialogResult.Yes)
            {
                var reports = new ErrorReports();
                Portfolio.SetFilePath("");
                Sectors = Portfolio.LoadPortfolio(Portfolio.FilePath, reports);
                UpdateMainWindow();
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
                Portfolio.SetFilePath(saving.FileName);
                Portfolio.SavePortfolio(Sectors, saving.FileName, reports);
            }

            
            saving.Dispose();
            UpdateMainWindow();
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
                Portfolio.SetFilePath(openFile.FileName);
                Sectors = Portfolio.LoadPortfolio(openFile.FileName, reports);
                reports.AddReport($"Loaded new database from {openFile.FileName}");
            }
            openFile.Dispose();

            if (reports.Any())
            {
                UpdateReports(reports);
            }
            UpdateMainWindow();
        }

        private void RefreshData(object obj)
        {
            UpdateMainWindow();
        }

        private async void ExecuteUpdateData(Object obj)
        {
            var reports = new ErrorReports();
            await DataUpdater.Downloader(Portfolio, Sectors, UpdateReports, reports).ConfigureAwait(false);

            if (reports.Any())
            {
                UpdateReports(reports);
            }

            UpdateMainWindow();
        }
    }
}
