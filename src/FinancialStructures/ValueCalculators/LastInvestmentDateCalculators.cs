using System;

using Effanville.FinancialStructures.FinanceStructures;

namespace Effanville.FinancialStructures.ValueCalculators
{
    public static class LastInvestmentDateCalculators
    {
        public static Func<IValueList, DateTime> DefaultCalculator
            => valueList => Calculate(valueList);

        static DateTime Calculate(IValueList valueList)
        {
            if (valueList is not ISecurity sec)
            {
                return default; 
            }
            
            return sec.LastInvestment()?.Day ?? DateTime.MinValue;

        }
    }
}