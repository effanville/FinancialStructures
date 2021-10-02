﻿using System;
using FinancialStructures.Database;
using FinancialStructures.Database.Statistics;
using FinancialStructures.NamingStructures;

namespace FinancialStructures.Statistics
{
    internal class StatisticIRR3M : StatisticBase
    {
        internal StatisticIRR3M()
            : base(Statistic.IRR3M)
        {
        }

        /// <inheritdoc/>
        public override void Calculate(IPortfolio portfolio, Account account, TwoName name)
        {
            Value = 100 * portfolio.IRR(account, name, DateTime.Today.AddMonths(-3), DateTime.Today);
        }

        /// <inheritdoc/>
        public override void Calculate(IPortfolio portfolio, Totals total, TwoName name)
        {
            Value = (100 * portfolio.TotalIRR(total, DateTime.Today.AddMonths(-3), DateTime.Today, name));
        }
    }
}
