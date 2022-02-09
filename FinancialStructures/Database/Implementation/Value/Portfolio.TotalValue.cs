using System;

using FinancialStructures.Database.Extensions;
using FinancialStructures.FinanceStructures;
using FinancialStructures.NamingStructures;

namespace FinancialStructures.Database.Implementation
{
    public partial class Portfolio
    {
        /// <inheritdoc/>
        public decimal TotalValue(Totals elementType, TwoName names = null)
        {
            return TotalValue(elementType, DateTime.Today, names);
        }

        /// <inheritdoc/>
        public decimal TotalValue(Totals elementType, DateTime date, TwoName names = null)
        {
            return this.CalculateAggregateStatistic(
                elementType,
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
                        ICurrency currency = Currency(sec);
                        return sec.Value(date, currency).Value;
                    }
                }
                else if (valueList is IAmortisableAsset asset)
                {
                    if (asset.Any())
                    {
                        ICurrency currency = Currency(asset);
                        return asset.Value(date, currency).Value;
                    }
                }
                else if (valueList is IExchangableValueList eValueList)
                {
                    if (eValueList.Any())
                    {
                        ICurrency currency = Currency(eValueList);
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
