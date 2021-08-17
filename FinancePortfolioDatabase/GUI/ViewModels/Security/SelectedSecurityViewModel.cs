using System;
using System.Collections.Generic;
using System.Windows.Input;
using FinancialStructures.Database;
using FinancialStructures.Database.Download;
using FinancialStructures.DataStructures;
using FinancialStructures.FinanceStructures;
using FinancialStructures.NamingStructures;
using Common.Structure.DataStructures;
using Common.Structure.FileAccess;
using Common.Structure.Reporting;
using Common.UI.Commands;
using Common.UI.Services;
using Common.UI.ViewModelBases;
using Common.UI;
using FinancePortfolioDatabase.GUI.ViewModels.Common;
using System.Linq;
using System.Windows.Controls;
using FinancialStructures.Statistics;
using FinancialStructures.Database.Statistics;

namespace FinancePortfolioDatabase.GUI.ViewModels.Security
{
    public class SelectedSecurityViewModel : TabViewModelBase<IPortfolio>
    {
        private readonly Action<Action<IPortfolio>> UpdateDataCallback;
        private readonly IReportLogger fReportLogger;
        private readonly UiGlobals fUiGlobals;

        internal SecurityTrade fOldSelectedTrade;
        internal SecurityTrade SelectedTrade;

        /// <inheritdoc/>
        public override bool Closable => true;

        public NameData SelectedName
        {
            get;
        }

        private TimeListViewModel fTLVM;
        public TimeListViewModel TLVM
        {
            get => fTLVM;
            set => SetAndNotify(ref fTLVM, value, nameof(TLVM));
        }

        private List<SecurityTrade> fTrades = new List<SecurityTrade>();
        public List<SecurityTrade> Trades
        {
            get => fTrades;
            set => SetAndNotify(ref fTrades, value, nameof(Trades));
        }

        private AccountStatistics fSecurityStats;
        public AccountStatistics SecurityStats
        {
            get => fSecurityStats;
            set => SetAndNotify(ref fSecurityStats, value, nameof(SecurityStats));
        }

        private List<DailyValuation> fValues;
        public List<DailyValuation> Values
        {
            get => fValues;
            set => SetAndNotify(ref fValues, value, nameof(Values));
        }

        public SelectedSecurityViewModel(IPortfolio portfolio, Action<Action<IPortfolio>> updateData, IReportLogger reportLogger, UiGlobals globals, NameData selectedName)
            : base(selectedName != null ? selectedName.ToString() : "No-Name", portfolio)
        {
            fReportLogger = reportLogger;
            fUiGlobals = globals;
            SelectedName = selectedName;
            DeleteValuationCommand = new RelayCommand(ExecuteDeleteValuation);
            AddCsvData = new RelayCommand(ExecuteAddCsvData);
            ExportCsvData = new RelayCommand(ExecuteExportCsvData);
            DeleteTradeKeyDownCommand = new RelayCommand<KeyEventArgs>(ExecuteDeleteTradeKeyDownValuation);
            DownloadCommand = new RelayCommand(ExecuteDownloadCommand);
            AddEditDataCommand = new RelayCommand(ExecuteAddEditData);
            SelectionChangedCommand = new RelayCommand<object>(ExecuteSelectionChanged);
            UpdateDataCallback = updateData;


            if (portfolio.TryGetAccount(Account.Security, SelectedName, out IValueList desired))
            {
                ISecurity security = desired as ISecurity;
                TLVM = new TimeListViewModel(security.UnitPrice, "UnitPrice", globals, value => DeleteValue(SelectedName, value), (old, newVal) => ExecuteAddEditUnitPriceData(SelectedName, old, newVal));
            }
            else
            {
                TLVM = new TimeListViewModel(null, "UnitPrice", globals, value => DeleteValue(SelectedName, value), (old, newVal) => ExecuteAddEditUnitPriceData(SelectedName, old, newVal));
            }

            UpdateData(portfolio, null);
        }

