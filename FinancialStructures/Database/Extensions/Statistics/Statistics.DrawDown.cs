using System;
using System.Collections.Generic;
using System.Linq;
using Common.Structure.DataStructures;
using Common.Structure.MathLibrary.Finance;
using FinancialStructures.Database.Extensions.Values;
using FinancialStructures.FinanceStructures;
using FinancialStructures.NamingStructures;

namespace FinancialStructures.Database.Extensions.Statistics
{
    public static partial class Statistics
    {
        /// <summary>
        /// Calculates the total IRR for the portfolio and the account type given over the time frame specified.
        /// </summary>
        public static double TotalDrawdown(this IPortfolio portfolio, Totals total, TwoName name = null)
        {
            DateTime earlierTime = portfolio.FirstValueDate(total, name);
            DateTime laterTime = portfolio.LatestDate(total, name);
            return portfolio.TotalDrawdown(total, earlierTime, laterTime, name);
        }

        /// <summary>
        /// Calculates the total IRR for the portfolio and the account type given over the time frame specified.
        /// </summary>
        public static double TotalDrawdown(this IPortfolio portfolio, Totals total, DateTime earlierTime, DateTime laterTime, TwoName name = null)
        {
            return TotalDrawdownOf(portfolio.Accounts(total, name), earlierTime, laterTime);
        }

        private static List<DailyValuation> CalculateValuesOf(IReadOnlyList<IValueList> accounts, DateTime earlierTime, DateTime laterTime)
        {
            if (!accounts.Any())
            {
                return null;
            }

            List<DateTime> values = new List<DateTime>();
            foreach (IValueList valueList in accounts)
            {
                List<DailyValuation> vals = valueList.ListOfValues().Where(value => value.Day >= earlierTime && value.Day <= laterTime && !value.Value.Equals(0.0)).ToList();
                foreach (DailyValuation val in vals)
                {
                    if (!values.Any(value => value.Equals(val.Day)))
                    {
                        values.Add(val.Day);
                    }
                }
            }

            values.Sort();
            List<DailyValuation> valuations = new List<DailyValuation>();
            foreach (DateTime date in values)
            {
                DailyValuation val = new DailyValuation(date, 0.0m);
                foreach (IValueList sec in accounts)
                {
                    val.Value += sec.Value(date)?.Value ?? 0.0m;
                }

                valuations.Add(val);
            }

            return valuations;
        }

        private static double TotalDrawdownOf(IReadOnlyList<IValueList> accounts, DateTime earlierTime, DateTime laterTime)
        {
            var valuations = CalculateValuesOf(accounts, earlierTime, laterTime);
            decimal dd = FinanceFunctions.Drawdown(valuations);
            if (dd == decimal.MaxValue)
            {
                return 0.0;
            }

            return (double)dd;
        }

        /// <summary>
        /// Calculates the IRR for the account with specified account and name.
        /// </summary>
        public static double Drawdown(this IPortfolio portfolio, Account accountType, TwoName names)
        {
            DateTime firstDate = portfolio.FirstDate(accountType, names);
            DateTime lastDate = portfolio.LatestDate(accountType, names);
            return portfolio.Drawdown(accountType, names, firstDate, lastDate);
        }

        /// <summary>
        /// Calculates the MDD for the account with specified account and name between the times specified.
        /// </summary>
        public static double Drawdown(this IPortfolio portfolio, Account accountType, TwoName names, DateTime earlierTime, DateTime laterTime)
        {
            return portfolio.CalculateStatistic(
               accountType,
               names,
               valueList => Calculate(valueList),
               double.NaN);

            double Calculate(IValueList valueList)
            {
                List<DailyValuation> values = valueList.ListOfValues().Where(value => value.Day >= earlierTime && value.Day <= laterTime && !value.Value.Equals(0.0)).ToList();
                decimal dd = FinanceFunctions.Drawdown(values);
                if (dd == decimal.MaxValue)
                {
                    return 0.0;
                }

                return (double)dd;
            }
        }
    }
}
