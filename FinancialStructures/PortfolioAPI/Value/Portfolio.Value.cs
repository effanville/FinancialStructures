using FinancialStructures.Database;
using FinancialStructures.FinanceStructures;
using FinancialStructures.GUIFinanceStructures;
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
        public static double LatestValue(this Portfolio portfolio, AccountType elementType, NameData name)
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
        public static double Value(this Portfolio portfolio, AccountType elementType, NameData name, DateTime date)
        {
            switch (elementType)
            {
                case (AccountType.Security):
                    {
                        return portfolio.SecurityValue(name.Company, name.Name, date);
                    }
                case (AccountType.Currency):
                    {
                        return portfolio.CurrencyValue(name.Name, date);
                    }
                case (AccountType.BankAccount):
                    {
                        return portfolio.BankAccountLatestValue(name.Company, name.Name);
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

        private static double SecurityValue(this Portfolio portfolio, string company, string name, DateTime date)
        {
            if (!portfolio.TryGetSecurity(company, name, out Security desired) || !desired.Any())
            {
                return double.NaN;
            }
            var currency = Currency(portfolio, AccountType.Security, desired);
            return desired.Value(date, currency).Value;
        }

        private static double CurrencyValue(this Portfolio portfolio, string currencyName, DateTime date)
        {
            foreach (var currency in portfolio.Currencies)
            {
                if (currency.GetName() == currencyName)
                {
                    return currency.Value(date).Value;
                }
            }

            return 1.0;
        }

        private static double BankAccountLatestValue(this Portfolio portfolio, string company, string name)
        {
            if (!portfolio.TryGetBankAccount(company, name, out CashAccount desired))
            {
                return double.NaN;
            }

            return desired.LatestValue().Value;
        }
    }
}
