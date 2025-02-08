using System;
using Effanville.Common.Structure.DataStructures;
using Effanville.FinancialStructures.DataStructures;
using Effanville.FinancialStructures.FinanceStructures;
using Effanville.FinancialStructures.NamingStructures;

namespace Effanville.FinancialStructures.Database.Extensions.DataEdit
{
    /// <summary>
    /// Contains static extension methods for deleting account data.
    /// </summary>
    public static class PortfolioDataDelete
    {
        /// <summary>
        /// Attempts to remove trade data from the account.
        /// </summary>
        /// <param name="portfolio">The portfolio which holds the account</param>
        /// <param name="account">The type of data to remove from.</param>
        /// <param name="name">The name to remove from.</param>
        /// <param name="date">The date on which to remove data.</param>
        /// <returns>Success or failure.</returns>
        public static bool TryDeleteTradeData(this IPortfolio portfolio, Account account, TwoName name, DateTime date)
        {
            return portfolio.TryPerformEdit<ISecurity, SecurityTrade>(
               account,
               name,
               (acc, n) => acc == Account.Security || acc == Account.Pension,
               security => security.TryDeleteTradeData(date)).Success;
        }

        /// <summary>
        /// Attempts to remove asset debt from the account.
        /// </summary>
        /// <param name="portfolio">The portfolio which holds the account</param>
        /// <param name="account">The type of data to remove from.</param>
        /// <param name="name">The name to remove from.</param>
        /// <param name="date">The date on which to remove data.</param>
        /// <returns>Success or failure.</returns>
        public static bool TryDeleteAssetDebt(this IPortfolio portfolio, Account account, TwoName name, DateTime date)
        {
            return portfolio.TryPerformEdit<IAmortisableAsset, DailyValuation>(
               account,
               name,
               (acc, n) => acc == Account.Asset,
               asset => asset.TryDeleteDebt(date)).Success;
        }

        /// <summary>
        /// Attempts to remove an asset payment from the account.
        /// </summary>
        /// <param name="portfolio">The portfolio which holds the account</param>
        /// <param name="account">The type of data to remove from.</param>
        /// <param name="name">The name to remove from.</param>
        /// <param name="date">The date on which to remove data.</param>
        /// <returns>Success or failure.</returns>
        public static bool TryDeleteAssetPayment(this IPortfolio portfolio, Account account, TwoName name, DateTime date)
        {
            return portfolio.TryPerformEdit<IAmortisableAsset, DailyValuation>(
               account,
               name,
               (acc, n) => acc == Account.Asset,
               asset => asset.TryDeletePayment(date)).Success;
        }

        /// <summary>
        /// Attempts to remove data from the account.
        /// </summary>
        /// <param name="portfolio">The portfolio which holds the account</param>
        /// <param name="account">The type of data to remove from.</param>
        /// <param name="name">The name to remove from.</param>
        /// <param name="date">The date on which to remove data.</param>
        /// <returns>Success or failure.</returns>
        public static bool TryDeleteData(this IPortfolio portfolio, Account account, TwoName name, DateTime date)
        {
            return portfolio.TryPerformEdit<IValueList, DailyValuation>(
               account,
               name,
               account => account.TryDeleteData(date)).Success;
        }
    }
}
