using System;
using FinancialStructures.FinanceInterfaces;
using FinancialStructures.FinanceStructures;
using FinancialStructures.NamingStructures;
using StructureCommon.Reporting;

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
        public static bool TryAdd(this IPortfolio portfolio, AccountType elementType, NameData name, IReportLogger reportLogger = null)
        {
            if (string.IsNullOrEmpty(name.Name) && string.IsNullOrEmpty(name.Company))
            {
                _ = reportLogger?.Log(ReportSeverity.Critical, ReportType.Error, ReportLocation.AddingData, $"Adding {elementType}: Company '{name.Company}' and name '{name.Name}' cannot both be empty.");
                return false;
            }

            if (portfolio.Exists(elementType, name))
            {
                _ = reportLogger?.Log(ReportSeverity.Critical, ReportType.Error, ReportLocation.AddingData, $"{elementType} `{name.Company}'-`{name.Name}' already exists.");
                portfolio.OnPortfolioChanged(null, new EventArgs());
                return false;
            }

            switch (elementType)
            {
                case (AccountType.Security):
                {
                    Security toAdd = new Security(name);
                    toAdd.DataEdit += portfolio.OnPortfolioChanged;
                    portfolio.Funds.Add(toAdd);
                    portfolio.OnPortfolioChanged(toAdd, new EventArgs());
                    break;
                }
                case (AccountType.Currency):
                {
                    if (string.IsNullOrEmpty(name.Company))
                    {
                        name.Company = "GBP";
                    }
                    Currency toAdd = new Currency(name);
                    toAdd.DataEdit += portfolio.OnPortfolioChanged;
                    portfolio.Currencies.Add(toAdd);
                    portfolio.OnPortfolioChanged(toAdd, new EventArgs());
                    break;
                }
                case (AccountType.BankAccount):
                {
                    CashAccount toAdd = new CashAccount(name);
                    toAdd.DataEdit += portfolio.OnPortfolioChanged;
                    portfolio.BankAccounts.Add(toAdd);
                    portfolio.OnPortfolioChanged(toAdd, new EventArgs());
                    break;
                }
                case (AccountType.Sector):
                {
                    Sector toAdd = new Sector(name);
                    toAdd.DataEdit += portfolio.OnPortfolioChanged;
                    portfolio.BenchMarks.Add(toAdd);
                    portfolio.OnPortfolioChanged(toAdd, new EventArgs());
                    break;
                }
                default:
                    _ = reportLogger?.LogUseful(ReportType.Error, ReportLocation.EditingData, $"Editing an Unknown type.");
                    return false;
            }

            _ = reportLogger?.Log(ReportSeverity.Detailed, ReportType.Report, ReportLocation.AddingData, $"{elementType} `{name.Company}'-`{name.Name}' added to database.");
            return true;
        }
    }
}
