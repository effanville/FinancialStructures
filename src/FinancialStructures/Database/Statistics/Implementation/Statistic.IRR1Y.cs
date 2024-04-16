using System;

using Effanville.FinancialStructures.Database.Extensions.Rates;
using Effanville.FinancialStructures.Database.Extensions.Values;
using Effanville.FinancialStructures.FinanceStructures;
using Effanville.FinancialStructures.NamingStructures;

namespace Effanville.FinancialStructures.Database.Statistics.Implementation
{
    /// <summary>
    /// Statistic for the <see cref="Statistic.IRR1Y"/> enum value.
    /// This calculates the Internal rate of return of the object
    /// in the portfolio.
    /// </summary>
    internal class StatisticIRR1Y : StatisticBase
    {
        /// <summary>
        /// Default constructor setting the statistic type.
        /// </summary>
        internal StatisticIRR1Y()
            : base(Statistic.IRR1Y)
        {
        }

        /// <inheritdoc/>
        public override void Calculate(IValueList valueList, IPortfolio portfolio, DateTime date, Account account,
            TwoName name)
        {            

            Value = 100 * IRRCalcHelpers.CalcIRR(portfolio, account, valueList, date.AddMonths(-12), date);
        }

        /// <inheritdoc/>
        public override void Calculate(IPortfolio portfolio, DateTime date, Totals total, TwoName name)
        {
            Value = 100 * portfolio.TotalIRR(total, date.AddMonths(-12), date, name);
        }
    }

    internal static class IRRCalcHelpers
    {
        public static double CalcIRR(IPortfolio portfolio, Account accountType, IValueList valueList, DateTime earlierTime,
            DateTime laterTime)
        {
            DateTime earliestTime = portfolio.FirstDate(accountType, valueList.Names);
            if (earlierTime < earliestTime)
            {
                earlierTime = earliestTime;
            }

            DateTime latestTime = portfolio.LatestDate(accountType, valueList.Names);
            if (laterTime > latestTime)
            {
                laterTime = latestTime;
            }

            if (valueList is ISecurity sec)
            {
                ICurrency currency = portfolio.Currency(sec);
                return sec.IRR(earlierTime, laterTime, currency);
            }

            return valueList.CAR(earlierTime, laterTime);
        }
    }
}
