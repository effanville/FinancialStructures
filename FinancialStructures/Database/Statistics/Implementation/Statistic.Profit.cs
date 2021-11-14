using FinancialStructures.Database.Extensions.Statistics;
using FinancialStructures.NamingStructures;

namespace FinancialStructures.Database.Statistics.Implementation
{
    internal class StatisticProfit : StatisticBase
    {
        internal StatisticProfit()
            : base(Statistic.Profit)
        {
        }

        /// <inheritdoc/>
        public override void Calculate(IPortfolio portfolio, Account account, TwoName name)
        {
            fCurrency = portfolio.BaseCurrency;
            Value = (double)portfolio.Profit(account, name);
        }

        /// <inheritdoc/>
        public override void Calculate(IPortfolio portfolio, Totals total, TwoName name)
        {
            Value = (double)portfolio.TotalProfit(total, name);
            fCurrency = portfolio.BaseCurrency;
        }
    }
}
