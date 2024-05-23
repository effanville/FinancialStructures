using System;
using System.Linq;

using Effanville.FinancialStructures.DataStructures;
using Effanville.FinancialStructures.FinanceStructures;

namespace Effanville.FinancialStructures.ValueCalculators
{
    public static class LastPurchaseCalculators
    {
        public static Func<IValueList, DateTime> DefaultCalculator
            => valueList => Calculate(valueList);

        static DateTime Calculate(IValueList valuelist)
        {
            if (valuelist is not ISecurity sec)
            {
                return default;
            }

            DateTime latest = sec.Trades.LastOrDefault(trade => trade.TradeType.Equals(TradeType.Buy))?.Day ?? default(DateTime);
            return latest;
        }
    }
}