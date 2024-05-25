using System;

using Effanville.FinancialStructures.Database.Extensions;
using Effanville.FinancialStructures.FinanceStructures;
using Effanville.FinancialStructures.NamingStructures;
using Effanville.FinancialStructures.ValueCalculators;

namespace Effanville.FinancialStructures.Database.Statistics.Implementation
{
    internal class StatisticDebt : StatisticBase
    {
        internal StatisticDebt()
            : base(Statistic.Debt)
        {
        }

        /// <inheritdoc/>
        public override void Calculate(IPortfolio portfolio, IValueList valueList, DateTime date)
        {
            fCurrency = portfolio.BaseCurrency;
            Value = (double)valueList.CalculateValue(
                DebtCalculators.DefaultCalculator(date),
                DebtCalculators.Calculators(portfolio, date));
        }

        /// <inheritdoc/>
        public override void Calculate(IPortfolio portfolio, DateTime date, Totals total, TwoName name)
        {
            fCurrency = portfolio.BaseCurrency;
            Value = (double)portfolio.CalculateAggregateValue(
                total,
                name,
                (acc, n) => acc.ToAccount() == Account.Asset,
                0.0m,
                (a,b) => a + b,
                DebtCalculators.DefaultCalculator(date),
                DebtCalculators.Calculators(portfolio, date));
        }
    }
}
