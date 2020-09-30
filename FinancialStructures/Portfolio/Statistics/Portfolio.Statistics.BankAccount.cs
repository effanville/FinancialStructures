using System;
using System.Collections.Generic;
using FinancialStructures.DataStructures;
using FinancialStructures.FinanceInterfaces;

namespace FinancialStructures.Database.Statistics
{
    public static partial class PortfolioStatisticGenerators
    {
        public static List<DayValue_Named> GenerateCompanyBankAccountStatistics(this IPortfolio portfolio, string company, bool DisplayValueFunds)
        {
            List<DayValue_Named> namesAndCompanies = new List<DayValue_Named>();

            foreach (ICashAccount account in portfolio.BankAccounts)
            {
                if (account.Company == company)
                {
                    if ((DisplayValueFunds && account.LatestValue().Value != 0) || !DisplayValueFunds)
                    {
                        ICurrency currency = portfolio.Currency(Account.BankAccount, account);
                        namesAndCompanies.Add(new DayValue_Named(account.Company, account.Name, account.LatestValue(currency).Day, account.LatestValue(currency).Value));
                    }
                }
            }

            namesAndCompanies.Sort();
            if (namesAndCompanies.Count > 1)
            {
                namesAndCompanies.Add(new DayValue_Named(company, "Totals", DateTime.Today, portfolio.CompanyValue(Account.BankAccount, company, DateTime.Today)));
            }
            return namesAndCompanies;
        }

        /// <summary>
        /// Produces a list of all companies holding bank accounts in the portfolio.
        /// </summary>
        public static List<DayValue_Named> BankAccountCompanyStatistics(this IPortfolio portfolio)
        {
            List<DayValue_Named> namesAndCompanies = new List<DayValue_Named>();
            if (portfolio != null)
            {
                foreach (string company in portfolio.Companies(Account.BankAccount))
                {
                    namesAndCompanies.Add(new DayValue_Named(company, "Totals", DateTime.Today, portfolio.CompanyValue(Account.BankAccount, company, DateTime.Today)));
                }
            }

            return namesAndCompanies;
        }

        public static List<DayValue_Named> GenerateBankAccountStatistics(this IPortfolio portfolio, bool DisplayValueFunds)
        {
            if (portfolio != null)
            {
                List<DayValue_Named> namesAndCompanies = new List<DayValue_Named>();

                foreach (ICashAccount acc in portfolio.BankAccounts)
                {
                    if ((DisplayValueFunds && acc.LatestValue().Value != 0) || !DisplayValueFunds)
                    {
                        ICurrency currency = portfolio.Currency(Account.BankAccount, acc);
                        DayValue_Named latest = new DayValue_Named(acc.Company, acc.Name, acc.LatestValue(currency));
                        namesAndCompanies.Add(latest);
                    }
                }

                namesAndCompanies.Sort();
                return namesAndCompanies;
            }

            return new List<DayValue_Named>();
        }

        public static DayValue_Named GenerateBankAccountTotalStatistics(this IPortfolio portfolio)
        {
            return new DayValue_Named("", "Totals", DateTime.Today, portfolio.TotalValue(Account.BankAccount));
        }
    }
}
