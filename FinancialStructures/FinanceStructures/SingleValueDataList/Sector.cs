using System;
using FinancialStructures.Database;
using FinancialStructures.FinanceInterfaces;
using FinancialStructures.NamingStructures;
using StructureCommon.DataStructures;

namespace FinancialStructures.FinanceStructures
{
    public class Sector : SingleValueDataList, ISector
    {
        internal override void OnDataEdit(object edited, EventArgs e)
        {
            base.OnDataEdit(edited, new PortfolioEventArgs(Account.Benchmark));
        }

        /// <summary>
        /// default constructor.
        /// </summary>
        public Sector()
            : base()
        {
        }

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