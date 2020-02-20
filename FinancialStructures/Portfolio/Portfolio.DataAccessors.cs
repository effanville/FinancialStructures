using FinancialStructures.DataStructures;
using FinancialStructures.FinanceStructures;
using FinancialStructures.GUIFinanceStructures;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FinancialStructures.Database
{
    public static class PortfolioAccessors
    {

        /// <summary>
        /// Returns a copy of the currently held portfolio. 
        /// Note one cannot use this portfolio to edit as it makes a copy.
        /// </summary>
        public static Portfolio GetPortfolio(this Portfolio portfolio)
        {
            var PortfoCopy = new Portfolio();

            foreach (var security in portfolio.Funds)
            {
                PortfoCopy.Funds.Add(security);
            }
            foreach (var bankAcc in portfolio.BankAccounts)
            {
                PortfoCopy.BankAccounts.Add(bankAcc);
            }
            foreach (var currency in portfolio.Currencies)
            {
                PortfoCopy.Currencies.Add(currency);
            }

            return PortfoCopy;
        }

        /// <summary>
        /// returns a copy of the 
        /// </summary>
        public static List<Currency> GetCurrencies(this Portfolio portfolio)
        {
            var output = new List<Currency>();
            foreach (var sector in portfolio.Currencies)
            {
                output.Add(sector);
            }
            return output;
        }

        public static List<NameData> GetCurrencyNames(this Portfolio portfolio)
        {
            var outputs = new List<NameData>();
            if (portfolio.Currencies != null)
            {
                foreach (Currency thing in portfolio.Currencies)
                {
                    outputs.Add(new NameData(thing.GetName(), string.Empty, string.Empty, thing.GetUrl(), false));
                }
            }
            return outputs;
        }

        public static Currency GetCurrencyFromName(this Portfolio portfolio, string name)
        {
            var benchmarks = portfolio.GetCurrencies();
            foreach (var currency in benchmarks)
            {
                if (currency.GetName() == name)
                {
                    return currency.Copy();
                }
            }

            return null;
        }

        public static List<CashAccount> GetBankAccounts(this Portfolio portfolio)
        {
            var output = new List<CashAccount>();
            foreach (var acc in portfolio.BankAccounts)
            {
                output.Add(acc.Copy());
            }
            return output;
        }

        public static CashAccount GetBankAccountFromName(this Portfolio portfolio, string name, string company)
        {
            foreach (var account in portfolio.BankAccounts)
            {
                if (account.GetName() == name && account.GetCompany() == company)
                {
                    return account.Copy();
                }
            }

            return null;
        }

        public static Security GetSecurityFromName(this Portfolio portfolio, string name, string company)
        {
            foreach (var security in portfolio.Funds)
            {
                if (security.GetName() == name && security.GetCompany() == company)
                {
                    return security.Copy();
                }
            }

            return null;
        }

        /// <summary>
        /// returns a sorted list of all funds in portfolio, ordering by company then by fund name.
        /// </summary>
        /// <returns></returns>
        public static List<SecurityStatsHolder> GenerateSecurityStatistics(this Portfolio portfolio, bool DisplayValueFunds)
        {
            if (portfolio != null)
            {
                var funds = portfolio.GetSecurities();
                var namesAndCompanies = new List<SecurityStatsHolder>();

                foreach (var security in funds)
                {
                    var latest = new SecurityStatsHolder(security.GetName(), security.GetCompany());
                    portfolio.AddSecurityStats(latest, DateTime.Today);
                    if ((DisplayValueFunds && latest.LatestVal > 0) || !DisplayValueFunds)
                    {
                        namesAndCompanies.Add(latest);
                    }
                }
                namesAndCompanies.Sort();

                var totals = new SecurityStatsHolder("Totals", "");
                portfolio.AddSecurityStats(totals, DateTime.Today);
                if ((DisplayValueFunds && totals.LatestVal > 0) || !DisplayValueFunds)
                {
                    namesAndCompanies.Add(totals);
                }
                return namesAndCompanies;
            }

            return new List<SecurityStatsHolder>();
        }


        /// <summary>
        /// returns the securities under the company name.
        /// </summary>
        public static List<SecurityStatsHolder> GenerateCompanyFundsStatistics(this Portfolio portfolio, string company)
        {
            if (portfolio != null)
            {
                var funds = portfolio.GetSecurities();
                var namesAndCompanies = new List<SecurityStatsHolder>();

                foreach (var security in funds)
                {
                    if (security.GetCompany() == company)
                    {
                        var latest = new SecurityStatsHolder(security.GetName(), security.GetCompany());
                        portfolio.AddSecurityStats(latest, DateTime.Today);
                        namesAndCompanies.Add(latest);
                    }
                }

                namesAndCompanies.Sort();
                if (namesAndCompanies.Count > 1)
                {
                    var totals = new SecurityStatsHolder("Totals", company);
                    portfolio.AddSecurityStats(totals, DateTime.Today);
                    namesAndCompanies.Add(totals);
                }

                return namesAndCompanies;
            }

            return new List<SecurityStatsHolder>();
        }


        /// <summary>
        /// returns the securities under the company name.
        /// </summary>
        public static SecurityStatsHolder GenerateSectorFundsStatistics(this Portfolio portfolio, List<Sector> sectors, string sectorName)
        {
            if (portfolio != null)
            {
                var totals = new SecurityStatsHolder(sectorName, "Totals");
                portfolio.AddSectorStats(totals, DateTime.Today, sectors);
                return totals;
            }

            return new SecurityStatsHolder();
        }

        /// <summary>
        /// returns the securities under the company name.
        /// </summary>
        public static SecurityStatsHolder GenerateBenchMarkStatistics(this Portfolio portfolio, List<Sector> sectors, string sectorName)
        {
            if (portfolio != null)
            {
                var totals = new SecurityStatsHolder(sectorName, "BenchMark");
                portfolio.AddSectorStats(totals, DateTime.Today, sectors);
                return totals;
            }

            return new SecurityStatsHolder();
        }

        public static SecurityStatsHolder GenerateCompanyStatistics(this Portfolio portfolio, string company)
        {
            if (portfolio != null)
            {
                var totals = new SecurityStatsHolder("Totals", company);
                portfolio.AddSecurityStats(totals, DateTime.Today);
                return totals;
            }
            return new SecurityStatsHolder();
        }

        public static SecurityStatsHolder GeneratePortfolioStatistics(this Portfolio portfolio)
        {
            if (portfolio != null)
            {
                var totals = new SecurityStatsHolder("Totals", string.Empty);
                portfolio.AddSecurityStats(totals, DateTime.Today);
                return totals;
            }

            return new SecurityStatsHolder();
        }

        public static int LongestName(this Portfolio portfolio)
        {
            return portfolio.SecurityNames().Max().Length;
        }

        public static int LongestCompany(this Portfolio portfolio)
        {
            var companies = portfolio.GetSecuritiesCompanyNames();
            return companies.Select(c => c.Length).Max();
        }

        public static List<DayValue_Named> GenerateBankAccountStatistics(this Portfolio portfolio, bool DisplayValueFunds)
        {
            if (portfolio != null)
            {
                var accs = portfolio.GetBankAccounts();
                var namesAndCompanies = new List<DayValue_Named>();

                foreach (var acc in accs)
                {
                    if ((DisplayValueFunds && acc.LatestValue().Value != 0) || !DisplayValueFunds)
                    {
                        var latest = new DayValue_Named(acc.GetName(), acc.GetCompany(), acc.LatestValue());
                        namesAndCompanies.Add(latest);
                    }
                }

                namesAndCompanies.Sort();
                var totals = new DayValue_Named("Totals", "", DateTime.Today, portfolio.BankAccountTotal());
                namesAndCompanies.Add(totals);
                return namesAndCompanies;
            }

            return new List<DayValue_Named>();
        }
    }
}
