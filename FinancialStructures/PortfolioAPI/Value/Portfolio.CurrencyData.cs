using FinancialStructures.FinanceInterfaces;
using FinancialStructures.FinanceStructures;

namespace FinancialStructures.PortfolioAPI
{
    public static partial class PortfolioValues
    {
        /// <summary>
        /// returns the currency associated to the account.
        /// </summary>
        internal static ICurrency Currency(IPortfolio portfolio, AccountType elementType, object account)
        {
            switch (elementType)
            {
                case (AccountType.Security):
                {
                    string currencyName = ((ISecurity)account).Currency;
                    ICurrency currency = portfolio.Currencies.Find(cur => cur.BaseCurrency == currencyName && cur.QuoteCurrency == portfolio.BaseCurrency);
                    if (currency != null)
                    {
                        return currency;
                    }

                    return portfolio.Currencies.Find(cur => cur.Company == portfolio.BaseCurrency && cur.QuoteCurrency == currencyName)?.Inverted();
                }
                case (AccountType.Currency):
                {
                    return (ICurrency)account;
                }
                case (AccountType.BankAccount):
                {
                    string currencyName = ((ICashAccount)account).Currency;
                    ICurrency currency = portfolio.Currencies.Find(cur => cur.BaseCurrency == currencyName && cur.QuoteCurrency == portfolio.BaseCurrency);
                    if (currency != null)
                    {
                        return currency;
                    }

                    return portfolio.Currencies.Find(cur => cur.BaseCurrency == portfolio.BaseCurrency && cur.QuoteCurrency == currencyName)?.Inverted();
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
