using FinanceCommonViewModels;
using FinanceWindows;
using FinancialStructures.DatabaseInterfaces;
using FinancialStructures.PortfolioAPI;
using FinancialStructures.ReportLogging;
using GUISupport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Input;

namespace FinanceWindowsViewModels
{
    internal class OptionsToolbarViewModel : ViewModelBase
    {
        private string fFileName;
        private string fDirectory;
        private readonly Action<Action<IPortfolio>> DataUpdateCallback;
        private readonly LogReporter ReportLogger;
        private string fBaseCurrency;

        public string BaseCurrency
        {
            get { return fBaseCurrency; }
            set
            {
                if (fBaseCurrency != value)
                {
                    fBaseCurrency = value;
                    OnPropertyChanged(nameof(BaseCurrency));
                    DataUpdateCallback(portfolio => portfolio.BaseCurrency = BaseCurrency);
                }
            }
        }

        private List<string> fCurrencies;

        public List<string> Currencies
        {
            get { return fCurrencies; }
            set { fCurrencies = value; OnPropertyChanged(); }
        }

        public OptionsToolbarViewModel(IPortfolio portfolio, Action<Action<IPortfolio>> updateData, LogReporter reportLogger)
            : base("Options")
        {
            ReportLogger = reportLogger;
            DataUpdateCallback = updateData;
            UpdateData(portfolio);

            OpenHelpCommand = new BasicCommand(OpenHelpDocsCommand);
            NewDatabaseCommand = new BasicCommand(ExecuteNewDatabase);
            SaveDatabaseCommand = new BasicCommand(ExecuteSaveDatabase);
            LoadDatabaseCommand = new BasicCommand(ExecuteLoadDatabase);
            UpdateDataCommand = new BasicCommand(ExecuteUpdateData);
            RefreshCommand = new BasicCommand(ExecuteRefresh);
        }


        public override void UpdateData(IPortfolio portfolio)
        {
            fFileName = portfolio.DatabaseName + portfolio.Extension;
            fDirectory = portfolio.Directory;
            Currencies = portfolio.Names(AccountType.Currency).Concat(portfolio.Companies(AccountType.Currency)).Distinct().ToList();
            BaseCurrency = portfolio.BaseCurrency;
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
                DataUpdateCallback(programPortfolio => programPortfolio.SetFilePath(""));
                DataUpdateCallback(programPortfolio => programPortfolio.LoadPortfolio("", ReportLogger));
            }
        }

        public ICommand SaveDatabaseCommand { get; }
        private void ExecuteSaveDatabase(Object obj)
        {
            SaveFileDialog saving = new SaveFileDialog() { DefaultExt = "xml", FileName = fFileName, InitialDirectory = fDirectory };
            saving.Filter = "XML Files|*.xml|All Files|*.*";
            if (saving.ShowDialog() == DialogResult.OK)
            {
                DataUpdateCallback(programPortfolio => programPortfolio.SetFilePath(saving.FileName));
                DataUpdateCallback(programPortfolio => programPortfolio.SavePortfolio(saving.FileName, ReportLogger));
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
                DataUpdateCallback(programPortfolio => programPortfolio.SetFilePath(openFile.FileName));
                DataUpdateCallback(programPortfolio => programPortfolio.LoadPortfolio(openFile.FileName, ReportLogger));

            }
            openFile.Dispose();
        }

        public ICommand UpdateDataCommand { get; }
        private void ExecuteUpdateData(Object obj)
        {
            DataUpdateCallback(async programPortfolio => await PortfolioDataUpdater.Downloader(programPortfolio, ReportLogger).ConfigureAwait(false));
        }

        public ICommand RefreshCommand { get; }

        private void ExecuteRefresh(Object obj)
        {
            DataUpdateCallback(programPortfolio => programPortfolio.SetFilePath(fFileName));
        }
    }
}
