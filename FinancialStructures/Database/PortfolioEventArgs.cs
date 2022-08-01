using System;

namespace FinancialStructures.Database
{
    /// <summary>
    /// Contains <see cref="IPortfolio"/> specific event arguments.
    /// </summary>
    public class PortfolioEventArgs : EventArgs
    {
        /// <summary>
        /// Is the change pertaining to Securities.
        /// </summary>
        public bool IsSecurity => ChangedAccount == Account.Security || ChangedAccount == Account.All;

        /// <summary>
        /// Is the change pertaining to Benchmarks.
        /// </summary>
        public bool IsBenchmark => ChangedAccount == Account.Benchmark || ChangedAccount == Account.All;

        /// <summary>
        /// Is the change pertaining to BankAccounts.
        /// </summary>
        public bool IsBankAccount => ChangedAccount == Account.BankAccount || ChangedAccount == Account.All;

        /// <summary>
        /// Is the change pertaining to Currencies.
        /// </summary>
        public bool IsCurrency => ChangedAccount == Account.Currency || ChangedAccount == Account.All;

        /// <summary>
        /// Has this change altered the portfolio.
        /// </summary>
        public bool ChangedPortfolio
        {
            get;
            set;
        }

        /// <summary>
        /// The type of account this refers to.
        /// </summary>
        public Account ChangedAccount
        {
            get;
        }

        /// <summary>
        /// Constructor taking an account type.
        /// </summary>
        public PortfolioEventArgs(bool changedPortfolio)
            : base()
        {
            ChangedPortfolio = changedPortfolio;
        }

        /// <summary>
        /// Constructor taking an account type.
        /// </summary>
        public PortfolioEventArgs(Account type)
            : base()
        {
            ChangedAccount = type;
        }

        /// <summary>
        /// Default Constructor that alerts to all changed.
        /// </summary>
        public PortfolioEventArgs()
            : base()
        {
            ChangedAccount = Account.All;
        }

        /// <summary>
        /// Should the type of account be updated or not.
        /// </summary>
        public bool ShouldUpdate(Account dataType)
        {
            return ChangedAccount == Account.All || dataType.Equals(ChangedAccount) || dataType == Account.All;
        }
    }
}
