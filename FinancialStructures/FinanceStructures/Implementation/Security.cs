using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using FinancialStructures.Database;
using FinancialStructures.NamingStructures;
using Common.Structure.DataStructures;

namespace FinancialStructures.FinanceStructures.Implementation
{
    /// <summary>
    /// Class to model a stock, or a unit trust.
    /// </summary>
    public partial class Security : ValueList, ISecurity
    {
        private TimeList fInvestments = new TimeList();
        private TimeList fShares = new TimeList();
        private TimeList fUnitPrice = new TimeList();

        /// <summary>
        /// For backwards compatibility with old systems where this was the true store of sectors.
        /// </summary>
        [XmlIgnore]
        public HashSet<string> Sectors
        {
            get
            {
                return Names.Sectors;
            }
        }

        /// <inheritdoc/>
        public TimeList Shares
        {
            get
            {
                return fShares;
            }
            set
            {
                fShares = value;
            }
        }

        /// <inheritdoc/>
        public TimeList UnitPrice
        {
            get
            {
                return fUnitPrice;
            }
            set
            {
                fUnitPrice = value;
            }
        }

        /// <inheritdoc/>
        public TimeList Investments
        {
            get
            {
                return fInvestments;
            }
            set
            {
                fInvestments = value;
            }
        }

        /// <summary>
        /// An empty constructor.
        /// </summary>
        private Security()
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
            fShares = shares;
            fUnitPrice = prices;
            fInvestments = investments;
            SetupEventListening();
        }

        /// <summary>
        /// Event that controls when data is edited.
        /// </summary>
        public override event EventHandler<PortfolioEventArgs> DataEdit;

        /// <summary>
        /// Raises the <see cref="DataEdit"/> event.
        /// </summary>
        internal override void OnDataEdit(object edited, EventArgs e)
        {
            DataEdit?.Invoke(edited, new PortfolioEventArgs(Account.Security));
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

        /// <inheritdoc/>
        public override string ToString()
        {
            return Names.ToString();
        }

        /// <inheritdoc/>
        public new ISecurity Copy()
        {
            return new Security(Names, fShares, fUnitPrice, fInvestments);
        }

        /// <inheritdoc/>
        public override bool Any()
        {
            if (fUnitPrice.Any() && fShares.Any())
            {
                return true;
            }

            return false;
        }

        /// <inheritdoc/>
        public override int Count()
        {
            return fUnitPrice.Count();
        }

        /// <inheritdoc/>
        public override bool IsEqualTo(IValueList otherList)
        {
            if (otherList is ISecurity otherSecurity)
            {
                return IsEqualTo(otherSecurity);
            }

            return false;
        }

        /// <inheritdoc/>
        public bool IsEqualTo(ISecurity otherSecurity)
        {
            return base.IsEqualTo(otherSecurity);
        }
    }
}
