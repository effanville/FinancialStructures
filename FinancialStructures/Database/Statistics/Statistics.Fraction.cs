using System;
using FinancialStructures.FinanceStructures;
using FinancialStructures.NamingStructures;

namespace FinancialStructures.Database.Statistics
{
    public static partial class Statistics
    {
        /// <summary>
        /// returns the fraction a held in the account has out of its company.
        /// </summary>
        public static double AccountInCompanyFraction(this IPortfolio portfolio, Totals elementType, TwoName names, DateTime date)
        {
            Totals companyTotals = AccountToTotalsConverter.ConvertTotalToCompanyTotal(elementType);
            double companyFraction = portfolio.Fraction(companyTotals, names, date);
            if (companyFraction.Equals(0.0))
            {
                return 0.0;
            }
            return portfolio.Fraction(elementType, names, date) / companyFraction;
        }

        /// <summary>
        /// Returns the fraction of investments in the account out of the portfolio.
        /// </summary>
        public static double Fraction(this IPortfolio portfolio, Totals elementType, TwoName names, DateTime date)
        {
            switch (elementType)
            {
                case Totals.All:
                {
                    return 1.0;
                }
                case Totals.Security:
                {
                    if (portfolio.TryGetAccount(Account.Security, names, out IValueList desired))
                    {
                        if (desired.Any())
                        {
                            var security = desired as ISecurity;
                            ICurrency currency = portfolio.Currency(Account.Security, security);
                            return security.Value(date, currency).Value / portfolio.TotalValue(Totals.Security, date);
                        }
                    }

                    return 1.0;
                }
                case Totals.BankAccount:
                {
                    if (portfolio.TryGetAccount(AccountToTotalsConverter.ConvertTotalToAccount(elementType), names, out IValueList desired))
                    {
                        var cashAcc = desired as ICashAccount;

                        if (cashAcc.Any())
                        {
                            ICurrency currency = portfolio.Currency(Account.BankAccount, cashAcc);
                            return cashAcc.Value(date, currency).Value / portfolio.TotalValue(Totals.BankAccount, date);
                        }
                    }

                    return double.NaN;
                }
                case Totals.Benchmark:
                case Totals.Currency:
                {
                    if (portfolio.TryGetAccount(AccountToTotalsConverter.ConvertTotalToAccount(elementType), names, out IValueList desired))
                    {
                        return desired.Value(date).Value / portfolio.TotalValue(elementType, date);
                    }

                    return double.NaN;
                }
                case Totals.Sector:
                case Totals.CurrencySector:
                case Totals.SecuritySector:
                case Totals.BankAccountSector:
                case Totals.SecurityCurrency:
                case Totals.BankAccountCurrency:
                case Totals.SecurityCompany:
                case Totals.BankAccountCompany:
                case Totals.Company:
                {
                    return portfolio.TotalValue(elementType, date, names) / portfolio.TotalValue(Totals.All, date);
                }
                default:
                {
                    return double.NaN;
                }
            }
        }
    }
}
