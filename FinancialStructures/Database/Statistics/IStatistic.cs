using System;
using FinancialStructures.NamingStructures;

namespace FinancialStructures.Database.Statistics
{
    /// <summary>
    /// Base for a statistic of an account
    /// </summary>
    public interface IStatistic : IComparable<IStatistic>
    {
        /// <summary>
        /// The type of this statistic, e.g. is it IRR or a total value.
        /// </summary>
        Statistic StatType
        {
            get;
        }

        /// <summary>
        /// Gets whether the statistic is numeric.
        /// </summary>
        bool IsNumeric
        {
            get;
        }

        /// <summary>
        /// The value of this statistic.
        /// </summary>
        double Value
        {
            get;
        }

        /// <summary>
        /// The value to be displayed.
        /// </summary>
        string StringValue
        {
            get;
        }

        /// <summary>
        /// The value to be displayed as an object.
        /// </summary>
        object ValueAsObject
        {
            get;
        }

        /// <summary>
        /// Calculates the value for this statistic from the account in question.
        /// </summary>
        void Calculate(IPortfolio portfolio, Account account, TwoName name);

        /// <summary>
        /// Calculates the total statistic from the total type in question.
        /// </summary>
        void Calculate(IPortfolio portfolio, Totals total, TwoName name);

        /// <summary>
        /// The string representation of the value in the statistic.
        /// </summary>
        string ToString();
    }
}
