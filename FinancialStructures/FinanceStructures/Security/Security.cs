using FinancialStructures.DataStructures;
using FinancialStructures.FinanceInterfaces;
using FinancialStructures.NamingStructures;
using System;
using System.Collections.Generic;

namespace FinancialStructures.FinanceStructures
{
    /// <summary>
    /// Class to model a stock, or a unit trust.
    /// </summary>
    public partial class Security : ISecurity, IComparable
    {
        /// <summary>
        /// Event that controls when data is edited.
        /// </summary>
        public event EventHandler DataEdit;

        internal void OnDataEdit(object edited, EventArgs e)
        {
            DataEdit?.Invoke(edited, e);
        }

        public void SetupEventListening()
        {
            UnitPrice.DataEdit += OnDataEdit;
            Shares.DataEdit += OnDataEdit;
            Investments.DataEdit += OnDataEdit;
        }

        /// <summary>
        /// Returns a string describing this security.
        /// </summary>
        public override string ToString()
        {
            return Names.ToString();
        }

        private NameData fNames;

        /// <summary>
        /// Any name type data associated to this security.
        /// </summary>
        public NameData Names
        {
            get { return fNames; }
            set { fNames = value; }
        }

        /// <summary>
        /// This should only be used for serialisation.
        /// </summary>
        public string Name
        {
            get { return Names.Name; }
            set { Names.Name = value; }
        }

        /// <summary>
        /// This should only be used for serialisation.
        /// </summary>
        public string Company
        {
            get { return Names.Company; }
            set { Names.Company = value; }
        }

        /// <summary>
        /// The url for this security.
        /// </summary>
        public string Url
        {
            get { return Names.Url; }
            set { Names.Url = value; }
        }

        /// <summary>
        /// The currency this security is valued in.
        /// </summary>
        public string Currency
        {
            get { return Names.Currency; }
            set { Names.Currency = value; }
        }

        /// <summary>
        /// The number of shares held in this security.
        /// </summary>
        private TimeList fShares = new TimeList();

        /// <summary>
        /// This should only be used for serialisation.
        /// </summary>
        public TimeList Shares
        {
            get { return fShares; }
            set { fShares = value; }
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

        /// <summary>
        /// The data of the price per unit/share of this security.
        /// </summary>
        private TimeList fUnitPrice = new TimeList();

        /// <summary>
        /// This should only be used for serialisation.
        /// </summary>
        public TimeList UnitPrice
        {
            get { return fUnitPrice; }
            set { fUnitPrice = value; }
        }

        /// <summary>
        /// A list of investments made in this security.
        /// </summary>
        private TimeList fInvestments = new TimeList();

        /// <summary>
        /// This should only be used for serialisation.
        /// </summary>
        public TimeList Investments
        {
            get { return fInvestments; }
            set { fInvestments = value; }
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
        /// Method of comparison
        /// </summary>
        public int CompareTo(object obj)
        {
            if (obj is ISecurity value)
            {
                return Names.CompareTo(value.Names);
            }

            return 0;
        }
    }
}
