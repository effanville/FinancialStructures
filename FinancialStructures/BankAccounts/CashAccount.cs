using FinancialStructures.DataStructures;
using System;
using System.Collections.Generic;

namespace FinancialStructures.FinanceStructures
{
    /// <summary>
    /// Contains data to model a cash account.
    /// </summary>
    /// <remarks>
    /// Currently only suitable for a bank account.
    /// Eventually will also work for cash isa etc.
    /// </remarks>
    /// <!--The name and company are used to uniquely specify this.-->
    public partial class CashAccount : IComparable
    {
        /// <summary>
        /// Method of comparison
        /// </summary>
        public int CompareTo(object obj)
        {
            if (obj is CashAccount value)
            {
                if (fCompany == value.Company)
                {
                    return fName.CompareTo(value.Name);
                }

                return fCompany.CompareTo(value.Company);
            }

            return 0;
        }

        /// <summary>
        /// The name associated to the account
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
        /// The company name associated to the account.
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

        /// <summary>
        /// The company name associated to the account.
        /// </summary>
        private string fCurrency;

        /// <summary>
        /// This should only be used for serialisation.
        /// </summary>
        public string Currency
        {
            get { return fCurrency; }
            set { fCurrency = value; }
        }

        /// <summary>
        /// The url associated to the account.
        /// </summary>
        private string fUrl;

        /// <summary>
        /// This should only be used for serialisation.
        /// </summary>
        public string Url
        {
            get { return fUrl; }
            set { fUrl = value; }
        }

        /// <summary>
        /// The time indexed data for the Cash Account.
        /// </summary>
        private TimeList fAmounts = new TimeList();

        /// <summary>
        /// This should only be used for serialisation.
        /// </summary>
        public TimeList Amounts
        {
            get { return fAmounts; }
            set { fAmounts = value; }
        }

        private List<string> fSectors = new List<string>();

        /// <summary>
        /// The Sectors associated with this CashAccount
        /// </summary>
        public List<string> Sectors
        {
            get { return fSectors; }
            set { fSectors = value; }
        }

        /// <summary>
        /// Default constructor where no data is known.
        /// </summary>
        internal CashAccount(string name, string company, string currency)
        {
            fName = name;
            fCompany = company;
            fCurrency = currency;
        }

        /// <summary>
        /// Constructor used when data is known.
        /// </summary>
        private CashAccount(string name, string company, string currency, TimeList amounts)
        {
            fName = name;
            fCompany = company;
            fCurrency = currency;
            fAmounts = amounts;
        }

        /// <summary>
        /// Parameterless constructor for serialisation.
        /// </summary>
        private CashAccount()
        {
        }

        /// <summary>
        /// Checks whether a non null non zero length data list.
        /// </summary>
        public bool Any()
        {
            return fAmounts != null && fAmounts.Any();
        }
    }
}
