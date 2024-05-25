using System;
using System.Collections.Generic;
using System.Linq;

using Effanville.Common.Structure.DataStructures;
using Effanville.Common.Structure.NamingStructures;
using Effanville.FinancialStructures.Database.Extensions.Values;
using Effanville.FinancialStructures.FinanceStructures;
using Effanville.FinancialStructures.NamingStructures;
using Effanville.FinancialStructures.ValueCalculators;

namespace Effanville.FinancialStructures.Database.Statistics.Implementation
{
    internal class StatisticInvestment : StatisticBase
    {
        internal StatisticInvestment()
            : base(Statistic.Investment)
        {
        }

        /// <inheritdoc/>
        public override void Calculate(IPortfolio portfolio, IValueList valueList, DateTime date)
        {
            fCurrency = portfolio.BaseCurrency;
            Value = (double)valueList.CalculateValue(
                InvestmentCalculators.DefaultCalculator,
                InvestmentCalculators.Calculators(portfolio, date));
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
