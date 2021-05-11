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
        public static DateTime LatestDate(this IPortfolio portfolio, Totals elementType, string name = null)
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
                    foreach (ISecurity sec in portfolio.CompanySecurities(name))
                    {
                        if (sec.LatestValue().Day > output)
                        {
                            output = sec.LatestValue().Day;
                        }
                    }

                    return output;
                }
                case Totals.Sector:
                {
                    foreach (ISecurity sec in portfolio.SectorSecurities(name))
                    {
                        if (sec.LatestValue().Day > output)
                        {
                            output = sec.LatestValue().Day;
                        }
                    }
                    break;
                }
                case Totals.BankAccount:
                {
                    foreach (ICashAccount cashAccount in portfolio.BankAccountsThreadSafe)
                    {
                        if (cashAccount.LatestValue().Day > output)
                        {
                            output = cashAccount.LatestValue().Day;
                        }
                    }

                    break;
                }
                case Totals.Benchmark:
                {
                    foreach (ICashAccount cashAccount in portfolio.BenchMarksThreadSafe)
                    {
                        if (cashAccount.LatestValue().Day > output)
                        {
                            output = cashAccount.LatestValue().Day;
                        }
                    }

                    break;
                }
                case Totals.Currency:
                {
                    foreach (ICashAccount cashAccount in portfolio.CurrenciesThreadSafe)
                    {
                        if (cashAccount.LatestValue().Day > output)
                        {
                            output = cashAccount.LatestValue().Day;
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