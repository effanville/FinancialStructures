using FinancialStructures.ReportingStructures;
using FinancialStructures.Database;
using GUISupport;
using System;
using System.Linq;
using FinanceCommonViewModels;
using FinancialStructures.GUIFinanceStructures;
using SectorHelperFunctions;
using System.Collections.Generic;
using FinancialStructures.FinanceStructures;

namespace FinanceWindowsViewModels
{
    public class MainWindowViewModel : PropertyChangedBase
    {
        public Portfolio portfolio = new Portfolio();

        public List<Sector> benchMarks = new List<Sector>();

        public MainWindowViewModel()
        {
            OptionsToolbarCommands = new OptionsToolbarViewModel(portfolio, benchMarks, UpdateWindow, UpdateReports);
            DataView = new BasicDataViewModel(portfolio, benchMarks);
            SecurityEditViewModel = new SecurityEditWindowViewModel(portfolio, benchMarks, UpdateWindow, UpdateReports);
            BankAccEditViewModel = new SingleValueEditWindowViewModel(portfolio, benchMarks, UpdateWindow, UpdateReports, bankAccEditMethods);
            SectorEditViewModel = new SingleValueEditWindowViewModel(portfolio, benchMarks, UpdateWindow, UpdateReports, sectorEditMethods);
            CurrencyEditViewModel = new SingleValueEditWindowViewModel(portfolio, benchMarks, UpdateWindow, UpdateReports, currencyEditMethods);
            StatsEditViewModel = new StatsCreatorWindowViewModel(portfolio, benchMarks, UpdateWindow, UpdateReports);
            ReportsViewModel = new ReportingWindowViewModel();
        }

        private EditMethods bankAccEditMethods = new EditMethods(
            (portfolio, sectors, name, reportUpdate, reports) => DataUpdater.DownloadBankAccount(portfolio, name, reportUpdate, reports), 
            (portfolio, sectors) => portfolio.GetBankAccountNamesAndCompanies(), 
            (portfolio, sectors, name, reports) => portfolio.TryAddBankAccount(name, reports), 
            (portfolio, sectors, oldName, newName, reports) => portfolio.TryEditBankAccountName(oldName, newName, reports), 
            (portfolio, sectors, name, reports) => portfolio.TryRemoveBankAccount(name, reports),
            (portfolio, sectors, name, reports) => portfolio.BankAccountData(name, reports),
            (portfolio, sectors, name, data, reports) => portfolio.TryAddDataToBankAccount(name, data, reports),
            (portfolio, sectors, name, oldData, newData, reports) => portfolio.TryEditBankAccount(name, oldData, newData, reports),
            (portfolio, sectors, name, data, reports) => portfolio.TryDeleteBankAccountData(name, data, reports));

        private EditMethods sectorEditMethods = new EditMethods(
    (portfolio, sectors, name, reportUpdate, reports) => DataUpdater.DownloadSector(sectors, name, reportUpdate, reports),
    (portfolio, sectors) => sectors.Select(sector => new NameData(sector.GetName(), string.Empty, string.Empty, sector.GetUrl(), false)).ToList(),
    (portfolio, sectors, name, reports) => SectorEditor.TryAddSector(sectors, name, reports),
    (portfolio, sectors, oldName, newName, reports) => SectorEditor.TryEditSectorName(sectors, oldName, newName, reports),
    (portfolio, sectors, name, reports) => SectorEditor.TryDeleteSector(sectors, name, reports),
    (portfolio, sectors, name, reports) => SectorEditor.SectorData(sectors, name, reports),
    (portfolio, sectors, name, data, reports) => SectorEditor.TryAddDataToSector(sectors, name, data, reports),
    (portfolio, sectors, name, oldData, newData, reports) => SectorEditor.TryEditSector(sectors, name, oldData, newData, reports),
    (portfolio, sectors, name, data, reports) => SectorEditor.TryDeleteSectorData(sectors, name, data, reports));

