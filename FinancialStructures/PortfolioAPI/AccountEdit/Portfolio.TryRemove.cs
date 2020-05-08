using FinancialStructures.FinanceInterfaces;
using FinancialStructures.FinanceStructures;
using FinancialStructures.NamingStructures;
using FinancialStructures.Reporting;

namespace FinancialStructures.PortfolioAPI
{
    public static class PortfolioDeleteMethods
    {
        /// <summary>
        /// Removes the account from the database if it can.
        /// </summary>
        /// <param name="portfolio">The database to query.</param>
        /// <param name="elementType">The type of account to remove.</param>
        /// <param name="name">The name of the account to remove.</param>
        /// <param name="reportLogger">(optional) A report callback.</param>
        /// <returns>Success or failure.</returns>
        public static bool TryRemove(this IPortfolio portfolio, AccountType elementType, NameData name, IReportLogger reportLogger = null)
        {
            if (string.IsNullOrEmpty(name.Name) && string.IsNullOrEmpty(name.Company))
            {
                reportLogger?.Log(ReportSeverity.Critical, ReportType.Error, ReportLocation.DeletingData, $"Adding {elementType}: Company `{name.Company}' or name `{name.Name}' cannot both be empty.");
                return false;
            }

            switch (elementType)
            {
                case (AccountType.Security):
                    {
                        foreach (Security sec in portfolio.Funds)
                        {
                            if (name.IsEqualTo(sec.Names))
                            {
                                portfolio.Funds.Remove(sec);
                                reportLogger?.Log(ReportSeverity.Detailed, ReportType.Report, ReportLocation.DeletingData, $"Security `{name.Company}'-`{name}' removed from the database.");
                                return true;
                            }
                        }

                        break;
                    }
                case (AccountType.Currency):
                    {
                        foreach (Currency currency in portfolio.Currencies)
                        {
                            if (name.IsEqualTo(currency.Names))
                            {
                                reportLogger?.Log(ReportSeverity.Detailed, ReportType.Report, ReportLocation.DeletingData, $"Deleted sector {currency.Name}");
                                portfolio.Currencies.Remove(currency);
                                return true;
                            }
                        }

                        break;
                    }
                case (AccountType.BankAccount):
                    {
                        foreach (CashAccount acc in portfolio.BankAccounts)
                        {
                            if (name.IsEqualTo(acc.Names))
                            {
                                portfolio.BankAccounts.Remove(acc);
                                reportLogger?.Log(ReportSeverity.Detailed, ReportType.Report, ReportLocation.DeletingData, $"Deleting Bank Account: Deleted `{name.Company}'-`{name.Name}'.");
                                return true;
                            }
                        }

                        break;
                    }
                case (AccountType.Sector):
                    {
                        foreach (Sector sector in portfolio.BenchMarks)
                        {
                            if (name.IsEqualTo(sector.Names))
                            {
                                reportLogger?.Log(ReportSeverity.Detailed, ReportType.Report, ReportLocation.DeletingData, $"Deleted sector {sector.Name}");
                                portfolio.BenchMarks.Remove(sector);
                                return true;
                            }
                        }

                        break;
                    }
                default:
                    reportLogger?.LogUseful(ReportType.Error, ReportLocation.DeletingData, $"Editing an Unknown type.");
                    return false;
            }

            reportLogger?.LogUseful(ReportType.Error, ReportLocation.AddingData, $"{elementType.ToString()} - {name.ToString()} could not be found in the database.");
            return false;
        }
    }
}
