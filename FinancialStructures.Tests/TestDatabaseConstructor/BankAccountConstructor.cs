using System;
using FinancialStructures.FinanceStructures.Implementation;
using FinancialStructures.NamingStructures;

namespace FinancialStructures.Tests.TestDatabaseConstructor
{
    public class BankAccountConstructor
    {
        public const string DefaultName = "Current";
        public const string DefaultCompany = "Santander";
        public static readonly DateTime[] DefaultDates = new DateTime[] { new DateTime(2010, 1, 1), new DateTime(2011, 1, 1), new DateTime(2012, 5, 1), new DateTime(2015, 4, 3), new DateTime(2018, 5, 6), new DateTime(2020, 1, 1) };
        public static readonly double[] DefaultValues = new double[] { 100.0, 100.0, 125.2, 90.6, 77.7, 101.1 };

        public const string SecondaryName = "Current";
        public const string SecondaryCompany = "Halifax";
        public static readonly DateTime[] SecondaryDates = new DateTime[] { new DateTime(2010, 1, 1), new DateTime(2011, 1, 1), new DateTime(2012, 5, 1), new DateTime(2015, 4, 3), new DateTime(2018, 5, 6), new DateTime(2020, 1, 1) };
        public static readonly double[] SecondaryValues = new double[] { 1100.0, 2100.0, 1125.2, 900.6, 770.7, 1001.1 };

        public CashAccount Item;

        private BankAccountConstructor(string company, string name, string currency = null, string url = null, string sectors = null)
        {
            NameData names = new NameData(company, name, currency, url)
            {
                SectorsFlat = sectors
            };
            Item = new CashAccount(names);
        }

        private BankAccountConstructor WithData(DateTime date, double price)
        {
            Item.Amounts.SetData(date, price);
            return this;
        }

        public static BankAccountConstructor Default()
        {
            return FromNameAndData(DefaultCompany, DefaultName, dates: DefaultDates, values: DefaultValues);
        }

        public static BankAccountConstructor Secondary()
        {
            return FromNameAndData(SecondaryCompany, SecondaryName, currency: DatabaseConstructor.DefaultCurrencyCompany, dates: SecondaryDates, values: SecondaryValues);
        }

        public static BankAccountConstructor FromName(string company, string name, string currency = null, string url = null, string sectors = null)
        {
            return new BankAccountConstructor(company, name, currency, url, sectors);
        }

        public static BankAccountConstructor FromNameAndData(string company, string name, string currency = null, string url = null, string sectors = null, DateTime[] dates = null, double[] values = null)
        {
            BankAccountConstructor bankConstructor = new BankAccountConstructor(company, name, currency, url, sectors);
            if (dates != null)
            {
                for (int i = 0; i < dates.Length; i++)
                {
                    _ = bankConstructor.WithData(dates[i], values[i]);
                }
            }

            return bankConstructor;
        }
    }
}
