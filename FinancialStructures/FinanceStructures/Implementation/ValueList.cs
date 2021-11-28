using System;
using System.Xml.Serialization;
using Common.Structure.DataStructures;
using FinancialStructures.Database;
using FinancialStructures.NamingStructures;

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
            get => Names.Name;

            set => Names.Name = value;
        }

        /// <summary>
        /// This should only be used for serialisation.
        /// </summary>
        [XmlIgnore]
        public string Company
        {
            get => Names.Company;

            set => Names.Company = value;
        }

        /// <summary>
        /// A url to retrieve data for this list.
        /// </summary>
        [XmlIgnore]
        public string Url
        {
            get => Names.Url;

            set => Names.Url = value;
        }

        /// <summary>
        /// The currency the data in this list is associated with.
        /// </summary>
        [XmlIgnore]
        public string Currency
        {
            get => Names.Currency;
            set => Names.Currency = value;
        }

        /// <inheritdoc />
        public TimeList Values
        {
            get;
            set;
        }

        /// <summary>
        /// default constructor.
        /// </summary>
        public ValueList()
        {
            Names = new NameData();
            Values = new TimeList();
            SetupEventListening();
        }

        /// <summary>
        /// default constructor.
        /// </summary>
        public ValueList(NameData names)
        {
            Names = names;
            Values = new TimeList();
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
        public event EventHandler<PortfolioEventArgs> DataEdit;

        /// <summary>
        /// Raises the <see cref="DataEdit"/> event.
        /// </summary>
        protected virtual void OnDataEdit(object edited, EventArgs e)
        {
            PortfolioEventArgs args = e is PortfolioEventArgs pe ? pe : new PortfolioEventArgs();
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
        public virtual bool Equals(IValueList other)
        {
            return Names.IsEqualTo(other.Names);
        }

        /// <inheritdoc />
        public int CompareTo(object obj)
        {
            if (obj is IValueList otherList)
            {
                return CompareTo(otherList);
            }

            return 0;
        }

        /// <inheritdoc />
        public virtual int CompareTo(IValueList other)
        {
            return Names.CompareTo(other.Names);
        }

        /// <inheritdoc />
        public int ValueComparison(IValueList otherList, DateTime dateTime)
        {
            decimal thislistValue = Value(dateTime)?.Value ?? 0.0m;
            decimal otherListValue = otherList.Value(dateTime)?.Value ?? 0.0m;
            return otherListValue.CompareTo(thislistValue);
        }
    }
}
