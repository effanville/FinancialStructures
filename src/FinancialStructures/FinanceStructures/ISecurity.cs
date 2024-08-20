using System;
using Effanville.Common.Structure.Reporting;
using Effanville.FinancialStructures.DataStructures;

namespace Effanville.FinancialStructures.FinanceStructures
{
    /// <summary>
    /// A named entity with Share, unit price and investment lists to detail price history.
    /// </summary>
    public interface ISecurity : IReadOnlySecurity, IExchangeableValueList, IEquatable<ISecurity>, IComparable<ISecurity>
    {
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

        /// <summary>
        /// Replaces all trades of type <see cref="TradeType.ShareReprice"/> with the <see cref="TradeType.ShareReset"/> 
        /// type.
        /// </summary>
        void MigrateRepriceToReset();
    }
}
