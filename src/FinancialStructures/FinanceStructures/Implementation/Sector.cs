using System;

using Effanville.Common.Structure.DataStructures;
using Effanville.FinancialStructures.Database;
using Effanville.FinancialStructures.NamingStructures;

namespace Effanville.FinancialStructures.FinanceStructures.Implementation
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
            => base.OnDataEdit(edited, new PortfolioEventArgs(Account.Benchmark));

        /// <summary>
        /// Empty constructor.
        /// </summary>
        internal Sector()
            : base(Account.Benchmark)
        {
        }

        /// <summary>
        /// Default constructor.
        /// </summary>
        internal Sector(NameData names)
            : base(Account.Benchmark, names)
        {
        }

        internal Sector(NameData names, TimeList values)
            : base(Account.Benchmark, names, values)
        {
        }

        /// <inheritdoc/>
        public override IValueList Copy() => new Sector(Names, Values);
    }
}