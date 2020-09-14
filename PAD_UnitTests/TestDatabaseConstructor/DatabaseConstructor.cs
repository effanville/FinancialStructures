using System;
using FinancialStructures.FinanceStructures;
using FinancialStructures.Database;
using FinancialStructures.NamingStructures;
using FinancialStructures.PortfolioAPI;

namespace FinancialStructures_UnitTests.TestDatabaseConstructor
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

        public TwoName DefaultNameQuery(AccountType acctype)
        {
            switch (acctype)
            {
                case AccountType.Security:
                    return new TwoName(DefaultSecurityCompany, DefaultSecurityName);
                case AccountType.BankAccount:
                    return new TwoName(DefaultBankAccountCompany, DefaultBankAccountName);
                default:
                    return null;
            }
        }

        public DatabaseConstructor WithDefaultFromType(AccountType acctype)
        {
            switch (acctype)
            {
                case AccountType.Security:
                    return WithDefaultSecurity();
                case AccountType.BankAccount:
                    return WithDefaultBankAccount();
                default:
                    return null;
            }
        }

        public DatabaseConstructor WithDefaultSecurity()
        {
            return WithSecurityFromNameAndData(DefaultSecurityCompany, DefaultSecurityName, dates: DefaultSecurityDates, sharePrice: DefaultSecurityUnitPrices, numberUnits: DefaultSecurityShareValues, investment: DefaultSecurityInvestments);
        }

        public string DefaultBankAccountName = "Current";
        public string DefaultBankAccountCompany = "Santander";
        public DateTime[] DefaultBankAccountDates = new DateTime[] { new DateTime(2010, 1, 1), new DateTime(2011, 1, 1), new DateTime(2012, 5, 1), new DateTime(2015, 4, 3), new DateTime(2018, 5, 6), new DateTime(2020, 1, 1) };
        public double[] DefaultBankAccountValues = new double[] { 100.0, 100.0, 125.2, 90.6, 77.7, 101.1 };

        public DatabaseConstructor WithDefaultBankAccount()
        {
            return WithBankAccountFromNameAndData(DefaultBankAccountCompany, DefaultBankAccountName, date: DefaultBankAccountDates, value: DefaultBankAccountValues);
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
