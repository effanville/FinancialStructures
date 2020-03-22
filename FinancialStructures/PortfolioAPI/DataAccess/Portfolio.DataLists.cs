using FinancialStructures.Database;
using FinancialStructures.DatabaseInterfaces;
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
        public static List<Security> CompanySecurities(this IPortfolio portfolio, string company)
        {
            var securities = new List<Security>();
            foreach (var sec in portfolio.Funds)
            {
                if (sec.GetCompany() == company)
                {
                    securities.Add(sec.Copy());
                }
            }
            securities.Sort();
            return securities;
        }



        public static List<CashAccount> CompanyBankAccounts(this IPortfolio portfolio, string company)
        {
            var accounts = new List<CashAccount>();
            foreach (var acc in portfolio.BankAccounts)
            {
                if (acc.GetCompany() == company)
                {
                    accounts.Add(acc);
                }
            }

            accounts.Sort();
            return accounts;
        }
    }
}
