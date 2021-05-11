using System;
using System.Collections.Generic;
using System.Linq;
using FinancePortfolioDatabase.GUI.ViewModels.Common;
using FinancialStructures.Database;
using FinancialStructures.DataStructures;
using StructureCommon.Extensions;

namespace FinancePortfolioDatabase.GUI.ViewModels
{
    internal class BasicDataViewModel : DataDisplayViewModelBase
    {
        private string fPortfolioNameText;
        public string PortfolioNameText
        {
            get
            {
                return fPortfolioNameText;
            }
            set
            {
                SetAndNotify(ref fPortfolioNameText, value, nameof(PortfolioNameText));
            }
        }

        private bool fHasValues;
        public bool HasValues
        {
            get
            {
                return fHasValues;
            }
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

        public bool NoValues
        {
            get
            {
                return !fHasValues;
            }
        }

        private string fSecurityTotalText;
        public string SecurityTotalText
        {
            get
            {
                return fSecurityTotalText;
            }
            set
            {
                SetAndNotify(ref fSecurityTotalText, value, nameof(SecurityTotalText));
            }
        }

        private string fSecurityAmountText;
        public string SecurityAmountText
        {
            get
            {
                return fSecurityAmountText;
            }
            set
            {
                SetAndNotify(ref fSecurityAmountText, value, nameof(SecurityAmountText));
            }
        }

        private List<DayValue_Named> fTopSecurities;
        public List<DayValue_Named> TopSecurities
        {
            get
            {
                return fTopSecurities;
            }
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
            get
            {
                return fBankAccountTotalText;
            }
            set
            {
                SetAndNotify(ref fBankAccountTotalText, value, nameof(BankAccountTotalText));
            }
        }

        private string fBankAccountAmountText;
        public string BankAccountAmountText
        {
            get
            {
                return fBankAccountAmountText;
            }
            set
            {
                SetAndNotify(ref fBankAccountAmountText, value, nameof(BankAccountAmountText));
            }
        }

        private List<DayValue_Named> fTopBankAccounts;
        public List<DayValue_Named> TopBankAccounts
        {
            get
            {
                return fTopBankAccounts;
            }
            set
            {
                SetAndNotify(ref fTopBankAccounts, value, nameof(TopBankAccounts));
            }
        }

        public BasicDataViewModel(IPortfolio portfolio)
            : base("Overview", Account.All, portfolio)
        {
            UpdateData(portfolio);
        }

        public override void UpdateData(IPortfolio portfolio)
        {
            PortfolioNameText = string.IsNullOrWhiteSpace(portfolio.DatabaseName) ? "Unsaved database loaded" : $"Portfolio: {portfolio.DatabaseName} loaded.";
            HasValues = portfolio.NumberOf(Account.All) != 0;
            SecurityTotalText = $"Total Securities: {portfolio.NumberOf(Account.Security)}";
            SecurityAmountText = $"Total Value: {portfolio.TotalValue(Totals.Security).Truncate()} {portfolio.BaseCurrency}";
            var securities = portfolio.Funds.ToList();
            securities.Sort((fund, otherFund) => otherFund.Value(DateTime.Today).Value.CompareTo(fund.Value(DateTime.Today).Value));
            TopSecurities = securities.Take(5).Select(name => new DayValue_Named(name.Names.Company, name.Names.Name, name.Value(DateTime.Today))).ToList();

            BankAccountTotalText = $"Total Bank Accounts: {portfolio.NumberOf(Account.BankAccount)}";
            BankAccountAmountText = $"Total Value: {portfolio.TotalValue(Totals.BankAccount)} {portfolio.BaseCurrency}";
            var bankAccounts = portfolio.BankAccounts.ToList();
            bankAccounts.Sort((bank, otherBank) => otherBank.Value(DateTime.Today).Value.CompareTo(bank.Value(DateTime.Today).Value));
            TopBankAccounts = bankAccounts.Take(5).Select(name => new DayValue_Named(name.Names.Company, name.Names.Name, name.Value(DateTime.Today))).ToList();
        }
    }
}
