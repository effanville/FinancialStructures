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
    public partial class ValueList : IValueList
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
        }
        /// <summary>
        /// default constructor.
        /// </summary>
        public ValueList(Account account, NameData names, TimeList values)
        {
            AccountType = account;
            Names = names;
            Values = values;
        }

        /// <inheritdoc />
        public override string ToString() => Names.ToString();

        public virtual IValueList Copy() => new ValueList(AccountType, Names, Values);

        /// <inheritdoc />
        public virtual bool Any() => Values != null && Values.Any();

        /// <inheritdoc/>
        public virtual int Count() => Values.Count();

        /// <inheritdoc/>
        public virtual bool Equals(IReadOnlyValueList other) => Names.IsEqualTo(other?.Names);

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
        public virtual int CompareTo(IReadOnlyValueList other) => Names.CompareTo(other.Names);

        public int ValueComparison(IReadOnlyValueList otherList, DateTime dateTime)
        {
            decimal thisListValue = Value(dateTime)?.Value ?? 0.0m;
            decimal otherListValue = otherList.Value(dateTime)?.Value ?? 0.0m;
            return otherListValue.CompareTo(thisListValue);
        }
    }
}
