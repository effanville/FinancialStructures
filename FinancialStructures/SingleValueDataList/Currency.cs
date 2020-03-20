using FinancialStructures.DataStructures;
using FinancialStructures.NamingStructures;

namespace FinancialStructures.FinanceStructures
{
    public class Currency : SingleValueDataList
    {
        public string BaseCurrency
        {
            get
            {
                return Names.Company;
            }
        }

        public string QuoteCurrency
        {
            get
            {
                return Names.Name;
            }
        }

        public new Currency Copy()
        {
            return new Currency(Names, Values);
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
