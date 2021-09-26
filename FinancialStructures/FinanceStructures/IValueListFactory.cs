using FinancialStructures.Database;
using FinancialStructures.FinanceStructures.Implementation;

namespace FinancialStructures.FinanceStructures
{
    /// <summary>
    /// Provides factory methods for the <see cref="IValueList"/> interface implementations.
    /// </summary>
    public static class IValueListFactory
    {
        /// <summary>
        /// Creates an instance of an IValueList.
        /// </summary>
        /// <param name="account">The type of ValueList to create.</param>
        /// <returns></returns>
        public static IValueList Create(Account account)
        {
            switch (account)
            {
                case Account.Security:
                    return new Security();
                case Account.Benchmark:
                    return new Sector();
                case Account.BankAccount:
                    return new CashAccount();
                case Account.Currency:
                    return new Currency();
                case Account.All:
                default:
                    return null;
            }
        }
    }
}
