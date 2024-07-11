using Effanville.FinancialStructures.NamingStructures;
using Effanville.FinancialStructures.ValueCalculators;

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
        public static decimal TotalProfit(this IPortfolio portfolio, Totals totals, TwoName names = null)
        {
            return portfolio.CalculateAggregateValue(
                totals,
                names,
                0.0m,
                (value, currentTotal) => value + currentTotal,
                ProfitCalculators.DefaultCalculator,
                ProfitCalculators.Calculators(portfolio));
        }

        /// <summary>
        /// Returns the profit of the company over its lifetime in the portfolio.
        /// </summary>
        public static decimal Profit(this IPortfolio portfolio, Account account, TwoName name)
        {
            return portfolio.CalculateValue(
                account,
                name,
                ProfitCalculators.DefaultCalculator,
                ProfitCalculators.Calculators(portfolio));
        }
    }
}
