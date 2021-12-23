using System;
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
        public override void Calculate(IPortfolio portfolio, DateTime date, Account account, TwoName name)
        {
            decimal sum = 0.0m;
            List<Labelled<TwoName, DailyValuation>> investments = portfolio.Investments(account, name);
            if (investments != null && investments.Any())
            {
                foreach (Labelled<TwoName, DailyValuation> investment in investments)
                {
                    sum += investment.Instance.Value;
                }
            }

            fCurrency = portfolio.BaseCurrency;
            Value = (double)sum;
        }

        /// <inheritdoc/>
        public override void Calculate(IPortfolio portfolio, DateTime date, Totals total, TwoName name)
        {
            decimal sum = 0.0m;
            List<Labelled<TwoName, DailyValuation>> investments = portfolio.TotalInvestments(total, name);
            if (investments != null && investments.Any())
            {
                foreach (Labelled<TwoName, DailyValuation> investment in investments)
                {
                    sum += investment.Instance.Value;
                }
            }

            fCurrency = portfolio.BaseCurrency;
            Value = (double)sum;
        }
    }
}
