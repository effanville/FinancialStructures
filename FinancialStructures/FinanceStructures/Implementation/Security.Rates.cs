using System;
using System.Collections.Generic;
using FinancialStructures.DataStructures;
using StructureCommon.DataStructures;
using StructureCommon.FinanceFunctions;

namespace FinancialStructures.FinanceStructures.Implementation
{
    public partial class Security
    {
        public double TotalInvestment(ICurrency currency = null)
        {
            List<DailyValuation> investments = InvestmentsBetween(FirstValue().Day, LatestValue().Day, currency);
            double sum = 0;
            foreach (DailyValuation investment in investments)
            {
                sum += investment.Value;
            }
            return sum;
        }

        /// <inheritdoc/>
        public override DailyValuation LatestValue()
        {
            return LatestValue(null);
        }

        /// <inheritdoc/>
        public DailyValuation LatestValue(ICurrency currency)
        {
            DailyValuation latestDate = fUnitPrice.LatestValuation();
            if (latestDate == null)
            {
                return new DailyValuation(DateTime.Today, 0.0);
            }

            double currencyValue = currency == null ? 1.0 : currency.Value(latestDate.Day).Value;
            double latestValue = latestDate.Value * fShares.LatestValue() * currencyValue;

            return new DailyValuation(latestDate.Day, latestValue);
        }

        /// <inheritdoc/>
        public DailyValuation FirstValue()
        {
            return FirstValue(null);
        }

        /// <inheritdoc/>
        public DailyValuation FirstValue(ICurrency currency)
        {
            DailyValuation firstDate = fUnitPrice.FirstValuation();
            if (firstDate == null)
            {
                return new DailyValuation(DateTime.MinValue, 0.0);
                ;
            }
            double currencyValue = currency == null ? 1.0 : currency.Value(firstDate.Day).Value;
            double latestValue = firstDate.Value * fShares.FirstValue() * currencyValue;

            return new DailyValuation(firstDate.Day, latestValue);
        }

        /// <inheritdoc/>
        public DailyValuation Value(DateTime date)
        {
            return Value(date, null);
        }

        /// <inheritdoc/>
        public DailyValuation Value(DateTime date, ICurrency currency)
        {
            DailyValuation perSharePrice = fUnitPrice.Value(date);
            double currencyValue = currency == null ? 1.0 : currency.Value(date).Value;
            double value = perSharePrice.Value * fShares.NearestEarlierValue(date).Value * currencyValue;
            return new DailyValuation(date, value);
        }

        /// <inheritdoc/>
        public DailyValuation RecentPreviousValue(DateTime date)
        {
            return RecentPreviousValue(date, null);
        }

        /// <inheritdoc/>
        public DailyValuation RecentPreviousValue(DateTime date, ICurrency currency)
        {
            DailyValuation val = fUnitPrice.RecentPreviousValue(date);
            if (val == null)
            {
                return new DailyValuation(date, 0.0);
            }

            double currencyValue = currency == null ? 1.0 : currency.Value(val.Day).Value;
            double latestValue = fShares.RecentPreviousValue(date).Value * val.Value * currencyValue;
            return new DailyValuation(date, latestValue);
        }

        /// <summary>
        /// Returns most recent valuation on or before the date specified.
        /// </summary>
        public DailyValuation NearestEarlierValuation(DateTime date, ICurrency currency)
        {
            DailyValuation val = fUnitPrice.NearestEarlierValue(date);
            if (val == null)
            {
                return new DailyValuation(date, 0.0);
            }

            double currencyValue = currency == null ? 1.0 : currency.Value(val.Day).Value;
            double latestValue = fShares.NearestEarlierValue(date).Value * val.Value * currencyValue;
            return new DailyValuation(date, latestValue);
        }

        /// <summary>
        /// Returns earliest valuation after the date specified.
        /// </summary>
        private DailyValuation NearestLaterValuation(DateTime date, ICurrency currency = null)
        {
            DailyValuation val = fUnitPrice.NearestLaterValue(date);
            if (val == null)
            {
                return new DailyValuation(date, 0.0);
            }
            double currencyValue = currency == null ? 1.0 : currency.Value(val.Day).Value;
            double latestValue = fShares.NearestLaterValue(date).Value * val.Value * currencyValue;

            return new DailyValuation(date, latestValue);
        }

        /// <summary>
        /// Returns earliest valuation after the date specified.
        /// </summary>
        public List<DailyValuation> InvestmentsBetween(DateTime earlierDate, DateTime laterDate, ICurrency currency = null)
        {
            List<DailyValuation> values = fInvestments.GetValuesBetween(earlierDate, laterDate);
            foreach (DailyValuation value in values)
            {
                double currencyValue = currency == null ? 1.0 : currency.Value(value.Day).Value;
                value.SetValue(value.Value * currencyValue);
            }

            return values;
        }

        /// <summary>
        /// returns a list of all investments with the name of the security.
        /// </summary>
        public List<DayValue_Named> AllInvestmentsNamed(ICurrency currency = null)
        {
            List<DailyValuation> values = fInvestments.GetValuesBetween(fInvestments.FirstDate(), fInvestments.LatestDate());
            List<DayValue_Named> namedValues = new List<DayValue_Named>();
            foreach (DailyValuation value in values)
            {
                if (value != null && value.Value != 0)
                {
                    double currencyValue = currency == null ? 1.0 : currency.Value(value.Day).Value;
                    value.SetValue(value.Value * currencyValue);
                    namedValues.Add(new DayValue_Named(Names.Company, Names.Name, value));
                }
            }
            return namedValues;
        }

        /// <inheritdoc/>
        public double CAR(DateTime earlierTime, DateTime laterTime)
        {
            return CAR(earlierTime, laterTime, null);
        }

        /// <inheritdoc/>
        public double CAR(DateTime earlierTime, DateTime laterTime, ICurrency currency = null)
        {
            return FinancialFunctions.CAR(Value(earlierTime, currency), Value(laterTime, currency));
        }

        /// <summary>
        /// Internal rate of return of the investment over the past timelength months
        /// </summary>
        public double IRRTime(DateTime earlierDate, DateTime laterDate, ICurrency currency = null)
        {
            if (Any())
            {
                List<DailyValuation> invs = InvestmentsBetween(earlierDate, laterDate, currency);
                DailyValuation latestTime = Value(laterDate, currency);
                DailyValuation firstTime = Value(earlierDate, currency);
                return FinancialFunctions.IRRTime(firstTime, invs, latestTime);
            }
            return double.NaN;
        }

        /// <summary>
        /// Internal rate of return of the investment over entire history
        /// </summary>
        public double IRR(ICurrency currency = null)
        {
            return IRRTime(FirstValue().Day, LatestValue().Day, currency);
        }
    }
}
