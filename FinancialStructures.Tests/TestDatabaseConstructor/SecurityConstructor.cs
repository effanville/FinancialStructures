using System;
using FinancialStructures.FinanceStructures.Implementation;
using FinancialStructures.NamingStructures;

namespace FinancialStructures.Tests.TestDatabaseConstructor
{
    public class SecurityConstructor
    {
        public const string DefaultName = "UK Stock";
        public const string DefaultCompany = "BlackRock";
        public static readonly DateTime[] DefaultDates = new DateTime[] { new DateTime(2010, 1, 1), new DateTime(2011, 1, 1), new DateTime(2012, 5, 1), new DateTime(2015, 4, 3), new DateTime(2018, 5, 6), new DateTime(2020, 1, 1) };
        public static readonly double[] DefaultShareValues = new double[] { 2.0, 1.5, 17.3, 4, 5.7, 5.5 };
        public static readonly double[] DefaultUnitPrices = new double[] { 100.0, 100.0, 125.2, 90.6, 77.7, 101.1 };
        public static readonly double[] DefaultInvestments = new double[] { 100.0, 0.0, 0.0, 0.0, 0.0, 0.0 };

        public const string SecondaryName = "China Stock";
        public const string SecondaryCompany = "Prudential";
        public static readonly DateTime[] SecondaryDates = new DateTime[] { new DateTime(2010, 1, 5), new DateTime(2011, 2, 1), new DateTime(2012, 5, 5), new DateTime(2016, 4, 3), new DateTime(2019, 5, 6), new DateTime(2020, 1, 1) };
        public static readonly double[] SecondaryShareValues = new double[] { 2.0, 2.5, 17.3, 22.5, 22.7, 25.5 };
        public static readonly double[] SecondaryUnitPrices = new double[] { 1010.0, 1110.0, 1215.2, 900.6, 1770.7, 1001.1 };
        public static readonly double[] SecondaryInvestments = new double[] { 2020.0, 0.0, 21022.96, 0.0, 0.0, 0.0 };

        public Security Item;

        private SecurityConstructor(string company, string name, string currency = null, string url = null, string sectors = null)
        {
            var names = new NameData(company, name, currency, url);
            names.SectorsFlat = sectors;
            Item = new Security(names);
        }

        private SecurityConstructor WithData(DateTime date, double sharePrice, double numberUnits, double investment = 0)
        {
            Item.Shares.SetData(date, numberUnits);
            Item.UnitPrice.SetData(date, sharePrice);
            if (investment != 0)
            {
                Item.Investments.SetData(date, investment);
            }

            return this;
        }

        public static SecurityConstructor Default()
        {
            return FromNameAndData(DefaultCompany, DefaultName, dates: DefaultDates, sharePrice: DefaultUnitPrices, numberUnits: DefaultShareValues, investment: DefaultInvestments);
        }

        public static SecurityConstructor Secondary()
        {
            return FromNameAndData(SecondaryCompany, SecondaryName, currency: DatabaseConstructor.DefaultCurrencyCompany, dates: SecondaryDates, sharePrice: SecondaryUnitPrices, numberUnits: SecondaryShareValues, investment: SecondaryInvestments);
        }

        public static SecurityConstructor FromName(string company, string name, string currency = null, string url = null, string sectors = null)
        {
            return new SecurityConstructor(company, name, currency, url, sectors);
        }

        public static SecurityConstructor FromNameAndData(string company, string name, string currency = null, string url = null, string sectors = null, DateTime[] dates = null, double[] sharePrice = null, double[] numberUnits = null, double[] investment = null)
        {
            var securityConstructor = new SecurityConstructor(company, name, currency, url, sectors);
            if (dates != null)
            {
                for (int i = 0; i < dates.Length; i++)
                {
                    if (dates[i] != default(DateTime))
                    {
                        _ = securityConstructor.WithData(dates[i], sharePrice[i], numberUnits?[i] ?? 0.0, investment?[i] ?? 0.0);
                    }
                }
            }
            return securityConstructor;
        }
    }
}
