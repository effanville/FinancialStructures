using System;
using System.Xml.Serialization;
using FinancialStructures.Database;
using FinancialStructures.NamingStructures;
using Common.Structure.DataStructures;

namespace FinancialStructures.FinanceStructures.Implementation
{
    /// <summary>
    /// A named list containing values.
    /// </summary>
    public partial class ValueList : IValueList
    {
        /// <inheritdoc/>
        public NameData Names
        {
            get;
            set;
        }

        /// <summary>
        /// This should only be used for serialisation.
        /// </summary>
        [XmlIgnore]
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
        [XmlIgnore]
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
        [XmlIgnore]
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
        [XmlIgnore]
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

        /// <inheritdoc />
        public TimeList Values
        {
            get;
            set;
        } = new TimeList();

        /// <summary>
        /// default constructor.
        /// </summary>
        public ValueList()
        {
            Names = new NameData();
            SetupEventListening();
        }

        /// <summary>
        /// default constructor.
        /// </summary>
        public ValueList(NameData names)
        {
            Names = names;
            SetupEventListening();
        }

        /// <summary>
        /// default constructor.
        /// </summary>
        public ValueList(NameData names, TimeList values)
        {
            Names = names;
            Values = values;
            SetupEventListening();
        }

        /// <summary>
        /// Event that controls when data is edited.
        /// </summary>
        public virtual event EventHandler<PortfolioEventArgs> DataEdit;

        /// <summary>
        /// Raises the <see cref="DataEdit"/> event.
        /// </summary>
        internal virtual void OnDataEdit(object edited, EventArgs e)
        {
            var args = e is PortfolioEventArgs pe ? pe : new PortfolioEventArgs();
            DataEdit?.Invoke(edited, args);
        }

        /// <summary>
        /// Ensures that events for data edit are subscribed to.
        /// </summary>
        public virtual void SetupEventListening()
        {
            Values.DataEdit += OnDataEdit;
        }

        /// <inheritdoc />
        public override string ToString()
        {
            return Names.ToString();
        }

        /// <inheritdoc />
        public virtual IValueList Copy()
        {
            return new ValueList(Names, Values);
        }

        /// <inheritdoc />
        public virtual bool Any()
        {
            return Values != null && Values.Any();
        }

        /// <inheritdoc/>
        public virtual int Count()
        {
            return Values.Count();
        }

        /// <inheritdoc/>
        public virtual bool IsEqualTo(IValueList otherAccount)
        {
            return Names.IsEqualTo(otherAccount.Names);
        }

        /// <inheritdoc />
        public int CompareTo(object obj)
        {
            if (obj is IValueList otherList)
            {
                return Names.CompareTo(otherList.Names);
            }

            return 0;
        }
    }
}
