using System;
using FinancialStructures.NamingStructures;

namespace FinancialStructures.Database.Extensions.Statistics
{
    public static partial class Statistics
    {
        /// <summary>
        /// Returns the fraction of investments in the account out of the totals in the portfolio.
        /// </summary>
        public static double Fraction(this IPortfolio portfolio, Totals totals, Account account, TwoName names, DateTime date)
        {
            decimal totalValue = portfolio.TotalValue(totals, date, names);
            if (totalValue == 0)
            {
                return double.NaN;
            }

            if (portfolio.Exists(account, names))
            {
                return (double)(portfolio.Value(account, names, date) / totalValue);
            }

            return 0.0;
        }

        /// <summary>
        /// Returns the fraction of investments in the Totals out of the value in the portfolio.
        /// </summary>
        public static double TotalFraction(this IPortfolio portfolio, Totals totals, TwoName names, DateTime date)
        {
            decimal totalValue = portfolio.TotalValue(Totals.All, date);
            if (totalValue == 0)
            {
                return double.NaN;
            }

            return (double)(portfolio.TotalValue(totals, date, names) / totalValue);
        }
    }
}
