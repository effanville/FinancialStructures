using System;
using FinancialStructures.FinanceInterfaces;
using FinancialStructures.NamingStructures;
using StructureCommon.DataStructures;

namespace FinancialStructures.FinanceStructures
{
    /// <summary>
    /// Acts as an overall change of an area of funds.
    /// </summary>
    /// <example>
    /// e.g. FTSE100 or MSCI-Asia
    /// </example>
    public partial class SingleValueDataList : IComparable, ISingleValueDataList
    {
        /// <summary>
        /// Event that controls when data is edited.
        /// </summary>
        public event EventHandler<PortfolioEventArgs> DataEdit;

        internal virtual void OnDataEdit(object edited, EventArgs e)
        {
            var args = e is PortfolioEventArgs pe ? pe : new PortfolioEventArgs();
            DataEdit?.Invoke(edited, args);
        }

        /// <inheritdoc />
        public void SetupEventListening()
        {
            Values.DataEdit += OnDataEdit;
        }

        /// <summary>
        /// The string representation of this list.
        /// </summary>
        public override string ToString()
        {
            return Names.ToString();
        }

        /// <summary>
        /// Method of comparison
        /// </summary>
        public int CompareTo(object obj)
        {
            if (obj is ICashAccount value)
            {
                return Names.CompareTo(value.Names);
            }

            return 0;
        }

        /// <summary>
        /// Any name type data associated to this security.
        /// </summary>
        public NameData Names
        {
            get;
            set;
        }

        /// <summary>
        /// This should only be used for serialisation.
        /// </summary>
        public virtual string Name
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
        /// A url to retrieve data for this list.
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
        /// The currency the data in this list is associated with.
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
        /// This should only be used for serialisation.
        /// </summary>
        public TimeList Values
        {
            get;
            set;
        } = new TimeList();

        /// <summary>
        /// default constructor.
        /// </summary>
        public SingleValueDataList()
        {
            Names = new NameData();
            SetupEventListening();
        }

        /// <summary>
        /// default constructor.
        /// </summary>
        public SingleValueDataList(NameData names)
        {
            Names = names;
            SetupEventListening();
        }

        /// <summary>
        /// default constructor.
        /// </summary>
        public SingleValueDataList(NameData names, TimeList values)
        {
            Names = names;
            Values = values;
            SetupEventListening();
        }

        /// <summary>
        /// Performs a copy of the list.
        /// </summary>
        public ISingleValueDataList Copy()
        {
            return new SingleValueDataList(Names, Values);
        }

        /// <summary>
        /// Does the list contain any values?
        /// </summary>
        public bool Any()
        {
            return Values != null && Values.Any();
        }
    }
}
