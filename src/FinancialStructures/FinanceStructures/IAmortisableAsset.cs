using System;
using Effanville.Common.Structure.DataEdit;
using Effanville.Common.Structure.DataStructures;

namespace Effanville.FinancialStructures.FinanceStructures
{
    /// <summary>
    /// Contains logic for an object that can have a debt against it.
    /// For example it can be used to model a house (where the value of the house is stored in the value)
    /// and where an amount of debt is held against the house.
    /// </summary>
    public interface IAmortisableAsset : IReadOnlyAmortisableAsset, IExchangeableValueList
    {
        /// <summary>
        /// Tries to add a debt value for the date specified if it doesnt exist, or edits data if it exists.
        /// If cannot add any value that one wants to, then doesn't add all the values chosen.
        /// </summary>
        /// <param name="oldDate">The existing date held.</param>
        /// <param name="date">The date to add data to.</param>
        /// <param name="value">The value of debt to add.</param>
        /// <returns>Was adding or editing successful.</returns>
        UpdateResult<DailyValuation> TryEditDebt(DateTime oldDate, DateTime date, decimal value);

        /// <summary>
        /// Sets a debt value on the date specified to the value given. This overwrites the existing
        /// value if it exists.
        /// </summary>
        /// <param name="date">The date to add data to.</param>
        /// <param name="value">The value of debt to add.</param>
        UpdateResult<DailyValuation> SetDebt(DateTime date, decimal value);

        /// <summary>
        /// Attempts to delete a debt value on the date specified.
        /// </summary>
        /// <param name="date">The date to delete data on.</param>
        /// <returns>Whether data was deleted or not.</returns>
        UpdateResult<DailyValuation> TryDeleteDebt(DateTime date);

        /// <summary>
        /// Tries to add a payment for the date specified if it doesnt exist, or edits data if it exists.
        /// If cannot add any value that one wants to, then doesn't add all the values chosen.
        /// </summary>
        /// <param name="oldDate">The existing date held.</param>
        /// <param name="date">The date to add data to.</param>
        /// <param name="value">The value of the payment to add.</param>
        /// <returns>Was adding or editing successful.</returns>
        UpdateResult<DailyValuation> TryEditPayment(DateTime oldDate, DateTime date, decimal value);

        /// <summary>
        /// Sets a payment on the date specified to the value given. This overwrites the existing
        /// value if it exists.
        /// </summary>
        /// <param name="date">The date to add data to.</param>
        /// <param name="value">The value of the payment to add.</param>
        UpdateResult<DailyValuation> SetPayment(DateTime date, decimal value);

        /// <summary>
        /// Attempts to delete a payment on the date specified.
        /// </summary>
        /// <param name="date">The date to delete data on.</param>
        /// <returns>Whether data was deleted or not.</returns>
        UpdateResult<DailyValuation> TryDeletePayment(DateTime date);
    }
}
