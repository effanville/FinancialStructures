using FinancialStructures.Database;
using FinancialStructures.FinanceStructures;
using FinancialStructures.GUIFinanceStructures;
using System;
using System.Collections.Generic;

namespace FinancialStructures.PortfolioAPI
{
    public static class PortfolioAddMethods
    {
        /// <summary>
        /// Adds data to the portfolio, unless data already exists.
        /// </summary>
        /// <param name="portfolio">The database to add to.</param>
        /// <param name="elementType">The type of data to add.</param>
        /// <param name="name">The name data to add.</param>
        /// <param name="reportLogger">Report callback action.</param>
        /// <returns>Success or failure of adding.</returns>
        public static bool TryAdd(this Portfolio portfolio, PortfolioElementType elementType, NameData name, Action<string, string, string> reportLogger)
        {
            List<string> sectorList = new List<string>();
            if (!string.IsNullOrEmpty(name.Sectors))
            {
                var sectorsSplit = name.Sectors.Split(',');

                sectorList.AddRange(sectorsSplit);
                for (int i = 0; i < sectorList.Count; i++)
                {
                    sectorList[i] = sectorList[i].Trim(' ');
                }
            }

            if (string.IsNullOrEmpty(name.Name) && string.IsNullOrEmpty(name.Company))
            {
                reportLogger("Error", "AddingData", $"Adding {elementType}: Company `{name.Company}' or name `{name.Name}' cannot both be empty.");
                return false;
            }

            if (portfolio.Exists(elementType, name, reportLogger))
            {
                reportLogger("Error", "AddingData", $"{elementType.ToString()} `{name.Company}'-`{name.Name}' already exists.");
                return false;
            }

            switch (elementType)
            {
                case (PortfolioElementType.Security):
                    {
                        Security newSecurity = new Security(name.Company, name.Name, name.Currency, name.Url, sectorList);
                        portfolio.Funds.Add(newSecurity);
                        reportLogger("Report", "AddingData", $"Security `{name.Company}'-`{name.Name}' added to database.");
                        return true;
                    }
                case (PortfolioElementType.Currency):
                    {
                        Currency newSector = new Currency(name.Name, name.Url);
                        reportLogger("Report", "AddingData", $"Currency '{name}' added to database.");
                        portfolio.Currencies.Add(newSector);
                        return true;
                    }
                case (PortfolioElementType.BankAccount):
                    {
                        var NewAccount = new CashAccount(name.Company, name.Name, name.Currency);
                        foreach (var sector in sectorList)
                        {
                            NewAccount.TryAddSector(sector);
                        }

                        portfolio.BankAccounts.Add(NewAccount);
                        reportLogger("Report", "AddingData", $"BankAccount `{name.Company}'-`{name.Name}' added to database.");
                        return true;
                    }
                case (PortfolioElementType.Sector):
                default:
                    reportLogger("Error", "EditingData", $"Editing an Unknown type.");
                    return false;
            }
        }
    }
}
