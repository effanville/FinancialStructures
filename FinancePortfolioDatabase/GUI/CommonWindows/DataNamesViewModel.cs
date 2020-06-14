using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using FinancialStructures.FinanceInterfaces;
using FinancialStructures.NamingStructures;
using FinancialStructures.PortfolioAPI;
using StructureCommon.Reporting;
using UICommon.Commands;
using UICommon.ViewModelBases;

namespace FinanceCommonViewModels
{
    /// <summary>
    /// Data store behind view for a list of names and associated update name methods.
    /// </summary>
    internal class DataNamesViewModel : TabViewModelBase<IPortfolio>
    {
        private readonly AccountType TypeOfAccount;

        /// <summary>
        /// Backing field for <see cref="DataNames"/>.
        /// </summary>
        private List<NameCompDate> fDataNames = new List<NameCompDate>();

        /// <summary>
        /// Name data of the names to be displayed in this view.
        /// </summary>
        public List<NameCompDate> DataNames
        {
            get
            {
                return fDataNames;
            }
            set
            {
                fDataNames = value;
                OnPropertyChanged(nameof(DataNames));
            }
        }

        private NameCompDate fPreEditSelectedName;

        /// <summary>
        /// Backing field for <see cref="SelectedName"/>.
        /// </summary>
        private NameCompDate fSelectedName;

        /// <summary>
        /// Name and Company data of the selected account in the list <see cref="DataNames"/>
        /// </summary>
        public NameCompDate SelectedName
        {
            get
            {
                return fSelectedName;
            }
            set
            {
                if (fSelectedName != value)
                {
                    if (SelectedName != null && !SelectedName.Equals(value))
                    {
                        fPreEditSelectedName = value?.Copy();
                    }
                    if (SelectedName == null)
                    {
                        fPreEditSelectedName = value?.Copy();
                    }

                    fSelectedName = value;
                    OnPropertyChanged(nameof(SelectedName));
                }
            }
        }

        private int fSelectedIndex;
        public int SelectedIndex
        {
            get
            {
                return fSelectedIndex;
            }
            set
            {
                if (fSelectedIndex != value)
                {
                    fSelectedIndex = value;
                    if (fSelectedIndex > -1 && fSelectedIndex < DataNames.Count)
                    {
                        SelectedName = DataNames[fSelectedIndex];
                    }
                    OnPropertyChanged(nameof(SelectedIndex));
                }
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
        public DataNamesViewModel(IPortfolio portfolio, Action<Action<IPortfolio>> updateDataCallback, IReportLogger reportLogger, Action<object> loadSelectedData, AccountType accountType)
            : base("Accounts", portfolio, loadSelectedData)
        {
            UpdateDataCallback = updateDataCallback;
            TypeOfAccount = accountType;
            ReportLogger = reportLogger;
            DataNames = portfolio.NameData(accountType);
            DataNames.Sort();

            CreateCommand = new RelayCommand(ExecuteCreateEdit);
            DeleteCommand = new RelayCommand(ExecuteDelete);
            DownloadCommand = new RelayCommand(ExecuteDownloadCommand);
            OpenTabCommand = new RelayCommand(OpenTab);
        }

        /// <summary>
        /// Command that opens a tab associated to the selected entry.
        /// </summary>
        public ICommand OpenTabCommand
        {
            get;
        }
        private void OpenTab()
        {
            LoadSelectedTab(SelectedName);
        }

        /// <summary>
        /// Updates the data in this view model from the given portfolio.
        /// </summary>
        public override void UpdateData(IPortfolio portfolio, Action<object> removeTab)
        {
            base.UpdateData(portfolio);

            var values = portfolio.NameData(TypeOfAccount);
            if (values.Count != DataNames.Count)
            {
                DataNames = null;
                DataNames = values;
                DataNames.Sort();
            }
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
            if (SelectedName != null)
            {
                NameData names = SelectedName;
                UpdateDataCallback(async programPortfolio => await PortfolioDataUpdater.DownloadOfType(TypeOfAccount, programPortfolio, names, ReportLogger).ConfigureAwait(false));
            }
        }

        /// <summary>
        /// Adds a new entry if the view has more than the repository, or edits an entry if these are the same.
        /// </summary>
        public ICommand CreateCommand
        {
            get; set;
        }
        private void ExecuteCreateEdit()
        {
            bool edited = false;
            if (!DataStore.NameData(TypeOfAccount).Any(item => item.Name == SelectedName.Name && item.Company == SelectedName.Company))
            {
                NameData name = new NameData(SelectedName.Company, SelectedName.Name, SelectedName.Currency, SelectedName.Url, SelectedName.Sectors);
                UpdateDataCallback(programPortfolio => edited = programPortfolio.TryAdd(TypeOfAccount, name, ReportLogger));
            }
            else
            {
                // maybe fired from editing stuff. Try that
                if (!string.IsNullOrEmpty(SelectedName.Name) || !string.IsNullOrEmpty(SelectedName.Company))
                {
                    NameData name = new NameData(SelectedName.Company, SelectedName.Name, SelectedName.Currency, SelectedName.Url, SelectedName.Sectors);
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
            if (SelectedName.Name != null)
            {
                UpdateDataCallback(programPortfolio => programPortfolio.TryRemove(TypeOfAccount, SelectedName, ReportLogger));
            }
            else
            {
                _ = ReportLogger.LogWithStrings("Critical", "Error", "DeletingData", "Nothing was selected when trying to delete.");
            }
        }
    }
}
