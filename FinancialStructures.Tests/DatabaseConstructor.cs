﻿using System;
using System.IO.Abstractions;
using FinancialStructures.Database;
using FinancialStructures.Database.Implementation;
using FinancialStructures.FinanceStructures.Implementation;
using FinancialStructures.NamingStructures;

namespace FinancialStructures.Tests.TestDatabaseConstructor
{
    public class DatabaseConstructor
    {
        internal Portfolio Database;
        public DatabaseConstructor(string filePath = null, string currency = null)
        {
            Database = new Portfolio
            {
                FilePath = filePath ?? null,
                BaseCurrency = currency ?? DefaultCurrencyName
            };
        }

        public DatabaseConstructor LoadDatabaseFromFilepath(IFileSystem fileSystem, string filepath)
        {
            Database.LoadPortfolio(filepath, fileSystem, null);
            return this;
        }

        public static TwoName DefaultName(Account acctype)
        {
            switch (acctype)
            {
                case Account.Security:
                    return new TwoName(SecurityConstructor.DefaultCompany, SecurityConstructor.DefaultName);
                case Account.BankAccount:
                    return new TwoName(BankAccountConstructor.DefaultCompany, BankAccountConstructor.DefaultName);
                case Account.Currency:
                    return new TwoName(DefaultCurrencyCompany, DefaultCurrencyName);
                case Account.All:
                case Account.Benchmark:
                default:
                    return null;
            }
        }

