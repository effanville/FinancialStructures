using System.Linq;
using FinancialStructures.Database;
using FinancialStructures.Database.Statistics;
using FinancialStructures.NamingStructures;

namespace FinancialStructures.Statistics
{
    internal class StatisticInvestment : StatisticBase
    {
        internal StatisticInvestment()
            : base(Statistic.Investment)
        {
        }

        /// <inheritdoc/>
        public override void Calculate(IPortfolio portfolio, Account account, TwoName name)
        {
            double sum = 0.0;
            var investments = portfolio.Investments(account, name);
            if (investments != null && investments.Any())
            {
                foreach (var investment in investments)
                {
                    sum += investment.Instance.Value;
                }
            }

            Value = sum;
        }

        /// <inheritdoc/>
        public override void Calculate(IPortfolio portfolio, Totals total, TwoName name)
        {
            double sum = 0.0;
            var investments = portfolio.TotalInvestments(total, name);
            if (investments != null && investments.Any())
            {
                foreach (var investment in investments)
                {
                    sum += investment.Instance.Value;
                }
            }

            Value = sum;
        }
    }
}
