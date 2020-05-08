using FinancialStructures.FinanceInterfaces;
using FinancialStructures.NamingStructures;
using System;

namespace FinancialStructures.PortfolioAPI
{
    public static partial class PortfolioValues
    {
        /// <summary>
        /// Get the latest value of the selected element.
        /// </summary>
        /// <param name="portfolio">The database to query.</param>
        /// <param name="elementType">The type of element to find.</param>
        /// <param name="name">The name of the element to find.</param>
        /// <returns>The latest value if it exists.</returns>
        public static double LatestValue(this IPortfolio portfolio, AccountType elementType, TwoName name)
        {
            return portfolio.Value(elementType, name, DateTime.Today);
        }

        /// <summary>
        /// Get the value of the selected element.
        /// </summary>
        /// <param name="portfolio">The database to query.</param>
        /// <param name="elementType">The type of element to find.</param>
        /// <param name="name">The name of the element to find.</param>
        /// <param name="date">The date on which to find the value.</param>
        /// <returns>The  value if it exists.</returns>
        public static double Value(this IPortfolio portfolio, AccountType elementType, TwoName name, DateTime date)
        {
            switch (elementType)
            {
                case (AccountType.Security):
                    {
                        if (!portfolio.TryGetSecurity(name, out ISecurity desired) || !desired.Any())
                        {
                            return double.NaN;
                        }
                        var currency = Currency(portfolio, AccountType.Security, desired);
                        return desired.Value(date, currency).Value;
                    }
                case (AccountType.Currency):
                    {
                        foreach (ICurrency currency in portfolio.Currencies)
                        {
                            if (name.IsEqualTo(currency.Names))
                            {
                                return currency.Value(date).Value;
                            }
                        }

                        return 1.0;
                    }
                case (AccountType.BankAccount):
                    {
                        if (!portfolio.TryGetAccount(AccountType.BankAccount, name, out var desired))
                        {
                            return double.NaN;
                        }

                        return desired.LatestValue().Value;
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
