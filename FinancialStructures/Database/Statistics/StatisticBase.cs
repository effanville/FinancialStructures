using System.Globalization;
using Common.Structure.Extensions;
using FinancialStructures.NamingStructures;

namespace FinancialStructures.Database.Statistics
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
            if (!IsNumeric)
            {
                return StringValue;
            }

            if (fCurrency == null)
            {
                return Value.TruncateToString();
            }

            string format = FormatName();
            if (format == fCurrency)
            {
                return $"{fCurrency}{Value.TruncateToString()}";
            }

            CultureInfo culture = CultureInfo.CreateSpecificCulture(format);
            return Value.ToString("C", culture);
        }

        private string FormatName()
        {
            switch (fCurrency)
            {
                case "GBP":
                case "£":
                    return "en-GB";
                case "HKD":
                    return "zh-HK";
                case "USD":
                    return "en-US";
                case "SAR":
                    return "en-ZA";
                default:
                    return fCurrency;
            }
        }
    }
}
