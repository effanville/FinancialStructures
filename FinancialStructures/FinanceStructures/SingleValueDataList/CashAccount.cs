using System;
using FinancialStructures.FinanceInterfaces;
using FinancialStructures.NamingStructures;
using StructureCommon.DataStructures;

namespace FinancialStructures.FinanceStructures
{
    public class CashAccount : SingleValueDataList, ICashAccount
    {
        public new ICashAccount Copy()
        {
            return new CashAccount(Names, Values);
        }

        public TimeList Amounts
        {
            get
            {
                return Values;
            }
            set
            {
                Values = value;
            }
        }

        /// <summary>
        /// Default constructor where no data is known.
        /// </summary>
        internal CashAccount(NameData names)
            : base(names)
        {
        }

        /// <summary>
        /// Constructor used when data is known.
        /// </summary>
        private CashAccount(NameData names, TimeList amounts)
            : base(names, amounts)
        {
        }

        /// <summary>
        /// Parameterless constructor for serialisation.
        /// </summary>
        private CashAccount()
            : base()
        {
        }

        /// <summary>
        /// Returns the interpolated value of the security on the date provided.
        /// </summary>
        public DailyValuation Value(DateTime date, ICurrency currency = null)
        {
            DailyValuation perSharePrice = Values.ValueZeroBefore(date);
            double currencyValue = currency == null ? 1.0 : currency.Value(date).Value;
            double value = perSharePrice.Value * currencyValue;
            return new DailyValuation(date, value);
        }

        /// <summary>
        /// Returns the latest valuation of the OldCashAccount.
        /// </summary>
        public DailyValuation LatestValue(ICurrency currency = null)
        {
            DailyValuation latestDate = Values.LatestValuation();
            if (latestDate == null)
            {
                return new DailyValuation(DateTime.Today, 0.0);
                ;
            }

            double currencyValue = currency == null ? 1.0 : currency.Value(latestDate.Day).Value;
            double latestValue = latestDate.Value * currencyValue;

            return new DailyValuation(latestDate.Day, latestValue);
        }

        public DailyValuation LastEarlierValuation(DateTime date, ICurrency currency = null)
        {
            DailyValuation val = Values.RecentPreviousValue(date);
            double currencyValue = currency == null ? 1.0 : currency.Value(val.Day).Value;
            if (val == null)
            {
                return new DailyValuation(date, 0.0);
            }
            val.Value *= currencyValue;
            return val;
        }

        /// <summary>
        /// Returns the first valuation of the OldCashAccount.
        /// </summary>
        public DailyValuation FirstValue(ICurrency currency = null)
        {
            DailyValuation firstDate = Values.FirstValuation();
            if (firstDate == null)
            {
                return new DailyValuation(DateTime.Today, 0.0);
                ;
            }

            double currencyValue = currency == null ? 1.0 : currency.Value(firstDate.Day).Value;
            double latestValue = firstDate.Value * currencyValue;

            return new DailyValuation(firstDate.Day, latestValue);
        }

        /// <summary>
        /// Returns the latest earlier valuation of the OldCashAccount to <paramref name="date"/>.
        /// </summary>
        public DailyValuation NearestEarlierValuation(DateTime date, ICurrency currency = null)
        {
            DailyValuation value = Values.NearestEarlierValue(date);
            double currencyValue = currency == null ? 1.0 : currency.Value(value.Day).Value;
            value.SetValue(value.Value * currencyValue);
            return value;
        }
    }
}
