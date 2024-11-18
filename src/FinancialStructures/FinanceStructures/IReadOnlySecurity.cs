using System;
using System.Collections.Generic;
using Effanville.Common.Structure.DataStructures;
using Effanville.Common.Structure.NamingStructures;
using Effanville.FinancialStructures.DataStructures;
using Effanville.FinancialStructures.NamingStructures;

namespace Effanville.FinancialStructures.FinanceStructures
{
    public interface IReadOnlySecurity : IReadOnlyExchangeableValueList
    {
        /// <summary>
        /// The Share data for this Security
        /// </summary>
        TimeList Shares { get; }

        /// <summary>
        /// The unit price data for this fund.
        /// </summary>
        TimeList UnitPrice { get; }

        /// <summary>
        /// The investments in this security.
        /// </summary>
        TimeList Investments { get; }

        /// <summary>
        /// The list of Trades made in this <see cref="ISecurity"/>.
        /// </summary>
        IReadOnlyList<SecurityTrade> Trades { get; }
        
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
    }
}