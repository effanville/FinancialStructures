using System;
using System.Collections.Generic;

using Effanville.Common.Structure.DataStructures;
using Effanville.Common.Structure.Reporting;
using Effanville.FinancialStructures.Database.Extensions;
using Effanville.FinancialStructures.Database.Extensions.Values;
using Effanville.FinancialStructures.DataStructures;
using Effanville.FinancialStructures.FinanceStructures;
using Effanville.FinancialStructures.NamingStructures;

namespace Effanville.FinancialStructures.Database.Implementation
{
    public partial class Portfolio
    {
        /// <inheritdoc/>
        public IReadOnlyList<SecurityDayData> SecurityData(TwoName name, IReportLogger reportLogger = null)
        {
            return PortfolioCalculateStatistic.CalculateStatistic<ISecurity, IReadOnlyList<SecurityDayData>>(this, Account.Security,
                name,
                valueList => valueList.GetDataForDisplay(),
                DateTime.Today,
                nameof(NumberData),
                Cache,
                new List<SecurityDayData>());
        }

        /// <inheritdoc/>
        public IReadOnlyList<DailyValuation> NumberData(Account elementType, TwoName name,  IReportLogger reportLogger = null)
        {
            return PortfolioCalculateStatistic.CalculateStatistic((IPortfolio)this, elementType,
                name,
                valueList => valueList.ListOfValues(),
                DateTime.Today,
                nameof(NumberData),
                Cache,
                new List<DailyValuation>());
        }
    }
}
