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
    /// <summary>
    /// Contains extension methods for calculating statistics.
    /// </summary>
    public static partial class Statistics
    {
        /// <summary>
        /// Calculates the total IRR for the portfolio and the account type given over the time frame specified.
        /// </summary>
        public static double TotalMDD(this IPortfolio portfolio, Totals total, TwoName name = null)
        {
            DateTime earlierTime = portfolio.FirstValueDate(total, name);
            DateTime laterTime = portfolio.LatestDate(total, name);
            return portfolio.TotalMDD(total, earlierTime, laterTime, name);
        }

        /// <summary>
        /// Calculates the total IRR for the portfolio and the account type given over the time frame specified.
        /// </summary>
        public static double TotalMDD(this IPortfolio portfolio, Totals accountType, DateTime earlierTime, DateTime laterTime, TwoName name = null)
        {
            return TotalMDDOf(portfolio.Accounts(accountType, name), earlierTime, laterTime);
        }

        private static double TotalMDDOf(IReadOnlyList<IValueList> securities, DateTime earlierTime, DateTime laterTime)
        {
            var valuations = CalculateValuesOf(securities, earlierTime, laterTime);
            decimal dd = FinanceFunctions.MDD(valuations);
            if (dd == decimal.MaxValue)
            {
                return 0.0;
            }

            return (double)dd;
        }

        /// <summary>
        /// Calculates the IRR for the account with specified account and name.
        /// </summary>
        public static double MDD(this IPortfolio portfolio, Account accountType, TwoName names)
        {
            DateTime firstDate = portfolio.FirstDate(accountType, names);
            DateTime lastDate = portfolio.LatestDate(accountType, names);
            return portfolio.MDD(accountType, names, firstDate, lastDate);
        }

        /// <summary>
        /// Calculates the MDD for the account with specified account and name between the times specified.
        /// </summary>
        public static double MDD(this IPortfolio portfolio, Account account, TwoName names, DateTime earlierTime, DateTime laterTime)
        {
            return portfolio.CalculateStatistic(
                account,
                names,
                valueList => Calculate(valueList),
                double.NaN);

            double Calculate(IValueList valueList)
            {
                List<DailyValuation> values = valueList.ListOfValues().Where(value => value.Day >= earlierTime && value.Day <= laterTime && !value.Value.Equals(0.0)).ToList();
                return (double)FinanceFunctions.MDD(values);
            }
        }
    }
}
