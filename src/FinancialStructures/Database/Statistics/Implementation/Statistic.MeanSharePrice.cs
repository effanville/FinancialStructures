using System;

using Effanville.FinancialStructures.DataStructures;
using Effanville.FinancialStructures.FinanceStructures;
using Effanville.FinancialStructures.FinanceStructures.Statistics;
using Effanville.FinancialStructures.NamingStructures;

namespace Effanville.FinancialStructures.Database.Statistics.Implementation
{
    internal class StatisticMeanSharePrice : StatisticBase
    {
        internal StatisticMeanSharePrice()
            : base(Statistic.MeanSharePrice)
        {
        }

        /// <inheritdoc/>
        public override void Calculate(IPortfolio portfolio, DateTime date, Account account, TwoName name)
        {
            if (!portfolio.TryGetAccount(account, name, out IValueList desired))
            {
                Value = 0.0;
            }
            if (desired is ISecurity security)
            {
                fCurrency = portfolio.BaseCurrency;
                Value = (double)security.MeanSharePrice(TradeType.Buy);
            }
        }

        /// <inheritdoc/>
        public override void Calculate(IPortfolio portfolio, DateTime date, Totals total, TwoName name)
        {
            if (total == Totals.Company || total == Totals.All)
            {
                Value = 0;
            }
        }
    }
}
