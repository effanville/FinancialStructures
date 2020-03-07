using FinancialStructures.Database;
using FinancialStructures.FinanceStructures;
using FinancialStructures.GUIFinanceStructures;
using FinanceCommonViewModels;
using SectorHelperFunctions;
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

        private List<NameData> fAccountNames;
        public List<NameData> AccountNames
        {
            get { return fAccountNames; }
            set { fAccountNames = value; OnPropertyChanged(); }
        }

        private List<NameData> fSectorNames;
        public List<NameData> SectorNames
        {
            get { return fSectorNames; }
            set { fSectorNames = value; OnPropertyChanged(); }
        }

        private List<NameData> fCurrencyNames;
        public List<NameData> CurrencyNames
        {
            get { return fCurrencyNames; }
            set { fCurrencyNames = value; OnPropertyChanged(); }
        }

        public BasicDataViewModel(Portfolio portfolio, List<Sector> sectors)
            : base("Database Overview")
        {
            UpdateData(portfolio, sectors);
        }

        public override void UpdateData(Portfolio portfolio, List<Sector> sectors)
        {
            FundNames = portfolio.SecurityNamesAndCompanies();
            FundNames.Sort();
            AccountNames = portfolio.GetBankAccountNamesAndCompanies();
            AccountNames.Sort();
            SectorNames = SectorEditor.GetSectorNames(sectors);
            SectorNames.Sort();
            CurrencyNames = portfolio.GetCurrencyNames();
            CurrencyNames.Sort();
        }
    }
}
