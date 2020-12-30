using System;
using System.Collections.Generic;
using StructureCommon.DataStructures;
using StructureCommon.FinanceFunctions;

namespace FinancialStructures.FinanceStructures.Implementation
{
    public partial class Security
    {
        /// <inheritdoc/>
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
        public override DailyValuation FirstValue()
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
        public override DailyValuation Value(DateTime date)
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
        public override DailyValuation RecentPreviousValue(DateTime date)
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

        /// <inheritdoc/>
        public override DailyValuation NearestEarlierValuation(DateTime date)
        {
            return NearestEarlierValuation(date, null);
        }

        /// <inheritdoc/>
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

        /// <inheritdoc/>
        public override double CAR(DateTime earlierTime, DateTime laterTime)
        {
            return CAR(earlierTime, laterTime, null);
        }

        /// <inheritdoc/>
        public double CAR(DateTime earlierTime, DateTime laterTime, ICurrency currency)
        {
            return FinancialFunctions.CAR(Value(earlierTime, currency), Value(laterTime, currency));
        }

        /// <inheritdoc/>
        public double IRR(DateTime earlierDate, DateTime laterDate, ICurrency currency = null)
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

        /// <inheritdoc/>
        public double IRR(ICurrency currency = null)
        {
            return IRR(FirstValue().Day, LatestValue().Day, currency);
        }
    }
}
