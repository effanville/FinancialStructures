using FinancialStructures.DataStructures;
using FinancialStructures.FinanceFunctionsList;
using System;

namespace FinancialStructures.FinanceStructures
{
    public partial class Currency
    {
        /// <summary>
        /// Returns the latest valuation of the CashAccount.
        /// </summary>
        internal DayValue LatestValue()
        {
            DateTime latestDate = fValues.LatestDate();
            double latestValue = fValues.LatestValue();

            return new DayValue(latestDate, latestValue);
        }

        /// <summary>
        /// Returns the first valuation of the CashAccount.
        /// </summary>
        internal DayValue FirstValue()
        {
            DateTime firstDate = fValues.FirstDate();
            double latestValue = fValues.FirstValue();

            return new DayValue(firstDate, latestValue);
        }

        /// <summary>
        /// Returns the latest earlier valuation of the CashAccount to <paramref name="date"/>.
        /// </summary>
        public DayValue Value(DateTime date)
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
