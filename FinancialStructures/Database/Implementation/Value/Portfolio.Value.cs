using System;
using FinancialStructures.Database.Extensions;
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
        public decimal Value(Account account, TwoName name, DateTime date)
        {
            return this.CalculateStatistic(
                account,
                name,
                valueList => CalculateValue(valueList),
                DefaultValue());
            decimal DefaultValue()
            {
                if (account == Account.Currency || account == Account.Benchmark)
                {
                    return 1.0m;
                }

                return 0.0m;
            }

            decimal CalculateValue(IValueList valueList)
            {
                if (!valueList.Any())
                {
                    return 0;
                }

                if (valueList is not IExchangableValueList eValueList)
                {
                    return valueList.Value(date)?.Value ?? 0.0m;
                }

                ICurrency currency = Currency(eValueList);

                if (account is Account.BankAccount)
                {
                    return eValueList.ValueOnOrBefore(date, currency)?.Value ?? 0.0m;
                }

                return eValueList.Value(date, currency)?.Value ?? 0.0m;
            }
        }
    }
}
