using FinancialStructures.Database;
using FinancialStructures.FinanceStructures;
using FinancialStructures.GUIFinanceStructures;
using GUISupport;
using SectorHelperFunctions;
using System.Collections.Generic;

namespace FinanceWindowsViewModels
{
    internal class BasicDataViewModel : PropertyChangedBase
    {
        public string TitleText {get;set;} = "Database Overview"; 
        private Portfolio Portfolio;
        private List<Sector> Sectors;

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

        public void DataUpdate(Portfolio portfolio, List<Sector> sectors)
        {
            Portfolio = portfolio;
            Sectors = sectors;
            FundNames = Portfolio.SecurityNamesAndCompanies();
            FundNames.Sort();
            AccountNames = Portfolio.GetBankAccountNamesAndCompanies();
            AccountNames.Sort();
            SectorNames = SectorEditor.GetSectorNames(sectors);
            SectorNames.Sort();
            CurrencyNames = Portfolio.GetCurrencyNames();
            CurrencyNames.Sort();
        }

        public BasicDataViewModel(Portfolio portfolio, List<Sector> sectors)
        {
            DataUpdate(portfolio, sectors);
        }
    }
}
