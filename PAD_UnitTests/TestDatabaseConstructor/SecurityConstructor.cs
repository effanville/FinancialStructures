using System;
using FinancialStructures.FinanceStructures;
using FinancialStructures.NamingStructures;
using StructureCommon.DataStructures;

namespace FinancialStructures_UnitTests.TestDatabaseConstructor
{
    public class SecurityConstructor
    {
        public Security item;

        public SecurityConstructor(string company, string name, string currency = null, string url = null, string sectors = null)
        {
            var names = new NameData(company, name, currency, url);
            names.SectorsFlat = sectors;
            item = new Security(names);
        }

        public SecurityConstructor WithData(DateTime date, double sharePrice, double numberUnits, double investment = 0)
        {
            item.Shares.Values.Add(new DailyValuation(date, numberUnits));
            item.UnitPrice.Values.Add(new DailyValuation(date, sharePrice));
            if (investment != 0)
            {
                item.Investments.Values.Add(new DailyValuation(date, investment));
            }

            return this;
        }
    }
}
