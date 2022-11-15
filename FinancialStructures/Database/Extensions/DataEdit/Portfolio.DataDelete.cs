using System;
using Common.Structure.Reporting;
using FinancialStructures.FinanceStructures;
using FinancialStructures.NamingStructures;

namespace FinancialStructures.Database.Extensions
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
        /// <param name="reportLogger">Report callback.</param>
        /// <returns>Success or failure.</returns>
        public static bool TryDeleteTradeData(this IPortfolio portfolio, Account account, TwoName name, DateTime date, IReportLogger reportLogger = null)
        {
            return portfolio.TryPerformEdit<ISecurity>(
               account,
               name,
               (acc, n) => acc == Account.Security || acc == Account.Pension,
               security => security.TryDeleteTradeData(date, reportLogger),
               ReportLocation.DeletingData,
               reportLogger);
        }

        /// <summary>
        /// Attempts to remove asset debt from the account.
        /// </summary>
        /// <param name="portfolio">The portfolio which holds the account</param>
        /// <param name="account">The type of data to remove from.</param>
        /// <param name="name">The name to remove from.</param>
        /// <param name="date">The date on which to remove data.</param>
        /// <param name="reportLogger">Report callback.</param>
        /// <returns>Success or failure.</returns>
        public static bool TryDeleteAssetDebt(this IPortfolio portfolio, Account account, TwoName name, DateTime date, IReportLogger reportLogger = null)
        {
            return portfolio.TryPerformEdit<IAmortisableAsset>(
               account,
               name,
               (acc, n) => acc == Account.Asset,
               asset => asset.TryDeleteDebt(date, reportLogger),
               ReportLocation.DeletingData,
               reportLogger);
        }

        /// <summary>
        /// Attempts to remove an asset payment from the account.
        /// </summary>
        /// <param name="portfolio">The portfolio which holds the account</param>
        /// <param name="account">The type of data to remove from.</param>
        /// <param name="name">The name to remove from.</param>
        /// <param name="date">The date on which to remove data.</param>
        /// <param name="reportLogger">Report callback.</param>
        /// <returns>Success or failure.</returns>
        public static bool TryDeleteAssetPayment(this IPortfolio portfolio, Account account, TwoName name, DateTime date, IReportLogger reportLogger = null)
        {
            return portfolio.TryPerformEdit<IAmortisableAsset>(
               account,
               name,
               (acc, n) => acc == Account.Asset,
               asset => asset.TryDeletePayment(date, reportLogger),
               ReportLocation.DeletingData,
               reportLogger);
        }

        /// <summary>
        /// Attempts to remove data from the account.
        /// </summary>
        /// <param name="portfolio">The portfolio which holds the account</param>
        /// <param name="account">The type of data to remove from.</param>
        /// <param name="name">The name to remove from.</param>
        /// <param name="date">The date on which to remove data.</param>
        /// <param name="reportLogger">Report callback.</param>
        /// <returns>Success or failure.</returns>
        public static bool TryDeleteData(this IPortfolio portfolio, Account account, TwoName name, DateTime date, IReportLogger reportLogger = null)
        {
            return portfolio.TryPerformEdit(
               account,
               name,
               account => account.TryDeleteData(date, reportLogger),
               ReportLocation.DeletingData,
               reportLogger);
        }
    }
}
