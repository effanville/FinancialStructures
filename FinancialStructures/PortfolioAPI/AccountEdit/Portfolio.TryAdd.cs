using FinancialStructures.FinanceInterfaces;
using FinancialStructures.FinanceStructures;
using FinancialStructures.NamingStructures;
using FinancialStructures.ReportLogging;

namespace FinancialStructures.PortfolioAPI
{
    /// <summary>
    /// Collection of methods to add to a database.
    /// </summary>
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
        public static bool TryAdd(this IPortfolio portfolio, AccountType elementType, NameData name, LogReporter reportLogger)
        {
            if (string.IsNullOrEmpty(name.Name) && string.IsNullOrEmpty(name.Company))
            {
                reportLogger.LogDetailed("Critical", "Error", "AddingData", $"Adding {elementType}: Company `{name.Company}' or name `{name.Name}' cannot both be empty.");
                return false;
            }

            if (portfolio.Exists(elementType, name))
            {
                reportLogger.LogDetailed("Critical", "Error", "AddingData", $"{elementType.ToString()} `{name.Company}'-`{name.Name}' already exists.");
                return false;
            }

            switch (elementType)
            {
                case (AccountType.Security):
                    {
                        portfolio.Funds.Add(new Security(name));
                        break;
                    }
                case (AccountType.Currency):
                    {
                        if (string.IsNullOrEmpty(name.Company))
                        {
                            name.Company = "GBP";
                        }
                        portfolio.Currencies.Add(new Currency(name));
                        break;
                    }
                case (AccountType.BankAccount):
                    {
                        portfolio.BankAccounts.Add(new CashAccount(name));
                        break;
                    }
                case (AccountType.Sector):
                    {
                        portfolio.BenchMarks.Add(new Sector(name));
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
