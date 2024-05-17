using System;

using Effanville.FinancialStructures.FinanceStructures;
using Effanville.FinancialStructures.NamingStructures;

namespace Effanville.FinancialStructures.Database.Extensions.Values
{
    public static partial class Values
    {
        /// <summary>
        /// Total value of all accounts of type specified today.
        /// </summary>
        /// <param name="portfolio">The portfolio to calculate value for.</param>
        /// <param name="totals">The type to find the total of.</param>
        /// <param name="names">Any name associated with this total, e.g. the Sector name</param>
        /// <returns>The total value held on today.</returns>
        public static decimal TotalValue(
            this IPortfolio portfolio,
            Totals totals,
            TwoName names = null,
            IPortfolioStatisticsCache cache = null)
        {
            return portfolio.TotalValue(totals, DateTime.Today,  names, cache);
        }

        /// <summary>
        /// Total value of all accounts of type specified on date given.
        /// </summary>
        /// <param name="portfolio">The portfolio to calculate value for.</param>
        /// <param name="totals">The type to find the total of.</param>
        /// <param name="date">The date to find the total on.</param>
        /// <param name="names">Any name associated with this total, e.g. the Sector name</param>
        /// <returns>The total value held.</returns>
        public static decimal TotalValue(
            this IPortfolio portfolio,
            Totals totals, 
            DateTime date,
            TwoName names = null,
            IPortfolioStatisticsCache cache = null)
        {
            if (cache?.TryGetValue(totals, names, date, nameof(TotalValue),
                    out object result) ?? false)
            {
                return (decimal)result;
            }
            decimal value = portfolio.CalculateAggregateStatistic<decimal>(
                totals,
                names,
                0,
                valueList => CalculateValue(portfolio, valueList, date),
                (a, b) => a + b);
            cache?.AddValue(totals, names, date, nameof(TotalValue), value);
            return value;
        }

        private static decimal CalculateValue(IPortfolio portfolio, IValueList valueList, DateTime date)
        {
            if (!valueList.Any())
            {
                return 0.0m;
            }

            decimal value;
            switch (valueList.AccountType)
            {
                case Account.Pension:
                case Account.Security:
                {
                    if (valueList is not ISecurity sec)
                    {
                        value = 0.0m;
                        break;
                    }

                    ICurrency currency = portfolio.Currency(sec);
                    value = sec.Value(date, currency)?.Value ?? 0.0m;
                    break;
                }
                case Account.Asset:
                {
                    if (valueList is not IAmortisableAsset asset)
                    {
                        value = 0.0m;
                        break;
                    }

                    ICurrency currency = portfolio.Currency(asset);
                    value = asset.Value(date, currency)?.Value ?? 0.0m;
                    break;
                }

                case Account.BankAccount:
                {
                    if (valueList is not IExchangableValueList eValueList)
                    {
                        value = 0.0m;
                        break;
                    }

                    ICurrency currency = portfolio.Currency(eValueList);
                    value = eValueList.ValueOnOrBefore(date, currency)?.Value ?? 0.0m;
                    break;
                }
                case Account.Unknown:
                case Account.All:
                case Account.Benchmark:
                case Account.Currency:
                default:
                    value = valueList.Value(date)?.Value ?? 0.0m;
                    break;
            }
            
            return value;
        }
    }
}