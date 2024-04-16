using System;

using Effanville.FinancialStructures.Database.Extensions.Rates;
using Effanville.FinancialStructures.FinanceStructures;
using Effanville.FinancialStructures.NamingStructures;

namespace Effanville.FinancialStructures.Database.Statistics.Implementation
{
    internal class StatisticIRR5Y : StatisticBase
    {
        internal StatisticIRR5Y()
            : base(Statistic.IRR5Y)
        {
        }

        /// <inheritdoc/>
        public override void Calculate(IValueList valueList, IPortfolio portfolio, DateTime date, Account account,
            TwoName name)
        {
            Value = 100 * IRRCalcHelpers.CalcIRR(portfolio, account, valueList,date.AddMonths(-60), date);
        }

        /// <inheritdoc/>
        public override void Calculate(IPortfolio portfolio, DateTime date, Totals total, TwoName name)
        {
            Value = (100 * portfolio.TotalIRR(total, date.AddMonths(-60), date, name));
        }
    }
}
