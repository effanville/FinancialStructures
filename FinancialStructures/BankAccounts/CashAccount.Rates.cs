using FinancialStructures.DataStructures;
using System;

namespace FinancialStructures.FinanceStructures
{
    /// <summary>
    /// Provides rates and date evaluations to use with a CashAccount.
    /// </summary>
    public partial class CashAccount
    {
        /// <summary>
        /// Returns the latest valuation of the CashAccount.
        /// </summary>
        public DayValue LatestValue(Currency currency = null)
        {
            DayValue latestDate = fAmounts.LatestValuation();
            if (latestDate == null)
            {
                return new DayValue(DateTime.Today, 0.0); ;
            }

            double currencyValue = currency == null ? 1.0 : currency.Value(latestDate.Day).Value;
            double latestValue = latestDate.Value * currencyValue;

            return new DayValue(latestDate.Day, latestValue);
        }

        /// <summary>
        /// Returns the interpolated value of the security on the date provided.
        /// </summary>
        internal DayValue Value(DateTime date, Currency currency = null)
        {
            DayValue perSharePrice = fAmounts.ValueZeroBefore(date);
            double currencyValue = currency == null ? 1.0 : currency.Value(date).Value;
            double value = perSharePrice.Value * currencyValue;
            return new DayValue(date, value);
        }

        /// <summary>
        /// Returns the first valuation of the CashAccount.
        /// </summary>
        internal DayValue FirstValue(Currency currency = null)
        {
            DayValue firstDate = fAmounts.FirstValuation();
            if (firstDate == null)
            {
                return new DayValue(DateTime.Today, 0.0); ;
            }

            double currencyValue = currency == null ? 1.0 : currency.Value(firstDate.Day).Value;
            double latestValue = firstDate.Value * currencyValue;

            return new DayValue(firstDate.Day, latestValue);
        }

        /// <summary>
        /// Returns the latest earlier valuation of the CashAccount to <paramref name="date"/>.
        /// </summary>
        internal DayValue NearestEarlierValuation(DateTime date, Currency currency = null)
        {
            var value = fAmounts.NearestEarlierValue(date);
            double currencyValue = currency == null ? 1.0 : currency.Value(value.Day).Value;
            value.SetValue(value.Value * currencyValue);
            return value;
        }

        /// <summary>
        /// Returns earliest valuation after the date specified. 
        /// </summary>
        internal DayValue NearestLaterValuation(DateTime date, Currency currency = null)
        {
            var value = fAmounts.NearestLaterValue(date);
            double currencyValue = currency == null ? 1.0 : currency.Value(value.Day).Value;
            value.SetValue(value.Value * currencyValue);
            return value;
        }
    }
}
