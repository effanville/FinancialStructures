using Common.Structure.Reporting;

using FinancialStructures.Database.Extensions;
using FinancialStructures.NamingStructures;

namespace FinancialStructures.Database.Implementation
{
    public partial class Portfolio
    {
        /// <inheritdoc/>
        public bool TryEditName(Account elementType, NameData oldName, NameData newName, IReportLogger reportLogger = null)
        {
            return this.TryPerformEdit(
                elementType,
                oldName,
                valueList => valueList.EditNameData(newName),
                ReportLocation.EditingData,
                reportLogger);
        }
    }
}
