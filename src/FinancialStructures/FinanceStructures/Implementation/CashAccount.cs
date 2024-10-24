﻿using System;

using Effanville.Common.Structure.DataStructures;
using Effanville.FinancialStructures.Database;
using Effanville.FinancialStructures.NamingStructures;

namespace Effanville.FinancialStructures.FinanceStructures.Implementation
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
        internal CashAccount(NameData names, TimeList amounts)
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
            decimal value = perSharePrice?.Value * GetCurrencyValue(date, currency) ?? 0.0m;
            return new DailyValuation(date, value);
        }

        /// <inheritdoc/>
        public DailyValuation LatestValue(ICurrency currency)
        {
            DailyValuation latestDate = Values.LatestValuation();
            if (latestDate == null)
            {
                return new DailyValuation(DateTime.Today, 0.0m);
            }

            decimal latestValue = latestDate.Value * GetCurrencyValue(latestDate.Day, currency);

            return new DailyValuation(latestDate.Day, latestValue);
        }

        /// <inheritdoc/>
        public DailyValuation ValueBefore(DateTime date, ICurrency currency)
        {
            DailyValuation val = Values.ValueBefore(date);

            if (val == null)
            {
                return new DailyValuation(date, 0.0m);
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
                return new DailyValuation(DateTime.Today, 0.0m);
            }

            decimal latestValue = firstDate.Value * GetCurrencyValue(firstDate.Day, currency);

            return new DailyValuation(firstDate.Day, latestValue);
        }

        /// <inheritdoc/>
        public DailyValuation ValueOnOrBefore(DateTime date, ICurrency currency = null)
        {
            DailyValuation value = Values.ValueOnOrBefore(date);
            if (value == null)
            {
                return new DailyValuation(date, 0.0m);
            }
            var currencyValue = GetCurrencyValue(value.Day, currency);
            value.Value *= currencyValue;
            return value;
        }

        private static decimal GetCurrencyValue(DateTime date, ICurrency currency)
        {
            return currency == null ? 1.0m : currency.Value(date)?.Value ?? 1.0m;
        }
    }
}
