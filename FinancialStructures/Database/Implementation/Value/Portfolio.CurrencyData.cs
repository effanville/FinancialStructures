using FinancialStructures.FinanceStructures;
using FinancialStructures.FinanceStructures.Implementation;

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
                    string currencyName = ((ISecurity)account).Names.Currency;
                    ICurrency currency = Currencies.Find(cur => cur.BaseCurrency == currencyName && cur.QuoteCurrency == BaseCurrency);
                    if (currency != null)
                    {
                        return currency;
                    }

                    return Currencies.Find(cur => cur.BaseCurrency == BaseCurrency && cur.QuoteCurrency == currencyName)?.Inverted();
                }
                case (Account.BankAccount):
                {
                    string currencyName = ((ICashAccount)account).Names.Currency;
                    ICurrency currency = Currencies.Find(cur => cur.BaseCurrency == currencyName && cur.QuoteCurrency == BaseCurrency);
                    if (currency != null)
                    {
                        return currency;
                    }

                    return Currencies.Find(cur => cur.BaseCurrency == BaseCurrency && cur.QuoteCurrency == currencyName)?.Inverted();
                }
                case (Account.Currency):
                {
                    return (ICurrency)account;
                }
                case (Account.Benchmark):
                default:
                {
                    return new Currency();
                }
            }
        }
    }
}