        public ICommand DeleteValuationCommand
        {
            get;
        }

        private void ExecuteDeleteValuation()
        {
            DeleteValue(SelectedName, TLVM.SelectedValuation);
        }

        private void DeleteValue(NameData name, DailyValuation value)
        {
            if (name != null && value != null)
            {
                UpdateDataCallback(programPortfolio => programPortfolio.TryDeleteData(Account.Security, name.ToTwoName(), value.Day, fReportLogger));
            }
            else
            {
                _ = fReportLogger.Log(ReportSeverity.Critical, ReportType.Error, ReportLocation.DeletingData, "No Account was selected when trying to delete data.");
            }
        }

        /// <summary>
        /// Downloads the latest data for the selected entry.
        /// </summary>
        public ICommand DownloadCommand
        {
            get;
        }

        private void ExecuteDownloadCommand()
        {
            _ = fReportLogger.Log(ReportSeverity.Detailed, ReportType.Information, ReportLocation.DatabaseAccess, $"Download selected for account {SelectedName} - a {Account.Security}");
            if (SelectedName != null)
            {
                NameData names = SelectedName;
                UpdateDataCallback(async programPortfolio => await PortfolioDataUpdater.Download(Account.Security, programPortfolio, names, fReportLogger).ConfigureAwait(false));
            }
        }

        public ICommand AddCsvData
        {
            get;
        }

        private void ExecuteAddCsvData()
        {
            _ = fReportLogger.Log(ReportSeverity.Detailed, ReportType.Information, ReportLocation.DatabaseAccess, $"Selected Security {SelectedName} adding data from csv.");
            if (SelectedName != null)
            {
                FileInteractionResult result = fUiGlobals.FileInteractionService.OpenFile("csv", filter: "Csv Files|*.csv|All Files|*.*");
                List<object> outputs = null;
                bool exists = DataStore.TryGetAccount(Account.Security, SelectedName, out IValueList account);
                if (result.Success != null && (bool)result.Success && exists)
                {
                    ISecurity security = account as ISecurity;
                    outputs = CsvReaderWriter.ReadFromCsv(security, result.FilePath, fReportLogger);
                }
                if (outputs != null)
                {
                    foreach (object objec in outputs)
                    {
                        if (objec is SecurityDayData view)
                        {
                            UpdateDataCallback(programPortfolio => programPortfolio.TryAddOrEditDataToSecurity(SelectedName, view.Date, view.Date, view.ShareNo, view.UnitPrice, view.NewInvestment, null, fReportLogger));
                        }
                        else
                        {
                            _ = fReportLogger.LogUseful(ReportType.Error, ReportLocation.StatisticsPage, "Have the wrong type of thing");
                        }
                    }
                }
            }
        }

        public ICommand ExportCsvData
        {
            get;
        }

        private void ExecuteExportCsvData()
        {
            _ = fReportLogger.Log(ReportSeverity.Detailed, ReportType.Information, ReportLocation.DatabaseAccess, $"Selected Security {SelectedName} exporting data to csv.");
            if (SelectedName != null)
            {
                FileInteractionResult result = fUiGlobals.FileInteractionService.SaveFile("csv", string.Empty, DataStore.Directory(fUiGlobals.CurrentFileSystem), "Csv Files|*.csv|All Files|*.*");
                if (result.Success != null && (bool)result.Success)
                {
                    if (DataStore.TryGetAccount(Account.Security, SelectedName, out IValueList account))
                    {
                        ISecurity security = account as ISecurity;
                        CsvReaderWriter.WriteToCSVFile(security, result.FilePath, fReportLogger);
                    }
                    else
                    {
                        _ = fReportLogger.Log(ReportSeverity.Critical, ReportType.Error, ReportLocation.Saving, "Could not find security.");
                    }
                }
            }
        }

