using FinancialStructures.DataStructures;
using FinancialStructures.NamingStructures;

namespace FinancialStructures.FinanceStructures
{
    public class Currency : SingleValueDataList
    {
        public new Currency Copy()
        {
            return new Currency(Names, Values);
        }

        private Currency(string name)
            : this(new NameData("", name))
        {
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

        private Currency(string name, string url, TimeList values)
            : base(name, url, values)
        {
        }
    }
}
