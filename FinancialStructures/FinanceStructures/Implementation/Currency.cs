using System;
using FinancialStructures.Database;
using FinancialStructures.NamingStructures;
using StructureCommon.DataStructures;

namespace FinancialStructures.FinanceStructures.Implementation
{
    /// <summary>
    /// A wrapper class of a single list to desribe a currency pair.
    /// </summary>
    public class Currency : SingleValueDataList, ICurrency
    {
        internal override void OnDataEdit(object edited, EventArgs e)
        {
            base.OnDataEdit(edited, new PortfolioEventArgs(Account.Currency));
        }

        /// <summary>
        /// The base currency the currency is derived from.
        /// E.g. in the pair GBP.HKD this is the GBP.
        /// </summary>
        public string BaseCurrency
        {
            get
            {
                return Names.Company;
            }
        }

        /// <summary>
        /// The currency of the valuation.
        /// E.g. in the pair GBP.HKD this is the HKD.
        /// </summary>
        public string QuoteCurrency
        {
            get
            {
                return Names.Name;
            }
        }

        public new ICurrency Copy()
        {
            return new Currency(Names, Values);
        }

        /// <summary>
        /// This provides a currency with values given by the reciprocal of the current
        /// currency values.
        /// </summary>
        public ICurrency Inverted()
        {
            return new Currency(Names, Values.Inverted());
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
    }
}
