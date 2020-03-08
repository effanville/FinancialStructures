using FinancialStructures.Database;
using FinancialStructures.GUIFinanceStructures;
using System;
using System.Collections.Generic;

namespace FinancialStructures.PortfolioAPI
{
    public static class PortfolioEditMethods
    {
        /// <summary>
        /// Edits the name of the data currently held.
        /// </summary>
        /// <param name="portfolio">The database to edit.</param>
        /// <param name="elementType">The type of data to edit.</param>
        /// <param name="oldName">The existing name of the data.</param>
        /// <param name="newName">The new name of the data.</param>
        /// <param name="reportLogger">Report callback.</param>
        /// <returns>Success or failure of editing.</returns>
        public static bool TryEditName(this Portfolio portfolio, PortfolioElementType elementType, NameData oldName, NameData newName, Action<string, string, string> reportLogger)
        {
            List<string> sectorList = new List<string>();
            if (!string.IsNullOrEmpty(newName.Sectors))
            {
                var sectorsSplit = newName.Sectors.Split(',');
                sectorList.AddRange(sectorsSplit);
                for (int i = 0; i < sectorList.Count; i++)
                {
                    sectorList[i] = sectorList[i].Trim(' ');
                }
            }

            switch (elementType)
            {
                case (PortfolioElementType.Security):
                    return portfolio.TryEditSecurityName(oldName.Company, oldName.Name, newName.Company, newName.Name, newName.Currency, newName.Url, sectorList, reportLogger);
                case (PortfolioElementType.Currency):
                    return portfolio.TryEditCurrencyName(oldName.Name, newName.Name, newName.Url, reportLogger);
                case (PortfolioElementType.BankAccount):
                    return portfolio.TryEditBankAcountName(oldName.Company, oldName.Name, newName.Company, newName.Name, newName.Currency, sectorList, reportLogger);
                case (PortfolioElementType.Sector):
                default:
                    reportLogger("Error", "EditingData", $"Editing an Unknown type.");
                    return false;
            }
        }

        private static bool TryEditSecurityName(this Portfolio portfolio, string company, string name, string newCompany, string newName, string currency, string url, List<string> sectors, Action<string, string, string> reportLogger)
        {
            for (int fundIndex = 0; fundIndex < portfolio.Funds.Count; fundIndex++)
            {
                if (portfolio.Funds[fundIndex].GetCompany() == company && portfolio.Funds[fundIndex].GetName() == name)
                {
                    // now edit data
                    return portfolio.Funds[fundIndex].EditNameData(newName, newCompany, currency, url, sectors, reportLogger);
                }
            }

            return false;
        }

        private static bool TryEditBankAcountName(this Portfolio portfolio, string company, string name, string newCompany, string newName, string currency, List<string> sectors, Action<string, string, string> reportLogger)
        {
            for (int AccountIndex = 0; AccountIndex < portfolio.Funds.Count; AccountIndex++)
            {
                if (portfolio.BankAccounts[AccountIndex].GetCompany() == company && portfolio.BankAccounts[AccountIndex].GetName() == name)
                {
                    // now edit data
                    return portfolio.BankAccounts[AccountIndex].EditNameData(newCompany, newName, string.Empty, currency, sectors);
                }
            }

            reportLogger("Error", "EditingData", $"Renaming BankAccount: Could not find bank account `{company}'-`{name}'.");
            return false;
        }

        private static bool TryEditCurrencyName(this Portfolio portfolio, string oldName, string newName, string url, Action<string, string, string> reportLogger)
        {
            foreach (var currency in portfolio.Currencies)
            {
                if (currency.GetName() == oldName)
                {
                    currency.EditNameData("", newName, url);
                    reportLogger("Report", "EditingData", $"Renamed sector {oldName} with new name {newName}.");
                    return true;
                }
            }

            reportLogger("Error", "EditingData", $"Could not rename sector {oldName} with new name {newName}.");
            return false;
        }
    }
}
