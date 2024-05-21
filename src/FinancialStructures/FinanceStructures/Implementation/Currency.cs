using System;

using Effanville.Common.Structure.DataStructures;
using Effanville.FinancialStructures.Database;
using Effanville.FinancialStructures.NamingStructures;

namespace Effanville.FinancialStructures.FinanceStructures.Implementation
{
    /// <summary>
    /// A wrapper class of a single list to desribe a currency pair.
    /// </summary>
    public class Currency : ValueList, ICurrency
    {
        /// <inheritdoc/>
        protected override void OnDataEdit(object edited, EventArgs e)
        {
            base.OnDataEdit(edited, new PortfolioEventArgs(Account.Currency));
        }

        /// <inheritdoc/>
        public string BaseCurrency => Names.Company;

        /// <inheritdoc/>
        public string QuoteCurrency => Names.Name;

        internal Currency(NameData names)
            : base(Account.Currency, names)
        {
        }

        internal Currency(NameData name, TimeList values)
            : base(Account.Currency, name, values)
        {
        }

        /// <summary>
        /// default constructor.
        /// </summary>
        public Currency()
            : base(Account.Currency)
        {
        }

        /// <inheritdoc/>
        public override IValueList Copy()
        {
            return new Currency(Names, Values);
        }

        /// <inheritdoc/>
        public ICurrency Inverted()
        {
            return new Currency(new NameData(Names.Name, Names.Company, Names.Currency, Names.Url, Names.Sectors), Values.Inverted());
        }
    }
}
