using System;
using System.Collections.Generic;
using Common.Structure.DataStructures;
using Common.Structure.FileAccess;
using Common.Structure.NamingStructures;
using Common.Structure.Reporting;
using FinancialStructures.DataStructures;
using FinancialStructures.NamingStructures;

namespace FinancialStructures.FinanceStructures
{
    /// <summary>
    /// A named entity with Share, unit price and investment lists to detail price history.
    /// </summary>
    public interface ISecurity : ICSVAccess, IExchangableValueList, IValueList, IEquatable<ISecurity>, IComparable<ISecurity>
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
        /// The list of Trades made in this <see cref="ISecurity"/>.
        /// </summary>
        IReadOnlyList<SecurityTrade> Trades
        {
            get;
        }

        /// <summary>
        /// Produces the data for the security on the day specified.
        /// </summary>
        SecurityDayData DayData(DateTime day);

        /// <summary>
        /// Produces a list of data for visual display purposes. Display in the base currency
        /// of the fund ( so this does not modify values due to currency)
        /// </summary>
        IReadOnlyList<SecurityDayData> GetDataForDisplay();

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
        decimal TotalInvestment(ICurrency currency = null);

        /// <summary>
        /// Returns the last investment in the <see cref="ISecurity"/>.
        /// </summary>
        /// <returns></returns>
        DailyValuation LastInvestment(ICurrency currency = null);

        /// <summary>
        /// Returns a list of all investments with the name of the security.
        /// </summary>
        /// <param name="currency">An optional currency to exchange the value with.</param>
        List<Labelled<TwoName, DailyValuation>> AllInvestmentsNamed(ICurrency currency = null);

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
        /// <param name="trade">The details of any trade on this date.</param>
        /// <param name="reportLogger">An optional logger to log progress.</param>
        /// <returns>Was adding or editing successful.</returns>
        bool AddOrEditData(DateTime oldDate, DateTime date, decimal unitPrice, decimal shares, decimal investment = 0.0m, SecurityTrade trade = null, IReportLogger reportLogger = null);

        /// <summary>
        /// Tries to add data for the date specified if it doesnt exist, or edits data if it exists.
        /// If cannot add any value that one wants to, then doesn't add all the values chosen.
        /// </summary>
        /// <param name="oldTrade">The existing trade held.</param>
        /// <param name="newTrade">The new trade to overwrite the old with.</param>
        /// <param name="reportLogger">An optional logger to log progress.</param>
        /// <returns>Was adding or editing successful.</returns>
        bool TryAddOrEditTradeData(SecurityTrade oldTrade, SecurityTrade newTrade, IReportLogger reportLogger = null);

        /// <summary>
        /// Attempts to delete trade data on the date given.
        /// </summary>
        /// <param name="date">The date to delete data on</param>
        /// <param name="reportLogger">An optional logger to log progress.</param>
        /// <returns>True if has deleted, false if failed to delete.</returns>
        bool TryDeleteTradeData(DateTime date, IReportLogger reportLogger = null);

        /// <summary>
        /// Removes unnecessary investment and Share number values to reduce size.
        /// </summary>
        void CleanData();
    }
}
