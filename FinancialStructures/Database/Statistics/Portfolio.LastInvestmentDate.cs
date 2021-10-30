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
        /// Returns the earliest date held in the portfolio.
        /// </summary>
        /// <param name="portfolio">The database to query</param>
        /// <param name="elementType">The type of element to search for. All searches for Bank accounts and securities.</param>
        /// <param name="name">An ancillary name to use in the case of Sectors</param>
        public static DateTime LastInvestmentDate(this IPortfolio portfolio, Totals elementType, TwoName name = null)
        {
            switch (elementType)
            {
                case Totals.Security:
                {
                    return LastInvestmentDateOf(portfolio.FundsThreadSafe, portfolio);
                }
                case Totals.SecurityCompany:
                {
                    return LastInvestmentDateOf(portfolio.CompanyAccounts(Account.Security, name.Company), portfolio);
                }
                case Totals.Sector:
                case Totals.SecuritySector:
                {
                    return LastInvestmentDateOf(portfolio.SectorAccounts(Account.Security, name), portfolio);
                }
                case Totals.BankAccount:
                case Totals.Benchmark:
                case Totals.Currency:
                {
                    return default(DateTime);
                }
                case Totals.All:
                default:
                {
                    DateTime earlySecurity = portfolio.LastInvestmentDate(Totals.Security);
                    return earlySecurity;
                }
            }
        }

        private static DateTime LastInvestmentDateOf(IReadOnlyList<IValueList> accounts, IPortfolio portfolio)
        {
            DateTime output = DateTime.MinValue;
            foreach (ISecurity valueList in accounts)
            {
                if (valueList.Any())
                {
                    ICurrency currency = portfolio.Currency(Account.Security, valueList);
                    DateTime latest = valueList.LastInvestment(currency).Day;
                    if (latest > output)
                    {
                        output = latest;
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
        public static DateTime LastInvestmentDate(this IPortfolio portfolio, Account elementType, TwoName name)
        {
            if (portfolio.TryGetAccount(elementType, name, out IValueList desired))
            {
                if (desired is ISecurity sec)
                {
                    ICurrency currency = portfolio.Currency(Account.Security, sec);
                    return sec.LastInvestment(currency)?.Day ?? DateTime.MinValue;
                }
            }

            return default(DateTime);
        }
    }
}