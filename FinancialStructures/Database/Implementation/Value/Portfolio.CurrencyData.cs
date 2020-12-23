using FinancialStructures.FinanceInterfaces;
using FinancialStructures.FinanceStructures;

namespace FinancialStructures.Database.Implementation
{
    public partial class Portfolio
    {
        /// <summary>
        /// returns the currency associated to the account.
        /// </summary>
        public ICurrency Currency(Account elementType, object account)
        {
            switch (elementType)
            {
                case (Account.Security):
                {
                    string currencyName = ((ISecurity)account).Currency;
                    ICurrency currency = Currencies.Find(cur => cur.BaseCurrency == currencyName && cur.QuoteCurrency == BaseCurrency);
                    if (currency != null)
                    {
                        return currency;
                    }

                    return Currencies.Find(cur => cur.Company == BaseCurrency && cur.QuoteCurrency == currencyName)?.Inverted();
                }
                case (Account.Currency):
                {
                    return (ICurrency)account;
                }
                case (Account.BankAccount):
                {
                    string currencyName = ((ICashAccount)account).Currency;
                    ICurrency currency = Currencies.Find(cur => cur.BaseCurrency == currencyName && cur.QuoteCurrency == BaseCurrency);
                    if (currency != null)
                    {
                        return currency;
                    }

                    return Currencies.Find(cur => cur.BaseCurrency == BaseCurrency && cur.QuoteCurrency == currencyName)?.Inverted();
                }
                case (Account.Benchmark):
                {
                    return new Currency();
                }
                default:
                    return new Currency();
            }
        }
    }
}
