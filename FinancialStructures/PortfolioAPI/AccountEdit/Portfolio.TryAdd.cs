using FinancialStructures.Database;
using FinancialStructures.FinanceStructures;
using FinancialStructures.NamingStructures;
using FinancialStructures.ReportLogging;

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
                        Security newSecurity = new Security(name.Company, name.Name, name.Currency, name.Url, name.Sectors);
                        portfolio.Funds.Add(newSecurity);
                        break;
                    }
                case (AccountType.Currency):
                    {
                        if (string.IsNullOrEmpty(name.Company))
                        {
                            name.Company = "GBP";
                        }
                        Currency newSector = new Currency(name);
                        portfolio.Currencies.Add(newSector);
                        break;
                    }
                case (AccountType.BankAccount):
                    {
                        var NewAccount = new CashAccount(name);
                        foreach (var sector in name.Sectors)
                        {
                            NewAccount.TryAddSector(sector);
                        }

                        portfolio.BankAccounts.Add(NewAccount);
                        break;
                    }
                case (AccountType.Sector):
                    {
                        Sector newSector = new Sector(name);
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
