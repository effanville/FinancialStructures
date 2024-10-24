﻿using System;
using System.Collections.Generic;
using System.Linq;

using Effanville.FinancialStructures.Database.Extensions.Statistics;
using Effanville.FinancialStructures.FinanceStructures;
using Effanville.FinancialStructures.NamingStructures;

namespace Effanville.FinancialStructures.Database.Statistics.Implementation
{
    internal class StatisticNumberEntries : StatisticBase
    {
        internal StatisticNumberEntries()
            : base(Statistic.NumberEntries)
        {
        }

        /// <inheritdoc/>
        public override void Calculate(IPortfolio portfolio, DateTime date, Account account, TwoName name)
        {
            if (!portfolio.TryGetAccount(account, name, out IValueList desired))
            {
                return;
            }

            Value = desired.Count();
        }

        /// <inheritdoc/>
        public override void Calculate(IPortfolio portfolio, DateTime date, Totals total, TwoName name)
        {
            Dictionary<DateTime, int> distribution = portfolio.EntryDistribution(total, name);
            Value = distribution.Count();
        }
    }
}
