using System;
using FinancialStructures.NamingStructures;

namespace FinancialStructures.Database.Statistics
{
    public static partial class Statistics
    {
        /// <summary>
        /// Returns the fraction of investments in the account out of the totals in the portfolio.
        /// </summary>
        public static double Fraction(this IPortfolio portfolio, Totals totals, Account account, TwoName names, DateTime date)
        {
            if (portfolio.TotalValue(totals, date, names) == 0)
            {
                return double.NaN;
            }

            if (portfolio.Exists(account, names))
            {
                return portfolio.Value(account, names, date) / portfolio.TotalValue(totals, date, names);
            }

            return 0.0;
        }


        /// <summary>
        /// Returns the fraction of investments in the Totals out of the value in the portfolio.
        /// </summary>
        public static double TotalFraction(this IPortfolio portfolio, Totals totals, TwoName names, DateTime date)
        {
            if (portfolio.TotalValue(Totals.All, date) == 0)
            {
                return double.NaN;
            }

            return portfolio.TotalValue(totals, date, names) / portfolio.TotalValue(Totals.All, date);
        }
    }
}
