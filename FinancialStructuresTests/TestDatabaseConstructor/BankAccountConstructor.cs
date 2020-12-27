using System;
using FinancialStructures.FinanceStructures.Implementation;
using FinancialStructures.NamingStructures;
using StructureCommon.DataStructures;

namespace FinancialStructures.Tests.TestDatabaseConstructor
{
    public class BankAccountConstructor
    {
        public CashAccount item;

        public BankAccountConstructor(string company, string name, string currency = null, string url = null, string sectors = null)
        {
            var names = new NameData(company, name, currency, url);
            names.SectorsFlat = sectors;
            item = new CashAccount(names);
        }

        public BankAccountConstructor WithData(DateTime date, double price)
        {
            item.Amounts.Values.Add(new DailyValuation(date, price));

            return this;
        }
    }
}
