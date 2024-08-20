using System.Linq;

using Effanville.FinancialStructures.FinanceStructures;

namespace Effanville.FinancialStructures.Database.Implementation
{
    public partial class Portfolio
    {
        /// <summary>
        /// returns the currency associated to the account.
        /// </summary>
        public ICurrency Currency(IValueList valueList)
        {
            if (valueList is IExchangeableValueList)
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
            ICurrency currency = Enumerable.FirstOrDefault<ICurrency>(Currencies, cur => cur.BaseCurrency == currencyName && cur.QuoteCurrency == BaseCurrency);
            return currency ?? (Enumerable.FirstOrDefault<ICurrency>(Currencies, cur => cur.BaseCurrency == BaseCurrency && cur.QuoteCurrency == currencyName)?.Inverted());
        }
    }
}
