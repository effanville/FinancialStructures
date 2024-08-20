using Effanville.FinancialStructures.Database;
using Effanville.FinancialStructures.NamingStructures;

namespace Effanville.FinancialStructures.FinanceStructures
{
    public interface IReadOnlyNamedFinancialObject
    {
        /// <summary>
        /// The type of this value list.
        /// </summary>
        Account AccountType { get; }
        
        /// <summary>
        /// The Name data for this list, including company, name and urls.
        /// </summary>
        NameData Names { get; }
        
        /// <summary>
        /// Is the sector listed in this <see cref="IValueList"/>
        /// </summary>
        /// <param name="sectorName">The sector to check.</param>
        bool IsSectorLinked(TwoName sectorName);
    }
}