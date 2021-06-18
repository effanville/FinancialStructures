using System;
using FinancialStructures.FinanceStructures.Implementation;
using FinancialStructures.NamingStructures;

namespace FinancialStructures.Tests.TestDatabaseConstructor
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
            item.Shares.SetData(date, numberUnits);
            item.UnitPrice.SetData(date, sharePrice);
            if (investment != 0)
            {
                item.Investments.SetData(date, investment);
            }

            return this;
        }
    }
}
