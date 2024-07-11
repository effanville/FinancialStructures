using System;
using System.Xml.Serialization;

using Effanville.Common.Structure.DataStructures;
using Effanville.FinancialStructures.Database;
using Effanville.FinancialStructures.NamingStructures;

namespace Effanville.FinancialStructures.FinanceStructures.Implementation
{
    /// <summary>
    /// A named list containing values.
    /// </summary>
    public partial class ValueList : IValueList, IDisposable
    {
        /// <inheritdoc/>
        public Account AccountType { get; private set; }
        
        /// <inheritdoc/>
        public NameData Names { get; set; }

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
        public TimeList Values { get; set; }

        /// <summary>
        /// default constructor.
        /// </summary>
        public ValueList() 
            : this(Account.Unknown)
        { }
        
        /// <summary>
        /// default constructor.
        /// </summary>
        public ValueList(Account account) 
            : this(account, new NameData()) 
        { }
        
        /// <summary>
        /// default constructor.
        /// </summary>
        public ValueList(Account account, NameData names)
            : this(account, names, new TimeList())
        { }
        /// <summary>
        /// default constructor.
        /// </summary>
        public ValueList(NameData names, TimeList values)
        {
            AccountType = Account.Unknown;
            Names = names;
            Values = values;
            SetupEventListening();
        }
        /// <summary>
        /// default constructor.
        /// </summary>
        public ValueList(Account account,NameData names, TimeList values)
        {
            AccountType = account;
            Names = names;
            Values = values;
            SetupEventListening();
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Disposes of objects, and unsubscribes from events.
        /// </summary>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                Values.DataEdit -= OnDataEdit;
            }
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
        public override string ToString() => Names.ToString();

        /// <inheritdoc />
        public virtual IValueList Copy() => new ValueList(AccountType, Names, Values);

        /// <inheritdoc />
        public virtual bool Any() => Values != null && Values.Any();

        /// <inheritdoc/>
        public virtual int Count() => Values.Count();

        /// <inheritdoc/>
        public virtual bool Equals(IValueList other) => Names.IsEqualTo(other?.Names);

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
        public virtual int CompareTo(IValueList other) => Names.CompareTo(other.Names);

        /// <inheritdoc />
        public int ValueComparison(IValueList otherList, DateTime dateTime)
        {
            decimal thisListValue = Value(dateTime)?.Value ?? 0.0m;
            decimal otherListValue = otherList.Value(dateTime)?.Value ?? 0.0m;
            return otherListValue.CompareTo(thisListValue);
        }
    }
}
