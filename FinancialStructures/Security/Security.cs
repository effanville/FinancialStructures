using FinancialStructures.DataStructures;
using System;
using System.Collections.Generic;

namespace FinancialStructures.FinanceStructures
{
    /// <summary>
    /// Class to model a stock, or a unit trust.
    /// </summary>
    public partial class Security : IComparable
    {
        public override string ToString()
        {
            return fCompany + " - " + fName;
        }
        /// <summary>
        /// The name of the security in question.
        /// </summary>
        private string fName;

        /// <summary>
        /// This should only be used for serialisation.
        /// </summary>
        public string Name
        {
            get { return fName; }
            set { fName = value; }
        }

        /// <summary>
        /// The company the security belongs with.
        /// </summary>
        private string fCompany;

        /// <summary>
        /// This should only be used for serialisation.
        /// </summary>
        public string Company
        {
            get { return fCompany; }
            set { fCompany = value; }
        }

        private string fUrl;

        public string Url
        {
            get { return fUrl; }
            set { fUrl = value; }
        }

        private string fCurrency;

        public string Currency
        {
            get { return fCurrency; }
            set { fCurrency = value; }
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
        /// The collection of sectors that this security is part of.
        /// </summary>
        private List<string> fSectors = new List<string>();

        /// <summary>
        /// For serialisation only.
        /// </summary>
        public List<string> Sectors
        {
            get { return fSectors; }
            set { fSectors = value; }
        }

        /// <summary>
        /// An empty constructor.
        /// </summary>
        private Security()
        {
        }


        /// <summary>
        /// Constructor creating a new security.
        /// </summary>
        internal Security(string company, string name, string currency = "GBP", string url = null, List<string> sectors = null)
        {
            fName = name;
            fCompany = company;
            fCurrency = currency;
            fUrl = url;
            fSectors = sectors;
        }

        /// <summary>
        /// Constructor to make a new security from known data.
        /// </summary>
        private Security(string company, string name, string currency, string url, TimeList shares, TimeList prices, TimeList investments)
        {
            fName = name;
            fCompany = company;
            fCurrency = currency;
            fUrl = url;
            fShares = shares;
            fUnitPrice = prices;
            fInvestments = investments;
        }

        /// <summary>
        /// Method of comparison
        /// </summary>
        public int CompareTo(object obj)
        {
            if (obj is Security value)
            {
                if (fCompany == value.fCompany)
                {
                    return fName.CompareTo(value.fName);
                }

                return fCompany.CompareTo(value.fCompany);
            }

            return 0;
        }
    }
}
