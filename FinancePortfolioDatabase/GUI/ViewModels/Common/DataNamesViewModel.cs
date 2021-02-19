using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using FinancialStructures.Database;
using FinancialStructures.Database.Download;
using FinancialStructures.FinanceStructures;
using FinancialStructures.NamingStructures;
using StructureCommon.DataStructures;
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
        private int fSelectedRowIndex;

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
        private List<NameData> fDataNames = new List<NameData>();

        /// <summary>
        /// Name data of the names to be displayed in this view.
        /// </summary>
        public List<NameData> DataNames
        {
            get
            {
                return fDataNames;
            }
            set
            {
                SetAndNotify(ref fDataNames, value, nameof(DataNames));
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
            }
        }

        private NameData fSelectedName;

        /// <summary>
        /// The selected name with any alterations made by the user.
        /// These alterations update the database when certain commands are executed.
        /// </summary>
        public NameData SelectedName
        {
            get
            {
                return fSelectedName;
            }
            set
            {
                SetAndNotify(ref fSelectedName, value, nameof(SelectedName));
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
            fSelectedRowIndex = -1;
            ReportLogger = reportLogger;
            DataNames = portfolio.NameData(accountType);
            DataNames.Sort();

            CreateCommand = new RelayCommand<DataGridRowEditEndingEventArgs>(ExecuteCreateEdit);
            SelectionChangedCommand = new RelayCommand<SelectionChangedEventArgs>(ExecuteSelectionChanged);
            DeleteCommand = new RelayCommand(ExecuteDelete);
            DownloadCommand = new RelayCommand(ExecuteDownloadCommand);
            OpenTabCommand = new RelayCommand(() => LoadSelectedTab(PreEditSelectedName));
            SelectedTextChangedCommand = new RelayCommand<RoutedEventArgs>(SelectedNameEdited);
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
            base.UpdateData(portfolio);

            var values = portfolio.NameData(TypeOfAccount);
            DataNames = null;
            DataNames = values;
            DataNames.Sort();
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
            if (PreEditSelectedName != null)
            {
                NameData names = PreEditSelectedName;
                UpdateDataCallback(async programPortfolio => await PortfolioDataUpdater.Download(TypeOfAccount, programPortfolio, names, ReportLogger).ConfigureAwait(false));
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
        private void ExecuteSelectionChanged(SelectionChangedEventArgs e)
        {
            if (e.Source is DataGrid dg)
            {
                if (dg.CurrentItem != null && dg.CurrentItem is NameData name)
                {
                    if (DataNames != null)
                    {
                        int index = DataNames.FindIndex(x => x.IsEqualTo(name));
                        if (fSelectedRowIndex == -1 || fSelectedRowIndex != index)
                        {
                            fSelectedRowIndex = index;
                            PreEditSelectedName = name.Copy();
                            SelectedName = name.Copy();
                            _ = DataStore.TryGetAccount(TypeOfAccount, name, out var desired);

                            ISecurity security = desired as ISecurity;
                            var unitPrices = security?.UnitPrice;
                            SelectedLatestDate = desired.LatestValue().Day;
                            DateTime calculationDate = desired.FirstValue().Day;
                            var outputs = new List<DailyValuation>();
                            var prices = new List<DailyValuation>();

                            while (calculationDate < DateTime.Today)
                            {
                                var calcuationDateStatistics = desired.Value(calculationDate);
                                outputs.Add(new DailyValuation(calculationDate, calcuationDateStatistics.Value));
                                if (unitPrices != null)
                                {
                                    prices.Add(new DailyValuation(calculationDate, unitPrices.Value(calculationDate).Value));
                                }

                                calculationDate = calculationDate.AddDays(30);
                            }
                            if (calculationDate == DateTime.Today)
                            {
                                var calcuationDateStatistics = desired.Value(calculationDate);
                                outputs.Add(new DailyValuation(calculationDate, calcuationDateStatistics.Value));

                                if (unitPrices != null)
                                {
                                    prices.Add(new DailyValuation(calculationDate, unitPrices.Value(calculationDate).Value));
                                }
                            }

                            SelectedValueHistory = outputs;
                            SelectedPriceHistory = prices;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Called when text in the <see cref="SelectedName"/> has changed.
        /// </summary>
        public ICommand SelectedTextChangedCommand
        {
            get;
            set;
        }

        private void SelectedNameEdited(RoutedEventArgs e)
        {
            bool edited = false;
            UpdateDataCallback(portfolio => edited = portfolio.TryEditName(TypeOfAccount, PreEditSelectedName, SelectedName.Copy(), ReportLogger));

            if (!edited)
            {
                _ = ReportLogger.LogWithStrings("Critical", "Error", "EditingData", "Was not able to edit desired.");
            }
        }

        /// <summary>
        /// Adds a new entry if the view has more than the repository, or edits an entry if these are the same.
        /// </summary>
        public ICommand CreateCommand
        {
            get;
            set;
        }
        private void ExecuteCreateEdit(DataGridRowEditEndingEventArgs e)
        {
            bool edited = false;
            var originRowName = e.Row.DataContext as NameData;
            if (!DataStore.NameData(TypeOfAccount).Any(item => item.Name == PreEditSelectedName.Name && item.Company == PreEditSelectedName.Company))
            {
                NameData name = new NameData(originRowName.Company, originRowName.Name, originRowName.Currency, originRowName.Url, originRowName.Sectors, originRowName.Notes);
                UpdateDataCallback(programPortfolio => edited = programPortfolio.TryAdd(TypeOfAccount, name, ReportLogger));
            }
            else
            {
                // maybe fired from editing stuff. Try that
                if (!string.IsNullOrEmpty(originRowName.Name) || !string.IsNullOrEmpty(originRowName.Company))
                {
                    NameData name = new NameData(originRowName.Company, originRowName.Name, originRowName.Currency, originRowName.Url, originRowName.Sectors, originRowName.Notes);
                    UpdateDataCallback(programPortfolio => edited = programPortfolio.TryEditName(TypeOfAccount, fPreEditSelectedName, name, ReportLogger));
                }
                if (!edited)
                {
                    _ = ReportLogger.LogWithStrings("Critical", "Error", "EditingData", "Was not able to edit desired.");
                }
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
            if (PreEditSelectedName != null)
            {
                UpdateDataCallback(programPortfolio => programPortfolio.TryRemove(TypeOfAccount, PreEditSelectedName, ReportLogger));
            }
            else
            {
                _ = ReportLogger.LogWithStrings("Critical", "Error", "DeletingData", "Nothing was selected when trying to delete.");
            }
        }
    }
}
