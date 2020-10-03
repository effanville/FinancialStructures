﻿using System;
using FinancialStructures.FinanceStructures;
using FinancialStructures.Database;
using FinancialStructures.NamingStructures;
using FinancialStructures.FinanceInterfaces;

namespace FinancialStructures.Tests.TestDatabaseConstructor
{
    public class DatabaseConstructor
    {
        public Portfolio database;
        public DatabaseConstructor()
        {
            database = new Portfolio();
        }

        public DatabaseConstructor LoadDatabaseFromFilepath(string filepath)
        {
            database.LoadPortfolio(filepath, null);
            return this;
        }

        public const string DefaultSecurityName = "UK Stock";
        public const string DefaultSecurityCompany = "BlackRock";
        public readonly DateTime[] DefaultSecurityDates = new DateTime[] { new DateTime(2010, 1, 1), new DateTime(2011, 1, 1), new DateTime(2012, 5, 1), new DateTime(2015, 4, 3), new DateTime(2018, 5, 6), new DateTime(2020, 1, 1) };
        public readonly double[] DefaultSecurityShareValues = new double[] { 2.0, 1.5, 17.3, 4, 5.7, 5.5 };
        public readonly double[] DefaultSecurityUnitPrices = new double[] { 100.0, 100.0, 125.2, 90.6, 77.7, 101.1 };
        public readonly double[] DefaultSecurityInvestments = new double[] { 1, 0.0, 0.0, 0.0, 0.0, 0.0 };

        public TwoName DefaultNameQuery(Account acctype)
        {
            switch (acctype)
            {
                case Account.Security:
                    return new TwoName(DefaultSecurityCompany, DefaultSecurityName);
                case Account.BankAccount:
                    return new TwoName(DefaultBankAccountCompany, DefaultBankAccountName);
                default:
                    return null;
            }
        }

        public DatabaseConstructor WithDefaultFromType(Account acctype)
        {
            switch (acctype)
            {
                case Account.Security:
                    return WithDefaultSecurity();
                case Account.BankAccount:
                    return WithDefaultBankAccount();
                default:
                    return null;
            }
        }

        public DatabaseConstructor WithSecondaryFromType(Account acctype)
        {
            switch (acctype)
            {
                case Account.Security:
                    return WithSecondarySecurity();
                case Account.BankAccount:
                    return WithSecondaryBankAccount();
                default:
                    return null;
            }
        }

        public DatabaseConstructor WithDefaultSecurity()
        {
            return WithSecurityFromNameAndData(DefaultSecurityCompany, DefaultSecurityName, dates: DefaultSecurityDates, sharePrice: DefaultSecurityUnitPrices, numberUnits: DefaultSecurityShareValues, investment: DefaultSecurityInvestments);
        }

        public const string SecondarySecurityName = "China Stock";
        public const string SecondarySecurityCompany = "Prudential";
        public readonly DateTime[] SecondarySecurityDates = new DateTime[] { new DateTime(2010, 1, 5), new DateTime(2011, 2, 1), new DateTime(2012, 5, 5), new DateTime(2016, 4, 3), new DateTime(2019, 5, 6), new DateTime(2020, 1, 1) };
        public readonly double[] SecondarySecurityShareValues = new double[] { 2.0, 2.5, 17.3, 22.5, 22.7, 25.5 };
        public readonly double[] SecondarySecurityUnitPrices = new double[] { 1010.0, 1110.0, 1215.2, 900.6, 1770.7, 1001.1 };
        public readonly double[] SecondarySecurityInvestments = new double[] { 1, 0.0, 1, 0.0, 0.0, 0.0 };

        public DatabaseConstructor WithSecondarySecurity()
        {
            return WithSecurityFromNameAndData(SecondarySecurityCompany, SecondarySecurityName, dates: SecondarySecurityDates, sharePrice: SecondarySecurityUnitPrices, numberUnits: SecondarySecurityShareValues, investment: SecondarySecurityInvestments);
        }

        public string DefaultBankAccountName = "Current";
        public string DefaultBankAccountCompany = "Santander";
        public DateTime[] DefaultBankAccountDates = new DateTime[] { new DateTime(2010, 1, 1), new DateTime(2011, 1, 1), new DateTime(2012, 5, 1), new DateTime(2015, 4, 3), new DateTime(2018, 5, 6), new DateTime(2020, 1, 1) };
        public double[] DefaultBankAccountValues = new double[] { 100.0, 100.0, 125.2, 90.6, 77.7, 101.1 };

        public DatabaseConstructor WithDefaultBankAccount()
        {
            return WithBankAccountFromNameAndData(DefaultBankAccountCompany, DefaultBankAccountName, date: DefaultBankAccountDates, value: DefaultBankAccountValues);
        }


