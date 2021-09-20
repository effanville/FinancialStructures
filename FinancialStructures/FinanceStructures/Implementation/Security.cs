using System;
using System.Collections.Generic;
using FinancialStructures.Database;
using FinancialStructures.DataStructures;
using FinancialStructures.NamingStructures;
using Common.Structure.DataStructures;

namespace FinancialStructures.FinanceStructures.Implementation
{
    /// <summary>
    /// Class to model a stock, or a unit trust.
    /// </summary>
    public partial class Security : ValueList, ISecurity
    {
        /// <inheritdoc/>
        public TimeList Shares { get; set; } = new TimeList();

        /// <inheritdoc/>
        public TimeList UnitPrice { get; set; } = new TimeList();

        /// <inheritdoc/>
        public TimeList Investments { get; set; } = new TimeList();

        /// <inheritdoc/>
        public List<SecurityTrade> SecurityTrades
        {
            get;
            set;
        } = new List<SecurityTrade>();

        /// <summary>
        /// An empty constructor.
        /// </summary>
        internal Security()
        {
            Names = new NameData();
        }

        internal Security(NameData names)
        {
            Names = names;
            SetupEventListening();
        }

        /// <summary>
        /// Constructor creating a new security.
        /// </summary>
        internal Security(string company, string name, string currency = "GBP", string url = null, HashSet<string> sectors = null)
        {
            Names = new NameData(company, name, currency, url, sectors);
            SetupEventListening();
        }

        /// <summary>
        /// Constructor to make a new security from known data.
        /// </summary>
        private Security(NameData names, TimeList shares, TimeList prices, TimeList investments)
        {
            Names = names.Copy();
            Shares = shares;
            UnitPrice = prices;
            Investments = investments;
            SetupEventListening();
        }

        /// <inheritdoc/>
        protected override void OnDataEdit(object edited, EventArgs e)
        {
            base.OnDataEdit(edited, new PortfolioEventArgs(Account.Security));
        }

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
        {
            return new Security(Names, Shares, UnitPrice, Investments);
        }

        /// <inheritdoc/>
        public override bool Any()
        {
            if (UnitPrice.Any() && Shares.Any())
            {
                return true;
            }

            return false;
        }

        /// <inheritdoc/>
        public override int Count()
        {
            return UnitPrice.Count();
        }

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
        public bool Equals(ISecurity otherSecurity)
        {
            return base.Equals(otherSecurity);
        }

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
        public int CompareTo(ISecurity other)
        {
            return base.CompareTo(other);
        }
    }
}
