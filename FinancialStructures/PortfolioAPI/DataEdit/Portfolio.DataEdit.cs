using FinancialStructures.DataStructures;
using FinancialStructures.FinanceInterfaces;
using FinancialStructures.NamingStructures;
using FinancialStructures.Reporting;
using System;
using System.Collections.Generic;

namespace FinancialStructures.PortfolioAPI
{
    public static partial class PortfolioData
    {
        /// <summary>
        /// Edits the data of the security, if possible.
        /// </summary>
        public static bool TryEditSecurityData(this IPortfolio portfolio, TwoName names, DateTime oldDate, DateTime newDate, double shares, double unitPrice, double Investment, IReportLogger reportLogger = null)
        {
            for (int fundIndex = 0; fundIndex < portfolio.NumberOf(AccountType.Security); fundIndex++)
            {
                if (names.IsEqualTo(portfolio.Funds[fundIndex].Names))
                {
                    // now edit data
                    return portfolio.Funds[fundIndex].TryEditData(oldDate, newDate, shares, unitPrice, Investment, reportLogger);
                }
            }

            return false;
        }

        /// <summary>
        /// Attempts to edit data in the account.
        /// </summary>
        /// <param name="portfolio">The database to edit.</param>
        /// <param name="elementType">The type of data to edit.</param>
        /// <param name="name">The name to edit.</param>
        /// <param name="oldData">The data to edit.</param>
        /// /// <param name="newData">The data to edit.</param>
        /// <param name="reportLogger">Report callback.</param>
        /// <returns>Success or failure.</returns>
        /// <remarks> This cannot currently be used to add to securities due to different type of data.</remarks>
        public static bool TryEditData(this IPortfolio portfolio, AccountType elementType, NameData name, DayValue_ChangeLogged oldData, DayValue_ChangeLogged newData, IReportLogger reportLogger = null)
        {
            switch (elementType)
            {
                case (AccountType.Security):
                    {
                        return false;
                    }
                case (AccountType.Currency):
                    {
                        return TryEditSingleDataList(portfolio.Currencies, elementType, name, oldData, newData, reportLogger);
                    }
                case (AccountType.BankAccount):
                    {
                        return TryEditSingleDataList(portfolio.BankAccounts, elementType, name, oldData, newData, reportLogger);
                    }
                case (AccountType.Sector):
                    {
                        return TryEditSingleDataList(portfolio.BenchMarks, elementType, name, oldData, newData, reportLogger);
                    }
                default:
                    reportLogger?.LogUseful(ReportType.Error, ReportLocation.EditingData, $"Editing an Unknown type.");
                    return false;
            }
        }

        private static bool TryEditSingleDataList<T>(List<T> values, AccountType elementType, NameData name, DayValue_ChangeLogged oldData, DayValue_ChangeLogged newData, IReportLogger reportLogger = null) where T : ISingleValueDataList
        {
            for (int AccountIndex = 0; AccountIndex < values.Count; AccountIndex++)
            {
                if (name.IsEqualTo(values[AccountIndex].Names))
                {
                    // now edit data
                    return values[AccountIndex].TryEditData(oldData.Day, newData.Day, newData.Value, reportLogger);
                }
            }

            reportLogger?.Log(ReportSeverity.Critical, ReportType.Error, ReportLocation.EditingData, $"Could not find {elementType.ToString()} with name {name.ToString()}.");
            return false;
        }
    }
}
