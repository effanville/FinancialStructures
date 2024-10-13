using System;

using Effanville.FinancialStructures.Database.Extensions;
using Effanville.FinancialStructures.FinanceStructures;
using Effanville.FinancialStructures.FinanceStructures.Extensions;
using Effanville.FinancialStructures.NamingStructures;

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
            ICurrency currency = portfolio.Currency(valueList);
            Value = (double)valueList.CalculateValue(
                vl => vl.Debt(currency, date));
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
                vl =>
                {
                    ICurrency currency = portfolio.Currency(vl);
                    return vl.Debt(currency, date);
                });
        }
    }
}
