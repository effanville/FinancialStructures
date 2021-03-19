using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Input;
using FinancialStructures.Database;
using FinancialStructures.Database.Download;
using FinancialStructures.Database.Statistics;
using FinancialStructures.FinanceStructures;
using FinancialStructures.NamingStructures;
using StructureCommon.DataStructures;
using StructureCommon.DisplayClasses;
using StructureCommon.Reporting;
using UICommon.Commands;
using UICommon.ViewModelBases;

namespace FinancePortfolioDatabase.GUI.ViewModels.Common
{
    /// <summary>
    /// Data store behind view for a list of names and associated update name methods.
    /// </summary>
    internal class DataNamesViewModel : TabViewModelBase<IPortfolio>
    {
        private readonly Account TypeOfAccount;

        public bool ShowPriceHistory
        {
            get
            {
                return TypeOfAccount == Account.Security;
            }
        }

        /// <summary>
        /// Backing field for <see cref="DataNames"/>.
        /// </summary>
        private List<SelectableEquatable<NameData>> fDataNames = new List<SelectableEquatable<NameData>>();

        /// <summary>
        /// Name data of the names to be displayed in this view.
        /// </summary>
        public List<SelectableEquatable<NameData>> DataNames
        {
            get
            {
                return fDataNames;
            }
            set
            {
                SetAndNotify(ref fDataNames, value, nameof(DataNames));
                _ = ReportLogger.Log(ReportSeverity.Detailed, ReportType.Report, ReportLocation.DatabaseAccess, "Set Datanames");
            }
        }

        private NameData fPreEditSelectedName;

        /// <summary>
        /// The selected name at the start of selection.
        /// Holds the data before any possible editing.
        /// </summary>
        public NameData PreEditSelectedName
        {
            get
            {
                return fPreEditSelectedName;
            }
            set
            {
                SetAndNotify(ref fPreEditSelectedName, value, nameof(PreEditSelectedName));
                _ = ReportLogger.Log(ReportSeverity.Detailed, ReportType.Report, ReportLocation.DatabaseAccess, "Set PreEditSelectedName");
            }
        }

        private SelectableEquatable<NameData> fSelectedName;

        /// <summary>
        /// The selected name with any alterations made by the user.
        /// These alterations update the database when certain commands are executed.
        /// </summary>
        public SelectableEquatable<NameData> SelectedName
        {
            get
            {
                return fSelectedName;
            }
            set
            {
                SetAndNotify(ref fSelectedName, value, nameof(SelectedName));
                _ = ReportLogger.Log(ReportSeverity.Detailed, ReportType.Report, ReportLocation.DatabaseAccess, "Set SelectedName");
                if (SelectedName != null)
                {
                    OnPropertyChanged(nameof(SelectedNameSet));
                }
            }
        }

        public bool SelectedNameSet
        {
            get
            {
                return SelectedName != null;
            }
        }

        private DateTime fSelectedLatestDate;
        public DateTime SelectedLatestDate
        {
            get
            {
                return fSelectedLatestDate;
            }
            set
            {
                SetAndNotify(ref fSelectedLatestDate, value, nameof(SelectedLatestDate));
                _ = ReportLogger.Log(ReportSeverity.Detailed, ReportType.Report, ReportLocation.DatabaseAccess, "Set SelectedLatestDate");
            }
        }

        private List<DailyValuation> fSelectedValueHistory;

        /// <summary>
        /// The evolution of the selected accounts total value.
        /// </summary>
        public List<DailyValuation> SelectedValueHistory
        {
            get
            {
                return fSelectedValueHistory;
            }
            set
            {
                SetAndNotify(ref fSelectedValueHistory, value, nameof(SelectedValueHistory));
                _ = ReportLogger.Log(ReportSeverity.Detailed, ReportType.Report, ReportLocation.DatabaseAccess, "Set ValueHistory");
            }
        }

        private List<DailyValuation> fSelectedPriceHistory;

        /// <summary>
        /// The history of the price evolution of the selected object.
        /// </summary>
        public List<DailyValuation> SelectedPriceHistory
        {
            get
            {
                return fSelectedPriceHistory;
            }
            set
            {
                SetAndNotify(ref fSelectedPriceHistory, value, nameof(SelectedPriceHistory));
                _ = ReportLogger.Log(ReportSeverity.Detailed, ReportType.Report, ReportLocation.DatabaseAccess, "Set PriceHistory");
            }
        }

        /// <summary>
        /// Function which updates the main data store.
        /// </summary>
        private readonly Action<Action<IPortfolio>> UpdateDataCallback;

