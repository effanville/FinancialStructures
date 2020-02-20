using FinancialStructures.DataStructures;
using FinancialStructures.FinanceFunctionsList;
using System;

namespace FinancialStructures.FinanceStructures
{
    public partial class SingleValueDataList
    {
        /// <summary>
        /// Returns the latest valuation of the CashAccount.
        /// </summary>
        internal DailyValuation LatestValue()
        {
            DateTime latestDate = fValues.LatestDate();
            double latestValue = fValues.LatestValue();

            return new DailyValuation(latestDate, latestValue);
        }

        /// <summary>
        /// Returns the first valuation of the CashAccount.
        /// </summary>
        internal DailyValuation FirstValue()
        {
            DateTime firstDate = fValues.FirstDate();
            double latestValue = fValues.FirstValue();

            return new DailyValuation(firstDate, latestValue);
        }

        /// <summary>
        /// Returns the latest earlier valuation of the CashAccount to <paramref name="date"/>.
        /// </summary>
        public DailyValuation Value(DateTime date)
        {
            return fValues.Value(date);
        }


        /// <summary>
        /// returns compound annual rate of security between the two times specified
        /// </summary>
        public double CAR(DateTime earlierTime, DateTime laterTime)
        {
            return FinancialFunctions.CAR(Value(earlierTime), Value(laterTime));
        }
    }
}
