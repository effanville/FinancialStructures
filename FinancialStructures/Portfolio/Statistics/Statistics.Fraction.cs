using System;
using FinancialStructures.FinanceInterfaces;
using FinancialStructures.NamingStructures;

namespace FinancialStructures.Database.Statistics
{
    public static partial class Statistics
    {
        /// <summary>
        /// The fraction of money held in the company out of the portfolio.
        /// </summary>
        public static double CompanyFraction(this IPortfolio portfolio, AccountType elementType, string company, DateTime date)
        {
            return portfolio.CompanyValue(elementType, company, date) / portfolio.TotalValue(elementType, date);
        }

        /// <summary>
        /// returns the fraction a held in the account has out of its company.
        /// </summary>
        public static double AccountInCompanyFraction(this IPortfolio portfolio, AccountType elementType, TwoName names, DateTime date)
        {
            double companyFraction = portfolio.CompanyFraction(elementType, names.Company, date);
            if (companyFraction.Equals(0.0))
            {
                return 0.0;
            }
            return portfolio.Fraction(elementType, names, date) / companyFraction;
        }

        /// <summary>
        /// Returns the fraction of investments in the account out of the portfolio.
        /// </summary>
        public static double Fraction(this IPortfolio portfolio, AccountType elementType, TwoName names, DateTime date)
        {
            switch (elementType)
            {
                case AccountType.Security:
                {
                    if (portfolio.TryGetSecurity(names, out ISecurity desired))
                    {
                        if (desired.Any())
                        {
                            ICurrency currency = portfolio.Currency(AccountType.Security, desired);
                            return desired.Value(date, currency).Value / portfolio.TotalValue(AccountType.Security, date);
                        }
                    }

                    return double.NaN;
                }
                case AccountType.BankAccount:
                {
                    if (portfolio.TryGetAccount(AccountType.BankAccount, names, out ISingleValueDataList desired))
                    {
                        if (desired is ICashAccount cashAcc)
                        {
                            if (cashAcc.Any())
                            {
                                ICurrency currency = portfolio.Currency(AccountType.BankAccount, cashAcc);
                                return cashAcc.Value(date, currency).Value / portfolio.TotalValue(AccountType.BankAccount, date);
                            }
                        }
                    }

                    return double.NaN;
                }
                case AccountType.Sector:
                {
                    return portfolio.Value(AccountType.Sector, names, date) / portfolio.TotalValue(date);
                }
                default:
                {
                    return double.NaN;
                }
            }
        }
    }
}
