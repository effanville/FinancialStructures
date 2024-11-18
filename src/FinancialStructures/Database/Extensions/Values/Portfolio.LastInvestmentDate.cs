using System;
using Effanville.FinancialStructures.FinanceStructures.Extensions;

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
        public static DateTime LastInvestmentDate(this IPortfolio portfolio, Totals total, string identifier = null)
        {
            return portfolio.CalculateAggregateValue(
               total,
               identifier,
               (tot, _) => tot == Totals.Security
                   || tot == Totals.SecurityCompany
                   || tot == Totals.Sector
                   || tot == Totals.SecuritySector
                   || tot == Totals.Pension
                   || tot == Totals.PensionCompany
                   || tot == Totals.PensionSector
                   || tot == Totals.All,
               DateTime.MinValue,
               (date, otherDate) => otherDate > date ? otherDate : date,
               valueList => valueList.LastInvestmentDate());
        }
    }
}
