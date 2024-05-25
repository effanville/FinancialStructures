using System;

using Effanville.FinancialStructures.FinanceStructures;
using Effanville.FinancialStructures.NamingStructures;

namespace Effanville.FinancialStructures.Database.Extensions.Values
{
    /// <summary>
    /// Holds static extension classes generating values data for the portfolio.
    /// </summary>
    public static partial class Values
    {
        /// <summary>
        /// Returns the latest date held in the portfolio.
        /// </summary>
        /// <param name="portfolio">The database to query</param>
        /// <param name="total">The type of element to search for. All searches for Bank accounts and securities.</param>
        /// <param name="name">An ancillary name to use in the case of Sectors</param>
        /// <returns></returns>
        public static DateTime LatestDate(this IPortfolio portfolio, Totals total, TwoName name = null)
        {
            return portfolio.CalculateAggregateValue(
                total,
                name,
                DateTime.MinValue,
                (newStat, previousStat) => newStat > previousStat ? newStat : previousStat,
                valueList => valueList.LatestDate());
        }

        /// <summary>
        /// Returns the latest date held in the portfolio.
        /// </summary>
        /// <param name="portfolio">The database to query</param>
        /// <param name="account">The type of element to search for. All searches for Bank accounts and securities.</param>
        /// <param name="name">An ancillary name to use in the case of Sectors</param>
        /// <returns></returns>
        public static DateTime LatestDate(this IPortfolio portfolio, Account account, TwoName name)
        {
            return portfolio.CalculateValue(
                account,
                name,
                valueList => valueList.LatestDate());
        }
    }
}
