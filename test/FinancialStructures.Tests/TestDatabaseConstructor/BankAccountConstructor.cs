﻿using System;

using Effanville.FinancialStructures.FinanceStructures.Implementation;
using Effanville.FinancialStructures.NamingStructures;

namespace Effanville.FinancialStructures.Tests.TestDatabaseConstructor
{
    public class BankAccountConstructor
    {
        public const string DefaultName = "Current";
        public const string DefaultCompany = "Santander";
        public static readonly DateTime[] DefaultDates = new DateTime[] { new DateTime(2010, 1, 1), new DateTime(2011, 1, 1), new DateTime(2012, 5, 1), new DateTime(2015, 4, 3), new DateTime(2018, 5, 6), new DateTime(2020, 1, 1) };
        public static readonly decimal[] DefaultValues = new decimal[] { 100.0m, 100.0m, 125.2m, 90.6m, 77.7m, 101.1m };

        public const string SecondaryName = "Current";
        public const string SecondaryCompany = "Halifax";
        public static readonly DateTime[] SecondaryDates = new DateTime[] { new DateTime(2010, 1, 1), new DateTime(2011, 1, 1), new DateTime(2012, 5, 1), new DateTime(2015, 4, 3), new DateTime(2018, 5, 6), new DateTime(2020, 1, 1) };
        public static readonly decimal[] SecondaryValues = new decimal[] { 1100.0m, 2100.0m, 1125.2m, 900.6m, 770.7m, 1001.1m };

        public CashAccount Item;

        private BankAccountConstructor(string company, string name, string currency = null, string url = null, string sectors = null)
        {
            NameData names = new NameData(company, name, currency, url)
            {
                SectorsFlat = sectors
            };
            Item = new CashAccount(names);
        }

        private BankAccountConstructor WithData(DateTime date, decimal price)
        {
            Item.Values.SetData(date, price);
            return this;
        }

        public static BankAccountConstructor Empty()
        {
            return new BankAccountConstructor(DefaultCompany, DefaultName);
        }

        public static BankAccountConstructor Default()
        {
            return FromNameAndData(DefaultCompany, DefaultName, dates: DefaultDates, values: DefaultValues);
        }

        public static BankAccountConstructor Secondary()
        {
            return FromNameAndData(SecondaryCompany, SecondaryName, currency: CurrencyConstructor.DefaultCompany, dates: SecondaryDates, values: SecondaryValues);
        }

        public static BankAccountConstructor FromName(string company, string name, string currency = null, string url = null, string sectors = null)
        {
            return new BankAccountConstructor(company, name, currency, url, sectors);
        }

        public static BankAccountConstructor FromNameAndData(string company, string name, string currency = null, string url = null, string sectors = null, DateTime[] dates = null, decimal[] values = null)
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
