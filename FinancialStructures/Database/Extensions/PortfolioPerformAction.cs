using System;

using Common.Structure.Reporting;

using FinancialStructures.FinanceStructures;
using FinancialStructures.NamingStructures;

namespace FinancialStructures.Database.Extensions
{
    /// <summary>
    /// Helper methods for performing an action on a portfolio.
    /// </summary>
    public static class PortfolioPerformAction
    {
        /// <summary>
        /// Performs an edit on the account specified if the preEditCheck succeeds and the account exists.
        /// </summary>
        /// <param name="portfolio">The portfolio which holds the account.</param>
        /// <param name="account">The type of data to remove from.</param>
        /// <param name="name">The name to remove from.</param>
        /// <param name="preEditCheck">A check to peform prior to attempting the edit.</param>
        /// <param name="performEdit">The function to perform on the value list.</param>
        /// <param name="location">The location for any errors</param>
        /// <param name="reportLogger">Report callback.</param>
        /// <returns>Success or failure.</returns>
        public static bool TryPerformEdit(
            this IPortfolio portfolio,
            Account account,
            TwoName name,
            Func<Account, TwoName, bool> preEditCheck,
            Func<IValueList, bool> performEdit,
            ReportLocation location,
            IReportLogger reportLogger = null)
        {
            if (!preEditCheck(account, name))
            {
                _ = reportLogger?.Log(ReportSeverity.Critical, ReportType.Error, location, $"Cannot perform edit for account and name {account} - {name}.");
                return false;
            }

            return TryPerformEdit(portfolio, account, name, performEdit, location, reportLogger);
        }

        /// <summary>
        /// Performs an edit on the account specified if the preEditCheck succeeds and the account exists.
        /// </summary>
        /// <param name="portfolio">The portfolio which holds the account.</param>
        /// <param name="account">The type of data to remove from.</param>
        /// <param name="name">The name to remove from.</param>
        /// <param name="preEditCheck">A check to peform prior to attempting the edit.</param>
        /// <param name="performEdit">The function to perform on the value list.</param>
        /// <param name="location">The location for any errors</param>
        /// <param name="reportLogger">Report callback.</param>
        /// <returns>Success or failure.</returns>
        public static bool TryPerformEdit<T>(
            this IPortfolio portfolio,
            Account account,
            TwoName name,
            Func<Account, TwoName, bool> preEditCheck,
            Func<T, bool> performEdit,
            ReportLocation location,
            IReportLogger reportLogger = null)
            where T : IValueList
        {
            if (!preEditCheck(account, name))
            {
                _ = reportLogger?.Log(ReportSeverity.Critical, ReportType.Error, location, $"Cannot perform edit for account and name {account} - {name}.");
                return false;
            }

            return TryPerformEdit(portfolio, account, name, performEdit, location, reportLogger);
        }

        /// <summary>
        /// Performs an edit on the account specified if it exists. This only performs a
        /// function upon an <see cref="IValueList"/>
        /// </summary>
        /// <param name="portfolio">The portfolio which holds the account.</param>
        /// <param name="account">The type of data to remove from.</param>
        /// <param name="name">The name to remove from.</param>
        /// <param name="performEdit">The function to perform on the value list.</param>
        /// <param name="location">The location for any errors</param>
        /// <param name="reportLogger">Report callback.</param>
        /// <returns>Success or failure.</returns>
        public static bool TryPerformEdit(
            this IPortfolio portfolio,
            Account account,
            TwoName name,
            Func<IValueList, bool> performEdit,
            ReportLocation location,
            IReportLogger reportLogger = null)
        {
            if (!portfolio.TryGetAccount(account, name, out IValueList valueList))
            {
                _ = reportLogger?.Log(ReportSeverity.Critical, ReportType.Error, location, $"Could not find {account} - {name}.");
                return false;
            }

            return performEdit(valueList);
        }

        /// <summary>
        /// Performs an edit on the account specified if it exists. This can perform a function on 
        /// any <typeparamref name="T"/> where <typeparamref name="T"/> : <see cref="IValueList"/>.
        /// </summary>
        /// <param name="portfolio">The portfolio which holds the account.</param>
        /// <param name="account">The type of data to remove from.</param>
        /// <param name="name">The name to remove from.</param>
        /// <param name="performEdit">The function to perform on the value list.</param>
        /// <param name="location">The location for any errors</param>
        /// <param name="reportLogger">Report callback.</param>
        /// <returns>Success or failure.</returns>
        public static bool TryPerformEdit<T>(
            this IPortfolio portfolio,
            Account account,
            TwoName name,
            Func<T, bool> performEdit,
            ReportLocation location,
            IReportLogger reportLogger = null) where T : IValueList
        {
            if (!portfolio.TryGetAccount(account, name, out IValueList valueList))
            {
                _ = reportLogger?.Log(ReportSeverity.Critical, ReportType.Error, location, $"Could not find {account} - {name}.");
                return false;
            }

            if (valueList is not T specialValueList)
            {
                _ = reportLogger?.Log(ReportSeverity.Critical, ReportType.Error, location, $"Could not convert {account} - {name} into account of type {typeof(T)}.");
                return false;
            }

            return performEdit(specialValueList);
        }
    }
}
