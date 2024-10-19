﻿using System;

using Effanville.FinancialStructures.FinanceStructures.Extensions;
using Effanville.FinancialStructures.NamingStructures;

namespace Effanville.FinancialStructures.Database.Extensions.Values
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
        /// <param name="identifier">An ancillary name to use in the case of Sectors</param>
        public static DateTime FirstValueDate(this IPortfolio portfolio, Totals total, string identifier = null)
        {
            return portfolio.CalculateAggregateValue(
                total,
                identifier,
                DateTime.MaxValue,
                (newStat, previousStat) => newStat < previousStat ? newStat : previousStat,
                valueList => valueList.FirstDate());
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
            return portfolio.CalculateValue(
                account,
                name,
                valueList => valueList.FirstDate());
        }
    }
}
