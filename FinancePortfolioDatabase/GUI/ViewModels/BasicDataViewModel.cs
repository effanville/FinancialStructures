using System;
using System.Collections.Generic;
using System.Linq;
using Common.Structure.DataStructures;
using Common.Structure.Extensions;
using Common.Structure.NamingStructures;
using Common.UI;
using FinancePortfolioDatabase.GUI.TemplatesAndStyles;
using FinancePortfolioDatabase.GUI.ViewModels.Common;
using FinancialStructures.Database;
using FinancialStructures.FinanceStructures;
using FinancialStructures.NamingStructures;

namespace FinancePortfolioDatabase.GUI.ViewModels
{
    internal class BasicDataViewModel : DataDisplayViewModelBase
    {
        private readonly UiGlobals fUiGlobals;
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

        public BasicDataViewModel(IPortfolio portfolio, UiStyles styles, UiGlobals globals)
            : base(styles, "Overview", Account.All, portfolio)
        {
            fUiGlobals = globals;
            UpdateData(portfolio);
        }

        public override void UpdateData(IPortfolio portfolio)
        {
            PortfolioNameText = string.IsNullOrWhiteSpace(portfolio.DatabaseName(fUiGlobals.CurrentFileSystem)) ? "Unsaved database loaded" : $"Portfolio: {portfolio.DatabaseName(fUiGlobals.CurrentFileSystem)} loaded.";
            HasValues = portfolio.NumberOf(Account.All) != 0;
            SecurityTotalText = $"Total Securities: {portfolio.NumberOf(Account.Security)}";
            SecurityAmountText = $"Total Value: {portfolio.TotalValue(Totals.Security).Truncate()} {portfolio.BaseCurrency}";
            List<ISecurity> securities = portfolio.FundsThreadSafe.ToList();
            securities.Sort((fund, otherFund) => otherFund.Value(DateTime.Today).Value.CompareTo(fund.Value(DateTime.Today).Value));
            TopSecurities = securities.Take(5).Select(name => new Labelled<TwoName, DailyValuation>(new TwoName(name.Names.Company, name.Names.Name), name.Value(DateTime.Today))).ToList();

            BankAccountTotalText = $"Total Bank Accounts: {portfolio.NumberOf(Account.BankAccount)}";
            BankAccountAmountText = $"Total Value: {portfolio.TotalValue(Totals.BankAccount)} {portfolio.BaseCurrency}";
            List<ICashAccount> bankAccounts = portfolio.BankAccountsThreadSafe.ToList();
            bankAccounts.Sort((bank, otherBank) => otherBank.Value(DateTime.Today).Value.CompareTo(bank.Value(DateTime.Today).Value));
            TopBankAccounts = bankAccounts.Take(5).Select(name => new Labelled<TwoName, DailyValuation>(new TwoName(name.Names.Company, name.Names.Name), name.Value(DateTime.Today) ?? new DailyValuation(DateTime.Today, 0.0))).ToList();
        }
    }
}
