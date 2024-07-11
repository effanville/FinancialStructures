using System;

using Effanville.FinancialStructures.NamingStructures;
using Effanville.FinancialStructures.ValueCalculators;

namespace Effanville.FinancialStructures.Database.Extensions.Values
{
    public static partial class Values
    {
        /// <summary>
        /// Total value of all accounts of type specified today.
        /// </summary>
        /// <param name="portfolio">The portfolio to calculate value for.</param>
        /// <param name="account">The type to find the total of.</param>
        /// <param name="names">Any name associated with this total, e.g. the Sector name</param>
        /// <returns>The total value held on today.</returns>
        public static decimal TotalValue(this IPortfolio portfolio, Totals account, TwoName names = null)
        {
            return portfolio.TotalValue(account, DateTime.Today, names);
        }

        /// <summary>
        /// Total value of all accounts of type specified on date given.
        /// </summary>
        /// <param name="portfolio">The portfolio to calculate value for.</param>
        /// <param name="totals">The type to find the total of.</param>
        /// <param name="date">The date to find the total on.</param>
        /// <param name="names">Any name associated with this total, e.g. the Sector name</param>
        /// <returns>The total value held.</returns>
        public static decimal TotalValue(this IPortfolio portfolio, Totals totals, DateTime date, TwoName names = null)
        {
            return portfolio.CalculateAggregateValue(
                totals,
                names,
                0,
                (a, b) => a + b,
                ValueCalculator.DefaultCalculator(date),
                ValueCalculator.Calculators(portfolio, date));
        }
    }
}
