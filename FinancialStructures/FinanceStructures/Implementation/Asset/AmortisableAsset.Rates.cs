using System;
using System.Collections.Generic;
using Common.Structure.DataStructures;
using Common.Structure.MathLibrary.Finance;

namespace FinancialStructures.FinanceStructures.Implementation.Asset
{
    public partial class AmortisableAsset
    {
        /// <inheritdoc/>
        public List<DailyValuation> PaymentsBetween(DateTime earlierDate, DateTime laterDate, ICurrency currency = null)
        {
            if (!Any())
            {
                return null;
            }

            List<DailyValuation> values = Payments.GetValuesBetween(earlierDate, laterDate);
            foreach (DailyValuation value in values)
            {
                value.Value *= GetCurrencyValue(value.Day, currency);
            }

            return values;
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

            List<DailyValuation> invs = PaymentsBetween(earlierDate, laterDate, currency);
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
