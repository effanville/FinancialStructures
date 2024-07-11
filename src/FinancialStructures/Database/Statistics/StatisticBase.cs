using System;

using Effanville.Common.Structure.Extensions;
using Effanville.FinancialStructures.FinanceStructures;
using Effanville.FinancialStructures.NamingStructures;

namespace Effanville.FinancialStructures.Database.Statistics
{
    /// <summary>
    /// A base of common statistic method implementations.
    /// </summary>
    internal class StatisticBase : IStatistic
    {
        protected string fCurrency;

        /// <inheritdoc/>
        public Statistic StatType
        {
            get;
        }

        /// <inheritdoc/>
        public double Value
        {
            get;
            protected set;
        }

        /// <inheritdoc/>
        public virtual string StringValue
        {
            get;
            protected set;
        }

        /// <inheritdoc/>
        public bool IsNumeric => true;

        /// <inheritdoc/>
        public object ValueAsObject => ToString();

        /// <summary>
        /// Constructor of an instance.
        /// </summary>
        internal StatisticBase(Statistic stat)
        {
            StatType = stat;
        }

        public virtual void Calculate(IPortfolio portfolio, IValueList valueList, DateTime date)
        {
        }

        /// <inheritdoc/>
        public virtual void Calculate(IPortfolio portfolio, DateTime date, Totals total, TwoName name)
        {
        }

        /// <inheritdoc/>
        public int CompareTo(IStatistic other)
        {
            if(!IsNumeric)
            {
                return StringValue.CompareTo(other.StringValue);
            }
            
            return Value.CompareTo(other.Value);
        }

        /// <inheritdoc/>
        public override string ToString()
        {
            if (!IsNumeric)
            {
                return StringValue;
            }

            if (fCurrency == null)
            {
                return Value.TruncateToString();
            }

            return Value.WithCurrencySymbol(fCurrency);
        }
    }
}
