using FinancialStructures.Database;
using FinancialStructures.DataStructures;
using FinancialStructures.FinanceStructures;
using FinancialStructures.FinanceStructures.Statistics;
using FinancialStructures.NamingStructures;

namespace FinancialStructures.Statistics
{
    internal class StatisticMeanSharePrice : StatisticBase
    {
        internal StatisticMeanSharePrice()
            : base(Statistic.MeanSharePrice)
        {
        }

        /// <inheritdoc/>
        public override void Calculate(IPortfolio portfolio, Account account, TwoName name)
        {
            if (!portfolio.TryGetAccount(account, name, out IValueList desired))
            {
                Value = 0.0;
            }
            if (desired is ISecurity security)
            {
                Value = security.MeanSharePrice(TradeType.Buy);
            }
        }

        /// <inheritdoc/>
        public override void Calculate(IPortfolio portfolio, Totals total, TwoName name)
        {
            if (total == Totals.Company || total == Totals.All)
            {
                Value = 0;
            }
        }
    }
}
