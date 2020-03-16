using FinancialStructures.Database;
using FinancialStructures.FinanceStructures;

namespace FinancialStructures.PortfolioAPI
{
    public static partial class PortfolioValues
    {
        /// <summary>
        /// returns the currency associated to the account.
        /// </summary>
        internal static Currency Currency(Portfolio portfolio, AccountType elementType, object security)
        {
            switch (elementType)
            {
                case (AccountType.Security):
                    {
                        var currencyName = ((Security)security).GetCurrency();
                        return portfolio.Currencies.Find(cur => cur.Name == currencyName);
                    }
                case (AccountType.Currency):
                    {
                        return (Currency)security;
                    }
                case (AccountType.BankAccount):
                    {
                        var currencyName = ((CashAccount)security).GetCurrency();
                        return portfolio.Currencies.Find(cur => cur.Name == currencyName);
                    }
                case (AccountType.Sector):
                    {
                        return new Currency();
                    }
                default:
                    return new Currency();
            }
        }
    }
}
