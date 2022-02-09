using System.Collections.Generic;

using Common.Structure.DataStructures;
using Common.Structure.Reporting;
using FinancialStructures.Database.Extensions;
using FinancialStructures.DataStructures;
using FinancialStructures.FinanceStructures;
using FinancialStructures.NamingStructures;

namespace FinancialStructures.Database.Implementation
{
    public partial class Portfolio
    {
        /// <inheritdoc/>
        public IReadOnlyList<SecurityDayData> SecurityData(TwoName name, IReportLogger reportLogger = null)
        {
            return this.CalculateStatistic<ISecurity, IReadOnlyList<SecurityDayData>>(
                Account.Security,
                name,
                valueList => valueList.GetDataForDisplay(),
                new List<SecurityDayData>());
        }

        /// <inheritdoc/>
        public IReadOnlyList<DailyValuation> NumberData(Account elementType, TwoName name, IReportLogger reportLogger = null)
        {
            return this.CalculateStatistic(
                elementType,
                name,
                valueList => valueList.ListOfValues(),
                new List<DailyValuation>());
        }
    }
}
