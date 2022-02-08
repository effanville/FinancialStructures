using System.Linq;
using FinancialStructures.FinanceStructures;

namespace FinancialStructures.Database.Implementation
{
    public partial class Portfolio
    {
        /// <summary>
        /// returns the currency associated to the account.
        /// </summary>
        public ICurrency Currency(IValueList valueList)
        {
            if (valueList is IExchangableValueList)
            {
                string currencyName = valueList.Names.Currency;
                return Currency(currencyName);
            }

            if (valueList is ICurrency currency)
            {
                return currency;
            }

            return null;
        }

        /// <summary>
        /// returns the currency associated to the account.
        /// </summary>
        public ICurrency Currency(string currencyName)
        {
            ICurrency currency = CurrenciesThreadSafe.FirstOrDefault(cur => cur.BaseCurrency == currencyName && cur.QuoteCurrency == BaseCurrency);
            return currency ?? (CurrenciesThreadSafe.FirstOrDefault(cur => cur.BaseCurrency == BaseCurrency && cur.QuoteCurrency == currencyName)?.Inverted());
        }
    }
}
