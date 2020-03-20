using FinancialStructures.DataStructures;
using FinancialStructures.NamingStructures;
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
                return Names.CompareTo(value.Names);
            }

            return 0;
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
        public virtual string Name
        {
            get => Names.Name;
            set => Names.Name = value;
        }

        /// <summary>
        /// This should only be used for serialisation.
        /// </summary>
        public string Company
        {
            get => Names.Company;
            set => Names.Company = value;
        }

        public string Url
        {
            get => Names.Url;
            set => Names.Url = value;
        }

        public string Currency
        {
            get { return Names.Currency; }
            set { Names.Currency = value; }
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
        {
            Names = new NameData();
        }

        /// <summary>
        /// default constructor.
        /// </summary>
        public SingleValueDataList(NameData names)
        {
            Names = names;
        }

        /// <summary>
        /// default constructor.
        /// </summary>
        public SingleValueDataList(NameData names, TimeList values)
        {
            Names = names;
            fValues = values;
        }

        public SingleValueDataList Copy()
        {
            return new SingleValueDataList(Names, fValues);
        }

        public bool Any()
        {
            return fValues != null && fValues.Any();
        }
    }
}
