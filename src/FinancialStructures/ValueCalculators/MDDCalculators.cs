using System;
using System.Collections.Generic;
using System.Linq;

using Effanville.Common.Structure.DataStructures;
using Effanville.Common.Structure.MathLibrary.Finance;
using Effanville.FinancialStructures.FinanceStructures;

namespace Effanville.FinancialStructures.ValueCalculators
{
    public static class MDDCalculators
    {
        public static Func<IValueList, double> DefaultCalculator(DateTime earlierTime, DateTime laterTime) 
            => valueList => Calculate(valueList, earlierTime, laterTime);

        static double Calculate(IValueList valueList, DateTime earlierTime, DateTime laterTime)
        {
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
    }
}