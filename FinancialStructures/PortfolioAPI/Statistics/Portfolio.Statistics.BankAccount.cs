using FinancialStructures.Database;
using FinancialStructures.DataStructures;
using System;
using System.Collections.Generic;

namespace FinancialStructures.PortfolioAPI
{
    public static partial class PortfolioStatistics
    {
        public static List<DayValue_Named> GenerateCompanyBankAccountStatistics(this Portfolio portfolio, string company, bool DisplayValueFunds)
        {
            var namesAndCompanies = new List<DayValue_Named>();

            foreach (var account in portfolio.GetBankAccounts())
            {
                if (account.GetCompany() == company)
                {
                    if ((DisplayValueFunds && account.LatestValue().Value != 0) || !DisplayValueFunds)
                    {
                        var currency = PortfolioValues.Currency(portfolio, AccountType.BankAccount, account);
                        namesAndCompanies.Add(new DayValue_Named(account.GetName(), account.GetCompany(), account.LatestValue(currency).Day, account.LatestValue(currency).Value));
                    }
                }
            }

            namesAndCompanies.Sort();
            if (namesAndCompanies.Count > 1)
            {
                namesAndCompanies.Add(new DayValue_Named("Totals", company, DateTime.Today, portfolio.CompanyValue(AccountType.BankAccount, company, DateTime.Today)));
            }
            return namesAndCompanies;
        }

        public static List<DayValue_Named> GenerateBankAccountStatistics(this Portfolio portfolio, bool DisplayValueFunds)
        {
            if (portfolio != null)
            {
                var namesAndCompanies = new List<DayValue_Named>();

                foreach (var acc in portfolio.GetBankAccounts())
                {
                    if ((DisplayValueFunds && acc.LatestValue().Value != 0) || !DisplayValueFunds)
                    {
                        var latest = new DayValue_Named(acc.GetName(), acc.GetCompany(), acc.LatestValue());
                        namesAndCompanies.Add(latest);
                    }
                }

                namesAndCompanies.Sort();
                if (namesAndCompanies.Count > 1)
                {
                    namesAndCompanies.Add(new DayValue_Named("Totals", "", DateTime.Today, portfolio.TotalValue(AccountType.BankAccount)));
                }
                return namesAndCompanies;
            }

            return new List<DayValue_Named>();
        }
    }
}
