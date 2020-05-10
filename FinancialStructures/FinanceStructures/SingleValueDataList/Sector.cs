using FinancialStructures.DataStructures;
using FinancialStructures.FinanceInterfaces;
using FinancialStructures.NamingStructures;

namespace FinancialStructures.FinanceStructures
{
    public class Sector : SingleValueDataList, ISector
    {
        /// <summary>
        /// default constructor.
        /// </summary>
        public Sector()
            : base()
        { }

        public Sector(NameData names)
            : base(names)
        {
        }

        private Sector(NameData names, TimeList values)
            : base(names, values)
        {
        }

        public new ISector Copy()
        {
            return new Sector(Names, Values);
        }
    }
}