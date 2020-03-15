using FinancialStructures.Database;
using FinancialStructures.FinanceStructures;
using FinancialStructures.GUIFinanceStructures;
using FinancialStructures.ReportLogging;
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
        public static bool TryAdd(this Portfolio portfolio, AccountType elementType, NameData name, LogReporter reportLogger)
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
                reportLogger.LogDetailed("Critical", "Error", "AddingData", $"Adding {elementType}: Company `{name.Company}' or name `{name.Name}' cannot both be empty.");
                return false;
            }

            if (portfolio.Exists(elementType, name, reportLogger.Log))
            {
                reportLogger.LogDetailed("Critical", "Error", "AddingData", $"{elementType.ToString()} `{name.Company}'-`{name.Name}' already exists.");
                return false;
            }

            switch (elementType)
            {
                case (AccountType.Security):
                    {
                        Security newSecurity = new Security(name.Company, name.Name, name.Currency, name.Url, sectorList);
                        portfolio.Funds.Add(newSecurity);
                        break;
                    }
                case (AccountType.Currency):
                    {
                        Currency newSector = new Currency(name.Name, name.Url);
                        portfolio.Currencies.Add(newSector);
                        break;
                    }
                case (AccountType.BankAccount):
                    {
                        var NewAccount = new CashAccount(name.Company, name.Name, name.Currency);
                        foreach (var sector in sectorList)
                        {
                            NewAccount.TryAddSector(sector);
                        }

                        portfolio.BankAccounts.Add(NewAccount);
                        break;
                    }
                case (AccountType.Sector):
                    {
                        Sector newSector = new Sector(name.Name, name.Url);
                        portfolio.BenchMarks.Add(newSector);
                        break;
                    }
                default:
                    reportLogger.Log("Error", "EditingData", $"Editing an Unknown type.");
                    return false;
            }

            reportLogger.LogDetailed("Detailed", "Report", "AddingData", $"{elementType.ToString()} `{name.Company}'-`{name.Name}' added to database.");
            return true;
        }
    }
}
