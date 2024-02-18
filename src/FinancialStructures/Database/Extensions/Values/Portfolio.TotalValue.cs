using System;

using FinancialStructures.FinanceStructures;
using FinancialStructures.NamingStructures;

namespace FinancialStructures.Database.Extensions.Values
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
        /// <param name="account">The type to find the total of.</param>
        /// <param name="date">The date to find the total on.</param>
        /// <param name="names">Any name associated with this total, e.g. the Sector name</param>
        /// <returns>The total value held.</returns>
        public static decimal TotalValue(this IPortfolio portfolio, Totals account, DateTime date, TwoName names = null)
        {
            return portfolio.CalculateAggregateStatistic(
                account,
                names,
                0,
                valueList => CalculateValue(valueList),
                (a, b) => a + b);

            decimal CalculateValue(IValueList valueList)
            {
                if (valueList is ISecurity sec)
                {
                    if (sec.Any())
                    {
                        ICurrency currency = portfolio.Currency(sec);
                        return sec.Value(date, currency).Value;
                    }
                }
                else if (valueList is IAmortisableAsset asset)
                {
                    if (asset.Any())
                    {
                        ICurrency currency = portfolio.Currency(asset);
                        return asset.Value(date, currency).Value;
                    }
                }
                else if (valueList is IExchangableValueList eValueList)
                {
                    if (eValueList.Any())
                    {
                        ICurrency currency = portfolio.Currency(eValueList);
                        return eValueList.ValueOnOrBefore(date, currency).Value;
                    }
                }
                else
                {
                    return valueList.Value(date).Value;
                }

                return 0.0m;
            }
        }
    }
}
