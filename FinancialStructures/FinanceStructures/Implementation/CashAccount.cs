using System;
using FinancialStructures.Database;
using FinancialStructures.NamingStructures;
using Common.Structure.DataStructures;

namespace FinancialStructures.FinanceStructures.Implementation
{
    /// <summary>
    /// An account simulating a bank account.
    /// </summary>
    public class CashAccount : ValueList, IExchangableValueList
    {
        /// <inheritdoc/>
        protected override void OnDataEdit(object edited, EventArgs e)
        {
            base.OnDataEdit(edited, new PortfolioEventArgs(Account.BankAccount));
        }

        /// <inheritdoc/>
        public override IValueList Copy()
        {
            return new CashAccount(Names.Copy(), Values);
        }

        /// <inheritdoc/>
        public TimeList Amounts
        {
            get => Values;
            set => Values = value;
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
        internal CashAccount()
            : base()
        {
        }

        /// <inheritdoc/>
        public DailyValuation Value(DateTime date, ICurrency currency = null)
        {
            DailyValuation perSharePrice = Values.ValueZeroBefore(date);
            double value = perSharePrice?.Value * GetCurrencyValue(date, currency) ?? 0.0;
            return new DailyValuation(date, value);
        }

        /// <inheritdoc/>
        public DailyValuation LatestValue(ICurrency currency)
        {
            DailyValuation latestDate = Values.LatestValuation();
            if (latestDate == null)
            {
                return new DailyValuation(DateTime.Today, 0.0);
            }

            double latestValue = latestDate.Value * GetCurrencyValue(latestDate.Day, currency);

            return new DailyValuation(latestDate.Day, latestValue);
        }

        /// <inheritdoc/>
        public DailyValuation RecentPreviousValue(DateTime date, ICurrency currency)
        {
            DailyValuation val = Values.RecentPreviousValue(date);

            if (val == null)
            {
                return new DailyValuation(date, 0.0);
            }

            val.Value *= GetCurrencyValue(val.Day, currency);
            return val;
        }

        /// <inheritdoc/>
        public DailyValuation FirstValue(ICurrency currency)
        {
            DailyValuation firstDate = Values.FirstValuation();
            if (firstDate == null)
            {
                return new DailyValuation(DateTime.Today, 0.0);
            }

            double latestValue = firstDate.Value * GetCurrencyValue(firstDate.Day, currency);

            return new DailyValuation(firstDate.Day, latestValue);
        }

        /// <inheritdoc/>
        public DailyValuation NearestEarlierValuation(DateTime date, ICurrency currency = null)
        {
            DailyValuation value = Values.NearestEarlierValue(date);
            value.Value *= GetCurrencyValue(value.Day, currency);
            return value;
        }

        private static double GetCurrencyValue(DateTime date, ICurrency currency)
        {
            return currency == null ? 1.0 : currency.Value(date)?.Value ?? 1.0;
        }
    }
}
