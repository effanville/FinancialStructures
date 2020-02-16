using FinancialStructures.FinanceStructures;
using FinancialStructures.GUIFinanceStructures;
using FinancialStructures.ReportingStructures;
using System;
using System.Collections.Generic;

namespace FinancialStructures.Database
{
    public static partial class PortfolioBankAccounts
    {
        public static List<string> GetBankAccountNames(this Portfolio portfolio)
        {
            var names = new List<string>();
            foreach (var bankAcc in portfolio.BankAccounts)
            {
                names.Add(bankAcc.GetName());
            }

            return names;
        }

        public static List<string> GetBankAccountCompanyNames(this Portfolio portfolio)
        {
            var companies = new List<string>();
            foreach (var bankAcc in portfolio.BankAccounts)
            {
                if (companies.IndexOf(bankAcc.GetCompany()) == -1)
                {
                    companies.Add(bankAcc.GetCompany());
                }
            }
            companies.Sort();

            return companies;
        }

        public static List<NameData> GetBankAccountNamesAndCompanies(this Portfolio portfolio)
        {
            var namesAndCompanies = new List<NameData>();

            foreach (var bankAcc in portfolio.BankAccounts)
            {
                namesAndCompanies.Add(new NameData(bankAcc.GetName(), bankAcc.GetCompany(), bankAcc.GetCurrency(), string.Empty, bankAcc.GetSectors(), false));
            }

            return namesAndCompanies;
        }

        public static bool DoesBankAccountExistFromName(this Portfolio portfolio, string name, string company)
        {
            foreach (CashAccount acc in portfolio.BankAccounts)
            {
                if (acc.GetName() == name && acc.GetCompany() == company)
                {
                    return true;
                }
            }

            return false;
        }

        public static bool TryGetAccountData(this Portfolio portfolio, string name, string company, out List<AccountDayDataView> data)
        {
            data = new List<AccountDayDataView>();
            foreach (CashAccount acc in portfolio.BankAccounts)
            {
                if (acc.GetName() == name && acc.GetCompany() == company)
                {
                    data = acc.GetDataForDisplay();
                    return true;
                }
            }

            return false;
        }


        public static List<AccountDayDataView> BankAccountData(this Portfolio portfolio, NameData name, ErrorReports reports)
        {
            foreach (CashAccount acc in portfolio.BankAccounts)
            {
                if (acc.GetName() == name.Name && acc.GetCompany() == name.Company)
                {
                    return acc.GetDataForDisplay();
                }
            }
            reports.AddReport($"Bank account {name.ToString()} does not exist.", Location.DatabaseAccess);
            return new List<AccountDayDataView>();
        }

        /// <summary>
        /// Outputs a copy of the BankAccount if it exists.
        /// </summary>
        public static bool TryGetBankAccount(this Portfolio portfolio, string name, string company, out CashAccount desired)
        {
            foreach (CashAccount sec in portfolio.BankAccounts)
            {
                if (sec.GetName() == name && sec.GetCompany() == company)
                {
                    desired = sec.Copy();
                    return true;
                }
            }

            desired = null;
            return false;
        }

        public static bool TryAddBankAccount(this Portfolio portfolio, NameData name, ErrorReports reports)
        {
            return portfolio.TryAddBankAccount(name.Name, name.Company, name.Currency, name.Sectors, reports);
        }

        /// <summary>
        /// Tries to add a CashAccount to the underlying global database
        /// </summary>
        public static bool TryAddBankAccount(this Portfolio portfolio, string name, string company, string currency, string sectors, ErrorReports reports)
        {
            List<string> sectorList = new List<string>();
            if (!string.IsNullOrEmpty(sectors))
            {
                var sectorsSplit = sectors.Split(',');

                sectorList.AddRange(sectorsSplit);
                for (int i = 0; i < sectorList.Count; i++)
                {
                    sectorList[i] = sectorList[i].Trim(' ');
                }
            }
            return portfolio.TryAddBankAccountFromName(name, company, currency, sectorList, reports);
        }

        public static bool TryEditBankAccountName(this Portfolio portfolio, NameData oldNames, NameData newName, ErrorReports reports)
        {
            return portfolio.TryEditBankAccountName(oldNames.Name, oldNames.Company, newName.Name, newName.Company, newName.Currency, newName.Sectors, reports);
        }


        /// <summary>
        /// Renames the BankAccount if this exists.
        /// </summary>
        public static bool TryEditBankAccountName(this Portfolio portfolio, string name, string company, string newName, string newCompany, string currency, string newSectors, ErrorReports reports)
        {
            List<string> sectorList = new List<string>();
            if (!string.IsNullOrEmpty(newSectors))
            {
                var sectorsSplit = newSectors.Split(',');

                sectorList.AddRange(sectorsSplit);
                for (int i = 0; i < sectorList.Count; i++)
                {
                    sectorList[i] = sectorList[i].Trim(' ');
                }
            }

            return portfolio.TryEditCashAcountNameCompany(name, company, newName, newCompany, currency, sectorList, reports);
        }

