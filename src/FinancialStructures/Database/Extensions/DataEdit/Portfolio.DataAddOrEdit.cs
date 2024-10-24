﻿using Effanville.Common.Structure.DataStructures;
using Effanville.Common.Structure.Reporting;
using Effanville.FinancialStructures.DataStructures;
using Effanville.FinancialStructures.FinanceStructures;
using Effanville.FinancialStructures.NamingStructures;

namespace Effanville.FinancialStructures.Database.Extensions.DataEdit
{
    /// <summary>
    /// Contains static extensions for editing account data.
    /// </summary>
    public static class PortfolioDataEdit
    {
        /// <summary>
        /// Adds the desired asset debt data if it can.
        /// </summary>
        public static bool TryAddOrEditAssetDebt(this IPortfolio portfolio, Account account, TwoName name, DailyValuation oldData, DailyValuation newData, IReportLogger reportLogger = null)
        {
            return portfolio.TryPerformEdit<IAmortisableAsset>(
                account,
                name,
                (acc, n) => acc == Account.Asset,
                asset => asset.TryEditDebt(oldData.Day, newData.Day, newData.Value, reportLogger),
                ReportLocation.AddingData,
                reportLogger);
        }

        /// <summary>
        /// Adds the desired asset payment data if it can.
        /// </summary>
        public static bool TryAddOrEditAssetPayment(this IPortfolio portfolio, Account account, TwoName name, DailyValuation oldData, DailyValuation newData, IReportLogger reportLogger = null)
        {
            return portfolio.TryPerformEdit<IAmortisableAsset>(
                account,
                name,
                (acc, n) => acc == Account.Asset,
                asset => asset.TryEditPayment(oldData.Day, newData.Day, newData.Value, reportLogger),
                ReportLocation.AddingData,
                reportLogger);
        }

        /// <summary>
        /// Adds the desired trade data if it can.
        /// </summary>
        public static bool TryAddOrEditTradeData(this IPortfolio portfolio, Account account, TwoName name, SecurityTrade oldTrade, SecurityTrade newTrade, IReportLogger reportLogger = null)
        {
            return portfolio.TryPerformEdit<ISecurity>(
                account,
                name,
                (acc, n) => acc == Account.Security || acc == Account.Pension,
                security => security.TryAddOrEditTradeData(oldTrade, newTrade, reportLogger),
                ReportLocation.AddingData,
                reportLogger);
        }

        /// <summary>
        /// Attempts to add data to the account.
        /// </summary>
        /// <param name="portfolio">The portfolio which holds the account</param>
        /// <param name="account">The type of data to add to.</param>
        /// <param name="name">The name to add to.</param>
        /// <param name="oldData"> The old data to edit.</param>
        /// <param name="data">The data to add.</param>
        /// <param name="reportLogger">Report callback.</param>
        /// <returns>Success or failure.</returns>
        /// <remarks> This cannot currently be used to add to securities due to different type of data.</remarks>
        public static bool TryAddOrEditData(this IPortfolio portfolio, Account account, TwoName name, DailyValuation oldData, DailyValuation data, IReportLogger reportLogger = null)
        {
            return portfolio.TryPerformEdit(
                account,
                name,
                valueList => valueList.TryEditData(oldData.Day, data.Day, data.Value, reportLogger),
                ReportLocation.AddingData,
                reportLogger);
        }
    }
}
