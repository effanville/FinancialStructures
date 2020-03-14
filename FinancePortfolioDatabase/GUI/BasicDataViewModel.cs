using FinancialStructures.Database;
using FinancialStructures.FinanceStructures;
using FinancialStructures.GUIFinanceStructures;
using FinanceCommonViewModels;
using SectorHelperFunctions;
using System.Collections.Generic;
using FinancialStructures.PortfolioAPI;

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

        private List<NameData> fSectorNames;
        public List<NameData> SectorNames
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

        public BasicDataViewModel(Portfolio portfolio, List<Sector> sectors)
            : base("Database Overview")
        {
            UpdateData(portfolio, sectors);
        }

        public override void UpdateData(Portfolio portfolio, List<Sector> sectors)
        {
            FundNames = portfolio.NameData(PortfolioElementType.Security, null);
            FundNames.Sort();
            AccountNames = portfolio.NameData(PortfolioElementType.BankAccount, null);
            AccountNames.Sort();
            SectorNames = SectorEditor.GetSectorNames(sectors);
            SectorNames.Sort();
            CurrencyNames = portfolio.NameData(PortfolioElementType.Currency, null);
            CurrencyNames.Sort();
        }
    }
}