        public static bool TryAddBankAccountFromName(this Portfolio portfolio, string name, string company, string currency, List<string> sectors, ErrorReports reports)
        {
            if (name == null || company == null)
            {
                reports.AddError("Name or Company provided were null.", Location.AddingData);
                return false;
            }
            if (portfolio.DoesBankAccountExistFromName(name, company))
            {
                return false;
            }

            var NewAccount = new CashAccount(name, company, currency);
            foreach (var sector in sectors)
            {
                NewAccount.TryAddSector(sector);
            }

            portfolio.BankAccounts.Add(NewAccount);
            return true;
        }

        public static bool TryRemoveBankAccount(this Portfolio portfolio, NameData name, ErrorReports reports)
        {
            return portfolio.TryRemoveBankAccount(name.Name, name.Company, reports);
        }

        public static bool TryRemoveBankAccount(this Portfolio portfolio, string name, string company, ErrorReports reports)
        {
            foreach (CashAccount acc in portfolio.BankAccounts)
            {
                if (acc.GetCompany() == company && acc.GetName() == name)
                {
                    portfolio.BankAccounts.Remove(acc);
                    reports.AddWarning($"Deleting Bank Account: Deleted `{company}'-`{name}'.", Location.DeletingData);
                    return true;
                }
            }
            reports.AddError($"Deleting Bank Account: Could not find account `{company}'-`{name}'.", Location.DeletingData);
            return false;
        }

        public static bool TryAddDataToBankAccount(this Portfolio portfolio, NameData name, AccountDayDataView data, ErrorReports reports)
        {
            return portfolio.TryAddDataToBankAccount(name.Name, name.Company, data.Date, data.Amount);
        }

        public static bool TryAddDataToBankAccount(this Portfolio portfolio, string name, string company, DateTime date, double value)
        {
            for (int accountIndex = 0; accountIndex < portfolio.BankAccounts.Count; accountIndex++)
            {
                if (portfolio.BankAccounts[accountIndex].GetCompany() == company && portfolio.BankAccounts[accountIndex].GetName() == name)
                {
                    // now edit data
                    return portfolio.BankAccounts[accountIndex].TryAddValue(date, value);
                }
            }

            return false;
        }

        public static bool TryEditBankAccount(this Portfolio portfolio, NameData name, AccountDayDataView oldData, AccountDayDataView newData, ErrorReports reports)
        {
            return portfolio.TryEditBankAccount(name.Name, name.Company, oldData.Date, newData.Date, newData.Amount, reports);
        }

        public static bool TryEditBankAccount(this Portfolio portfolio, string name, string company, DateTime oldDate, DateTime date, double value, ErrorReports reports)
        {
            for (int AccountIndex = 0; AccountIndex < portfolio.BankAccounts.Count; AccountIndex++)
            {
                if (portfolio.BankAccounts[AccountIndex].GetCompany() == company && portfolio.BankAccounts[AccountIndex].GetName() == name)
                {
                    // now edit data
                    return portfolio.BankAccounts[AccountIndex].TryEditValue(oldDate, date, value, reports);
                }
            }

            reports.AddError($"Editing BankAccount Data: Could not find bank account `{company}'-`{name}'.", Location.EditingData);
            return false;
        }

        public static bool TryEditCashAcountNameCompany(this Portfolio portfolio, string name, string company, string newName, string newCompany, string currency, List<string> newSectors, ErrorReports reports)
        {
            for (int AccountIndex = 0; AccountIndex < portfolio.Funds.Count; AccountIndex++)
            {
                if (portfolio.BankAccounts[AccountIndex].GetCompany() == company && portfolio.BankAccounts[AccountIndex].GetName() == name)
                {
                    // now edit data
                    return portfolio.BankAccounts[AccountIndex].EditNameCompany(newName, newCompany, currency, newSectors);
                }
            }

            reports.AddError($"Renaming BankAccount: Could not find bank account `{company}'-`{name}'.", Location.EditingData);
            return false;
        }

        public static bool TryDeleteBankAccountData(this Portfolio portfolio, NameData name, AccountDayDataView data, ErrorReports reports)
        {
            return portfolio.TryDeleteBankAccountData(name.Name, name.Company, data.Date, reports);
        }

        public static bool TryDeleteBankAccountData(this Portfolio portfolio, string name, string company, DateTime date, ErrorReports reports)
        {
            for (int AccountIndex = 0; AccountIndex < portfolio.BankAccounts.Count; AccountIndex++)
            {
                if (portfolio.BankAccounts[AccountIndex].GetCompany() == company && portfolio.BankAccounts[AccountIndex].GetName() == name)
                {
                    // now edit data
                    return portfolio.BankAccounts[AccountIndex].TryDeleteData(date, reports);
                }
            }

            reports.AddError($"Deleting Bank Account Data: Could not find bank account `{company}'-`{name}'.", Location.DeletingData);
            return false;
        }
    }
}
