using System;
using FinancialStructures.FinanceInterfaces;

namespace FinancialStructures.Database
{
    public partial class Portfolio
    {
        /// <inheritdoc/>
        public double TotalValue(DateTime date)
        {
            return TotalValue(AccountType.All, DateTime.Today);
        }

        /// <inheritdoc/>
        public double TotalValue(AccountType elementType)
        {
            return TotalValue(elementType, DateTime.Today);
        }

        /// <inheritdoc/>
        public double TotalValue(AccountType elementType, DateTime date)
        {
            switch (elementType)
            {
                case (AccountType.Security):
                {
                    double total = 0;
                    foreach (ISecurity sec in Funds)
                    {
                        if (sec.Any())
                        {
                            ICurrency currency = Currency(elementType, sec);
                            total += sec.Value(date, currency).Value;
                        }
                    }

                    return total;
                }
                case (AccountType.Currency):
                {
                    return 0.0;
                }
                case (AccountType.BankAccount):
                {
                    double sum = 0;
                    foreach (ICashAccount acc in BankAccounts)
                    {
                        ICurrency currency = Currency(elementType, acc);
                        sum += acc.Value(date, currency).Value;
                    }

                    return sum;
                }
                case (AccountType.Benchmark):
                {
                    break;
                }
                case (AccountType.All):
                {
                    return TotalValue(AccountType.Security, date) + TotalValue(AccountType.BankAccount, date);
                }
                default:
                    break;
            }

            return 0.0;
        }
    }
}
