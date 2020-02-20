using FinancialStructures.DataStructures;
using FinancialStructures.FinanceFunctionsList;
using System;
using System.Collections.Generic;

namespace FinancialStructures.FinanceStructures
{
    public partial class Security
    {
        internal double TotalInvestment(Currency currency = null)
        {
            var investments = InvestmentsBetween(FirstValue().Day, LatestValue().Day, currency);
            double sum = 0;
            foreach (var investment in investments)
            {
                sum += investment.Value;
            }
            return sum;
        }

        /// <summary>
        /// The date and latest value of the security
        /// </summary>
        internal DayValue LatestValue(Currency currency = null)
        {
            DayValue latestDate = fUnitPrice.LatestValuation();
            if (latestDate == null)
            {
                return new DayValue(DateTime.Today, 0.0); ;
            }

            double currencyValue = currency == null ? 1.0 : currency.Value(latestDate.Day).Value;
            double latestValue = latestDate.Value * fShares.LatestValue() * currencyValue;

            return new DayValue(latestDate.Day, latestValue);
        }

        /// <summary>
        /// The date and first value of the security
        /// </summary>
        internal DayValue FirstValue(Currency currency = null)
        {
            DayValue firstDate = fUnitPrice.FirstValuation();
            if (firstDate == null)
            {
                return new DayValue(DateTime.MinValue, 0.0); ;
            }
            double currencyValue = currency == null ? 1.0 : currency.Value(firstDate.Day).Value;
            double latestValue = firstDate.Value * fShares.FirstValue() * currencyValue;

            return new DayValue(firstDate.Day, latestValue);
        }

        /// <summary>
        /// Returns the interpolated value of the security on the date provided.
        /// </summary>
        internal DayValue Value(DateTime date, Currency currency = null)
        {
            DayValue perSharePrice = fUnitPrice.Value(date);
            double currencyValue = currency == null ? 1.0 : currency.Value(date).Value;
            double value = perSharePrice.Value * fShares.NearestEarlierValue(date).Value * currencyValue;
            return new DayValue(date, value);
        }

        /// <summary>
        /// Returns most recent valuation on or before the date specified. 
        /// </summary>
        internal DayValue LastEarlierValuation(DateTime date, Currency currency = null)
        {
            DayValue val = fUnitPrice.RecentPreviousValue(date);
            if (val == null)
            {
                return new DayValue(date, 0.0);
            }

            double currencyValue = currency == null ? 1.0 : currency.Value(val.Day).Value;
            double latestValue = fShares.RecentPreviousValue(date).Value * val.Value * currencyValue;
            return new DayValue(date, latestValue);
        }

        /// <summary>
        /// Returns most recent valuation on or before the date specified. 
        /// </summary>
        internal DayValue NearestEarlierValuation(DateTime date, Currency currency = null)
        {
            DayValue val = fUnitPrice.NearestEarlierValue(date);
            if (val == null)
            {
                return new DayValue(date, 0.0);
            }

            double currencyValue = currency == null ? 1.0 : currency.Value(val.Day).Value;
            double latestValue = fShares.NearestEarlierValue(date).Value * val.Value * currencyValue;
            return new DayValue(date, latestValue);
        }

        /// <summary>
        /// Returns earliest valuation after the date specified. 
        /// </summary>
        private DayValue NearestLaterValuation(DateTime date, Currency currency = null)
        {
            DayValue val = fUnitPrice.NearestLaterValue(date);
            if (val == null)
            {
                return new DayValue(date, 0.0);
            }
            double currencyValue = currency == null ? 1.0 : currency.Value(val.Day).Value;
            double latestValue = fShares.NearestLaterValue(date).Value * val.Value * currencyValue;

            return new DayValue(date, latestValue);
        }

        /// <summary>
        /// Returns earliest valuation after the date specified. 
        /// </summary>
        internal List<DayValue> InvestmentsBetween(DateTime earlierDate, DateTime laterDate, Currency currency = null)
        {
            List<DayValue> values = fInvestments.GetValuesBetween(earlierDate, laterDate);
            foreach (var value in values)
            {
                double currencyValue = currency == null ? 1.0 : currency.Value(value.Day).Value;
                value.SetValue(value.Value * currencyValue);
            }

            return values;
        }

        /// <summary>
        /// returns a list of all investments with the name of the security.
        /// </summary>
        internal List<DayValue_Named> AllInvestmentsNamed(Currency currency = null)
        {
            List<DayValue> values = fInvestments.GetValuesBetween(fInvestments.FirstDate(), fInvestments.LatestDate());
            List<DayValue_Named> namedValues = new List<DayValue_Named>();
            foreach (var value in values)
            {
                if (value != null && value.Value != 0)
                {
                    double currencyValue = currency == null ? 1.0 : currency.Value(value.Day).Value;
                    value.SetValue(value.Value * currencyValue);
                    namedValues.Add(new DayValue_Named(this.fName, this.fCompany, value));
                }
            }
            return namedValues;
        }

        /// <summary>
        /// returns compound annual rate of security between the two times specified
        /// </summary>
        internal double CAR(DateTime earlierTime, DateTime laterTime, Currency currency = null)
        {
            return FinancialFunctions.CAR(Value(earlierTime, currency), Value(laterTime, currency));
        }

        /// <summary>
        /// Internal rate of return of the investment over the past timelength months
        /// </summary>
        internal double IRRTime(DateTime earlierDate, DateTime laterDate, Currency currency = null)
        {
            if (Any())
            {
                var invs = InvestmentsBetween(earlierDate, laterDate, currency);
                var latestTime = Value(laterDate, currency);
                var firstTime = Value(earlierDate, currency);
                return FinancialFunctions.IRRTime(firstTime, invs, latestTime);
            }
            return double.NaN;
        }

        /// <summary>
        /// Internal rate of return of the investment over entire history
        /// </summary>
        internal double IRR(Currency currency = null)
        {
            return IRRTime(FirstValue().Day, LatestValue().Day, currency);
        }
    }
}
