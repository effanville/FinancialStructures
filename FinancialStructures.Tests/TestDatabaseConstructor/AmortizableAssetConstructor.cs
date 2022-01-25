using System;

using FinancialStructures.FinanceStructures.Implementation.Asset;
using FinancialStructures.NamingStructures;

namespace FinancialStructures.Tests.TestDatabaseConstructor
{
    public class AmortizableAssetConstructor
    {
        public const string DefaultName = "MyHouse";
        public const string DefaultCompany = "House";
        public static readonly DateTime[] DefaultValueDates = new DateTime[] { new DateTime(2010, 1, 1), new DateTime(2011, 1, 1), new DateTime(2012, 5, 1), new DateTime(2015, 4, 3), new DateTime(2018, 5, 6), new DateTime(2020, 1, 1) };
        public static readonly decimal[] DefaultValues = new decimal[] { 100000.0m, 150000m, 173000m, 220000m, 190000m, 200000m };
        public static readonly DateTime[] DefaultDebtDates = new DateTime[] { new DateTime(2010, 1, 1), new DateTime(2011, 1, 1), new DateTime(2012, 5, 1), new DateTime(2015, 4, 3), new DateTime(2018, 5, 6), new DateTime(2020, 1, 1) };
        public static readonly decimal[] DefaultDebt = new decimal[] { 80000.0m, 75000.0m, 60000m, 60000m, 50000m, 20000m };

        public const string SecondaryName = "MyRental";
        public const string SecondaryCompany = "House";
        public static readonly DateTime[] SecondaryValueDates = new DateTime[] { new DateTime(2010, 1, 5), new DateTime(2011, 2, 1), new DateTime(2012, 5, 5), new DateTime(2016, 4, 3), new DateTime(2019, 5, 6), new DateTime(2020, 1, 1) };
        public static readonly decimal[] SecondaryValues = new decimal[] { 1500m, 1600m, 1200m, 1300m, 1000m, 1500m };
        public static readonly DateTime[] SecondaryDebtDates = new DateTime[] { new DateTime(2010, 1, 5), new DateTime(2011, 2, 1), new DateTime(2019, 5, 6), new DateTime(2020, 1, 1) };
        public static readonly decimal[] SecondaryDebt = new decimal[] { 1010.0m, 1110.0m, 1215.2m, 900.6m };

        private readonly AmortisableAsset Item;

        private AmortizableAssetConstructor(string company, string name, string currency = null, string url = null, string sectors = null)
        {
            NameData names = new NameData(company, name, currency, url)
            {
                SectorsFlat = sectors
            };
            Item = new AmortisableAsset(names);
        }

        private void WithValueData(DateTime valueDate, decimal value)
        {
            Item.Values.SetData(valueDate, value);
        }

        private void WithDebtData(DateTime debtDate, decimal debtValue)
        {
            Item.Debt.SetData(debtDate, debtValue);
        }

        public static AmortisableAsset Empty()
        {
            return new AmortizableAssetConstructor(DefaultCompany, DefaultName).Item;
        }

        public static AmortisableAsset Default()
        {
            return FromNameAndData(
                DefaultCompany,
                DefaultName,
                valueDates: DefaultValueDates,
                value: DefaultValues,
                debtDates: DefaultDebtDates,
                debt: DefaultDebt).Item;
        }

        public static AmortisableAsset Secondary()
        {
            return FromNameAndData(
                SecondaryCompany,
                SecondaryName,
                currency: DatabaseConstructor.DefaultCurrencyCompany,
                valueDates: SecondaryValueDates,
                value: SecondaryValues,
                debtDates: SecondaryDebtDates,
                debt: SecondaryDebt).Item;
        }

        public static AmortizableAssetConstructor FromNameAndData(
            string company,
            string name,
            string currency = null,
            string url = null,
            string sectors = null,
            DateTime[] valueDates = null,
            decimal[] value = null,
            DateTime[] debtDates = null,
            decimal[] debt = null)
        {
            AmortizableAssetConstructor securityConstructor = new AmortizableAssetConstructor(company, name, currency, url, sectors);
            if (valueDates != null)
            {
                for (int i = 0; i < valueDates.Length; i++)
                {
                    if (valueDates[i] != default(DateTime))
                    {
                        securityConstructor.WithValueData(valueDates[i], value[i]);
                    }
                }
            }

            if (debtDates != null)
            {
                for (int i = 0; i < debtDates.Length; i++)
                {
                    if (debtDates[i] != default(DateTime))
                    {
                        securityConstructor.WithDebtData(debtDates[i], debt[i]);
                    }
                }
            }
            return securityConstructor;
        }
    }
}
