using FinancialStructures.FinanceStructures;
using FinancialStructures.FinanceStructures.Statistics;
using FinancialStructures.NamingStructures;

namespace FinancialStructures.Database.Extensions.Statistics
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
            return portfolio.CalculateAggregateStatistic(
                totals,
                names,
                0.0m,
                exchangableValueList => Calcuate(exchangableValueList),
                (value, currentTotal) => value + currentTotal);
            decimal Calcuate(IValueList valueList)
            {
                if (valueList is not IExchangableValueList exchangableValueList)
                {
                    return valueList.Profit();
                }

                ICurrency currency = portfolio.Currency(exchangableValueList);
                return exchangableValueList.Profit(currency);
            }
        }

        /// <summary>
        /// Returns the profit of the company over its lifetime in the portfolio.
        /// </summary>
        public static decimal Profit(this IPortfolio portfolio, Account account, TwoName name)
        {
            return portfolio.CalculateStatistic(
                account,
                name,
                valueList => valueList.Any() ? valueList.Profit() : 0.0m,
                valueList => Calculate(valueList));

            decimal Calculate(IExchangableValueList valueList)
            {
                if (!valueList.Any())
                {
                    return 0.0m;
                }
                ICurrency currency = portfolio.Currency(valueList);
                return valueList.Profit(currency);
            }
        }
    }
}
