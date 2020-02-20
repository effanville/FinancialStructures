using FinancialStructures.DataStructures;

namespace FinancialStructures.FinanceStructures
{
    internal class Currency : SingleValueDataList
    {
        public new Currency Copy()
        {
            return new Currency(Name, Values);
        }
        internal Currency(string name, string url)
            : base(name, url)
        { }

        private Currency(string name, TimeList values)
    : base(name, values)
        { }

        /// <summary>
        /// default constructor.
        /// </summary>
        private Currency()
                : base()
        { }

        /// <summary>
        /// Creates a new instance of a sector.
        /// </summary>
        private Currency(string name)
            : base(name)
        {
        }

        private Currency(string name, string url, TimeList values)
            : base(name, url, values)
        {
        }
    }
}
