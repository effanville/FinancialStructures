using FinancialStructures.DataStructures;

namespace FinancialStructures.FinanceStructures
{
    public class Sector : SingleValueDataList
    {

        /// <summary>
        /// default constructor.
        /// </summary>
        public Sector()
            : base()
        { }

        /// <summary>
        /// Creates a new instance of a sector.
        /// </summary>
        public Sector(string name)
            : base(name)
        {
        }

        public Sector(string name, string url)
            : base(name, url)
        {
        }

        private Sector(string name, TimeList values)
            : base(name, values)
        {
        }

        private Sector(string name, string url, TimeList values)
            : base(name, url,values)
        {
        }

        public new Sector Copy()
        {
            return new Sector(Name, Values);
        }
    }
}