using System;
using System.Linq;
using FinancialStructures.FinanceInterfaces;

namespace FinancialStructures.PortfolioAPI
{
    public static partial class PortfolioValues
    {
        /// <summary>
        /// Calculates the value held in the company.
        /// </summary>
        /// <param name="portfolio">The database to query.</param>
        /// <param name="elementType">The type of account to find.</param>
        /// <param name="company">The company name to search for.</param>
        /// <param name="date">The date to calculate value on.</param>
        /// <returns>The value held in the company.</returns>
        public static double CompanyValue(this IPortfolio portfolio, AccountType elementType, string company, DateTime date)
        {
            switch (elementType)
            {
                case (AccountType.Security):
                {
                    System.Collections.Generic.List<ISecurity> securities = portfolio.CompanySecurities(company);
                    double value = 0;
                    foreach (ISecurity security in securities)
                    {
                        if (security.Any())
                        {
                            ICurrency currency = Currency(portfolio, AccountType.Security, security);
                            value += security.Value(date, currency).Value;
                        }
                    }

                    return value;
                }
                case (AccountType.Currency):
                {
                    return 0.0;
                }
                case (AccountType.BankAccount):
                {
                    System.Collections.Generic.List<ICashAccount> bankAccounts = portfolio.CompanyBankAccounts(company);
                    if (bankAccounts.Count() == 0)
                    {
                        return double.NaN;
                    }
                    double value = 0;
                    foreach (ICashAccount account in bankAccounts)
                    {
                        if (account != null && account.Any())
                        {
                            ICurrency currency = Currency(portfolio, AccountType.BankAccount, account);
                            value += account.NearestEarlierValuation(date, currency).Value;
                        }
                    }

                    return value;
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
