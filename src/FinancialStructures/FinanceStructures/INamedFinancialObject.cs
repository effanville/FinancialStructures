using Effanville.FinancialStructures.NamingStructures;

namespace Effanville.FinancialStructures.FinanceStructures
{
    public interface INamedFinancialObject : IReadOnlyNamedFinancialObject
    {
        /// <summary>
        /// Edits the names of the Value list.
        /// </summary>
        /// <param name="newNames">The updated name to set.</param>
        /// <returns>Was updating name successful.</returns>
        bool EditNameData(NameData newNames);
    }
}