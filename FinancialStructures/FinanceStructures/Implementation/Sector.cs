using System;
using FinancialStructures.Database;
using FinancialStructures.NamingStructures;
using Common.Structure.DataStructures;

namespace FinancialStructures.FinanceStructures.Implementation
{
    /// <summary>
    /// A class to detail overall data for a sector.
    /// </summary>
    /// <remarks>
    /// This is rather pointless, but for historic reasons
    /// it is hard to get rid of. All previous Portfolio saves
    /// have sections with Sector listed, so cannot remove without
    /// causing migration pain there.
    /// </remarks>
    public class Sector : ValueList, IValueList
    {
        /// <inheritdoc/>
        protected override void OnDataEdit(object edited, EventArgs e)
        {
            base.OnDataEdit(edited, new PortfolioEventArgs(Account.Benchmark));
        }

        /// <summary>
        /// Empty constructor.
        /// </summary>
        internal Sector()
            : base()
        {
        }

        /// <summary>
        /// Default constructor.
        /// </summary>
        internal Sector(NameData names)
            : base(names)
        {
        }

        private Sector(NameData names, TimeList values)
            : base(names, values)
        {
        }

        /// <inheritdoc/>
        public override IValueList Copy()
        {
            return new Sector(Names, Values);
        }
    }
}