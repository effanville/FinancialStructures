using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows.Data;
using System.Windows.Input;
using Common.Structure.DataStructures;
using Common.Structure.NamingStructures;
using Common.UI;
using Common.UI.Commands;
using FinancePortfolioDatabase.GUI.TemplatesAndStyles;
using FinancePortfolioDatabase.GUI.ViewModels.Common;
using FinancialStructures;
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
            DataUpdateCallback = updateData;
            UpdateData(portfolio);
        }

        public override void UpdateData(IPortfolio portfolio)
        {
            DataStore = portfolio;
            PortfolioNameText = string.IsNullOrWhiteSpace(portfolio.DatabaseName(fUiGlobals.CurrentFileSystem)) ? "Unsaved database" : $"{portfolio.DatabaseName(fUiGlobals.CurrentFileSystem)}";
            HasValues = portfolio.NumberOf(Account.All) != 0;
            SecurityTotalText = $"Total Securities: {portfolio.NumberOf(Account.Security)}";
            CultureInfo culture = CurrencyCultureHelpers.CurrencyCultureInfo(portfolio.BaseCurrency);
            SecurityAmountText = $"Total Value: {portfolio.TotalValue(Totals.Security).ToString("C2", culture)}";
            List<ISecurity> securities = portfolio.FundsThreadSafe.ToList();

            securities.Sort((fund, otherFund) => fund.ValueComparison(otherFund, DateTime.Today));
            TopSecurities = securities.Take(5).Select(name => new Labelled<TwoName, DailyValuation>(new TwoName(name.Names.Company, name.Names.Name), name.Value(DateTime.Today))).ToList();

            BankAccountTotalText = $"Total Bank Accounts: {portfolio.NumberOf(Account.BankAccount)}";
            BankAccountAmountText = $"Total Value: {portfolio.TotalValue(Totals.BankAccount).ToString("C2", culture)}";
            List<IExchangableValueList> bankAccounts = portfolio.BankAccountsThreadSafe.ToList();
            bankAccounts.Sort((bank, otherBank) => bank.ValueComparison(otherBank, DateTime.Today));
            TopBankAccounts = bankAccounts.Take(5).Select(name => new Labelled<TwoName, DailyValuation>(new TwoName(name.Names.Company, name.Names.Name), name.Value(DateTime.Today) ?? new DailyValuation(DateTime.Today, 0.0m))).ToList();
            Notes = portfolio.Notes.ToList();
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

        internal void DeleteSelectedNote()
        {
            if (fSelectedNote != null)
            {
                DataUpdateCallback(portfolio => portfolio.RemoveNote(fSelectedNote));
            }
        }
    }
}
