using System;
using Effanville.Common.Structure.DataStructures;

namespace Effanville.FinancialStructures.FinanceStructures
{
    public interface IReadOnlyAmortisableAsset : IReadOnlyExchangeableValueList
    {
        /// <summary>
        /// The value of the debt at any given time.
        /// </summary>
        TimeList Debt { get; }

        /// <summary>
        /// The list of payments towards this debt.
        /// </summary>
        TimeList Payments { get; }
        
        /// <summary>
        /// The total cost of the debt. This is the sum of all payments.
        /// </summary>
        decimal TotalCost(ICurrency currency = null);

        /// <summary>
        /// The total cost of the asset up to the time. This is the sum of all payments
        /// made before the time.
        /// </summary>
        decimal TotalCost(DateTime date, ICurrency currency = null);

        /// <summary>
        /// The total cost of the debt over the time period. This is the sum of all payments
        /// made in the time period specified.
        /// </summary>
        decimal TotalCost(DateTime earlierDate, DateTime laterDate, ICurrency currency = null);

        /// <summary>
        /// Returns the Internal rate of return of the <see cref="IAmortisableAsset"/>.
        /// </summary>
        /// <param name="earlierDate">The earlier date to calculate from.</param>
        /// <param name="laterDate">The later date to calculate to.</param>
        /// <param name="currency">An optional currency to exchange with.</param>
        double IRR(DateTime earlierDate, DateTime laterDate, ICurrency currency = null);

        /// <summary>
        /// Returns the Internal rate of return of the <see cref="IAmortisableAsset"/> over the entire
        /// period the <see cref="IAmortisableAsset"/> has values for.
        /// </summary>
        /// <param name="currency">An optional currency to exchange with.</param>
        double IRR(ICurrency currency = null);
    }
}