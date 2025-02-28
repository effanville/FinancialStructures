using Effanville.Common.Structure.DataEdit;
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
        UpdateResult<NameData> EditNameData(NameData newNames);
    }
}