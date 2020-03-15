using FinancialStructures.FinanceStructures;
using System;
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
                return string.IsNullOrEmpty(fDatabaseFilePath) ? string.Empty : Path.GetExtension(fDatabaseFilePath);
            }
        }

        public string Directory
        {
            get
            {
                return string.IsNullOrEmpty(fDatabaseFilePath) ? string.Empty : Path.GetDirectoryName(fDatabaseFilePath);
            }
        }

        public string DatabaseName
        {
            get
            {
                return Path.GetFileNameWithoutExtension(fDatabaseFilePath);
            }
        }

        private List<Security> fFunds = new List<Security>();

        public List<Security> Funds
        {
            get { return fFunds; }
            private set { fFunds = value; }
        }

        private List<CashAccount> fBankAccounts = new List<CashAccount>();

        public List<CashAccount> BankAccounts
        {
            get { return fBankAccounts; }
            private set { fBankAccounts = value; }
        }

        private List<Currency> fCurrencies = new List<Currency>();

        public List<Currency> Currencies
        {
            get { return fCurrencies; }
            private set { fCurrencies = value; }
        }

        private List<Sector> fBenchMarks = new List<Sector>();

        internal List<Sector> BenchMarks
        {
            get => fBenchMarks;
            set => fBenchMarks = value;
        }

        public Portfolio()
        {
        }

        public void CopyData(Portfolio portfolio)
        {
            this.Funds = portfolio.Funds;
            this.BankAccounts = portfolio.BankAccounts;
            this.Currencies = portfolio.Currencies;
            this.BenchMarks = portfolio.BenchMarks;
        }

        public void SetBenchMarks(List<Sector> sectors)
        {
            fBenchMarks.Clear();
            fBenchMarks.AddRange(sectors);
        }

        public static event EventHandler portfolioChanged;

        protected void OnPortfolioChanged(EventArgs e)
        {
            EventHandler handler = portfolioChanged;
            if (handler != null)
            {
                handler?.Invoke(this, e);
            }
        }
    }
}
