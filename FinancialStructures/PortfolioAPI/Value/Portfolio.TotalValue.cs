using FinancialStructures.Database;
using System;

namespace FinancialStructures.PortfolioAPI
{
    public static partial class PortfolioValues
    {
        /// <summary>
        /// Total value of all accounts of type specified today.
        /// </summary>
        /// <param name="portfolio">The database to query.</param>
        /// <param name="elementType">The type to find the total of.</param>
        /// <returns>The total value held on today.</returns>
        public static double TotalValue(this Portfolio portfolio, PortfolioElementType elementType)
        {
            return portfolio.TotalValue(elementType, DateTime.Today);
        }

        /// <summary>
        /// Total value of all accounts of type specified on date given.
        /// </summary>
        /// <param name="portfolio">The database to query.</param>
        /// <param name="elementType">The type to find the total of.</param>
        /// <param name="date">The date to find the total on.</param>
        /// <returns>The total value held.</returns>
        public static double TotalValue(this Portfolio portfolio, PortfolioElementType elementType, DateTime date)
        {
            switch (elementType)
            {
                case (PortfolioElementType.Security):
                    {
                        double total = 0;
                        foreach (var sec in portfolio.GetSecurities())
                        {
                            if (sec.Any())
                            {
                                var currency = Currency(portfolio, elementType, sec);
                                total += sec.Value(date, currency).Value;
                            }
                        }

                        return total;
                    }
                case (PortfolioElementType.Currency):
                    {
                        return 0.0;
                    }
                case (PortfolioElementType.BankAccount):
                    {
                        double sum = 0;
                        foreach (var acc in portfolio.GetBankAccounts())
                        {
                            var currency = Currency(portfolio, elementType, acc);
                            sum += acc.Value(date, currency).Value;
                        }

                        return sum;
                    }
                case (PortfolioElementType.Sector):
                    {
                        break;
                    }
                default:
                    break;
            }

            return 0.0;
        }
    }
}