        public string SecondaryBankAccountName = "Current";
        public string SecondaryBankAccountCompany = "Halifax";
        public DateTime[] SecondaryBankAccountDates = new DateTime[] { new DateTime(2010, 1, 1), new DateTime(2011, 1, 1), new DateTime(2012, 5, 1), new DateTime(2015, 4, 3), new DateTime(2018, 5, 6), new DateTime(2020, 1, 1) };
        public double[] SecondaryBankAccountValues = new double[] { 1100.0, 2100.0, 1125.2, 900.6, 770.7, 1001.1 };

        public DatabaseConstructor WithSecondaryBankAccount()
        {
            return WithBankAccountFromNameAndData(SecondaryBankAccountCompany, SecondaryBankAccountName, date: SecondaryBankAccountDates, value: SecondaryBankAccountValues);
        }

        public DatabaseConstructor WithAccountFromNameAndData(Account accType, string company, string name, string currency = null, string url = null, string sectors = null, DateTime[] dates = null, double[] sharePrice = null, double[] numberUnits = null, double[] investment = null)
        {
            switch (accType)
            {
                case Account.Security:
                {
                    return WithSecurityFromNameAndData(company, name, currency, url, sectors, dates, sharePrice, numberUnits, investment);
                }
                case Account.BankAccount:
                {
                    return WithBankAccountFromNameAndData(company, name, currency, url, sectors, dates, numberUnits);
                }
                default:
                    return null;
            }
        }

        public DatabaseConstructor WithSecurityFromName(string company, string name, string currency = null, string url = null, string sectors = null)
        {
            var securityConstructor = new SecurityConstructor(company, name, currency, url, sectors);
            database.Funds.Add(securityConstructor.item);
            return this;
        }

        public DatabaseConstructor WithSecurityFromNameAndDataPoint(string company, string name, string currency = null, string url = null, string sectors = null, DateTime date = new DateTime(), double sharePrice = 0, double numberUnits = 0, double investment = 0)
        {
            var securityConstructor = new SecurityConstructor(company, name, currency, url, sectors);
            securityConstructor.WithData(date, sharePrice, numberUnits, investment);
            database.Funds.Add(securityConstructor.item);
            return this;
        }

        public DatabaseConstructor WithSecurityFromNameAndData(string company, string name, string currency = null, string url = null, string sectors = null, DateTime[] dates = null, double[] sharePrice = null, double[] numberUnits = null, double[] investment = null)
        {
            var securityConstructor = new SecurityConstructor(company, name, currency, url, sectors);
            if (dates != null)
            {
                for (int i = 0; i < dates.Length; i++)
                {
                    securityConstructor.WithData(dates[i], sharePrice[i], numberUnits[i], investment[i]);
                }
            }
            database.Funds.Add(securityConstructor.item);
            return this;
        }

        public DatabaseConstructor WithBankAccountFromName(string company, string name, string currency = null, string url = null, string sectors = null)
        {
            var bAConstructor = new BankAccountConstructor(company, name, currency, url, sectors);
            database.BankAccounts.Add(bAConstructor.item);
            return this;
        }

        public DatabaseConstructor WithBankAccountFromNameAndDataPoint(string company, string name, string currency = null, string url = null, string sectors = null, DateTime date = new DateTime(), double value = 0)
        {
            var bankConstructor = new BankAccountConstructor(company, name, currency, url, sectors);
            bankConstructor.WithData(date, value);
            database.BankAccounts.Add(bankConstructor.item);
            return this;
        }

        public DatabaseConstructor WithBankAccountFromNameAndData(string company, string name, string currency = null, string url = null, string sectors = null, DateTime[] date = null, double[] value = null)
        {
            var bankConstructor = new BankAccountConstructor(company, name, currency, url, sectors);
            if (date != null)
            {
                for (int i = 0; i < date.Length; i++)
                {

                    bankConstructor.WithData(date[i], value[i]);
                }
            }

            database.BankAccounts.Add(bankConstructor.item);
            return this;
        }

        public DatabaseConstructor WithCurrencyFromName(string company, string name, string url = null, string sectors = null)
        {
            var names = new NameData(company, name, null, url);
            names.SectorsFlat = sectors;
            database.Currencies.Add(new Currency(names));
            return this;
        }

        public DatabaseConstructor WithSectorFromName(string company, string name, string currency = null, string url = null)
        {
            database.BenchMarks.Add(new Sector(new NameData(company, name, currency, url)));
            return this;
        }

        public DatabaseConstructor WithSectorFromNameAndData(string company, string name, string currency = null, string url = null, DateTime[] date = null, double[] value = null)
        {
            var bankConstructor = new SectorConstructor(company, name, currency, url);
            if (date != null)
            {
                for (int i = 0; i < date.Length; i++)
                {

                    bankConstructor.WithData(date[i], value[i]);
                }
            }

            database.BenchMarks.Add(bankConstructor.item);
            return this;
        }
    }
}