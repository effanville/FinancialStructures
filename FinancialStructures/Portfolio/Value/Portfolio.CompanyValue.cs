using System;
using System.Linq;
using FinancialStructures.FinanceInterfaces;

namespace FinancialStructures.Database
{
    public partial class Portfolio
    {
        /// <inheritdoc/>
        public double CompanyValue(AccountType elementType, string company, DateTime date)
        {
            switch (elementType)
            {
                case (AccountType.Security):
                {
                    System.Collections.Generic.List<ISecurity> securities = CompanySecurities(company);
                    double value = 0;
                    foreach (ISecurity security in securities)
                    {
                        if (security.Any())
                        {
                            ICurrency currency = Currency(AccountType.Security, security);
                            value += security.Value(date, currency).Value;
                        }
                    }

                    return value;
                }
                case (AccountType.BankAccount):
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
                            ICurrency currency = Currency(AccountType.BankAccount, account);
                            value += account.NearestEarlierValuation(date, currency).Value;
                        }
                    }

                    return value;
                }
                case (AccountType.All):
                {
                    return CompanyValue(AccountType.BankAccount, company, date) + CompanyValue(AccountType.Security, company, date);
                }
                default:
                case (AccountType.Currency):
                case (AccountType.Benchmark):
                {
                    return 0.0;
                }
            }
        }
    }
}
