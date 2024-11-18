using System;
using System.Collections.Generic;
using System.Linq;
using Effanville.Common.Structure.DataStructures;
using Effanville.Common.Structure.MathLibrary.Finance;
using Effanville.FinancialStructures.Database;

namespace Effanville.FinancialStructures.FinanceStructures.Extensions
{
    /// <summary>
    /// Contains extension methods for <see cref="IValueList"/>s.
    /// </summary>
    // ReSharper disable once InconsistentNaming
    public static class IValueListExtensions
    {
        /// <summary>
        /// Returns the value from the list, and provides the default value if no value exists.
        /// </summary>
        public static decimal Value(this IValueList valueList, DateTime time, decimal defaultValue) 
            => valueList.Value(time)?.Value ?? defaultValue;

        public static DateTime LatestDate(this IValueList valueList)
            => valueList.LatestValue()?.Day ?? DateTime.MinValue;

        public static DateTime FirstDate(this IValueList valueList)
            => valueList.FirstValue()?.Day ?? DateTime.MaxValue;
        
        /// <summary>
        /// Calculates a statistic for an account.
        /// </summary>
        /// <param name="valueList">The valueList to find the value for.</param>
        /// <param name="calculator">The optional default calculator to use.</param>
        /// <param name="defaultValue">The optional default value to use.</param>
        /// <typeparam name="TValue">The type of the statistic to return.</typeparam>
        /// <returns>The statistic value</returns>
        public static TValue CalculateValue<TValue>(
            this IValueList valueList,            
            Func<IValueList, TValue> calculator,
            TValue defaultValue = default)
        {
            if (!valueList.Any())
            {
                return defaultValue;
            }
            
            return calculator != null ? calculator(valueList) : defaultValue;
        }
        
        public static double DrawDown(this IValueList valueList, DateTime earlierTime, DateTime laterTime)
        {
            if (!valueList.Any())
            {
                return double.NaN;
            }
            
            DateTime earliestTime = valueList.FirstValue()?.Day ?? DateTime.MaxValue;
            if (earlierTime < earliestTime)
            {
                earlierTime = earliestTime;
            }

            DateTime latestTime = valueList.LatestValue()?.Day ?? DateTime.MinValue;
            if (laterTime > latestTime)
            {
                laterTime = latestTime;
            }
            List<DailyValuation> values = valueList.ListOfValues()
                .Where(value => 
                    value.Day >= earlierTime 
                    && value.Day <= laterTime 
                    && !value.Value.Equals(0.0m))
                .ToList();
            decimal dd = FinanceFunctions.Drawdown(values);
            if (dd == decimal.MaxValue)
            {
                return 0.0;
            }

            return (double)dd;
        }
        
        public static double MaximumDrawDown(this IValueList valueList, DateTime earlierTime, DateTime laterTime)
        {
            if (!valueList.Any())
            {
                return double.NaN;
            }
            
            DateTime earliestTime = valueList.FirstDate();
            if (earlierTime < earliestTime)
            {
                earlierTime = earliestTime;
            }

            DateTime latestTime = valueList.LatestDate();
            if (laterTime > latestTime)
            {
                laterTime = latestTime;
            }
            List<DailyValuation> values = valueList.ListOfValues()
                .Where(value => 
                    value.Day >= earlierTime 
                    && value.Day <= laterTime 
                    && !value.Value.Equals(0.0m))
                .ToList();
            decimal dd = FinanceFunctions.MDD(values);
            if (dd == decimal.MaxValue)
            {
                return 0.0;
            }

            return (double)dd;
        }

        public static DateTime LastInvestmentDate(this IValueList valueList)
        {
            if (!valueList.Any() || (valueList.AccountType != Account.Security && valueList.AccountType != Account.Pension))
            {
                return default;
            }
            
            return valueList is not ISecurity sec
                ? default
                : sec.LastInvestmentDate();
        }
        
        public static DateTime LastPurchaseDate(this IValueList valueList)
        {
            if (!valueList.Any() || (valueList.AccountType != Account.Security && valueList.AccountType != Account.Pension))
            {
                return default;
            }
            
            return valueList is not ISecurity sec
                ? default
                : sec.LastPurchaseDate();
        }
        
