using System;

using FinancialStructures.FinanceStructures.Implementation;
using FinancialStructures.NamingStructures;

namespace Effanville.FinancialStructures.Tests.TestDatabaseConstructor
{
    public class CurrencyConstructor
    {
        public const string DefaultCompany = "HKD";
        public const string DefaultName = "GBP";
        private Currency _item;

        public CurrencyConstructor(string company, string name, string currency = null, string url = null, string sectors = null)
        {
            NameData names = new NameData(company, name, currency, url)
            {
                SectorsFlat = sectors
            };
            _item = new Currency(names);
        }

        public CurrencyConstructor WithData(DateTime date, decimal price)
        {
            _item.Values.SetData(date, price);

            return this;
        }

        public Currency GetInstance() => _item;
    }
}
