using Effanville.FinancialStructures.FinanceStructures;
using Effanville.FinancialStructures.FinanceStructures.Extensions;
using Effanville.FinancialStructures.NamingStructures;

namespace Effanville.FinancialStructures.Database.Extensions.Statistics
{
    /// <summary>
    /// Contains Extension methods for calculating statistics.
    /// </summary>
    public static partial class Statistics
    {
        /// <summary>
        /// returns the total profit in the portfolio.
        /// </summary>
        public static decimal TotalProfit(this IPortfolio portfolio, Totals totals, string identifier = null)
        {
            return portfolio.CalculateAggregateValue(
                totals,
                identifier,
                0.0m,
                (value, currentTotal) => value + currentTotal,
                vl =>
                {
                    ICurrency currency = portfolio.Currency(vl);
                    return vl.Profit(currency);
                });
        }

        /// <summary>
        /// Returns the profit of the company over its lifetime in the portfolio.
        /// </summary>
        public static decimal Profit(this IPortfolio portfolio, Account account, TwoName name)
        {
            return portfolio.CalculateValue(
                account,
                name,
                vl =>
                {
                    ICurrency currency = portfolio.Currency(vl);
                    return vl.Profit(currency);
                });
        }
    }
}
