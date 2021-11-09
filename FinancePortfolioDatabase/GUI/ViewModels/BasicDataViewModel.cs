using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using Common.Structure.DataStructures;
using Common.Structure.Extensions;
using Common.Structure.NamingStructures;
using Common.UI;
using Common.UI.Commands;
using FinancePortfolioDatabase.GUI.TemplatesAndStyles;
using FinancePortfolioDatabase.GUI.ViewModels.Common;
using FinancialStructures.Database;
using FinancialStructures.Database.Extensions;
using FinancialStructures.DataStructures;
using FinancialStructures.FinanceStructures;
using FinancialStructures.NamingStructures;

namespace FinancePortfolioDatabase.GUI.ViewModels
{
    internal class BasicDataViewModel : DataDisplayViewModelBase
    {
        private readonly Action<Action<IPortfolio>> DataUpdateCallback;

        private string fPortfolioNameText;
        public string PortfolioNameText
        {
            get => fPortfolioNameText;
            set => SetAndNotify(ref fPortfolioNameText, value, nameof(PortfolioNameText));
        }

        private bool fHasValues;
        public bool HasValues
        {
            get => fHasValues;
            set
            {
                if (fHasValues != value)
                {
                    fHasValues = value;
                    OnPropertyChanged(nameof(HasValues));
                    OnPropertyChanged(nameof(NoValues));
                }
            }
        }

        public bool NoValues => !fHasValues;

        private string fSecurityTotalText;
        public string SecurityTotalText
        {
            get => fSecurityTotalText;

            set => SetAndNotify(ref fSecurityTotalText, value, nameof(SecurityTotalText));

        }

        private string fSecurityAmountText;
        public string SecurityAmountText
        {
            get => fSecurityAmountText;
            set => SetAndNotify(ref fSecurityAmountText, value, nameof(SecurityAmountText));
        }

        private List<Labelled<TwoName, DailyValuation>> fTopSecurities;
        public List<Labelled<TwoName, DailyValuation>> TopSecurities
        {
            get => fTopSecurities;
            set
            {
                if (fTopSecurities != value)
                {
                    fTopSecurities = value;
                    OnPropertyChanged(nameof(TopSecurities));
                }
            }
        }

        private string fBankAccountTotalText;
        public string BankAccountTotalText
        {
            get => fBankAccountTotalText;
            set => SetAndNotify(ref fBankAccountTotalText, value, nameof(BankAccountTotalText));
        }

        private string fBankAccountAmountText;
        public string BankAccountAmountText
        {
            get => fBankAccountAmountText;
            set => SetAndNotify(ref fBankAccountAmountText, value, nameof(BankAccountAmountText));
        }

        private List<Labelled<TwoName, DailyValuation>> fTopBankAccounts;
        public List<Labelled<TwoName, DailyValuation>> TopBankAccounts
        {
            get => fTopBankAccounts;
            set => SetAndNotify(ref fTopBankAccounts, value, nameof(TopBankAccounts));
        }

        private List<Note> fNotes;
        public List<Note> Notes
        {
            get => fNotes;
            set => SetAndNotify(ref fNotes, value, nameof(Notes));
        }

        private Note fSelectedNote;

        public BasicDataViewModel(UiGlobals globals, UiStyles styles, IPortfolio portfolio, Action<Action<IPortfolio>> updateData)
            : base(globals, styles, portfolio, "Overview", Account.All)
        {
            SelectionChangedCommand = new RelayCommand<object>(ExecuteSelectionChanged);
            CreateCommand = new RelayCommand(ExecuteCreateEdit);
            DeleteValuationCommand = new RelayCommand<KeyEventArgs>(ExecuteDeleteValuation);
            AddDefaultDataCommand = new RelayCommand<AddingNewItemEventArgs>(DataGrid_AddingNewItem);
            DataUpdateCallback = updateData;
            UpdateData(portfolio);
        }

        public override void UpdateData(IPortfolio portfolio)
        {
            DataStore = portfolio;
            PortfolioNameText = string.IsNullOrWhiteSpace(portfolio.DatabaseName(fUiGlobals.CurrentFileSystem)) ? "Unsaved database" : $"{portfolio.DatabaseName(fUiGlobals.CurrentFileSystem)}";
            HasValues = portfolio.NumberOf(Account.All) != 0;
            SecurityTotalText = $"Total Securities: {portfolio.NumberOf(Account.Security)}";
            SecurityAmountText = $"Total Value: {portfolio.TotalValue(Totals.Security).Truncate()} {portfolio.BaseCurrency}";
            List<ISecurity> securities = portfolio.FundsThreadSafe.ToList();
            securities.Sort((fund, otherFund) => otherFund.Value(DateTime.Today).Value.CompareTo(fund.Value(DateTime.Today).Value));
            TopSecurities = securities.Take(5).Select(name => new Labelled<TwoName, DailyValuation>(new TwoName(name.Names.Company, name.Names.Name), name.Value(DateTime.Today))).ToList();

            BankAccountTotalText = $"Total Bank Accounts: {portfolio.NumberOf(Account.BankAccount)}";
            BankAccountAmountText = $"Total Value: {portfolio.TotalValue(Totals.BankAccount)} {portfolio.BaseCurrency}";
            List<IExchangableValueList> bankAccounts = portfolio.BankAccountsThreadSafe.ToList();
            bankAccounts.Sort((bank, otherBank) => otherBank.Value(DateTime.Today).Value.CompareTo(bank.Value(DateTime.Today).Value));
            TopBankAccounts = bankAccounts.Take(5).Select(name => new Labelled<TwoName, DailyValuation>(new TwoName(name.Names.Company, name.Names.Name), name.Value(DateTime.Today) ?? new DailyValuation(DateTime.Today, 0.0))).ToList();
            Notes = portfolio.Notes.ToList();
        }

        /// <summary>
        /// Command called to add default values.
        /// </summary>
        public ICommand AddDefaultDataCommand
        {
            get;
            set;
        }

        private void DataGrid_AddingNewItem(AddingNewItemEventArgs e)
        {
            e.NewItem = new Note()
            {
                TimeStamp = DateTime.Today
            };
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
            if (Notes != null && args is Note note)
            {
                fSelectedNote = note;
            }
            else if (args == CollectionView.NewItemPlaceholder)
            {
                fSelectedNote = null;
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

        private void ExecuteCreateEdit()
        {
            if (fSelectedNote != null && !DataStore.Notes.Contains(fSelectedNote))
            {
                DataUpdateCallback(portfolio => portfolio.AddNote(fSelectedNote.TimeStamp, fSelectedNote.Text));
            }
        }


        /// <summary>
        /// Command to delete values from the <see cref="TimeList"/>
        /// </summary>
        public ICommand DeleteValuationCommand
        {
            get;
            set;
        }

        private void ExecuteDeleteValuation(KeyEventArgs e)
        {
            if (e.Key == Key.Delete || e.Key == Key.Back)
            {
                if (e.OriginalSource is DataGridCell dgCell)
                {
                    if (fSelectedNote != null)
                    {
                        DataUpdateCallback(portfolio => portfolio.RemoveNote(fSelectedNote));
                    }
                }
            }
        }
    }
}