        private EditMethods currencyEditMethods = new EditMethods(
    (portfolio, sectors, name, reportUpdate, reports) => DataUpdater.DownloadCurrency(portfolio, name, reportUpdate, reports),
    (portfolio, sectors) => portfolio.GetCurrencyNames(),
    (portfolio, sectors, name, reports) => portfolio.TryAddCurrency(name, reports),
    (portfolio, sectors, oldName, newName, reports) => portfolio.TryEditCurrencyName(oldName, newName, reports),
    (portfolio, sectors, name, reports) => portfolio.TryDeleteCurrency(name, reports),
    (portfolio, sectors, name, reports) => portfolio.CurrencyData(name, reports),
    (portfolio, sectors, name, data, reports) => portfolio.TryAddDataToCurrency(name, data, reports),
    (portfolio, sectors, name, oldData, newData, reports) => portfolio.TryEditCurrency(name, oldData, newData, reports),
    (portfolio, sectors, name, data, reports) => portfolio.TryDeleteCurrencyData(name, data, reports));

        Action<ErrorReports> UpdateReports => (val) => AddReports(val);

        public void AddReports(ErrorReports reports)
        {
            ReportsViewModel.UpdateReports(reports);
        }

        Action UpdateWindow => () => UpdateViews(true);

        Action<Action<Portfolio, List<Sector>>> UpdateDataView => (updateAction) => UpdateData(updateAction);

        public void UpdateViews(object obj)
        {
            if (obj is bool updateReportsOnly)

            {
                DataView.DataUpdate(portfolio, benchMarks);
                SecurityEditViewModel.UpdateListBoxes(portfolio, benchMarks);
                BankAccEditViewModel.UpdateListBoxes(portfolio, benchMarks);
                SectorEditViewModel.UpdateListBoxes(portfolio, benchMarks);

                CurrencyEditViewModel.UpdateListBoxes(portfolio, benchMarks);
                StatsEditViewModel.GenerateStatistics(portfolio, benchMarks);
            }
        }

        public void UpdateData(object obj)
        {
            if(obj is Action <Portfolio, List<Sector>> updateAction)
            {
                updateAction(portfolio, benchMarks);
            }
        }

        private OptionsToolbarViewModel fOptionsToolbarCommands;

        public OptionsToolbarViewModel OptionsToolbarCommands
        {
            get { return fOptionsToolbarCommands; }
            set { fOptionsToolbarCommands = value; OnPropertyChanged(); }
        }

        private BasicDataViewModel fDataView;

        public BasicDataViewModel DataView
        {
            get { return fDataView; }
            set { fDataView = value; OnPropertyChanged(); }
        }

        private SecurityEditWindowViewModel fSecurityEditViewModel;
        public SecurityEditWindowViewModel SecurityEditViewModel
        {
            get { return fSecurityEditViewModel; }
            set { fSecurityEditViewModel = value; OnPropertyChanged(); }
        }


        private SingleValueEditWindowViewModel fSectorEditViewModel;

        public SingleValueEditWindowViewModel SectorEditViewModel
        {
            get { return fSectorEditViewModel; }
            set { fSectorEditViewModel = value; OnPropertyChanged(); }
        }

        private SingleValueEditWindowViewModel fCurrencyEditViewModel;

        public SingleValueEditWindowViewModel CurrencyEditViewModel
        {
            get { return fCurrencyEditViewModel; }
            set { fCurrencyEditViewModel = value; OnPropertyChanged(); }
        }

        private SingleValueEditWindowViewModel fBankAccEditViewModel;

        public SingleValueEditWindowViewModel BankAccEditViewModel
        {
            get { return fBankAccEditViewModel; }
            set { fBankAccEditViewModel = value; OnPropertyChanged(); }
        }

        private StatsCreatorWindowViewModel fStatsViewModel;

        public StatsCreatorWindowViewModel StatsEditViewModel
        {
            get { return fStatsViewModel; }
            set { fStatsViewModel = value; OnPropertyChanged(); }
        }

        private ReportingWindowViewModel fReports;
        public ReportingWindowViewModel ReportsViewModel
        {
            get { return fReports; }
            set { fReports = value; OnPropertyChanged(); }
        }
    }
}
