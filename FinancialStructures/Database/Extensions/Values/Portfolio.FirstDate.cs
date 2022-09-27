using System;

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
        public static DateTime FirstValueDate(this IPortfolio portfolio, Totals total, TwoName name = null)
        {
            return portfolio.CalculateAggregateStatistic(
                total,
                name,
                DateTime.MaxValue,
                valueList => valueList.FirstValue().Day,
                (newStat, previousStat) => newStat < previousStat ? newStat : previousStat);
        }

        /// <summary>
        /// Returns the latest date held in the portfolio.
        /// </summary>
        /// <param name="portfolio">The database to query</param>
        /// <param name="account">The type of element to search for. All searches for Bank accounts and securities.</param>
        /// <param name="name">An ancillary name to use in the case of Sectors</param>
        /// <returns></returns>
        public static DateTime FirstDate(this IPortfolio portfolio, Account account, TwoName name)
        {
            return portfolio.CalculateStatistic(
                account,
                name,
                valueList => valueList.FirstValue()?.Day ?? DateTime.MaxValue);
        }
    }
}
