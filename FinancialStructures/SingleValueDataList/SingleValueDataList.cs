using FinancialStructures.DataStructures;
using System;

namespace FinancialStructures.FinanceStructures
{
    /// <summary>
    /// Acts as an overall change of an area of funds.
    /// </summary>
    /// <example>
    /// e.g. FTSE100 or MSCI-Asia
    /// </example>
    public partial class SingleValueDataList : IComparable
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
        /// The name of the sector. 
        /// </summary>
        /// <remarks>
        /// These names must be unique
        /// </remarks>
        private string fName;

        /// <summary>
        /// This should only be used for serialisation.
        /// </summary>
        public virtual string Name
        {
            get => fName;
            set => fName = value;
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
            get => fCompany; 
            set => fCompany = value; 
        }

        private string fUrl;

        public string Url
        {
            get => fUrl;
            set => fUrl = value;
        }

        /// <summary>
        /// The values of the sector.
        /// </summary>
        private TimeList fValues = new TimeList();

        /// <summary>
        /// This should only be used for serialisation.
        /// </summary>
        public TimeList Values
        {
            get => fValues;
            set => fValues = value;
        }

        /// <summary>
        /// default constructor.
        /// </summary>
        public SingleValueDataList()
        { }

        /// <summary>
        /// Creates a new instance of a sector.
        /// </summary>
        public SingleValueDataList(string name)
        {
            fName = name;
            fValues = new TimeList();
        }

        public SingleValueDataList(string name, string url)
        {
            fName = name;
            fUrl = url;
            fValues = new TimeList();
        }

        protected SingleValueDataList(string name, TimeList values)
        {
            fName = name;
            fValues = values;
        }

        protected SingleValueDataList(string name, string url, TimeList values)
        {
            fName = name;
            fUrl = url;
            fValues = values;
        }

        public SingleValueDataList Copy()
        {
            return new SingleValueDataList(fName, fValues);
        }

        public bool Any()
        {
            return fValues != null && fValues.Any();
        }
    }
}
