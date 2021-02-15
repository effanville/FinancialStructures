using System;
using FinancialStructures.Database;
using FinancialStructures.NamingStructures;

namespace FinancialStructures.Statistics
{
    internal class StatisticNumberUnits : StatisticBase
    {
        internal StatisticNumberUnits()
            : base(Statistic.NumberUnits)
        {
        }

        /// <inheritdoc/>
        public override void Calculate(IPortfolio portfolio, Account account, TwoName name)
        {
            Value = portfolio.SecurityPrices(name, DateTime.Today, SecurityDataStream.NumberOfShares);
        }

        /// <inheritdoc/>
        public override void Calculate(IPortfolio portfolio, Totals total, TwoName name)
        {
            Value = 0.0;
        }
    }
}
