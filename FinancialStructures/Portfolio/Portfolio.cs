using FinancialStructures.FinanceStructures;
using System.Collections.Generic;
using System.IO;

namespace FinancialStructures.Database
{
    public partial class Portfolio
    {
        private string fDatabaseFilePath;

        public void SetFilePath(string path)
        {
            fDatabaseFilePath = path;
        }

        public string FilePath
        {
            get
            {
                return fDatabaseFilePath;
            }
        }

        public string Extension
        {
            get
            {
                return Path.GetExtension(fDatabaseFilePath);
            }
        }

        public string Directory
        {
            get
            {
                return Path.GetDirectoryName(fDatabaseFilePath);
            }
        }

        public string DatabaseName
        {
            get
            {
                return Path.GetFileNameWithoutExtension(fDatabaseFilePath);
            }
        }

        private List<Security> fFunds;

        public List<Security> Funds
        {
            get { return fFunds; }
            private set { fFunds = value; }
        }

        private List<CashAccount> fBankAccounts;

        public List<CashAccount> BankAccounts
        {
            get { return fBankAccounts; }
            private set { fBankAccounts = value; }
        }

        private List<Currency> fCurrencies;

        public List<Currency> Currencies
        {
            get { return fCurrencies; }
            private set { fCurrencies = value; }
        }

        public Portfolio()
        {
            fFunds = new List<Security>();
            fBankAccounts = new List<CashAccount>();
            fCurrencies = new List<Currency>();
        }

        public void CopyData(Portfolio portfolio)
        {
            this.Funds = portfolio.Funds;
            this.BankAccounts = portfolio.BankAccounts;
            this.Currencies = portfolio.Currencies;
        }
    }
}
