using System;
using System.Collections.Generic;
using Common.Structure.DataStructures;
using Common.Structure.FinanceFunctions;

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
            List<DailyValuation> values = Investments.GetValuesBetween(earlierDate, laterDate);
            foreach (DailyValuation value in values)
            {
                value.Value *= GetCurrencyValue(value.Day, currency);
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
            DailyValuation latestDate = UnitPrice.LatestValuation();
            if (latestDate == null)
            {
                return null;
            }

            double latestValue = latestDate.Value * Shares.LatestValue() * GetCurrencyValue(latestDate.Day, currency);

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
            DailyValuation firstDate = UnitPrice.FirstValuation();
            if (firstDate == null)
            {
                return null;
            }

            double latestValue = firstDate.Value * Shares.FirstValue() * GetCurrencyValue(firstDate.Day, currency);

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
            DailyValuation perSharePrice = UnitPrice.Value(date);
            double value = perSharePrice?.Value * Shares.ValueOnOrBefore(date).Value * GetCurrencyValue(date, currency) ?? 0.0;
            return new DailyValuation(date, value);
        }

        /// <inheritdoc/>
        public override DailyValuation ValueBefore(DateTime date)
        {
            return ValueBefore(date, null);
        }

        /// <inheritdoc/>
        public DailyValuation ValueBefore(DateTime date, ICurrency currency)
        {
            DailyValuation val = UnitPrice.ValueBefore(date);
            if (val == null)
            {
                return new DailyValuation(date, 0.0);
            }

            double latestValue = Shares.ValueOnOrBefore(date).Value * val.Value * GetCurrencyValue(val.Day, currency);
            return new DailyValuation(date, latestValue);
        }

        /// <inheritdoc/>
        public override DailyValuation ValuationOnOrBefore(DateTime date)
        {
            return ValuationOnOrBefore(date, null);
        }

        /// <inheritdoc/>
        public DailyValuation ValuationOnOrBefore(DateTime date, ICurrency currency)
        {
            DailyValuation val = UnitPrice.ValueOnOrBefore(date);
            if (val == null)
            {
                return new DailyValuation(date, 0.0);
            }

            double latestValue = Shares.ValueOnOrBefore(date).Value * val.Value * GetCurrencyValue(val.Day, currency);
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
