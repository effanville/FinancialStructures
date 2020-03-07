using FinancialStructures.FinanceStructures;
using FinancialStructures.GUIFinanceStructures;
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

        public static bool TryGetAccountData(this Portfolio portfolio, string name, string company, out List<DayValue_ChangeLogged> data)
        {
            data = new List<DayValue_ChangeLogged>();
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


        public static List<DayValue_ChangeLogged> BankAccountData(this Portfolio portfolio, NameData name, Action<string, string, string> reportLogger)
        {
            foreach (CashAccount acc in portfolio.BankAccounts)
            {
                if (acc.GetName() == name.Name && acc.GetCompany() == name.Company)
                {
                    return acc.GetDataForDisplay();
                }
            }
            reportLogger("Report", "DatabaseAccess", $"Bank account {name.ToString()} does not exist.");
            return new List<DayValue_ChangeLogged>();
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

        public static bool TryAddBankAccount(this Portfolio portfolio, NameData name, Action<string, string, string> reportLogger)
        {
            return portfolio.TryAddBankAccount(name.Name, name.Company, name.Currency, name.Sectors, reportLogger);
        }

        /// <summary>
        /// Tries to add a CashAccount to the underlying global database
        /// </summary>
        public static bool TryAddBankAccount(this Portfolio portfolio, string name, string company, string currency, string sectors, Action<string, string, string> reportLogger)
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
            return portfolio.TryAddBankAccountFromName(name, company, currency, sectorList, reportLogger);
        }

        public static bool TryEditBankAccountName(this Portfolio portfolio, NameData oldNames, NameData newName, Action<string, string, string> reportLogger)
        {
            return portfolio.TryEditBankAccountName(oldNames.Name, oldNames.Company, newName.Name, newName.Company, newName.Currency, newName.Sectors, reportLogger);
        }


        /// <summary>
        /// Renames the BankAccount if this exists.
        /// </summary>
        public static bool TryEditBankAccountName(this Portfolio portfolio, string name, string company, string newName, string newCompany, string currency, string newSectors, Action<string, string, string> reportLogger)
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

            return portfolio.TryEditCashAcountNameCompany(name, company, newName, newCompany, currency, sectorList, reportLogger);
        }

        public static bool TryAddBankAccountFromName(this Portfolio portfolio, string name, string company, string currency, List<string> sectors, Action<string, string, string> reportLogger)
        {
            if (name == null && company == null)
            {
                reportLogger("Error", "AddingData", "Name or Company provided were null.");
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

        public static bool TryRemoveBankAccount(this Portfolio portfolio, NameData name, Action<string, string, string> reportLogger)
        {
            return portfolio.TryRemoveBankAccount(name.Name, name.Company, reportLogger);
        }

        public static bool TryRemoveBankAccount(this Portfolio portfolio, string name, string company, Action<string, string, string> reportLogger)
        {
            foreach (CashAccount acc in portfolio.BankAccounts)
            {
                if (acc.GetCompany() == company && acc.GetName() == name)
                {
                    portfolio.BankAccounts.Remove(acc);
                    reportLogger("Warning", "DeletingData", $"Deleting Bank Account: Deleted `{company}'-`{name}'.");
                    return true;
                }
            }
            reportLogger("Error", "DeletingData", $"Deleting Bank Account: Could not find account `{company}'-`{name}'.");
            return false;
        }

        public static bool TryAddDataToBankAccount(this Portfolio portfolio, NameData name, DayValue_ChangeLogged data, Action<string, string, string> reportLogger)
        {
            return portfolio.TryAddDataToBankAccount(name.Name, name.Company, data.Day, data.Value, reportLogger);
        }

        public static bool TryAddDataToBankAccount(this Portfolio portfolio, string name, string company, DateTime date, double value, Action<string, string, string> reportLogger)
        {
            for (int accountIndex = 0; accountIndex < portfolio.BankAccounts.Count; accountIndex++)
            {
                if (portfolio.BankAccounts[accountIndex].GetCompany() == company && portfolio.BankAccounts[accountIndex].GetName() == name)
                {
                    // now edit data
                    return portfolio.BankAccounts[accountIndex].TryAddData(date, value, reportLogger);
                }
            }

            return false;
        }

        public static bool TryEditBankAccount(this Portfolio portfolio, NameData name, DayValue_ChangeLogged oldData, DayValue_ChangeLogged newData, Action<string, string, string> reportLogger)
        {
            return portfolio.TryEditBankAccount(name.Name, name.Company, oldData.Day, newData.Day, newData.Value, reportLogger);
        }

        public static bool TryEditBankAccount(this Portfolio portfolio, string name, string company, DateTime oldDate, DateTime date, double value, Action<string, string, string> reportLogger)
        {
            for (int AccountIndex = 0; AccountIndex < portfolio.BankAccounts.Count; AccountIndex++)
            {
                if (portfolio.BankAccounts[AccountIndex].GetCompany() == company && portfolio.BankAccounts[AccountIndex].GetName() == name)
                {
                    // now edit data
                    return portfolio.BankAccounts[AccountIndex].TryEditData(oldDate, date, value, reportLogger);
                }
            }

            reportLogger("Error", "EditingData", $"Editing BankAccount Data: Could not find bank account `{company}'-`{name}'.");
            return false;
        }

        public static bool TryEditCashAcountNameCompany(this Portfolio portfolio, string name, string company, string newName, string newCompany, string currency, List<string> newSectors, Action<string, string, string> reportLogger)
        {
            for (int AccountIndex = 0; AccountIndex < portfolio.Funds.Count; AccountIndex++)
            {
                if (portfolio.BankAccounts[AccountIndex].GetCompany() == company && portfolio.BankAccounts[AccountIndex].GetName() == name)
                {
                    // now edit data
                    return portfolio.BankAccounts[AccountIndex].EditNameData(newCompany, newName, string.Empty, currency, newSectors);
                }
            }

            reportLogger("Error", "EditingData", $"Renaming BankAccount: Could not find bank account `{company}'-`{name}'.");
            return false;
        }

        public static bool TryDeleteBankAccountData(this Portfolio portfolio, NameData name, DayValue_ChangeLogged data, Action<string, string, string> reportLogger)
        {
            return portfolio.TryDeleteBankAccountData(name.Name, name.Company, data.Day, reportLogger);
        }

        public static bool TryDeleteBankAccountData(this Portfolio portfolio, string name, string company, DateTime date, Action<string, string, string> reportLogger)
        {
            for (int AccountIndex = 0; AccountIndex < portfolio.BankAccounts.Count; AccountIndex++)
            {
                if (portfolio.BankAccounts[AccountIndex].GetCompany() == company && portfolio.BankAccounts[AccountIndex].GetName() == name)
                {
                    // now edit data
                    return portfolio.BankAccounts[AccountIndex].TryDeleteData(date, reportLogger);
                }
            }

            reportLogger("Error", "DeletingData", $"Deleting Bank Account Data: Could not find bank account `{company}'-`{name}'.");
                
            return false;
        }
    }
}
