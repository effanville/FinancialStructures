using FinanceCommonViewModels;
using FinancialStructures.Database;
using FinancialStructures.NamingStructures;
using FinancialStructures.PortfolioAPI;
using System.Collections.Generic;

namespace FinanceWindowsViewModels
{
    internal class BasicDataViewModel : ViewModelBase
    {
        private List<NameCompDate> fFundNames;
        public List<NameCompDate> FundNames
        {
            get { return fFundNames; }
            set { fFundNames = value; OnPropertyChanged(); }
        }

        private List<NameCompDate> fAccountNames;
        public List<NameCompDate> AccountNames
        {
            get { return fAccountNames; }
            set { fAccountNames = value; OnPropertyChanged(); }
        }

        private List<NameCompDate> fSectorNames;
        public List<NameCompDate> SectorNames
        {
            get { return fSectorNames; }
            set { fSectorNames = value; OnPropertyChanged(); }
        }

        private List<NameCompDate> fCurrencyNames;
        public List<NameCompDate> CurrencyNames
        {
            get { return fCurrencyNames; }
            set { fCurrencyNames = value; OnPropertyChanged(); }
        }

        public BasicDataViewModel(Portfolio portfolio)
            : base("Overview")
        {
            UpdateData(portfolio);
        }

        public override void UpdateData(Portfolio portfolio)
        {
            FundNames = portfolio.NameData(AccountType.Security);
            FundNames.Sort();
            AccountNames = portfolio.NameData(AccountType.BankAccount);
            AccountNames.Sort();
            SectorNames = portfolio.NameData(AccountType.Sector);
            SectorNames.Sort();
            CurrencyNames = portfolio.NameData(AccountType.Currency);
            CurrencyNames.Sort();
        }
    }
}
