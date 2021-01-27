using System;
using FinancialStructures.NamingStructures;

namespace FinancialStructures.Database.Statistics
{
    public static partial class Statistics
    {
        /// <summary>
        /// returns the fraction a held in the account has out of its company.
        /// </summary>
        public static double AccountInCompanyFraction(this IPortfolio portfolio, Totals elementType, Account account, TwoName names, DateTime date)
        {
            Totals companyTotals = AccountToTotalsConverter.ConvertTotalToCompanyTotal(elementType);
            double companyFraction = portfolio.Fraction(companyTotals, account, names, date);
            if (companyFraction.Equals(0.0))
            {
                return 0.0;
            }
            return portfolio.Fraction(elementType, account, names, date) / companyFraction;
        }

        /// <summary>
        /// Returns the fraction of investments in the account out of the totals in the portfolio.
        /// </summary>
        public static double Fraction(this IPortfolio portfolio, Totals totals, Account account, TwoName names, DateTime date)
        {
            if (portfolio.TotalValue(totals, date) == 0)
            {
                return double.NaN;
            }

            if (portfolio.Exists(account, names))
            {
                return portfolio.Value(account, names, date) / portfolio.TotalValue(totals, date);
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
