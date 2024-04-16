using System;

using Effanville.FinancialStructures.Database.Extensions;
using Effanville.FinancialStructures.FinanceStructures;
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
        public override void Calculate(IValueList valueList, IPortfolio portfolio, DateTime date, Account account,
            TwoName name)
        {
            fCurrency = portfolio.BaseCurrency;
            if (valueList is IAmortisableAsset asset)
            {
                Value = Calculate(portfolio, date, asset);
                return;
            }
            else
            {
                Value = 0.0;
                return;
            }
        }

        /// <inheritdoc/>
        public override void Calculate(IPortfolio portfolio, DateTime date, Totals total, TwoName name)
        {
            fCurrency = portfolio.BaseCurrency;
            if (total.ToAccount() == Account.Asset)
            {
                Value = portfolio.CalculateAggregateStatistic<IAmortisableAsset, double>(
                    total,
                    name,
                    (acc, n) => acc.ToAccount() == Account.Asset,
                    0.0,
                    asset => Calculate(portfolio, date, asset),
                    (a, b) => a + b);
            }
            else
            {
                Value = 0.0;
            }
        }

        private static double Calculate(IPortfolio portfolio, DateTime date, IAmortisableAsset asset)
        {
            ICurrency currency = portfolio.Currency(asset);
            var latestValue = asset.Debt.LatestValuation();
            decimal currencyValue = currency == null ? 1.0m : currency.Value(date)?.Value ?? 1.0m;
            return (double)(latestValue?.Value * currencyValue ?? 0.0m);
        }
    }
}
