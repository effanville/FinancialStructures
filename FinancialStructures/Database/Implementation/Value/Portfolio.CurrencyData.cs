using System.Linq;
using FinancialStructures.FinanceStructures;
using FinancialStructures.FinanceStructures.Implementation;

namespace FinancialStructures.Database.Implementation
{
    public partial class Portfolio
    {
        /// <summary>
        /// returns the currency associated to the account.
        /// </summary>
        public ICurrency Currency(Account account, IValueList valueList)
        {
            switch (account)
            {
                case Account.Security:
                case Account.BankAccount:
                {
                    string currencyName = valueList.Names.Currency;
                    ICurrency currency = CurrenciesThreadSafe.FirstOrDefault(cur => cur.BaseCurrency == currencyName && cur.QuoteCurrency == BaseCurrency);
                    return currency ?? CurrenciesThreadSafe.FirstOrDefault(cur => cur.BaseCurrency == BaseCurrency && cur.QuoteCurrency == currencyName)?.Inverted();
                }
                case Account.Currency:
                {
                    return (ICurrency)valueList;
                }
                case Account.Benchmark:
                case Account.All:
                default:
                {
                    return new Currency();
                }
            }
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
