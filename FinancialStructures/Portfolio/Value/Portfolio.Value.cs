using System;
using FinancialStructures.FinanceInterfaces;
using FinancialStructures.NamingStructures;

namespace FinancialStructures.Database
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
                case (Account.BankAccount):
                case (Account.Benchmark):
                {
                    if (!TryGetAccount(elementType, name, out ISingleValueDataList desired))
                    {
                        if (elementType == Account.BankAccount)
                        {

                            return double.NaN;
                        }
                        else
                        {
                            return 1.0;
                        }
                    }

                    return desired.Value(date).Value;
                }
                default:
                    break;
            }

            return 0.0;
        }
    }
}
