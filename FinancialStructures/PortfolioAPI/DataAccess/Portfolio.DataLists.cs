using FinancialStructures.Database;
using FinancialStructures.FinanceStructures;
using System.Collections.Generic;

namespace FinancialStructures.PortfolioAPI
{
    public static partial class PortfolioData
    {
        /// <summary>
        /// Returns a copy of the currently held portfolio. 
        /// Note one cannot use this portfolio to edit as it makes a copy.
        /// </summary>
        public static Portfolio CopyPortfolio(this Portfolio portfolio)
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
        /// Returns a copy of all securities in the portfolio
        /// </summary>
        public static List<Security> GetSecurities(this Portfolio portfolio)
        {
            var listOfFunds = new List<Security>();
            foreach (Security sec in portfolio.Funds)
            {
                listOfFunds.Add(sec.Copy());
            }
            return listOfFunds;
        }

        /// <summary>
        /// Returns a copy of all securities with the company as specified.
        /// </summary>
        public static List<Security> CompanySecurities(this Portfolio portfolio, string company)
        {
            var securities = new List<Security>();
            foreach (var sec in portfolio.GetSecurities())
            {
                if (sec.GetCompany() == company)
                {
                    securities.Add(sec.Copy());
                }
            }
            securities.Sort();
            return securities;
        }

        /// <summary>
        /// A copy of all currencies in the database.
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

        /// <summary>
        /// A copy of all bank accounts in the database.
        /// </summary>
        public static List<CashAccount> GetBankAccounts(this Portfolio portfolio)
        {
            var output = new List<CashAccount>();
            foreach (var acc in portfolio.BankAccounts)
            {
                output.Add(acc.Copy());
            }
            return output;
        }

        /// <summary>
        /// A copy of all bank accounts in the database.
        /// </summary>
        public static List<Sector> GetBenchMarks(this Portfolio portfolio)
        {
            var output = new List<Sector>();
            foreach (var acc in portfolio.BenchMarks)
            {
                output.Add(acc.Copy());
            }
            return output;
        }
    }
}
