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
            switch (account)
            {
                case Account.Security:
                {
                    if (!portfolio.TryGetSecurity(name, out var desired))
                    {
                        return;
                    }

                    Value = desired.Count();
                    return;
                }
                case Account.Benchmark:
                case Account.BankAccount:
                case Account.Currency:
                {
                    if (!portfolio.TryGetAccount(account, name, out var desired))
                    {
                        return;
                    }

                    Value = desired.Count();
                    return;
                }
                default:
                    return;
            }

        }

        /// <inheritdoc/>
        public override void Calculate(IPortfolio portfolio, Totals total, TwoName name)
        {
            var distribution = portfolio.EntryDistribution(total, name);
            Value = distribution.Count();
        }
    }
}
