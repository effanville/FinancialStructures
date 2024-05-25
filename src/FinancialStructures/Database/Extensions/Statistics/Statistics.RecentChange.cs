using Effanville.FinancialStructures.NamingStructures;
using Effanville.FinancialStructures.ValueCalculators;

namespace Effanville.FinancialStructures.Database.Extensions.Statistics
{
    public static partial class Statistics
    {
        /// <summary>
        /// returns the total profit in the portfolio.
        /// </summary>
        public static decimal RecentChange(this IPortfolio portfolio, Totals totals = Totals.Security, TwoName names = null)
        {
            return portfolio.CalculateAggregateValue(
                totals,
                names,
                (tot, n) => tot != Totals.Benchmark
                            || tot != Totals.Currency
                            || tot != Totals.CurrencySector
                            || tot != Totals.SecurityCurrency
                            || tot != Totals.BankAccountCurrency
                            || tot != Totals.AssetCurrency,
                0.0m,
                (value, runningTotal) => runningTotal + value,
                RecentChangeCalculators.DefaultCalculator,
                RecentChangeCalculators.Calculators(portfolio));
        }

        /// <summary>
        /// returns the change between the most recent two valuations of the security.
        /// </summary>
        public static decimal RecentChange(this IPortfolio portfolio, Account account, TwoName name)
        {
            return portfolio.CalculateValue(
                 account,
                 name,
                 RecentChangeCalculators.DefaultCalculator,
                 RecentChangeCalculators.Calculators(portfolio));
        }
    }
}
