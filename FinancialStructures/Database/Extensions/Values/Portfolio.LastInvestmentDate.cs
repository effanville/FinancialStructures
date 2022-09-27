using System;

using FinancialStructures.FinanceStructures;
using FinancialStructures.NamingStructures;

namespace FinancialStructures.Database.Extensions.Values
{
    /// <summary>
    /// Holds static extension classes generating values data for the portfolio.
    /// </summary>
    public static partial class Values
    {
        /// <summary>
        /// Returns the earliest date held in the portfolio.
        /// </summary>
        /// <param name="portfolio">The database to query</param>
        /// <param name="total">The type of element to search for. All searches for Bank accounts and securities.</param>
        /// <param name="name">An ancillary name to use in the case of Sectors</param>
        public static DateTime LastInvestmentDate(this IPortfolio portfolio, Totals total, TwoName name = null)
        {
            return portfolio.CalculateAggregateStatistic<ISecurity, DateTime>(
               total,
               name,
               (tot, n) => tot == Totals.Security
                   || tot == Totals.SecurityCompany
                   || tot == Totals.Sector
                   || tot == Totals.SecuritySector
                   || tot == Totals.All,
               DateTime.MinValue,
               valueList => Calculate(valueList),
               (date, otherDate) => otherDate > date ? otherDate : date);
            DateTime Calculate(ISecurity valueList)
            {
                ICurrency currency = portfolio.Currency(valueList);
                return valueList.LastInvestment(currency)?.Day ?? DateTime.MinValue;
            }
        }

        /// <summary>
        /// Returns the latest date held in the portfolio.
        /// </summary>
        /// <param name="portfolio">The database to query</param>
        /// <param name="account">The type of element to search for. All searches for Bank accounts and securities.</param>
        /// <param name="name">An ancillary name to use in the case of Sectors</param>
        public static DateTime LastInvestmentDate(this IPortfolio portfolio, Account account, TwoName name)
        {
            return portfolio.CalculateStatistic<ISecurity, DateTime>(
                account,
                name,
                (acc, n) => acc == Account.Security,
                security => Calculate(security));
            DateTime Calculate(ISecurity sec)
            {
                ICurrency currency = portfolio.Currency(sec);
                return sec.LastInvestment(currency)?.Day ?? DateTime.MinValue;
            }
        }
    }
}
