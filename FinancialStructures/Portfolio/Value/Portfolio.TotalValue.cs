using System;
using FinancialStructures.FinanceInterfaces;

namespace FinancialStructures.Database
{
    public partial class Portfolio
    {
        /// <inheritdoc/>
        public double TotalValue(DateTime date)
        {
            return TotalValue(Account.All, DateTime.Today);
        }

        /// <inheritdoc/>
        public double TotalValue(Account elementType)
        {
            return TotalValue(elementType, DateTime.Today);
        }

        /// <inheritdoc/>
        public double TotalValue(Account elementType, DateTime date)
        {
            switch (elementType)
            {
                case (Account.Security):
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
                case (Account.Currency):
                {
                    return 0.0;
                }
                case (Account.BankAccount):
                {
                    double sum = 0;
                    foreach (ICashAccount acc in BankAccounts)
                    {
                        ICurrency currency = Currency(elementType, acc);
                        sum += acc.NearestEarlierValuation(date, currency).Value;
                    }

                    return sum;
                }
                case (Account.Sector):
                {
                    double sum = 0;
                    if (Funds != null)
                    {
                        foreach (ISecurity fund in Funds)
                        {
                            if (fund.IsSectorLinked(name.Company))
                            {
                                sum += fund.NearestEarlierValuation(date).Value;
                            }
                        }
                    }

                    return sum;
                }
                case (Account.Benchmark):
                {
                    break;
                }
                case (Account.All):
                {
                    return TotalValue(Account.Security, date) + TotalValue(Account.BankAccount, date);
                }
                default:
                    break;
            }

            return 0.0;
        }
    }
}
