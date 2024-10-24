﻿using System;
using System.Collections.Generic;

using Effanville.Common.Structure.DataStructures;
using Effanville.Common.Structure.MathLibrary.Finance;

namespace Effanville.FinancialStructures.FinanceStructures.Implementation
{
    public partial class Security
    {
        /// <inheritdoc/>
        public decimal TotalInvestment(ICurrency currency = null)
        {
            if (!Any())
            {
                return 0.0m;
            }

            List<DailyValuation> investments = InvestmentsBetween(FirstValue().Day, LatestValue().Day, currency);
            decimal sum = 0;
            foreach (DailyValuation investment in investments)
            {
                sum += investment.Value;
            }

            return sum;
        }

        /// <inheritdoc/>
        public List<DailyValuation> InvestmentsBetween(DateTime earlierDate, DateTime laterDate, ICurrency currency = null)
        {
            if (!Any())
            {
                return null;
            }

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

            var numShares = Shares.LatestValuation();
            if (numShares == null)
            {
                return null;
            }

            DateTime date = numShares.Value != 0m ? latestDate.Day : numShares.Day;
            decimal latestValue = latestDate.Value * numShares.Value * GetCurrencyValue(latestDate.Day, currency);

            return new DailyValuation(date, latestValue);
        }

        /// <inheritdoc/>
        public override DailyValuation FirstValue()
        {
            return FirstValue(null);
        }

        /// <inheritdoc/>
        public DailyValuation FirstValue(ICurrency currency)
        {
            DailyValuation sharesFirstDate = Shares.FirstValuation();
            if (sharesFirstDate == null)
            {
                return null;
            }

            decimal latestValue = sharesFirstDate.Value * UnitPrice.Value(sharesFirstDate.Day)?.Value * GetCurrencyValue(sharesFirstDate.Day, currency) ?? 0.0m;

            return new DailyValuation(sharesFirstDate.Day, latestValue);
        }

        /// <inheritdoc/>
        public override DailyValuation Value(DateTime date)
        {
            return Value(date, null);
        }

        /// <inheritdoc/>
        public DailyValuation Value(DateTime date, ICurrency currency)
        {
            // if we have never held the security, have no value.
            if (!Shares.Any())
            {
                return null;
            }

            DailyValuation perSharePrice = UnitPrice.Value(date);
            decimal value = perSharePrice?.Value * Shares.ValueOnOrBefore(date)?.Value * GetCurrencyValue(date, currency) ?? 0.0m;
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
            // if we have never held the security, have no value.
            if (!Shares.Any())
            {
                return null;
            }

            DailyValuation val = UnitPrice.ValueBefore(date);
            if (val == null)
            {
                return new DailyValuation(date, 0.0m);
            }

            decimal latestValue = Shares.ValueBefore(date)?.Value * val.Value * GetCurrencyValue(val.Day, currency) ?? 0.0m;
            return new DailyValuation(date, latestValue);
        }

        /// <inheritdoc/>
        public override DailyValuation ValueOnOrBefore(DateTime date)
        {
            return ValueOnOrBefore(date, null);
        }

        /// <inheritdoc/>
        public DailyValuation ValueOnOrBefore(DateTime date, ICurrency currency)
        {
            // if we have never held the security, have no value.
            if (!Shares.Any())
            {
                return null;
            }

            DailyValuation val = UnitPrice.ValueOnOrBefore(date);
            if (val == null)
            {
                return new DailyValuation(date, 0.0m);
            }

            decimal latestValue = Shares.ValueOnOrBefore(date)?.Value * val.Value * GetCurrencyValue(val.Day, currency) ?? 0.0m;
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
            return FinanceFunctions.CAR(Value(earlierTime, currency), Value(laterTime, currency));
        }

        /// <inheritdoc/>
        public double IRR(DateTime earlierDate, DateTime laterDate, ICurrency currency = null)
        {
            if (!Any())
            {
                return double.NaN;
            }

            List<DailyValuation> invs = InvestmentsBetween(earlierDate, laterDate, currency);
            DailyValuation latestTime = Value(laterDate, currency);
            DailyValuation firstTime = Value(earlierDate, currency);
            return FinanceFunctions.IRR(firstTime, invs, latestTime, 10);

        }

        /// <inheritdoc/>
        public double IRR(ICurrency currency = null)
        {
            if (!Any())
            {
                return double.NaN;
            }

            return IRR(FirstValue().Day, LatestValue().Day, currency);
        }
    }
}
