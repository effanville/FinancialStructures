using System;
using Effanville.FinancialStructures.Database.Extensions.Values;
using Effanville.FinancialStructures.FinanceStructures;
using Effanville.FinancialStructures.NamingStructures;

namespace Effanville.FinancialStructures.Database.Statistics.Implementation
{
    internal class StatisticLatestValue : StatisticBase
    {
        internal StatisticLatestValue()
            : base(Statistic.LatestValue)
        {
        }

        /// <inheritdoc/>
        public override void Calculate(IValueList valueList, IPortfolio portfolio, DateTime date, Account account,
            TwoName name)
        {
            fCurrency = portfolio.BaseCurrency;
            Value = (double)valueList.CalculateValue(
                ValueCalculators.ValueCalculators.DefaultCalculator(date),
                ValueCalculators.ValueCalculators.Calculators(portfolio, date),
                defaultValue: DefaultValue());
            return;

            decimal DefaultValue()
            {
                if (account == Account.Currency || account == Account.Benchmark)
                {
                    return 1.0m;
                }

                return 0.0m;
            }
        }

        /// <inheritdoc/>
        public override void Calculate(IPortfolio portfolio, DateTime date, Totals total, TwoName name)
        {
            Value = (double)portfolio.TotalValue(total, DateTime.Today, name);
            fCurrency = portfolio.BaseCurrency;
        }
    }
}
