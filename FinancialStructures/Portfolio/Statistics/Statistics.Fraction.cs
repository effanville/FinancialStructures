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
        public static double CompanyFraction(this IPortfolio portfolio, Account elementType, string company, DateTime date)
        {
            return portfolio.CompanyValue(elementType, company, date) / portfolio.TotalValue(elementType, date);
        }

        /// <summary>
        /// returns the fraction a held in the account has out of its company.
        /// </summary>
        public static double AccountInCompanyFraction(this IPortfolio portfolio, Account elementType, TwoName names, DateTime date)
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
        public static double Fraction(this IPortfolio portfolio, Account elementType, TwoName names, DateTime date)
        {
            switch (elementType)
            {
                case Account.Security:
                {
                    if (portfolio.TryGetSecurity(names, out ISecurity desired))
                    {
                        if (desired.Any())
                        {
                            ICurrency currency = portfolio.Currency(Account.Security, desired);
                            return desired.Value(date, currency).Value / portfolio.TotalValue(Account.Security, date);
                        }
                    }

                    return double.NaN;
                }
                case Account.BankAccount:
                {
                    if (portfolio.TryGetAccount(Account.BankAccount, names, out ISingleValueDataList desired))
                    {
                        if (desired is ICashAccount cashAcc)
                        {
                            if (cashAcc.Any())
                            {
                                ICurrency currency = portfolio.Currency(Account.BankAccount, cashAcc);
                                return cashAcc.Value(date, currency).Value / portfolio.TotalValue(Account.BankAccount, date);
                            }
                        }
                    }

                    return double.NaN;
                }
                case Account.Sector:
                {
                    return portfolio.Value(Account.Sector, names, date) / portfolio.TotalValue(date);
                }
                default:
                {
                    return double.NaN;
                }
            }
        }
    }
}
