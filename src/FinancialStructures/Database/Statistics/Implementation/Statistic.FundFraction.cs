using System;
using System.Globalization;
using Effanville.Common.Structure.Extensions;
using Effanville.FinancialStructures.Database.Extensions.Statistics;
using Effanville.FinancialStructures.FinanceStructures;
using Effanville.FinancialStructures.NamingStructures;

namespace Effanville.FinancialStructures.Database.Statistics.Implementation
{
    /// <summary>
    /// Statistic for the <see cref="Statistic.FundFraction"/> enum value.
    /// This calculates the fraction held in an account out of the total held
    /// in the portfolio.
    /// </summary>
    internal class StatisticFundFraction : StatisticBase
    {
        /// <summary>
        /// <inheritdoc/> Overrides default by truncating to 4 d.p.
        /// </summary>
        public override string StringValue => Value.Truncate(4).ToString(CultureInfo.InvariantCulture);

        /// <summary>
        /// Default constructor setting Statistic type.
        /// </summary>
        internal StatisticFundFraction()
            : base(Statistic.FundFraction)
        {
        }


        /// <inheritdoc/>
        public override void Calculate(IPortfolio portfolio, IValueList valueList, DateTime date)
        {
            Value = portfolio.Fraction(valueList.AccountType.ToTotals(), valueList.AccountType, valueList.Names.ToTwoName(), date);
        }

        /// <inheritdoc/>
        public override void Calculate(IPortfolio portfolio, DateTime date, Totals total, TwoName name)
        {
            Value = portfolio.TotalFraction(total, name, date);
        }
    }
}