        private void ExecuteAddEditUnitPriceData(NameData name, DailyValuation oldValue, DailyValuation newValue)
        {
            if (newValue != null)
            {
                bool edited = false;
                UpdateDataCallback(programPortfolio => edited = programPortfolio.TryAddOrEditData(Account.Security, name.ToTwoName(), oldValue, newValue, fReportLogger));
                if (!edited)
                {
                    _ = fReportLogger?.Log(ReportSeverity.Critical, ReportType.Error, ReportLocation.EditingData, "Was not able to add or edit security data.");
                }
            }
        }

        public override void UpdateData(IPortfolio dataToDisplay, Action<object> removeTab)
        {
            _ = fReportLogger?.Log(ReportSeverity.Detailed, ReportType.Information, ReportLocation.DatabaseAccess, $"Selected Security {SelectedName} updating data.");
            base.UpdateData(dataToDisplay);

            if (SelectedName != null)
            {
                if (!dataToDisplay.Exists(Account.Security, SelectedName))
                {
                    removeTab?.Invoke(this);
                    return;
                }

                _ = dataToDisplay.TryGetAccount(Account.Security, SelectedName, out IValueList desired);

                ISecurity security = desired as ISecurity;
                TLVM?.UpdateData(security.UnitPrice);
                Trades = security.SecurityTrades.ToList();
                SecurityStats = dataToDisplay.GetStats(Account.Security, SelectedName, AccountStatisticsHelpers.AllStatistics()).Single();
                Values = dataToDisplay.NumberData(Account.Security, SelectedName, fReportLogger).ToList();
            }
        }

        public override void UpdateData(IPortfolio dataToDisplay)
        {
            UpdateData(dataToDisplay, null);
        }

        private RelayCommand fPreEditCommand;

        /// <summary>
        /// Called prior to an edit occurring in a row. This is used
        /// to record the state of the row before editing.
        /// </summary>
        public ICommand PreEditCommand
        {
            get
            {
                if (fPreEditCommand == null)
                {
                    fPreEditCommand = new RelayCommand(PreEdit);
                }

                return fPreEditCommand;
            }
        }

        private void PreEdit()
        {
            fOldSelectedTrade = SelectedTrade?.Copy();
        }

        public ICommand AddDefaultDataCommand
        {
            get;
            set;
        }

        private void DataGrid_AddingNewItem(object sender, AddingNewItemEventArgs e)
        {
            e.NewItem = new SecurityTrade()
            {
                TradeType = TradeType.Buy,
                Names = SelectedName,
                Day = DateTime.Today
            };
        }

        public ICommand SelectionChangedCommand
        {
            get;
            set;
        }

        private void ExecuteSelectionChanged(object obj)
        {
            if (Trades != null && obj is SecurityTrade data)
            {
                SelectedTrade = data;
            }
        }

        public ICommand AddEditDataCommand
        {
            get;
            set;
        }

        private void ExecuteAddEditData()
        {
            if (SelectedTrade != null)
            {
                bool edited = false;
                UpdateDataCallback(programPortfolio => edited = programPortfolio.TryAddOrEditTradeData(Account.Security, SelectedName.ToTwoName(), fOldSelectedTrade, SelectedTrade, fReportLogger));
                if (!edited)
                {
                    _ = fReportLogger?.Log(ReportSeverity.Critical, ReportType.Error, ReportLocation.EditingData, $"Was not able to add or edit {Account.Security} trade data.");
                }
            }
        }

        public ICommand DeleteTradeKeyDownCommand
        {
            get;
            set;
        }

        private void ExecuteDeleteTradeKeyDownValuation(KeyEventArgs e)
        {
            if (e.Key == Key.Delete || e.Key == Key.Back)
            {
                if (SelectedName != null && SelectedTrade != null)
                {
                    UpdateDataCallback(programPortfolio => programPortfolio.TryDeleteTradeData(Account.Security, SelectedName, SelectedTrade.Day, fReportLogger));
                }
                else
                {
                    _ = fReportLogger.Log(ReportSeverity.Critical, ReportType.Error, ReportLocation.DeletingData, "No Account was selected when trying to delete data.");
                }
            }
        }
    }
}
