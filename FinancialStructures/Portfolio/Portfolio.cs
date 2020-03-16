using FinancialStructures.FinanceStructures;
using System;
using System.Collections.Generic;
using System.IO;

namespace FinancialStructures.Database
{
    /// <summary>
    /// Data structure holding information about finances.
    /// </summary>
    public partial class Portfolio
    {
        private string fDatabaseFilePath;

        /// <summary>
        /// Set the path where the database will be stored.
        /// </summary>
        /// <param name="path"></param>
        public void SetFilePath(string path)
        {
            fDatabaseFilePath = path;
        }

        /// <summary>
        /// Access of the databse path.
        /// </summary>
        public string FilePath
        {
            get
            {
                return fDatabaseFilePath;
            }
        }

        /// <summary>
        /// The file extension of the path.
        /// </summary>
        public string Extension
        {
            get
            {
                return string.IsNullOrEmpty(fDatabaseFilePath) ? string.Empty : Path.GetExtension(fDatabaseFilePath);
            }
        }

        /// <summary>
        /// The directory where the database is stored.
        /// </summary>
        public string Directory
        {
            get
            {
                return string.IsNullOrEmpty(fDatabaseFilePath) ? string.Empty : Path.GetDirectoryName(fDatabaseFilePath);
            }
        }

        /// <summary>
        /// The non-extension part of the filename, considered to be the databse name.
        /// </summary>
        public string DatabaseName
        {
            get
            {
                return Path.GetFileNameWithoutExtension(fDatabaseFilePath);
            }
        }

        private List<Security> fFunds = new List<Security>();

        /// <summary>
        /// Securities stored in this database.
        /// </summary>
        public List<Security> Funds
        {
            get { return fFunds; }
            private set { fFunds = value; }
        }

        private List<CashAccount> fBankAccounts = new List<CashAccount>();

        /// <summary>
        /// Bank accounts stored in this database.
        /// </summary>
        public List<CashAccount> BankAccounts
        {
            get { return fBankAccounts; }
            private set { fBankAccounts = value; }
        }

        private List<Currency> fCurrencies = new List<Currency>();

        /// <summary>
        /// The currencies other objects are held in.
        /// </summary>
        public List<Currency> Currencies
        {
            get { return fCurrencies; }
            private set { fCurrencies = value; }
        }

        private List<Sector> fBenchMarks = new List<Sector>();

        /// <summary>
        /// Sector benchmarks for comparison of held data.
        /// </summary>
        internal List<Sector> BenchMarks
        {
            get => fBenchMarks;
            set => fBenchMarks = value;
        }

        public Portfolio()
        {
        }

        /// <summary>
        /// Copies references of other portfolio to this portfolio.
        /// </summary>
        public void CopyData(Portfolio portfolio)
        {
            this.Funds = portfolio.Funds;
            this.BankAccounts = portfolio.BankAccounts;
            this.Currencies = portfolio.Currencies;
            this.BenchMarks = portfolio.BenchMarks;
        }

        /// <summary>
        /// Sets the benchmark parts of this portfolio.
        /// </summary>
        /// <param name="sectors"></param>
        public void SetBenchMarks(List<Sector> sectors)
        {
            fBenchMarks.Clear();
            fBenchMarks.AddRange(sectors);
        }

        /// <summary>
        /// Event to be raised when elements are changed.
        /// </summary>
        public static event EventHandler portfolioChanged;

        /// <summary>
        /// handle the events raised in the above.
        /// </summary>
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
