﻿using System;

using Effanville.Common.Structure.Extensions;

using FinancialStructures.Database.Extensions.Statistics;
using FinancialStructures.Database.Extensions.Values;
using FinancialStructures.FinanceStructures;
using FinancialStructures.NamingStructures;

namespace FinancialStructures.Database.Statistics.Implementation
{
    internal class StatisticEntryYearDensity : StatisticBase
    {
        internal StatisticEntryYearDensity()
            : base(Statistic.EntryYearDensity)
        {
        }

        /// <inheritdoc/>
        public override void Calculate(IPortfolio portfolio, DateTime date, Account account, TwoName name)
        {
            if (!portfolio.TryGetAccount(account, name, out IValueList bankAcc))
            {
                return;
            }

            Value = ((bankAcc.LatestValue()?.Day - bankAcc.FirstValue()?.Day)?.Days ?? 0) / ((double)365 * bankAcc.Count());
        }

        /// <inheritdoc/>
        public override void Calculate(IPortfolio portfolio, DateTime date, Totals total, TwoName name)
        {
            Value = (portfolio.LatestDate(total, name) - portfolio.FirstValueDate(total, name)).Days / ((double)365 * portfolio.EntryDistribution(total, name).Count);
        }

        /// <inheritdoc/>
        public override string ToString()
        {
            return IsNumeric ? Value.TruncateToString(4) : StringValue;
        }
    }
}