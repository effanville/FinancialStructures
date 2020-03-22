using FinancialStructures.DatabaseInterfaces;
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
        public static double TotalValue(this IPortfolio portfolio, AccountType elementType)
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
        public static double TotalValue(this IPortfolio portfolio, AccountType elementType, DateTime date)
        {
            switch (elementType)
            {
                case (AccountType.Security):
                    {
                        double total = 0;
                        foreach (var sec in portfolio.Funds)
                        {
                            if (sec.Any())
                            {
                                var currency = Currency(portfolio, elementType, sec);
                                total += sec.Value(date, currency).Value;
                            }
                        }

                        return total;
                    }
                case (AccountType.Currency):
                    {
                        return 0.0;
                    }
                case (AccountType.BankAccount):
                    {
                        double sum = 0;
                        foreach (var acc in portfolio.BankAccounts)
                        {
                            var currency = Currency(portfolio, elementType, acc);
                            sum += acc.Value(date, currency).Value;
                        }

                        return sum;
                    }
                case (AccountType.Sector):
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
