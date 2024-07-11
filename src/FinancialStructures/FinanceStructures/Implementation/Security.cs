﻿using System;
using System.Collections.Generic;
using System.Linq;

using Effanville.Common.Structure.DataStructures;
using Effanville.FinancialStructures.Database;
using Effanville.FinancialStructures.DataStructures;
using Effanville.FinancialStructures.NamingStructures;

namespace Effanville.FinancialStructures.FinanceStructures.Implementation
{
    /// <summary>
    /// Class to model a stock, or a unit trust.
    /// </summary>
    public partial class Security : ValueList, ISecurity
    {
        private readonly object TradesLock = new object();

        /// <inheritdoc/>
        public TimeList Shares { get; set; } = new TimeList();

        /// <inheritdoc/>
        public TimeList UnitPrice { get; set; } = new TimeList();

        /// <inheritdoc/>
        public TimeList Investments { get; set; } = new TimeList();

        /// <summary>
        /// The list of Trades made in this <see cref="ISecurity"/>.
        /// </summary>
        public List<SecurityTrade> SecurityTrades { get; set; } = new List<SecurityTrade>();

        /// <inheritdoc/>
        public IReadOnlyList<SecurityTrade> Trades
        {
            get
            {
                lock (TradesLock)
                {
                    return SecurityTrades.Select(trade => trade.Copy()).ToList();
                }
            }
        }

        /// <summary>
        /// An empty constructor.
        /// </summary>
        internal Security()
            : base(Account.Security)
        {
        }
        
        internal Security(Account account)
            : this(account, new NameData())
        {
        }

        internal Security(Account account, NameData names)
            : base(account, names)
        {
        }
        
        /// <summary>
        /// Constructor to make a new security from known data, where the data is assumed to be consistent.
        /// </summary>
        internal Security(Account account, NameData names, TimeList unitPrices, TimeList shares, TimeList investments, List<SecurityTrade> trades)
            : base(account, names.Copy())
        {
            UnitPrice = unitPrices;
            Shares = shares;
            Investments = investments;
            SecurityTrades = trades;
        }

        /// <summary>
        /// Disposes of objects, and unsubscribes from events.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            if (disposing)
            {
                UnitPrice.DataEdit -= OnDataEdit;
                Shares.DataEdit -= OnDataEdit;
                Investments.DataEdit -= OnDataEdit;
            }
        }

        /// <inheritdoc/>
        protected override void OnDataEdit(object edited, EventArgs e) 
            => base.OnDataEdit(edited, new PortfolioEventArgs(Account.Security));

        /// <summary>
        /// Ensures that events for data edit are subscribed to.
        /// </summary>
        public override void SetupEventListening()
        {
            UnitPrice.DataEdit += OnDataEdit;
            Shares.DataEdit += OnDataEdit;
            Investments.DataEdit += OnDataEdit;
        }

        private void RemoveEventListening()
        {
            UnitPrice.DataEdit -= OnDataEdit;
            Shares.DataEdit -= OnDataEdit;
            Investments.DataEdit -= OnDataEdit;
        }

        /// <inheritdoc/>
        public override IValueList Copy() 
            => new Security(AccountType, Names, UnitPrice, Shares, Investments, Trades.ToList());

        /// <inheritdoc/>
        public override bool Any() => UnitPrice.Any() && Shares.Any();

        /// <inheritdoc/>
        public override int Count() => UnitPrice.Count();

        /// <inheritdoc/>
        public override bool Equals(IValueList otherList)
        {
            if (otherList is ISecurity otherSecurity)
            {
                return Equals(otherSecurity);
            }

            return false;
        }

        /// <inheritdoc/>
        public bool Equals(ISecurity otherSecurity) => base.Equals(otherSecurity);

        /// <inheritdoc />
        public override int CompareTo(IValueList other)
        {
            if (other is ISecurity otherSecurity)
            {
                return base.CompareTo(otherSecurity);
            }

            return 0;
        }

        /// <inheritdoc/>
        public int CompareTo(ISecurity other) => base.CompareTo(other);
    }
}
