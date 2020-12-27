using System;
using FinancialStructures.FinanceStructures;
using FinancialStructures.NamingStructures;

namespace FinancialStructures.Database.Implementation
{
    public partial class Portfolio
    {
        /// <inheritdoc/>
        public double LatestValue(Account elementType, TwoName name)
        {
            return Value(elementType, name, DateTime.Today);
        }

        /// <inheritdoc/>
        public double Value(Account elementType, TwoName name, DateTime date)
        {
            switch (elementType)
            {
                case (Account.Security):
                {
                    if (!TryGetSecurity(name, out ISecurity desired) || !desired.Any())
                    {
                        return double.NaN;
                    }
                    ICurrency currency = Currency(Account.Security, desired);
                    return desired.Value(date, currency).Value;
                }
                case (Account.Currency):
                case (Account.Benchmark):
                {
                    if (!TryGetAccount(elementType, name, out ISingleValueDataList desired))
                    {
                        return 1.0;

                    }

                    return desired.Value(date).Value;
                }
                case (Account.BankAccount):
                {
                    foreach (ICashAccount account in BankAccounts)
                    {
                        if (account.Names.IsEqualTo(name))
                        {
                            ICurrency currency = Currency(elementType, account);
                            return account.Value(date, currency).Value;
                        }
                    }

                    return double.NaN;
                }
                default:
                    break;
            }

            return 0.0;
        }
    }
}
