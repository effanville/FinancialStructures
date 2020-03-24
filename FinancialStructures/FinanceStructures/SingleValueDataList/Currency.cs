using FinancialStructures.DataStructures;
using FinancialStructures.FinanceInterfaces;
using FinancialStructures.NamingStructures;

namespace FinancialStructures.FinanceStructures
{
    /// <summary>
    /// A wrapper class of a single list to desribe a currency pair.
    /// </summary>
    public class Currency : SingleValueDataList, ICurrency
    {
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
        { }

        /// <summary>
        /// default constructor.
        /// </summary>
        internal Currency()
            : base()
        { }
    }
}