        public static decimal Debt(this IValueList valueList, ICurrency currency, DateTime time)
        {
            if (!valueList.Any() || valueList.AccountType != Account.Asset)
            {
                return default;
            }
            
            return valueList is not IAmortisableAsset asset 
                ? 0.0m 
                : asset.Debt(time, currency);
        }

        public static decimal TotalInvestment(this IValueList valueList, ICurrency currency, DateTime date)
        {
            if (!valueList.Any())
            {
                return default;
            }

            switch (valueList.AccountType)
            {
                case Account.Asset:
                {
                    return valueList is not IAmortisableAsset asset
                        ? 0.0m
                        : asset.TotalCost(date, currency);
                }
                case Account.Security:
                case Account.Pension:
                {
                    return valueList is not ISecurity security
                        ? 0.0m
                        : security.TotalInvestment(currency);
                }
                case Account.Unknown:
                case Account.All:
                case Account.Benchmark:
                case Account.BankAccount:
                case Account.Currency:
                default:
                    return 0.0m;
            }
        }

        public static Dictionary<DateTime, int> EntryDistribution(this IValueList valueList)
        {
            Dictionary<DateTime, int> totals = new ();

            if (valueList is ISecurity security && security.Any())
            {
                if (security.Any())
                {
                    foreach (DailyValuation value in security.Shares.Values())
                    {
                        if (totals.TryGetValue(value.Day, out _))
                        {
                            totals[value.Day] += 1;
                        }
                        else
                        {
                            totals.Add(value.Day, 1);
                        }
                    }

                    foreach (DailyValuation priceValue in security.UnitPrice.Values())
                    {
                        if (totals.TryGetValue(priceValue.Day, out _))
                        {
                            totals[priceValue.Day] += 1;
                        }
                        else
                        {
                            totals.Add(priceValue.Day, 1);
                        }
                    }

                    foreach (DailyValuation investmentValue in security.Investments.Values())
                    {
                        if (totals.TryGetValue(investmentValue.Day, out _))
                        {
                            totals[investmentValue.Day] += 1;
                        }
                        else
                        {
                            totals.Add(investmentValue.Day, 1);
                        }
                    }
                }

                return totals;
            }
        
            foreach (DailyValuation value in valueList.ListOfValues())
            {
                totals.Add(value.Day, 1);
            }

            return totals;
        }

        public static decimal Value(this IValueList valueList, ICurrency currency, DateTime time)
        {
            switch (valueList.AccountType)
            {
                case Account.BankAccount:
                {
                    if (valueList is not IExchangeableValueList eValueList)
                    {
                        return 0.0m;
                    }

                    return eValueList.ValueOnOrBefore(time, currency)?.Value ?? 0.0m;
                }
                case Account.Asset:
                case Account.Pension:
                case Account.Security:
                {
                    if (valueList is not IExchangeableValueList eValueList)
                    {
                        return 0.0m;
                    }

                    return eValueList.Value(time, currency)?.Value ?? 0.0m;
                }
                case Account.Unknown:
                case Account.All:
                case Account.Benchmark:
                case Account.Currency:
                default:
                    return valueList.Value(time)?.Value ?? 0.0m;
            }
        }

        public static double IRR(this IValueList valueList, ICurrency currency)
        {
            if (!valueList.Any())
            {
                return default;
            }
            
            return valueList.IRR(currency, valueList.FirstDate(), 
                valueList.LatestDate());
        }

        public static double IRR(this IValueList valueList, ICurrency currency, DateTime earlierTime, DateTime laterTime)
        {
            if (!valueList.Any())
            {
                return default;
            }
            
            DateTime earliestTime = valueList.FirstValue()?.Day ?? DateTime.MaxValue;
            if (earlierTime < earliestTime)
            {
                earlierTime = earliestTime;
            }

            DateTime latestTime = valueList.LatestValue()?.Day ?? DateTime.MinValue;
            if (laterTime > latestTime)
            {
                laterTime = latestTime;
            }
            
            if (valueList is ISecurity security)
            {

                return security.IRR(earlierTime, laterTime, currency);
            }
            
            return valueList.Any()
                ? valueList.CAR(earlierTime, laterTime)
                : double.NaN;
        }
    }
}
