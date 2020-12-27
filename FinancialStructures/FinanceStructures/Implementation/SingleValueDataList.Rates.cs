using System;
using StructureCommon.DataStructures;
using StructureCommon.FinanceFunctions;

namespace FinancialStructures.FinanceStructures.Implementation
{
    public partial class SingleValueDataList
    {
        /// <summary>
        /// Returns the latest valuation of the CashAccount.
        /// </summary>
        public virtual DailyValuation LatestValue()
        {
            DateTime latestDate = Values.LatestDate();
            double latestValue = Values.LatestValue();

            return new DailyValuation(latestDate, latestValue);
        }

        /// <summary>
        /// Returns the first valuation of the CashAccount.
        /// </summary>
        public DailyValuation FirstValue()
        {
            return Values.FirstValuation();
        }

        /// <summary>
        /// Returns the latest earlier valuation of the CashAccount to <paramref name="date"/>.
        /// </summary>
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

        /// <summary>
        /// Returns the latest earlier valuation of the OldCashAccount to <paramref name="date"/>.
        /// </summary>
        internal virtual DailyValuation NearestEarlierValuation(DateTime date)
        {
            return Values.NearestEarlierValue(date);
        }

        /// <summary>
        /// Returns earliest valuation after the date specified.
        /// </summary>
        internal DailyValuation NearestLaterValuation(DateTime date)
        {
            return Values.NearestLaterValue(date);
        }

        /// <summary>
        /// Returns the most recent value to <paramref name="date"/> that is prior to that date.
        /// </summary>
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
