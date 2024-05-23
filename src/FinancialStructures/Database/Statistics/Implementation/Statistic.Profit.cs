using System;

using Effanville.FinancialStructures.Database.Extensions.Statistics;
using Effanville.FinancialStructures.FinanceStructures;
using Effanville.FinancialStructures.NamingStructures;
using Effanville.FinancialStructures.ValueCalculators;

namespace Effanville.FinancialStructures.Database.Statistics.Implementation
{
    internal class StatisticProfit : StatisticBase
    {
        internal StatisticProfit()
            : base(Statistic.Profit)
        {
        }

        /// <inheritdoc/>
        public override void Calculate(IValueList valueList, IPortfolio portfolio, DateTime date, Account account,
            TwoName name)
        {
            fCurrency = portfolio.BaseCurrency;
            Value = (double)valueList.CalculateValue(
                ProfitCalculators.DefaultCalculator,
                ProfitCalculators.Calculators(portfolio));
        }
        
        /// <inheritdoc/>
        public override void Calculate(IPortfolio portfolio, DateTime date, Totals total, TwoName name)
        {
            Value = (double)portfolio.TotalProfit(total, name);
            fCurrency = portfolio.BaseCurrency;
        }
    }
}
