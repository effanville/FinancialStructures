using System;
using FinancialStructures.Database;
using FinancialStructures.NamingStructures;

namespace FinancialStructures.Statistics
{
    internal class StatisticUnitPrice : StatisticBase
    {
        internal StatisticUnitPrice()
            : base(Statistic.UnitPrice)
        {
        }

        /// <inheritdoc/>
        public override void Calculate(IPortfolio portfolio, Account account, TwoName name)
        {
            Value = portfolio.SecurityPrices(name, DateTime.Today, SecurityDataStream.SharePrice);
        }

        /// <inheritdoc/>
        public override void Calculate(IPortfolio portfolio, Totals total, TwoName name)
        {
            if (total == Totals.Company)
            {
                Value = 0;
            }

            if (total == Totals.All)
            {
                Value = 0;
            }
        }
    }
}
