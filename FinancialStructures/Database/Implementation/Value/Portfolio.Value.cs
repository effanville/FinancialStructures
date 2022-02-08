using System;

using FinancialStructures.FinanceStructures;
using FinancialStructures.NamingStructures;

namespace FinancialStructures.Database.Implementation
{
    public partial class Portfolio
    {
        /// <inheritdoc/>
        public decimal LatestValue(Account elementType, TwoName name)
        {
            return Value(elementType, name, DateTime.Today);
        }

        /// <inheritdoc/>
        public decimal Value(Account elementType, TwoName name, DateTime date)
        {
            switch (elementType)
            {
                case Account.Security:
                case Account.Asset:
                {
                    if (!TryGetAccount(elementType, name, out IValueList desired) || !desired.Any())
                    {
                        return 0.0m;
                    }

                    IExchangableValueList exchangableValueList = desired as IExchangableValueList;
                    ICurrency currency = Currency(exchangableValueList);
                    return exchangableValueList.Value(date, currency)?.Value ?? 0.0m;
                }
                case Account.Currency:
                case Account.Benchmark:
                {
                    if (!TryGetAccount(elementType, name, out IValueList desired))
                    {
                        // If doesnt exist, the default here is 1.0, as these values would
                        // be used in a multiplicative instance.
                        return 1.0m;
                    }

                    return desired.Value(date)?.Value ?? 0.0m;
                }
                case Account.BankAccount:
                {
                    if (!TryGetAccount(elementType, name, out IValueList account))
                    {
                        return 0.0m;
                    }

                    IExchangableValueList bankAccount = account as IExchangableValueList;
                    ICurrency currency = Currency(bankAccount);
                    return bankAccount.ValueOnOrBefore(date, currency)?.Value ?? 0.0m;
                }
                default:
                    return 0.0m;
            }
        }
    }
}
