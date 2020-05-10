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
                _ = reportLogger?.Log(ReportSeverity.Critical, ReportType.Error, ReportLocation.DeletingData, $"Adding {elementType}: Company `{name.Company}' or name `{name.Name}' cannot both be empty.");
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
                                _ = portfolio.Funds.Remove(sec);
                                _ = reportLogger?.Log(ReportSeverity.Detailed, ReportType.Report, ReportLocation.DeletingData, $"Security `{name.Company}'-`{name}' removed from the database.");
                                portfolio.OnPortfolioChanged(portfolio.Funds, new System.EventArgs());
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
                                _ = reportLogger?.Log(ReportSeverity.Detailed, ReportType.Report, ReportLocation.DeletingData, $"Deleted sector {currency.Name}");
                                _ = portfolio.Currencies.Remove(currency);
                                portfolio.OnPortfolioChanged(portfolio.Currencies, new System.EventArgs());
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
                                _ = portfolio.BankAccounts.Remove(acc);
                                _ = reportLogger?.Log(ReportSeverity.Detailed, ReportType.Report, ReportLocation.DeletingData, $"Deleting Bank Account: Deleted `{name.Company}'-`{name.Name}'.");
                                portfolio.OnPortfolioChanged(portfolio.BankAccounts, new System.EventArgs());
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
                                _ = reportLogger?.Log(ReportSeverity.Detailed, ReportType.Report, ReportLocation.DeletingData, $"Deleted sector {sector.Name}");
                                _ = portfolio.BenchMarks.Remove(sector);
                                portfolio.OnPortfolioChanged(portfolio.BenchMarks, new System.EventArgs());
                                return true;
                            }
                        }

                        break;
                    }
                default:
                    _ = reportLogger?.LogUseful(ReportType.Error, ReportLocation.DeletingData, $"Editing an Unknown type.");
                    return false;
            }

            _ = reportLogger?.LogUseful(ReportType.Error, ReportLocation.AddingData, $"{elementType} - {name} could not be found in the database.");
            return false;
        }
    }
}
