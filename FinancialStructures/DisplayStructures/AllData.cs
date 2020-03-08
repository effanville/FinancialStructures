using FinancialStructures.Database;
using FinancialStructures.FinanceStructures;
using FinancialStructures.GUIFinanceStructures;
using FinancialStructures.PortfolioAPI;
using SectorHelperFunctions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SavingClasses
{
    public class AllData
    {
        public Portfolio MyFunds { get; set; } = new Portfolio();

        public List<Sector> myBenchMarks { get; set; } = new List<Sector>();

        public static event EventHandler portfolioChanged;

        protected void OnPortfolioChanged(EventArgs e)
        {
            EventHandler handler = portfolioChanged;
            if (handler != null)
            {
                handler?.Invoke(this, e);
            }
        }

        public AllData()
        {
            bankAccEditMethods = new EditMethods(
               (portfolio, sectors, name, reportUpdate) => DataUpdater.DownloadBankAccount(portfolio, name, reportUpdate),
               (portfolio, sectors) => portfolio.GetBankAccountNamesAndCompanies(),
               (portfolio, sectors, name, reports) => portfolio.TryAdd(PortfolioElementType.BankAccount, name, reports),
               (portfolio, sectors, oldName, newName, reports) => portfolio.TryEditName(PortfolioElementType.BankAccount, oldName, newName, reports),
               (portfolio, sectors, name, reports) => portfolio.TryRemoveBankAccount(name, reports),
               (portfolio, sectors, name, reports) => portfolio.BankAccountData(name, reports),
               (portfolio, sectors, name, data, reports) => portfolio.TryAddDataToBankAccount(name, data, reports),
               (portfolio, sectors, name, oldData, newData, reports) => portfolio.TryEditBankAccount(name, oldData, newData, reports),
               (portfolio, sectors, name, data, reports) => portfolio.TryDeleteBankAccountData(name, data, reports));
            sectorEditMethods = new EditMethods(
    (portfolio, sectors, name, reportUpdate) => DataUpdater.DownloadSector(sectors, name, reportUpdate),
    (portfolio, sectors) => sectors.Select(sector => new NameData(sector.GetName(), string.Empty, portfolio.NumberSecuritiesInSector(sector.GetName()).ToString(), sector.GetUrl(), false)).ToList(),
    (portfolio, sectors, name, reports) => SectorEditor.TryAddSector(sectors, name, reports),
    (portfolio, sectors, oldName, newName, reports) => SectorEditor.TryEditSectorName(sectors, oldName, newName, reports),
    (portfolio, sectors, name, reports) => SectorEditor.TryDeleteSector(sectors, name, reports),
    (portfolio, sectors, name, reports) => SectorEditor.SectorData(sectors, name, reports),
    (portfolio, sectors, name, data, reports) => SectorEditor.TryAddDataToSector(sectors, name, data, reports),
    (portfolio, sectors, name, oldData, newData, reports) => SectorEditor.TryEditSector(sectors, name, oldData, newData, reports),
    (portfolio, sectors, name, data, reports) => SectorEditor.TryDeleteSectorData(sectors, name, data, reports));
            currencyEditMethods = new EditMethods(
    (portfolio, sectors, name, reportUpdate) => DataUpdater.DownloadCurrency(portfolio, name, reportUpdate),
    (portfolio, sectors) => portfolio.GetCurrencyNames(),
    (portfolio, sectors, name, reports) => portfolio.TryAdd(PortfolioElementType.Currency, name, reports),
    (portfolio, sectors, oldName, newName, reports) => portfolio.TryEditName(PortfolioElementType.Currency, oldName, newName, reports),
    (portfolio, sectors, name, reports) => portfolio.TryDeleteCurrency(name, reports),
    (portfolio, sectors, name, reports) => portfolio.CurrencyData(name, reports),
    (portfolio, sectors, name, data, reports) => portfolio.TryAddDataToCurrency(name, data, reports),
    (portfolio, sectors, name, oldData, newData, reports) => portfolio.TryEditCurrency(name, oldData, newData, reports),
    (portfolio, sectors, name, data, reports) => portfolio.TryDeleteCurrencyData(name, data, reports));
        }

        public AllData(Portfolio portfo, List<Sector> fSectors)
        {
            MyFunds = portfo;
            myBenchMarks = fSectors;
            
            bankAccEditMethods = new EditMethods(
                (portfolio, sectors, name, reportUpdate) => DataUpdater.DownloadBankAccount(portfolio, name, reportUpdate),
                (portfolio, sectors) => portfolio.GetBankAccountNamesAndCompanies(),
                (portfolio, sectors, name, reports) => portfolio.TryAdd(PortfolioElementType.BankAccount, name, reports),
                (portfolio, sectors, oldName, newName, reports) => portfolio.TryEditName(PortfolioElementType.BankAccount, oldName, newName, reports),
                (portfolio, sectors, name, reports) => portfolio.TryRemoveBankAccount(name, reports),
                (portfolio, sectors, name, reports) => portfolio.BankAccountData(name, reports),
                (portfolio, sectors, name, data, reports) => portfolio.TryAddDataToBankAccount(name, data, reports),
                (portfolio, sectors, name, oldData, newData, reports) => portfolio.TryEditBankAccount(name, oldData, newData, reports),
                (portfolio, sectors, name, data, reports) => portfolio.TryDeleteBankAccountData(name, data, reports));
            
            sectorEditMethods = new EditMethods(
                (portfolio, sectors, name, reportUpdate) => DataUpdater.DownloadSector(sectors, name, reportUpdate),
                (portfolio, sectors) => sectors.Select(sector => new NameData(sector.GetName(), string.Empty, portfolio.NumberSecuritiesInSector(sector.GetName()).ToString(), sector.GetUrl(), false)).ToList(),
                (portfolio, sectors, name, reports) => SectorEditor.TryAddSector(sectors, name, reports),
                (portfolio, sectors, oldName, newName, reports) => SectorEditor.TryEditSectorName(sectors, oldName, newName, reports),
                (portfolio, sectors, name, reports) => SectorEditor.TryDeleteSector(sectors, name, reports),
                (portfolio, sectors, name, reports) => SectorEditor.SectorData(sectors, name, reports),
                (portfolio, sectors, name, data, reports) => SectorEditor.TryAddDataToSector(sectors, name, data, reports),
                (portfolio, sectors, name, oldData, newData, reports) => SectorEditor.TryEditSector(sectors, name, oldData, newData, reports),
                (portfolio, sectors, name, data, reports) => SectorEditor.TryDeleteSectorData(sectors, name, data, reports));
            
            currencyEditMethods = new EditMethods(
                (portfolio, sectors, name, reportUpdate) => DataUpdater.DownloadCurrency(portfolio, name, reportUpdate),
                (portfolio, sectors) => portfolio.GetCurrencyNames(),
                (portfolio, sectors, name, reports) => portfolio.TryAdd(PortfolioElementType.Currency, name, reports),
                (portfolio, sectors, oldName, newName, reports) => portfolio.TryEditName(PortfolioElementType.Currency, oldName, newName, reports),
                (portfolio, sectors, name, reports) => portfolio.TryDeleteCurrency(name, reports),
                (portfolio, sectors, name, reports) => portfolio.CurrencyData(name, reports),
                (portfolio, sectors, name, data, reports) => portfolio.TryAddDataToCurrency(name, data, reports),
                (portfolio, sectors, name, oldData, newData, reports) => portfolio.TryEditCurrency(name, oldData, newData, reports),
                (portfolio, sectors, name, data, reports) => portfolio.TryDeleteCurrencyData(name, data, reports));
        }


        public EditMethods bankAccEditMethods;

        public EditMethods sectorEditMethods;

        public EditMethods currencyEditMethods;
    }
}
