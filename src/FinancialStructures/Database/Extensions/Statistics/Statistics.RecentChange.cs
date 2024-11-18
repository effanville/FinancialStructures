using Effanville.FinancialStructures.FinanceStructures;
using Effanville.FinancialStructures.FinanceStructures.Extensions;
using Effanville.FinancialStructures.NamingStructures;

namespace Effanville.FinancialStructures.Database.Extensions.Statistics
{
    public static partial class Statistics
    {
        /// <summary>
        /// returns the total profit in the portfolio.
        /// </summary>
        public static decimal RecentChange(this IPortfolio portfolio, Totals totals = Totals.Security, string identifier = null)
        {
            return portfolio.CalculateAggregateValue(
                totals,
                identifier,
                (tot, n) => tot != Totals.Benchmark
                            || tot != Totals.Currency
                            || tot != Totals.CurrencySector
                            || tot != Totals.SecurityCurrency
                            || tot != Totals.BankAccountCurrency
                            || tot != Totals.AssetCurrency,
                0.0m,
                (value, runningTotal) => runningTotal + value,
                vl =>
                {
                    ICurrency currency = portfolio.Currency(vl);
                    return vl.RecentChange(currency);
                }
                );
        }

        /// <summary>
        /// returns the change between the most recent two valuations of the security.
        /// </summary>
        public static decimal RecentChange(this IPortfolio portfolio, Account account, TwoName name)
        {
            return portfolio.CalculateValue(
                 account,
                 name,
                 vl =>
                 {
                     ICurrency currency = portfolio.Currency(vl);
                     return vl.RecentChange(currency);
                 });
        }
    }
}
