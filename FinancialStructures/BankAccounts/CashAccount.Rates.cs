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
        public DailyValuation LatestValue(Currency currency = null)
        {
            DailyValuation latestDate = fAmounts.LatestValuation();
            if (latestDate == null)
            {
                return new DailyValuation(DateTime.Today, 0.0); ;
            }

            double currencyValue = currency == null ? 1.0 : currency.Value(latestDate.Day).Value;
            double latestValue = latestDate.Value * currencyValue;

            return new DailyValuation(latestDate.Day, latestValue);
        }

        /// <summary>
        /// Returns the interpolated value of the security on the date provided.
        /// </summary>
        internal DailyValuation Value(DateTime date, Currency currency = null)
        {
            DailyValuation perSharePrice = fAmounts.ValueZeroBefore(date);
            double currencyValue = currency == null ? 1.0 : currency.Value(date).Value;
            double value = perSharePrice.Value * currencyValue;
            return new DailyValuation(date, value);
        }

        /// <summary>
        /// Returns the first valuation of the CashAccount.
        /// </summary>
        internal DailyValuation FirstValue(Currency currency = null)
        {
            DailyValuation firstDate = fAmounts.FirstValuation();
            if (firstDate == null)
            {
                return new DailyValuation(DateTime.Today, 0.0); ;
            }

            double currencyValue = currency == null ? 1.0 : currency.Value(firstDate.Day).Value;
            double latestValue = firstDate.Value * currencyValue;

            return new DailyValuation(firstDate.Day, latestValue);
        }

        /// <summary>
        /// Returns the latest earlier valuation of the CashAccount to <paramref name="date"/>.
        /// </summary>
        internal DailyValuation NearestEarlierValuation(DateTime date, Currency currency = null)
        {
            var value = fAmounts.NearestEarlierValue(date);
            double currencyValue = currency == null ? 1.0 : currency.Value(value.Day).Value;
            value.SetValue(value.Value * currencyValue);
            return value;
        }

        /// <summary>
        /// Returns earliest valuation after the date specified. 
        /// </summary>
        internal DailyValuation NearestLaterValuation(DateTime date, Currency currency = null)
        {
            var value = fAmounts.NearestLaterValue(date);
            double currencyValue = currency == null ? 1.0 : currency.Value(value.Day).Value;
            value.SetValue(value.Value * currencyValue);
            return value;
        }
    }
}
