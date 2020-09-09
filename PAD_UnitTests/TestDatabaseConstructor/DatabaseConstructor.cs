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
