using FinancialStructures.Database;
using FinanceCommonViewModels;
using System.Collections.Generic;
using FinancialStructures.PortfolioAPI;
using FinancialStructures.NamingStructures;

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
            FundNames = portfolio.NameData(AccountType.Security, null);
            FundNames.Sort();
            AccountNames = portfolio.NameData(AccountType.BankAccount, null);
            AccountNames.Sort();
            SectorNames = portfolio.NameData(AccountType.Sector, null);
            SectorNames.Sort();
            CurrencyNames = portfolio.NameData(AccountType.Currency, null);
            CurrencyNames.Sort();
        }
    }
}
