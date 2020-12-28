using System;
using System.Collections.Generic;
using FinancialStructures.DataStructures;
using StructureCommon.DataStructures;
using StructureCommon.FileAccess;
using StructureCommon.Reporting;

namespace FinancialStructures.FinanceStructures
{
    /// <summary>
    /// A named entity with Share, unit price and investment lists to detail price history.
    /// </summary>
    public interface ISecurity : ICSVAccess, IValueList
    {
        /// <summary>
        /// The Share data for this Security
        /// </summary>
        TimeList Shares
        {
            get;
        }

        /// <summary>
        /// The unit price data for this fund.
        /// </summary>
        TimeList UnitPrice
        {
            get;
        }

        /// <summary>
        /// The investments in this security.
        /// </summary>
        TimeList Investments
        {
            get;
        }

        /// <summary>
        /// Compares another <see cref="ISecurity"/> and determines if they both have the same name and company.
        /// </summary>
        bool IsEqualTo(ISecurity otherSecurity);

        /// <summary>
        /// Returns a copy of this <see cref="ISecurity"/>.
        /// </summary>
        new ISecurity Copy();

        /// <summary>
        /// The latest value and date stored in the value list.
        /// </summary>
        /// <param name="currency">An optional currency to transfer the value using.</param>
        DailyValuation LatestValue(ICurrency currency);

        /// <summary>
        /// The first value and date stored in this security.
        /// </summary>
        /// <param name="currency">An optional currency to transfer the value using.</param>
        DailyValuation FirstValue(ICurrency currency);

        /// <summary>
        /// Gets the value on the specific date specified. This
        /// </summary>
        /// <param name="date">The date to query the value on.</param>
        /// <param name="currency">An optional currency to transfer the value using.</param>
        DailyValuation Value(DateTime date, ICurrency currency);

        /// <summary>
        /// Returns the most recent value to <paramref name="date"/> that is prior to that date.
        /// This value is strictly prior to <paramref name="date"/>.
        /// </summary>
        /// <param name="date">The date to query the value on.</param>
        /// <param name="currency">An optional currency to transfer the value using.</param>
        DailyValuation RecentPreviousValue(DateTime date, ICurrency currency);

        DailyValuation NearestEarlierValuation(DateTime date, ICurrency currency = null);

        /// <summary>
        /// Retrieves data in a list ordered by date.
        /// </summary>
        List<SecurityDayData> GetDataForDisplay();

        double TotalInvestment(ICurrency currency = null);
        List<DailyValuation> InvestmentsBetween(DateTime earlierDate, DateTime laterDate, ICurrency currency = null);
        List<DayValue_Named> AllInvestmentsNamed(ICurrency currency = null);
        double IRRTime(DateTime earlierDate, DateTime laterDate, ICurrency currency = null);
        double IRR(ICurrency currency = null);

        /// <summary>
        /// Attempts to add data for the date specified.
        /// If cannot add any value that one wants to, then doesn't add all the values chosen.
        /// </summary>
        bool TryAddData(DateTime date, double unitPrice, double shares, double investment, IReportLogger reportLogger);
        bool TryEditData(DateTime oldDate, DateTime newDate, double shares, double unitPrice, double Investment, IReportLogger reportLogger = null);
    }
}
