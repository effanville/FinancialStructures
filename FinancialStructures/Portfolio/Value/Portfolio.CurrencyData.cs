using FinancialStructures.FinanceInterfaces;
using FinancialStructures.FinanceStructures;

namespace FinancialStructures.Database
{
    public partial class Portfolio
    {
        /// <summary>
        /// returns the currency associated to the account.
        /// </summary>
        public ICurrency Currency(AccountType elementType, object account)
        {
            switch (elementType)
            {
                case (AccountType.Security):
                {
                    string currencyName = ((ISecurity)account).Currency;
                    ICurrency currency = Currencies.Find(cur => cur.BaseCurrency == currencyName && cur.QuoteCurrency == BaseCurrency);
                    if (currency != null)
                    {
                        return currency;
                    }

                    return Currencies.Find(cur => cur.Company == BaseCurrency && cur.QuoteCurrency == currencyName)?.Inverted();
                }
                case (AccountType.Currency):
                {
                    return (ICurrency)account;
                }
                case (AccountType.BankAccount):
                {
                    string currencyName = ((ICashAccount)account).Currency;
                    ICurrency currency = Currencies.Find(cur => cur.BaseCurrency == currencyName && cur.QuoteCurrency == BaseCurrency);
                    if (currency != null)
                    {
                        return currency;
                    }

                    return Currencies.Find(cur => cur.BaseCurrency == BaseCurrency && cur.QuoteCurrency == currencyName)?.Inverted();
                }
                case (AccountType.Benchmark):
                {
                    return new Currency();
                }
                default:
                    return new Currency();
            }
        }
    }
}
