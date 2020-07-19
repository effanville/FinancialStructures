using System.Collections.Generic;
using FinancialStructures.FinanceInterfaces;
using FinancialStructures.NamingStructures;

namespace FinancialStructures.Database.Statistics
{
    public static partial class Statistics
    {
        /// <summary>
        /// returns the total profit in the portfolio.
        /// </summary>
        public static double TotalProfit(this IPortfolio portfolio, AccountType elementType)
        {
            double total = 0;
            foreach (ISecurity security in portfolio.Funds)
            {
                if (security.Any())
                {
                    ICurrency currency = portfolio.Currencies.Find(cur => cur.Name == security.Currency);
                    total += portfolio.Profit(elementType, security.Names);
                }
            }

            return total;
        }


        /// <summary>
        /// Returns the profit of the company over its lifetime in the portfolio.
        /// </summary>
        public static double Profit(this IPortfolio portfolio, AccountType elementType, TwoName names)
        {
            if (portfolio.TryGetSecurity(names, out ISecurity desired))
            {
                if (desired.Any())
                {
                    ICurrency currency = portfolio.Currency(AccountType.Security, desired);
                    return desired.LatestValue(currency).Value - desired.TotalInvestment(currency);
                }
            }

            return double.NaN;
        }

        /// <summary>
        /// returns the profit in the company.
        /// </summary>
        public static double CompanyProfit(this IPortfolio portfolio, string company)
        {
            List<ISecurity> securities = portfolio.CompanySecurities(company);
            double value = 0;
            foreach (ISecurity security in securities)
            {
                if (security.Any())
                {
                    ICurrency currency = portfolio.Currency(AccountType.Security, security);
                    value += security.LatestValue(currency).Value - security.TotalInvestment(currency);
                }
            }

            return value;
        }
    }
}
