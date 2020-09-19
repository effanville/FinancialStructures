using System;
using FinancialStructures.FinanceStructures;
using FinancialStructures.NamingStructures;
using StructureCommon.DataStructures;

namespace FinancialStructures.Tests.TestDatabaseConstructor
{
    public class CurrencyConstructor
    {
        public Currency item;

        public CurrencyConstructor(string company, string name, string currency = null, string url = null, string sectors = null)
        {
            var names = new NameData(company, name, currency, url);
            names.SectorsFlat = sectors;
            item = new Currency(names);
        }

        public CurrencyConstructor WithData(DateTime date, double price)
        {
            item.Values.Values.Add(new DailyValuation(date, price));

            return this;
        }
    }
}
