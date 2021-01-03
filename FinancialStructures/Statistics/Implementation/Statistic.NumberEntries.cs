using System.Linq;
using FinancialStructures.Database;
using FinancialStructures.Database.Statistics;
using FinancialStructures.NamingStructures;

namespace FinancialStructures.Statistics
{
    internal class StatisticNumberEntries : StatisticBase
    {
        internal StatisticNumberEntries()
            : base(Statistic.NumberEntries)
        {
        }

        /// <inheritdoc/>
        public override void Calculate(IPortfolio portfolio, Account account, TwoName name)
        {
            if (!portfolio.TryGetAccount(account, name, out var desired))
            {
                return;
            }

            Value = desired.Count();
        }

        /// <inheritdoc/>
        public override void Calculate(IPortfolio portfolio, Totals total, TwoName name)
        {
            var distribution = portfolio.EntryDistribution(total, name);
            Value = distribution.Count();
        }
    }
}
