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
                case Account.Security:
                {
                    if (!TryGetAccount(Account.Security, name, out IValueList desired) || !desired.Any())
                    {
                        return double.NaN;
                    }

                    ISecurity security = desired as ISecurity;
                    ICurrency currency = Currency(Account.Security, security);
                    return security.Value(date, currency)?.Value ?? 0.0;
                }
                case Account.Currency:
                case Account.Benchmark:
                {
                    if (!TryGetAccount(elementType, name, out IValueList desired))
                    {
                        // If doesnt exist, the default here is 1.0, as these values would
                        // be used in a multiplicative instance.
                        return 1.0;
                    }

                    return desired.Value(date)?.Value ?? 0.0;
                }
                case Account.BankAccount:
                {
                    if (!TryGetAccount(elementType, name, out IValueList account))
                    {
                        return double.NaN;
                    }

                    IExchangableValueList bankAccount = account as IExchangableValueList;
                    ICurrency currency = Currency(elementType, bankAccount);
                    return bankAccount.NearestEarlierValuation(date, currency)?.Value ?? 0.0;

                }
                default:
                    return 0.0;
            }
        }
    }
}
