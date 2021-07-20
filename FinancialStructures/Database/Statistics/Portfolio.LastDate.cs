using System;
using FinancialStructures.FinanceStructures;
using FinancialStructures.NamingStructures;

namespace FinancialStructures.Database.Statistics
{
    public static partial class PortfolioStatisticGenerators
    {
        /// <summary>
        /// Returns the latest date held in the portfolio.
        /// </summary>
        /// <param name="portfolio">The database to query</param>
        /// <param name="elementType">The type of element to search for. All searches for Bank accounts and securities.</param>
        /// <param name="name">An ancillary name to use in the case of Sectors</param>
        /// <returns></returns>
        public static DateTime LatestDate(this IPortfolio portfolio, Totals elementType, TwoName name = null)
        {
            DateTime output = DateTime.MinValue;
            switch (elementType)
            {
                case Totals.Security:
                {
                    foreach (ISecurity sec in portfolio.FundsThreadSafe)
                    {
                        if (sec.Any())
                        {
                            DateTime securityEarliest = sec.LatestValue().Day;
                            if (securityEarliest > output)
                            {
                                output = securityEarliest;
                            }
                        }
                    }

                    break;
                }
                case Totals.SecurityCompany:
                {
                    foreach (ISecurity sec in portfolio.CompanyAccounts(Account.Security, name.Company))
                    {
                        if (sec.Any() && sec.LatestValue().Day > output)
                        {
                            output = sec.LatestValue().Day;
                        }
                    }

                    break;
                }
                case Totals.Sector:
                case Totals.SecuritySector:
                {
                    foreach (ISecurity sector in portfolio.SectorAccounts(Account.Security, name.Name))
                    {
                        if (sector.Any() && sector.LatestValue().Day > output)
                        {
                            output = sector.LatestValue().Day;
                        }
                    }

                    break;
                }
                case Totals.BankAccount:
                {
                    foreach (ICashAccount cashAccount in portfolio.BankAccountsThreadSafe)
                    {
                        if (cashAccount.Any() && cashAccount.LatestValue().Day > output)
                        {
                            output = cashAccount.LatestValue().Day;
                        }
                    }

                    break;
                }
                case Totals.Benchmark:
                {
                    foreach (IValueList benchmark in portfolio.BenchMarksThreadSafe)
                    {
                        if (benchmark.Any() && benchmark.LatestValue().Day > output)
                        {
                            output = benchmark.LatestValue().Day;
                        }
                    }

                    break;
                }
                case Totals.Currency:
                {
                    foreach (ICashAccount currency in portfolio.CurrenciesThreadSafe)
                    {
                        if (currency.Any() && currency.LatestValue().Day > output)
                        {
                            output = currency.LatestValue().Day;
                        }
                    }

                    break;
                }
                case Totals.All:
                {
                    var earlySecurity = portfolio.LatestDate(Totals.Security);
                    var earlyBank = portfolio.LatestDate(Totals.BankAccount);
                    output = earlySecurity > earlyBank ? earlySecurity : earlyBank;
                    break;
                }
            }

            return output;
        }

        /// <summary>
        /// Returns the latest date held in the portfolio.
        /// </summary>
        /// <param name="portfolio">The database to query</param>
        /// <param name="elementType">The type of element to search for. All searches for Bank accounts and securities.</param>
        /// <param name="name">An ancillary name to use in the case of Sectors</param>
        /// <returns></returns>
        public static DateTime LatestDate(this IPortfolio portfolio, Account elementType, TwoName name)
        {
            if (portfolio.TryGetAccount(elementType, name, out var desired))
            {
                return desired.LatestValue()?.Day ?? DateTime.MinValue;
            }

            return DateTime.MinValue;
        }
    }
}