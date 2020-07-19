using System;
using FinancialStructures.FinanceInterfaces;
using FinancialStructures.NamingStructures;

namespace FinancialStructures.Database
{
    public partial class Portfolio
    {
        /// <inheritdoc/>
        public double LatestValue(AccountType elementType, TwoName name)
        {
            return Value(elementType, name, DateTime.Today);
        }

        /// <inheritdoc/>
        public double Value(AccountType elementType, TwoName name, DateTime date)
        {
            switch (elementType)
            {
                case (AccountType.Security):
                {
                    if (!TryGetSecurity(name, out ISecurity desired) || !desired.Any())
                    {
                        return double.NaN;
                    }
                    ICurrency currency = Currency(AccountType.Security, desired);
                    return desired.Value(date, currency).Value;
                }
                case (AccountType.Currency):
                case (AccountType.BankAccount):
                case (AccountType.Sector):
                {
                    if (!TryGetAccount(elementType, name, out ISingleValueDataList desired))
                    {
                        if (elementType == AccountType.BankAccount)
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