        /// <summary>
        /// Logs any possible issues in the routines here back to the user.
        /// </summary>
        private readonly IReportLogger ReportLogger;

        /// <summary>
        /// Construct an instance.
        /// </summary>
        public DataNamesViewModel(IPortfolio portfolio, Action<Action<IPortfolio>> updateDataCallback, IReportLogger reportLogger, Action<object> loadSelectedData, Account accountType)
            : base("Accounts", portfolio, loadSelectedData)
        {
            UpdateDataCallback = updateDataCallback;
            TypeOfAccount = accountType;
            ReportLogger = reportLogger;
            DataNames = portfolio.NameData(accountType).Select(name => new SelectableEquatable<NameData>(name, portfolio.LatestDate(accountType, name) == DateTime.Today)).ToList();
            DataNames.Sort();

            SelectionChangedCommand = new RelayCommand<object>(ExecuteSelectionChanged);
            CreateCommand = new RelayCommand(ExecuteCreateEdit);
            PreEditCommand = new RelayCommand(ExecutePreEdit);
            DeleteCommand = new RelayCommand(ExecuteDelete);
            DownloadCommand = new RelayCommand(ExecuteDownloadCommand);
            OpenTabCommand = new RelayCommand(() => LoadSelectedTab(SelectedName?.Instance));
            AddDefaultDataCommand = new RelayCommand<AddingNewItemEventArgs>(e => DataGrid_AddingNewItem(null, e));
        }

        /// <summary>
        /// Command that opens a tab associated to the selected entry.
        /// </summary>
        public ICommand OpenTabCommand
        {
            get;
        }

        /// <summary>
        /// Updates the data in this view model from the given portfolio.
        /// </summary>
        public override void UpdateData(IPortfolio portfolio, Action<object> removeTab)
        {
            _ = ReportLogger.Log(ReportSeverity.Detailed, ReportType.Report, ReportLocation.DatabaseAccess, $"Updating DataView for {TypeOfAccount} list.");
            base.UpdateData(portfolio);

            bool IsUpdated(NameData name)
            {
                return portfolio.LatestDate(TypeOfAccount, name) == DateTime.Today || portfolio.LatestValue(TypeOfAccount, name) == 0.0;
            }
            var values = portfolio.NameData(TypeOfAccount).Select(name => new SelectableEquatable<NameData>(name, IsUpdated(name))).ToList();
            DataNames = null;
            DataNames = values;
            DataNames.Sort((a, b) => a.Instance.CompareTo(b.Instance));
        }

        /// <summary>
        /// Updates the data in this view model from the given portfolio.
        /// </summary>
        public override void UpdateData(IPortfolio portfolio)
        {
            UpdateData(portfolio, null);
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
            _ = ReportLogger.Log(ReportSeverity.Detailed, ReportType.Report, ReportLocation.DatabaseAccess, $"Download selected for account {SelectedName} - a {TypeOfAccount}");
            if (SelectedName != null)
            {
                NameData names = SelectedName.Instance;
                UpdateDataCallback(async programPortfolio => await PortfolioDataUpdater.Download(TypeOfAccount, programPortfolio, names, ReportLogger).ConfigureAwait(false));
            }
        }

        public ICommand AddDefaultDataCommand
        {
            get;
            set;
        }

        private void DataGrid_AddingNewItem(object sender, AddingNewItemEventArgs e)
        {
            if (DataNames != null)
            {
                e.NewItem = new SelectableEquatable<NameData>(new NameData(), false);
            }
        }

        /// <summary>
        /// Enacts the selected item in the datagrid has changed.
        /// </summary>
        public ICommand SelectionChangedCommand
        {
            get;
            set;
        }

