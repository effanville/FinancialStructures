using Common.Structure.Extensions;
using FinancialStructures.Database;
using FinancialStructures.NamingStructures;

namespace FinancialStructures.Statistics
{
    /// <summary>
    /// A base of common statistic method implementations.
    /// </summary>
    internal class StatisticBase : IStatistic
    {
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

        public virtual void Calculate(IPortfolio portfolio, Account account, TwoName name)
        {
        }

        /// <inheritdoc/>
        public virtual void Calculate(IPortfolio portfolio, Totals total, TwoName name)
        {
        }

        /// <inheritdoc/>
        public int CompareTo(IStatistic other)
        {
            return Value.CompareTo(other.Value);
        }

        /// <inheritdoc/>
        public override string ToString()
        {
            return IsNumeric ? Value.TruncateToString() : StringValue;
        }
    }
}
