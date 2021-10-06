using System;
using FinancialStructures.FinanceStructures.Implementation;
using FinancialStructures.NamingStructures;

namespace FinancialStructures.Tests.TestDatabaseConstructor
{
    public class SectorConstructor
    {
        public Sector item;

        public SectorConstructor(string company, string name, string currency = null, string url = null)
        {
            NameData names = new NameData(company, name, currency, url);
            item = new Sector(names);
        }

        public SectorConstructor WithData(DateTime date, double valueToAdd)
        {
            item.Values.SetData(date, valueToAdd);
            return this;
        }
    }
}