        private void ExecuteSelectionChanged(object args)
        {
            if (DataNames != null && args is SelectableEquatable<NameData> selectableName && selectableName.Instance != null)
            {
                SelectedName = selectableName;
                var name = selectableName.Instance;
                _ = ReportLogger.Log(ReportSeverity.Detailed, ReportType.Report, ReportLocation.AddingData, $"Current item is a name {name}");

                _ = DataStore.TryGetAccount(TypeOfAccount, name, out var desired);
                if (desired != null)
                {
                    _ = ReportLogger.Log(ReportSeverity.Detailed, ReportType.Report, ReportLocation.AddingData, $"Stored item has been retrieved and is a {TypeOfAccount}.");
                    ISecurity security = desired as ISecurity;
                    var unitPrices = security?.UnitPrice;

                    SelectedLatestDate = desired.LatestValue()?.Day ?? DateTime.Today;
                    DateTime calculationDate = desired.FirstValue()?.Day ?? DateTime.Today.AddDays(-365);
                    var outputs = new List<DailyValuation>();
                    var prices = new List<DailyValuation>();

                    while (calculationDate < DateTime.Today)
                    {
                        var calcuationDateStatistics = desired.Value(calculationDate);
                        outputs.Add(new DailyValuation(calculationDate, calcuationDateStatistics?.Value ?? 0.0));
                        if (unitPrices != null)
                        {
                            prices.Add(new DailyValuation(calculationDate, unitPrices.Value(calculationDate)?.Value ?? 0.0));
                        }

                        calculationDate = calculationDate.AddDays(30);
                    }
                    if (calculationDate == DateTime.Today)
                    {
                        var calcuationDateStatistics = desired.Value(calculationDate);
                        outputs.Add(new DailyValuation(calculationDate, calcuationDateStatistics?.Value ?? 0.0));

                        if (unitPrices != null)
                        {
                            // This value here can be null, even if unitPrices is not.
                            prices.Add(new DailyValuation(calculationDate, unitPrices.Value(calculationDate)?.Value ?? 0.0));
                        }
                    }

                    SelectedValueHistory = outputs;
                    SelectedPriceHistory = prices;

                    _ = ReportLogger.Log(ReportSeverity.Detailed, ReportType.Report, ReportLocation.AddingData, $"Successfully updated SelectedItem.");
                }
            }
        }

        /// <summary>
        /// Called prior to an edit occurring in a row. This is used
        /// to record the state of the row before editing.
        /// </summary>
        public ICommand PreEditCommand
        {
            get;
            set;
        }

        private void ExecutePreEdit()
        {
            PreEditSelectedName = SelectedName?.Instance?.Copy();
        }

        /// <summary>
        /// Adds a new entry if the view has more than the repository, or edits an entry if these are the same.
        /// </summary>
        public ICommand CreateCommand
        {
            get;
            set;
        }

        private void ExecuteCreateEdit()
        {
            _ = ReportLogger.Log(ReportSeverity.Detailed, ReportType.Report, ReportLocation.DatabaseAccess, $"ExecuteCreateEdit called.");
            bool edited = false;
            var originRowName = SelectedName.Instance; //rowName.Instance;
            if (!DataStore.NameData(TypeOfAccount).Any(item => item.Name == PreEditSelectedName?.Name && item.Company == PreEditSelectedName?.Company))
            {
                _ = ReportLogger.Log(ReportSeverity.Detailed, ReportType.Report, ReportLocation.AddingData, $"Adding {originRowName} to the database");
                NameData name = new NameData(originRowName.Company, originRowName.Name, originRowName.Currency, originRowName.Url, originRowName.Sectors);
                UpdateDataCallback(programPortfolio => edited = programPortfolio.TryAdd(TypeOfAccount, name, ReportLogger));
            }
            else
            {
                // maybe fired from editing stuff. Try that
                if (!string.IsNullOrEmpty(originRowName.Name) || !string.IsNullOrEmpty(originRowName.Company))
                {
                    _ = ReportLogger.Log(ReportSeverity.Detailed, ReportType.Report, ReportLocation.EditingData, $"Editing {PreEditSelectedName} to {originRowName} in the database");
                    NameData name = new NameData(originRowName.Company, originRowName.Name, originRowName.Currency, originRowName.Url, originRowName.Sectors, originRowName.Notes);
                    UpdateDataCallback(programPortfolio => edited = programPortfolio.TryEditName(TypeOfAccount, PreEditSelectedName, name, ReportLogger));
                }
            }

            if (!edited)
            {
                _ = ReportLogger.Log(ReportSeverity.Critical, ReportType.Error, ReportLocation.EditingData, "Was not able to edit desired.");
            }
        }

        /// <summary>
        /// Deletes the selected entry.
        /// </summary>
        public ICommand DeleteCommand
        {
            get;
        }
        private void ExecuteDelete()
        {
            _ = ReportLogger.Log(ReportSeverity.Detailed, ReportType.Report, ReportLocation.DeletingData, $"Deleting {SelectedName} from the database");
            if (SelectedName != null)
            {
                UpdateDataCallback(programPortfolio => programPortfolio.TryRemove(TypeOfAccount, SelectedName.Instance, ReportLogger));
            }
            else
            {
                _ = ReportLogger.Log(ReportSeverity.Critical, ReportType.Error, ReportLocation.DeletingData, "Nothing was selected when trying to delete.");
            }
        }
    }
}
