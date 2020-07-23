using System.Collections.Generic;
using FinancialStructures.FinanceInterfaces;
using FinancialStructures.NamingStructures;
using UICommon.ViewModelBases;

namespace FinanceWindowsViewModels
{
    internal class BasicDataViewModel : ViewModelBase<IPortfolio>
    {
        private List<NameCompDate> fFundNames;
        public List<NameCompDate> FundNames
        {
            get
            {
                return fFundNames;
            }
            set
            {
                fFundNames = value;
                OnPropertyChanged();
            }
        }

        private List<NameCompDate> fAccountNames;
        public List<NameCompDate> AccountNames
        {
            get
            {
                return fAccountNames;
            }
            set
            {
                fAccountNames = value;
                OnPropertyChanged();
            }
        }

        private List<NameCompDate> fSectorNames;
        public List<NameCompDate> SectorNames
        {
            get
            {
                return fSectorNames;
            }
            set
            {
                fSectorNames = value;
                OnPropertyChanged();
            }
        }

        private List<NameCompDate> fCurrencyNames;
        public List<NameCompDate> CurrencyNames
        {
            get
            {
                return fCurrencyNames;
            }
            set
            {
                fCurrencyNames = value;
                OnPropertyChanged();
            }
        }

        public BasicDataViewModel(IPortfolio portfolio)
            : base("Overview")
        {
            UpdateData(portfolio);
        }

        public override void UpdateData(IPortfolio portfolio)
        {
            FundNames = portfolio.NameData(AccountType.Security);
            FundNames.Sort();
            AccountNames = portfolio.NameData(AccountType.BankAccount);
            AccountNames.Sort();
            SectorNames = portfolio.NameData(AccountType.Benchmark);
            SectorNames.Sort();
            CurrencyNames = portfolio.NameData(AccountType.Currency);
            CurrencyNames.Sort();
        }
    }
}
