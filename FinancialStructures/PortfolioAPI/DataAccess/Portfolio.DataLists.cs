using FinancialStructures.Database;
using FinancialStructures.FinanceInterfaces;
using System.Collections.Generic;

namespace FinancialStructures.PortfolioAPI
{
    public static partial class PortfolioData
    {
        /// <summary>
        /// Returns a copy of the currently held portfolio. 
        /// Note one cannot use this portfolio to edit as it makes a copy.
        /// </summary>
        public static IPortfolio CopyPortfolio(this IPortfolio portfolio)
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
        /// Returns a copy of all securities with the company as specified.
        /// </summary>
        public static List<ISecurity> CompanySecurities(this IPortfolio portfolio, string company)
        {
            var securities = new List<ISecurity>();
            foreach (ISecurity sec in portfolio.Funds)
            {
                if (sec.Company == company)
                {
                    securities.Add(sec.Copy());
                }
            }
            securities.Sort();
            return securities;
        }



        public static List<ICashAccount> CompanyBankAccounts(this IPortfolio portfolio, string company)
        {
            var accounts = new List<ICashAccount>();
            foreach (ICashAccount acc in portfolio.BankAccounts)
            {
                if (acc.Company == company)
                {
                    accounts.Add(acc);
                }
            }

            accounts.Sort();
            return accounts;
        }
    }
}
