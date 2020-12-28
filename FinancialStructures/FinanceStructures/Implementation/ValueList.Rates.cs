using System;
using StructureCommon.DataStructures;
using StructureCommon.FinanceFunctions;

namespace FinancialStructures.FinanceStructures.Implementation
{
    public partial class ValueList
    {
        /// <inheritdoc/>
        public virtual DailyValuation LatestValue()
        {
            DateTime latestDate = Values.LatestDate();
            double latestValue = Values.LatestValue();

            return new DailyValuation(latestDate, latestValue);
        }

        /// <inheritdoc/>
        public DailyValuation FirstValue()
        {
            return Values.FirstValuation();
        }

        /// <inheritdoc/>
        public DailyValuation Value(DateTime date)
        {
            return Values.Value(date);
        }

        /// <summary>
        /// returns compound annual rate of security between the two times specified
        /// </summary>
        public double CAR(DateTime earlierTime, DateTime laterTime)
        {
            return FinancialFunctions.CAR(Value(earlierTime), Value(laterTime));
        }

        /// <inheritdoc/>
        public DailyValuation RecentPreviousValue(DateTime date)
        {
            DailyValuation val = Values.RecentPreviousValue(date);
            if (val == null)
            {
                return new DailyValuation(date, 0.0);
            }

            return val;
        }
    }
}
