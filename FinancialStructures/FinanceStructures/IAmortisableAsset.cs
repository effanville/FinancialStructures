using System;

using Common.Structure.DataStructures;
using Common.Structure.Reporting;

namespace FinancialStructures.FinanceStructures
{
    /// <summary>
    /// Contains logic for an object that can have a debt against it.
    /// For example it can be used to model a house (where the value of the house is stored in the value)
    /// and where an amount of debt is held against the house.
    /// </summary>
    public interface IAmortisableAsset : IExchangableValueList
    {
        /// <summary>
        /// The value of the debt at any given time.
        /// </summary>
        TimeList Debt
        {
            get;
        }

        /// <summary>
        /// The list of payments towards this debt.
        /// </summary>
        TimeList Payments
        {
            get;
        }

        /// <summary>
        /// Tries to add a debt value for the date specified if it doesnt exist, or edits data if it exists.
        /// If cannot add any value that one wants to, then doesn't add all the values chosen.
        /// </summary>
        /// <param name="oldDate">The existing date held.</param>
        /// <param name="date">The date to add data to.</param>
        /// <param name="value">The value of debt to add.</param>
        /// <param name="reportLogger">An optional logger to log progress.</param>
        /// <returns>Was adding or editing successful.</returns>
        bool TryEditDebt(DateTime oldDate, DateTime date, decimal value, IReportLogger reportLogger = null);

        /// <summary>
        /// Sets a debt value on the date specified to the value given. This overwrites the existing
        /// value if it exists.
        /// </summary>
        /// <param name="date">The date to add data to.</param>
        /// <param name="value">The value of debt to add.</param>
        /// <param name="reportLogger">An optional logger to log progress.</param>
        void SetDebt(DateTime date, decimal value, IReportLogger reportLogger = null);

        /// <summary>
        /// Attempts to delete a debt value on the date specified.
        /// </summary>
        /// <param name="date">The date to delete data on.</param>
        /// <param name="reportLogger">An optional logger to log progress.</param>
        /// <returns>Whether data was deleted or not.</returns>
        bool TryDeleteDebt(DateTime date, IReportLogger reportLogger = null);

        /// <summary>
        /// Tries to add a payment for the date specified if it doesnt exist, or edits data if it exists.
        /// If cannot add any value that one wants to, then doesn't add all the values chosen.
        /// </summary>
        /// <param name="oldDate">The existing date held.</param>
        /// <param name="date">The date to add data to.</param>
        /// <param name="value">The value of the payment to add.</param>
        /// <param name="reportLogger">An optional logger to log progress.</param>
        /// <returns>Was adding or editing successful.</returns>
        bool TryEditPayment(DateTime oldDate, DateTime date, decimal value, IReportLogger reportLogger = null);

        /// <summary>
        /// Sets a payment on the date specified to the value given. This overwrites the existing
        /// value if it exists.
        /// </summary>
        /// <param name="date">The date to add data to.</param>
        /// <param name="value">The value of the payment to add.</param>
        /// <param name="reportLogger">An optional logger to log progress.</param>
        void SetPayment(DateTime date, decimal value, IReportLogger reportLogger = null);

        /// <summary>
        /// Attempts to delete a payment on the date specified.
        /// </summary>
        /// <param name="date">The date to delete data on.</param>
        /// <param name="reportLogger">An optional logger to log progress.</param>
        /// <returns>Whether data was deleted or not.</returns>
        bool TryDeletePayment(DateTime date, IReportLogger reportLogger = null);

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
