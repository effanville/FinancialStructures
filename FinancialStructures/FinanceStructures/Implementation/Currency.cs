using System;
using Common.Structure.DataStructures;
using FinancialStructures.Database;
using FinancialStructures.NamingStructures;

namespace FinancialStructures.FinanceStructures.Implementation
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
            : base(names)
        {
        }

        private Currency(NameData name, TimeList values)
            : base(name, values)
        {
        }

        /// <summary>
        /// default constructor.
        /// </summary>
        public Currency()
            : base()
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
