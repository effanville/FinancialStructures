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
        /// <param name="name">An ancillary name to use in the case of Sectors</param>
        public static DateTime LastInvestmentDate(this IPortfolio portfolio, Totals total, TwoName name = null)
        {
            return portfolio.CalculateAggregateValue(
               total,
               name,
               (tot, n) => tot == Totals.Security
                   || tot == Totals.SecurityCompany
                   || tot == Totals.Sector
                   || tot == Totals.SecuritySector
                   || tot == Totals.All,
               DateTime.MinValue,
               (date, otherDate) => otherDate > date ? otherDate : date,
               valueList => valueList.LastInvestmentDate());
        }
    }
}
