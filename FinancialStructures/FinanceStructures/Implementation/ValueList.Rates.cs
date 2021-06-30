using System;
using Common.Structure.DataStructures;
using Common.Structure.FinanceFunctions;

namespace FinancialStructures.FinanceStructures.Implementation
{
    public partial class ValueList
    {
        /// <inheritdoc/>
        public virtual DailyValuation LatestValue()
        {
            return Values.LatestValuation();
        }

        /// <inheritdoc/>
        public virtual DailyValuation FirstValue()
        {
            return Values.FirstValuation();
        }

        /// <inheritdoc/>
        public virtual DailyValuation Value(DateTime date)
        {
            return Values.Value(date);
        }

        /// <summary>
        /// returns compound annual rate of security between the two times specified
        /// </summary>
        public virtual double CAR(DateTime earlierTime, DateTime laterTime)
        {
            return FinancialFunctions.CAR(Value(earlierTime), Value(laterTime));
        }

        /// <inheritdoc/>
        public virtual DailyValuation RecentPreviousValue(DateTime date)
        {
            DailyValuation val = Values.RecentPreviousValue(date);
            if (val == null)
            {
                return new DailyValuation(date, 0.0);
            }

            return val;
        }

        /// <inheritdoc/>
        public virtual DailyValuation NearestEarlierValuation(DateTime date)
        {
            return Values.NearestEarlierValue(date);
        }
    }
}
