using System;
using System.Collections.Generic;
using System.Linq;

using Common.Structure.DataStructures;
using Common.Structure.NamingStructures;
using FinancialStructures.Database.Extensions;
using FinancialStructures.Database.Extensions.Values;
using FinancialStructures.FinanceStructures;
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
            fCurrency = portfolio.BaseCurrency;
            switch (account)
            {
                case Account.Security:
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

                    Value = (double)sum;
                    return;
                }
                case Account.Asset:
                {
                    Value = portfolio.CalculateStatistic<IAmortisableAsset, double>(
                        account,
                        name,
                        (acc, n) => acc == Account.Asset,
                        asset => Calculate(asset));
                    double Calculate(IAmortisableAsset asset)
                    {
                        ICurrency currency = portfolio.Currency(asset);
                        return (double)asset.TotalCost(date, currency);
                    }

                    return;
                }
                default:
                {
                    Value = 0.0;
                    return;
                }
            }
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
