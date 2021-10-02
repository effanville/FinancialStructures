using System;
using System.Collections.Generic;
using FinancialStructures.FinanceStructures;
using FinancialStructures.NamingStructures;

namespace FinancialStructures.Database.Statistics
{
    /// <summary>
    /// Holds static extension classes generating statistics for the portfolio.
    /// </summary>
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
            switch (elementType)
            {
                case Totals.Security:
                {
                    return LatestDateOf(portfolio.FundsThreadSafe);
                }
                case Totals.SecurityCompany:
                {
                    return LatestDateOf(portfolio.CompanyAccounts(Account.Security, name.Company));
                }
                case Totals.Sector:
                case Totals.SecuritySector:
                {
                    return LatestDateOf(portfolio.SectorAccounts(Account.Security, name));
                }
                case Totals.BankAccount:
                {
                    return LatestDateOf(portfolio.BankAccountsThreadSafe);
                }
                case Totals.Benchmark:
                {
                    return LatestDateOf(portfolio.BenchMarksThreadSafe);
                }
                case Totals.Currency:
                {
                    return LatestDateOf(portfolio.CurrenciesThreadSafe);
                }
                case Totals.All:
                default:
                {
                    var earlySecurity = portfolio.LatestDate(Totals.Security);
                    var earlyBank = portfolio.LatestDate(Totals.BankAccount);
                    return earlySecurity > earlyBank ? earlySecurity : earlyBank;
                }
            }
        }

        private static DateTime LatestDateOf(IReadOnlyList<IValueList> accounts)
        {
            DateTime output = DateTime.MinValue;
            foreach (ISecurity sec in accounts)
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