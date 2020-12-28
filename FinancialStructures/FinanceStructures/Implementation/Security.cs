using System;
using System.Collections.Generic;
using FinancialStructures.Database;
using FinancialStructures.NamingStructures;
using StructureCommon.DataStructures;

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

        /// <inheritdoc/>
        public NameData Names
        {
            get;
            set;
        }

        /// <summary>
        /// This should only be used for serialisation.
        /// </summary>
        public string Name
        {
            get
            {
                return Names.Name;
            }
            set
            {
                Names.Name = value;
            }
        }

        /// <summary>
        /// This should only be used for serialisation.
        /// </summary>
        public string Company
        {
            get
            {
                return Names.Company;
            }
            set
            {
                Names.Company = value;
            }
        }

        /// <summary>
        /// The url for this security.
        /// </summary>
        public string Url
        {
            get
            {
                return Names.Url;
            }
            set
            {
                Names.Url = value;
            }
        }

        /// <summary>
        /// The currency this security is valued in.
        /// </summary>
        public string Currency
        {
            get
            {
                return Names.Currency;
            }
            set
            {
                Names.Currency = value;
            }
        }

        /// <summary>
        /// For backwards compatibility with old systems where this was the true store of sectors.
        /// </summary>
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

        /// <inheritdoc/>
        public TimeList Values
        {
            get
            {
                throw new Exception();
            }
            set
            {
                throw new Exception();
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
        public event EventHandler<PortfolioEventArgs> DataEdit;

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
        public void SetupEventListening()
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
        public ISecurity Copy()
        {
            return new Security(Names, fShares, fUnitPrice, fInvestments);
        }

        /// <inheritdoc/>
        public bool Any()
        {
            if (fUnitPrice.Any() && fShares.Any())
            {
                return true;
            }

            return false;
        }
    }
}
