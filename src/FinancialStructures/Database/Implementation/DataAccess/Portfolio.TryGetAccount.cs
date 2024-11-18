using Effanville.FinancialStructures.FinanceStructures;
using Effanville.FinancialStructures.NamingStructures;

namespace Effanville.FinancialStructures.Database.Implementation
{
    public partial class Portfolio
    {
        /// <inheritdoc/>
        public bool TryGetAccount<TNamedFinancialObject>(
            Account accountType,
            TwoName names,
            out TNamedFinancialObject valueList)
            where TNamedFinancialObject : IReadOnlyNamedFinancialObject
        {
            valueList = default;
            switch (accountType)
            {
                case Account.Security:
                {
                    return _funds.TryGetAndCast(names, out valueList);
                }
                case Account.BankAccount:
                {
                    return _bankAccounts.TryGetAndCast(names, out valueList);
                }
                case Account.Currency:
                {
                    return _currencies.TryGetAndCast(names, out valueList);
                }
                case Account.Benchmark:
                {
                    return _benchmarks.TryGetAndCast(names, out valueList);
                }
                case Account.Asset:
                {
                    return _assets.TryGetAndCast(names, out valueList);
                }
                case Account.Pension:
                {
                    return _pensions.TryGetAndCast(names, out valueList);
                }
                default:
                case Account.All:
                case Account.Unknown:
                {
                    return false;
                }
            }
        }
    }
}