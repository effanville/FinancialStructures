using System;
using FinancialStructures.FinanceStructures;
using FinancialStructures.NamingStructures;
using StructureCommon.DataStructures;

namespace FinancialStructures_UnitTests.TestDatabaseConstructor
{
    public class SectorConstructor
    {
        public Sector item;

        public SectorConstructor(string company, string name, string currency = null, string url = null)
        {
            var names = new NameData(company, name, currency, url);
            item = new Sector(names);
        }

        public SectorConstructor WithData(DateTime date, double valueToAdd)
        {
            item.Values.Values.Add(new DailyValuation(date, valueToAdd));
            return this;
        }
    }
}
