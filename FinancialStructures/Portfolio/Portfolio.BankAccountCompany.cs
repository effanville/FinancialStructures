using FinancialStructures.DataStructures;
using FinancialStructures.FinanceStructures;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FinancialStructures.Database
{
    public partial class Portfolio
    {
        public List<CashAccount> CompanyBankAccounts(string company)
        {
            var accounts = new List<CashAccount>();
            foreach (var acc in BankAccounts)
            {
                if (acc.GetCompany() == company)
                {
                    accounts.Add(acc);
                }
            }
            accounts.Sort();
            return accounts;
        }

        public List<DayValue_Named> GenerateBankAccountStatistics(string company)
        {
            var namesAndCompanies = new List<DayValue_Named>();

            foreach (var account in BankAccounts)
            {
                if (account.GetCompany() == company)
                {
                    var currencyName = account.GetCurrency();
                    var currency = Currencies.Find(cur => cur.Name == currencyName);
                    namesAndCompanies.Add(new DayValue_Named(account.GetName(), account.GetCompany(), account.LatestValue(currency).Day, account.LatestValue(currency).Value));
                }
            }

            namesAndCompanies.Sort();
            if (namesAndCompanies.Count > 1)
            {
                namesAndCompanies.Add(new DayValue_Named("Totals", company, DateTime.Today, BankAccountCompanyValue(company, DateTime.Today)));
            }
            return namesAndCompanies;
        }

        public double BankAccountCompanyValue(string company, DateTime date)
        {
            var bankAccounts = CompanyBankAccounts(company);
            if (bankAccounts.Count() == 0)
            {
                return double.NaN;
            }
            double value = 0;
            foreach (var account in bankAccounts)
            {
                if (account != null && account.Any())
                {
                    var currencyName = account.GetCurrency();
                    var currency = Currencies.Find(cur => cur.Name == currencyName);
                    value += account.NearestEarlierValuation(date, currency).Value;
                }
            }

            return value;
        }
        public double BankAccountValue(DateTime date)
        {
            if (BankAccounts.Count() == 0)
            {
                return double.NaN;
            }
            double value = 0;
            foreach (var account in BankAccounts)
            {
                if (account != null && account.Any())
                {
                    var currencyName = account.GetCurrency();
                    var currency = Currencies.Find(cur => cur.Name == currencyName);
                    value += account.NearestEarlierValuation(date, currency).Value;
                }
            }

            return value;
        }
    }
}
