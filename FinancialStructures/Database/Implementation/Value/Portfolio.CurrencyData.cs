using FinancialStructures.FinanceStructures;
using FinancialStructures.FinanceStructures.Implementation;

namespace FinancialStructures.Database.Implementation
{
    public partial class Portfolio
    {
        /// <summary>
        /// returns the currency associated to the account.
        /// </summary>
        public ICurrency Currency(Account account, object valueList)
        {
            switch (account)
            {
                case (Account.Security):
                case (Account.BankAccount):
                {
                    string currencyName = ((IValueList)valueList).Names.Currency;
                    ICurrency currency = Currencies.Find(cur => cur.BaseCurrency == currencyName && cur.QuoteCurrency == BaseCurrency);
                    if (currency != null)
                    {
                        return currency;
                    }

                    return Currencies.Find(cur => cur.BaseCurrency == BaseCurrency && cur.QuoteCurrency == currencyName)?.Inverted();
                }
                case (Account.Currency):
                {
                    return (ICurrency)valueList;
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
