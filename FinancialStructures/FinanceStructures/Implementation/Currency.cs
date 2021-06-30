using System;
using FinancialStructures.Database;
using FinancialStructures.NamingStructures;
using Common.Structure.DataStructures;

namespace FinancialStructures.FinanceStructures.Implementation
{
    /// <summary>
    /// A wrapper class of a single list to desribe a currency pair.
    /// </summary>
    public class Currency : ValueList, ICurrency
    {
        internal override void OnDataEdit(object edited, EventArgs e)
        {
            base.OnDataEdit(edited, new PortfolioEventArgs(Account.Currency));
        }

        /// <inheritdoc/>
        public string BaseCurrency
        {
            get
            {
                return Names.Company;
            }
        }

        /// <inheritdoc/>
        public string QuoteCurrency
        {
            get
            {
                return Names.Name;
            }
        }

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
        public new ICurrency Copy()
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
