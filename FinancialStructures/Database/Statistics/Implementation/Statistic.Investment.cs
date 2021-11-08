using System.Collections.Generic;
using System.Linq;
using Common.Structure.DataStructures;
using Common.Structure.NamingStructures;
using FinancialStructures.Database.Extensions.Values;
using FinancialStructures.NamingStructures;

namespace FinancialStructures.Database.Statistics.Implementation
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
            List<Labelled<TwoName, DailyValuation>> investments = portfolio.Investments(account, name);
            if (investments != null && investments.Any())
            {
                foreach (Labelled<TwoName, DailyValuation> investment in investments)
                {
                    sum += investment.Instance.Value;
                }
            }

            fCurrency = portfolio.BaseCurrency;
            Value = sum;
        }

        /// <inheritdoc/>
        public override void Calculate(IPortfolio portfolio, Totals total, TwoName name)
        {
            double sum = 0.0;
            List<Labelled<TwoName, DailyValuation>> investments = portfolio.TotalInvestments(total, name);
            if (investments != null && investments.Any())
            {
                foreach (Labelled<TwoName, DailyValuation> investment in investments)
                {
                    sum += investment.Instance.Value;
                }
            }

            fCurrency = portfolio.BaseCurrency;
            Value = sum;
        }
    }
}
