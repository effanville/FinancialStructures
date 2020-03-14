using FinancialStructures.Database;
using FinancialStructures.FinanceStructures;

namespace FinancialStructures.PortfolioAPI
{
    public static partial class PortfolioValues
    {
        /// <summary>
        /// returns the currency associated to the account.
        /// </summary>
        internal static Currency Currency(Portfolio portfolio, PortfolioElementType elementType, object security)
        {
            switch (elementType)
            {
                case (PortfolioElementType.Security):
                    {
                        var currencyName = ((Security)security).GetCurrency();
                        return portfolio.Currencies.Find(cur => cur.Name == currencyName);
                    }
                case (PortfolioElementType.Currency):
                    {
                        return (Currency)security;
                    }
                case (PortfolioElementType.BankAccount):
                    {
                        var currencyName = ((CashAccount)security).GetCurrency();
                        return portfolio.Currencies.Find(cur => cur.Name == currencyName);
                    }
                case (PortfolioElementType.Sector):
                    {
                        return new Currency("Error", "");
                    }
                default:
                    return new Currency("Error", "");
            }
        }
    }
}
