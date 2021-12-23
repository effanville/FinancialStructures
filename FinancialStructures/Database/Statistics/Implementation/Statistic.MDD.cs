using System;
using FinancialStructures.Database.Extensions.Statistics;
using FinancialStructures.NamingStructures;

namespace FinancialStructures.Database.Statistics.Implementation
{
    internal class StatisticMDD : StatisticBase
    {
        internal StatisticMDD()
            : base(Statistic.MDD)
        {
        }

        /// <inheritdoc/>
        public override void Calculate(IPortfolio portfolio, DateTime date, Account account, TwoName name)
        {
            Value = portfolio.MDD(account, name);
        }

        /// <inheritdoc/>
        public override void Calculate(IPortfolio portfolio, DateTime date, Totals total, TwoName name)
        {
            Value = portfolio.TotalMDD(total, name);
        }
    }
}
