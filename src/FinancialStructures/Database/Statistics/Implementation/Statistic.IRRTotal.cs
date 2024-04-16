using System;

using Effanville.FinancialStructures.Database.Extensions.Rates;
using Effanville.FinancialStructures.FinanceStructures;
using Effanville.FinancialStructures.NamingStructures;

namespace Effanville.FinancialStructures.Database.Statistics.Implementation
{
    internal class StatisticIRRTotal : StatisticBase
    {
        internal StatisticIRRTotal()
            : base(Statistic.IRRTotal)
        {
        }

        /// <inheritdoc/>
        public override void Calculate(IValueList valueList, IPortfolio portfolio, DateTime date, Account account,
            TwoName name)
        {
            if (!valueList.Any())
            {
                return;
            }

            if (valueList is ISecurity security)
            {
                ICurrency currency = portfolio.Currency(security);
                Value = 100 * security.IRR(currency);
                return;
            }
            Value = 100 * valueList.CAR(
                valueList.FirstValue().Day, 
                valueList.LatestValue().Day);
        }

        /// <inheritdoc/>
        public override void Calculate(IPortfolio portfolio, DateTime date, Totals total, TwoName name)
        {
            Value = 100 * portfolio.TotalIRR(total, name);
        }
    }
}
