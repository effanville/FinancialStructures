using FinancialStructures.Database.Extensions.Rates;
using FinancialStructures.NamingStructures;

namespace FinancialStructures.Database.Statistics.Implementation
{
    internal class StatisticIRRTotal : StatisticBase
    {
        internal StatisticIRRTotal()
            : base(Statistic.IRRTotal)
        {
        }

        /// <inheritdoc/>
        public override void Calculate(IPortfolio portfolio, Account account, TwoName name)
        {
            Value = (100 * portfolio.IRR(account, name));
        }

        /// <inheritdoc/>
        public override void Calculate(IPortfolio portfolio, Totals total, TwoName name)
        {
            Value = 100 * portfolio.TotalIRR(total, name);
        }
    }
}
