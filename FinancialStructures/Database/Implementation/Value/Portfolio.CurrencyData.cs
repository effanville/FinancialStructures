using System.Collections.Generic;
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
                case Account.Asset:
                {
                    string currencyName = valueList.Names.Currency;
                    IReadOnlyList<ICurrency> currencies = CurrenciesThreadSafe;
                    foreach (ICurrency curr in currencies)
                    {
                        if (curr.BaseCurrency == currencyName && curr.QuoteCurrency == BaseCurrency)
                        {
                            return curr;
                        }
                        else if (curr.BaseCurrency == BaseCurrency && curr.QuoteCurrency == currencyName)
                        {
                            return curr.Inverted();
                        }
                    }

                    return null;
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
