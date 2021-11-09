using System;
using System.Collections.Generic;
using System.Linq;
using FinancialStructures.Database.Extensions.Statistics;
using FinancialStructures.FinanceStructures;
using FinancialStructures.NamingStructures;

namespace FinancialStructures.Database.Statistics.Implementation
{
    internal class StatisticNumberEntries : StatisticBase
    {
        internal StatisticNumberEntries()
            : base(Statistic.NumberEntries)
        {
        }

        /// <inheritdoc/>
        public override void Calculate(IPortfolio portfolio, Account account, TwoName name)
        {
            if (!portfolio.TryGetAccount(account, name, out IValueList desired))
            {
                return;
            }

            Value = desired.Count();
        }

        /// <inheritdoc/>
        public override void Calculate(IPortfolio portfolio, Totals total, TwoName name)
        {
            Dictionary<DateTime, int> distribution = portfolio.EntryDistribution(total, name);
            Value = distribution.Count();
        }
    }
}
