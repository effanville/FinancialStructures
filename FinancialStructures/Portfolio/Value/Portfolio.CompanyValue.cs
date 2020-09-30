using System;
using System.Linq;
using FinancialStructures.FinanceInterfaces;

namespace FinancialStructures.Database
{
    public partial class Portfolio
    {
        /// <inheritdoc/>
        public double CompanyValue(Account elementType, string company, DateTime date)
        {
            switch (elementType)
            {
                case (Account.Security):
                {
                    System.Collections.Generic.List<ISecurity> securities = CompanySecurities(company);
                    double value = 0;
                    foreach (ISecurity security in securities)
                    {
                        if (security.Any())
                        {
                            ICurrency currency = Currency(Account.Security, security);
                            value += security.Value(date, currency).Value;
                        }
                    }

                    return value;
                }
                case (Account.BankAccount):
                {
                    System.Collections.Generic.List<ICashAccount> bankAccounts = CompanyBankAccounts(company);
                    if (bankAccounts.Count() == 0)
                    {
                        return double.NaN;
                    }
                    double value = 0;
                    foreach (ICashAccount account in bankAccounts)
                    {
                        if (account != null && account.Any())
                        {
                            ICurrency currency = Currency(Account.BankAccount, account);
                            value += account.NearestEarlierValuation(date, currency).Value;
                        }
                    }

                    return value;
                }
                case (Account.All):
                {
                    return CompanyValue(Account.BankAccount, company, date) + CompanyValue(Account.Security, company, date);
                }
                default:
                case (Account.Currency):
                case (Account.Benchmark):
                {
                    return 0.0;
                }
            }
        }
    }
}