        public static TwoName SecondaryName(Account account)
        {
            switch (account)
            {
                case Account.Security:
                    return new TwoName(SecurityConstructor.SecondaryCompany, SecurityConstructor.SecondaryName);
                case Account.BankAccount:
                    return new TwoName(BankAccountConstructor.SecondaryCompany, BankAccountConstructor.SecondaryName);
                case Account.All:
                case Account.Benchmark:
                case Account.Currency:
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
                case Account.Currency:
                    return WithDefaultCurrency();
                case Account.All:
                case Account.Benchmark:
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

        public DatabaseConstructor SetFilePath(string filePath)
        {
            Database.FilePath = filePath;
            return this;
        }

        public DatabaseConstructor SetCurrencyAsGBP()
        {
            Database.BaseCurrency = "GBP";
            return this;
        }
        public DatabaseConstructor SetCurrency(string currency)
        {
            Database.BaseCurrency = currency;
            return this;
        }

        public DatabaseConstructor WithAccountFromNameAndData(
            Account accType,
            string company,
            string name,
            string currency = null,
            string url = null,
            string sectors = null,
            DateTime[] dates = null,
            decimal[] sharePrice = null,
            decimal[] numberUnits = null,
            decimal[] investment = null)
        {
            switch (accType)
            {
                case Account.Security:
                {
                    return WithSecurity(company, name, currency, url, sectors, dates, sharePrice, numberUnits, investment);
                }
                case Account.BankAccount:
                {
                    return WithBankAccount(company, name, currency, url, sectors, dates, numberUnits);
                }
                case Account.Currency:
                {
                    return WithCurrencyFromNameAndData(company, name, currency, url, dates, numberUnits);
                }
                default:
                    return null;
            }
        }

        public DatabaseConstructor WithAccountFromName(Account accType, string company, string name, string currency = null, string url = null, string sectors = null)
        {
            switch (accType)
            {
                case Account.Security:
                {
                    return WithSecurity(company, name, currency, url, sectors);
                }
                case Account.BankAccount:
                {
                    return WithBankAccount(company, name, currency, url, sectors);
                }
                case Account.Currency:
                {
                    return WithCurrencyFromNameAndData(company, name, currency, url);
                }
                case Account.Benchmark:
                {
                    return WithSectorFromName(company, name, currency, url);
                }
                default:
                    return null;
            }
        }

        public DatabaseConstructor WithDefaultSecurity()
        {
            Database.Funds.Add(SecurityConstructor.Default());
            return this;
        }

        public DatabaseConstructor WithSecondarySecurity()
        {
            Database.Funds.Add(SecurityConstructor.Secondary());
            return this;
        }

        public DatabaseConstructor WithSecurity(string company, string name)
        {
            Database.Funds.Add(SecurityConstructor.WithName(company, name).GetItem());
            return this;
        }

        public DatabaseConstructor WithSecurity(
            string company,
            string name,
            string currency = null,
            string url = null,
            string sectors = null,
            DateTime[] dates = null,
            decimal[] sharePrice = null,
            decimal[] numberUnits = null,
            decimal[] investment = null)
        {
            Database.Funds.Add(SecurityConstructor.WithNameAndData(company, name, currency, url, sectors, dates, sharePrice, numberUnits, investment).GetItem());
            return this;
        }

        public DatabaseConstructor WithDefaultBankAccount()
        {
            Database.BankAccounts.Add(BankAccountConstructor.Default().Item);
            return this;
        }

        public DatabaseConstructor WithSecondaryBankAccount()
        {
            Database.BankAccounts.Add(BankAccountConstructor.Secondary().Item);
            return this;
        }

        public DatabaseConstructor WithBankAccount(string company, string name)
        {
            Database.BankAccounts.Add(BankAccountConstructor.FromName(company, name).Item);
            return this;
        }

        public DatabaseConstructor WithBankAccount(string company, string name, string currency = null, string url = null, string sectors = null, DateTime[] dates = null, decimal[] values = null)
        {
            Database.BankAccounts.Add(BankAccountConstructor.FromNameAndData(company, name, currency, url, sectors, dates: dates, values: values).Item);
            return this;
        }

        public const string DefaultCurrencyCompany = "HKD";
        public const string DefaultCurrencyName = "GBP";
        public readonly DateTime[] DefaultCurrencyDateTimes = new DateTime[] { new DateTime(2011, 11, 1), new DateTime(2018, 1, 14), new DateTime(2020, 8, 3) };
        public readonly decimal[] DefaultCurrencyValues = new decimal[] { 0.081m, 0.09m, 0.0987m };

        public DatabaseConstructor WithDefaultCurrency()
        {
            return WithCurrencyFromNameAndData(DefaultCurrencyCompany, DefaultCurrencyName, date: DefaultCurrencyDateTimes, value: DefaultCurrencyValues);
        }

        public DatabaseConstructor WithCurrency(string company, string name, string url = null, string sectors = null)
        {
            Database.Currencies.Add(new CurrencyConstructor(company, name, null, url, sectors).item);
            return this;
        }

        public DatabaseConstructor WithCurrencyFromNameAndData(string company, string name, string currency = null, string url = null, DateTime[] date = null, decimal[] value = null)
        {
            CurrencyConstructor bankConstructor = new CurrencyConstructor(company, name, currency, url);
            if (date != null)
            {
                for (int i = 0; i < date.Length; i++)
                {
                    _ = bankConstructor.WithData(date[i], value[i]);
                }
            }

            Database.Currencies.Add(bankConstructor.item);
            return this;
        }

        public DatabaseConstructor WithSectorFromName(string company, string name, string currency = null, string url = null)
        {
            Database.BenchMarks.Add(new Sector(new NameData(company, name, currency, url)));
            return this;
        }

        public DatabaseConstructor WithSectorFromNameAndData(string company, string name, string currency = null, string url = null, DateTime[] date = null, decimal[] value = null)
        {
            SectorConstructor bankConstructor = new SectorConstructor(company, name, currency, url);
            if (date != null)
            {
                for (int i = 0; i < date.Length; i++)
                {
                    _ = bankConstructor.WithData(date[i], value[i]);
                }
            }

            Database.BenchMarks.Add(bankConstructor.item);
            return this;
        }

        public DatabaseConstructor ClearDatabase()
        {
            Database.Clear();
            return this;
        }
    }
}
