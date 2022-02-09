using FinancialStructures.FinanceStructures;
using FinancialStructures.FinanceStructures.Statistics;
using FinancialStructures.NamingStructures;

namespace FinancialStructures.Database.Extensions.Statistics
{
    public static partial class Statistics
    {
        /// <summary>
        /// returns the total profit in the portfolio.
        /// </summary>
        public static decimal RecentChange(this IPortfolio portfolio, Totals totals = Totals.Security, TwoName names = null)
        {
            return portfolio.CalculateAggregateStatistic<IExchangableValueList, decimal>(
                totals,
                names,
                (tot, n) => tot != Totals.Benchmark
                    || tot != Totals.Currency
                    || tot != Totals.CurrencySector
                    || tot != Totals.SecurityCurrency
                    || tot != Totals.BankAccountCurrency
                    || tot != Totals.AssetCurrency,
                0.0m,
                valueList => Calculate(valueList),
                (value, runningTotal) => runningTotal + value);
            decimal Calculate(IExchangableValueList valueList)
            {
                ICurrency currency = portfolio.Currency(valueList);
                return valueList.RecentChange(currency);
            }
        }

        /// <summary>
        /// returns the change between the most recent two valuations of the security.
        /// </summary>
        public static decimal RecentChange(this IPortfolio portfolio, Account account, TwoName name)
        {
            return portfolio.CalculateStatistic(
                 account,
                 name,
                 valueList => valueList.Any() ? valueList.RecentChange() : 0.0m,
                 valueList => Calculate(valueList));

            decimal Calculate(IExchangableValueList valueList)
            {
                if (!valueList.Any())
                {
                    return 0.0m;
                }

                ICurrency currency = portfolio.Currency(valueList);
                return valueList.RecentChange(currency);
            }
        }
    }
}
