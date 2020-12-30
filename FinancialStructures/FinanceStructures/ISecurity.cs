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
        /// The first value and date stored in this security.
        /// </summary>
        /// <param name="currency">An optional currency to transfer the value using.</param>
        DailyValuation FirstValue(ICurrency currency);

        /// <summary>
        /// The latest value and date stored in the value list.
        /// </summary>
        /// <param name="currency">An optional currency to transfer the value using.</param>
        DailyValuation LatestValue(ICurrency currency);

        /// <summary>
        /// Gets the value on the specific date specified.
        /// This is a linearly interpolated value from those values provided,
        /// with the initial value if date is less that the first value.
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

        /// <summary>
        /// Returns the latest valuation on or before the date <paramref name="date"/>.
        /// This value can be on the date <paramref name="date"/>.
        /// </summary>
        /// <param name="date">The date to query the value for.</param>
        /// <param name="currency">An optional currency to exchange the value with.</param>
        DailyValuation NearestEarlierValuation(DateTime date, ICurrency currency);

        /// <summary>
        /// Produces a list of data for visual display purposes. Display in the base currency
        /// of the fund ( so this does not modify values due to currency)
        /// </summary>
        new List<SecurityDayData> GetDataForDisplay();

        /// <summary>
        /// Produces a list of all investments (values in <see cref="Investments"/>) in the <see cref="ISecurity"/> between the dates requested, with a currency conversion if required.
        /// </summary>
        /// <param name="earlierDate">The date to get investments after.</param>
        /// <param name="laterDate">The date to get investments before.</param>
        /// <param name="currency">An optional currency to exchange the value with.</param>
        List<DailyValuation> InvestmentsBetween(DateTime earlierDate, DateTime laterDate, ICurrency currency = null);

        /// <summary>
        /// Returns the total investment value in the <see cref="ISecurity"/>. This is the sum of
        /// all values in <see cref="Investments"/>.
        /// </summary>
        /// <param name="currency">An optional currency to exchange the value with.</param>
        double TotalInvestment(ICurrency currency = null);

        /// <summary>
        /// Returns a list of all investments with the name of the security.
        /// </summary>
        /// <param name="currency">An optional currency to exchange the value with.</param>
        List<DayValue_Named> AllInvestmentsNamed(ICurrency currency = null);

        /// <summary>
        /// Calculates the compound annual rate of the Value list.
        /// </summary>
        /// <param name="earlierTime">The start time.</param>
        /// <param name="laterTime">The end time.</param>
        /// <param name="currency">An optional currency to exchange the value with.</param>
        double CAR(DateTime earlierTime, DateTime laterTime, ICurrency currency);

        /// <summary>
        /// Returns the Internal rate of return of the <see cref="ISecurity"/>.
        /// </summary>
        /// <param name="earlierDate">The earlier date to calculate from.</param>
        /// <param name="laterDate">The later date to calculate to.</param>
        /// <param name="currency">An optional currency to exchange with.</param>
        double IRR(DateTime earlierDate, DateTime laterDate, ICurrency currency = null);

        /// <summary>
        /// Returns the Internal rate of return of the <see cref="ISecurity"/> over the entire
        /// period the <see cref="ISecurity"/> has values for.
        /// </summary>
        /// <param name="currency">An optional currency to exchange with.</param>
        double IRR(ICurrency currency = null);

        /// <summary>
        /// Tries to add data for the date specified if it doesnt exist, or edits data if it exists.
        /// If cannot add any value that one wants to, then doesn't add all the values chosen.
        /// </summary>
        /// <param name="oldDate">The existing date held.</param>
        /// <param name="date">The date to add data to.</param>
        /// <param name="unitPrice">The unit price data to add.</param>
        /// <param name="shares">The number of shares data to add.</param>
        /// <param name="investment">The value of the investment.</param>
        /// <param name="reportLogger">An optional logger to log progress.</param>
        /// <returns>Was adding or editing successful.</returns>
        bool TryAddOrEditData(DateTime oldDate, DateTime date, double unitPrice, double shares, double investment, IReportLogger reportLogger = null);
    }
}
