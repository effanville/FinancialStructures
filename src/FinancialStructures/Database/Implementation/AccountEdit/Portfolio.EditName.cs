using Effanville.Common.Structure.Reporting;
using Effanville.FinancialStructures.Database.Extensions;
using Effanville.FinancialStructures.NamingStructures;

namespace Effanville.FinancialStructures.Database.Implementation
{
    public partial class Portfolio
    {
        /// <inheritdoc/>
        public bool TryEditName(Account elementType, NameData oldName, NameData newName, IReportLogger reportLogger = null)
        {
            return PortfolioPerformAction.TryPerformEdit(this, elementType,
                oldName.ToTwoName(),
                valueList => valueList.EditNameData(newName),
                ReportLocation.EditingData,
                reportLogger);
        }
    }
}
