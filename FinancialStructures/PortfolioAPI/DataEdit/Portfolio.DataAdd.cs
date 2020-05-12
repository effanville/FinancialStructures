using FinancialStructures.DataStructures;
using FinancialStructures.FinanceInterfaces;
using FinancialStructures.NamingStructures;
using StructureCommon.Reporting;
using System;
using System.Collections.Generic;

namespace FinancialStructures.PortfolioAPI
{
    public static partial class PortfolioData
    {
        /// <summary>
        /// Adds the desired data to the security if it can.
        /// </summary>
        public static bool TryAddDataToSecurity(this IPortfolio portfolio, TwoName names, DateTime date, double shares, double unitPrice, double Investment, IReportLogger reportLogger = null)
        {
            for (int fundIndex = 0; fundIndex < portfolio.NumberOf(AccountType.Security); fundIndex++)
            {
                if (names.IsEqualTo(portfolio.Funds[fundIndex].Names))
                {
                    return portfolio.Funds[fundIndex].TryAddData(date, unitPrice, shares, Investment, reportLogger);
                }
            }
            _ = reportLogger?.Log(ReportSeverity.Critical, ReportType.Error, ReportLocation.AddingData, $"Security `{names.Company}'-`{names.Name}' could not be found in the database.");
            return false;
        }

        /// <summary>
        /// Attempts to add data to the account.
        /// </summary>
        /// <param name="portfolio">The database to add to.</param>
        /// <param name="elementType">The type of data to add to.</param>
        /// <param name="name">The name to add to.</param>
        /// <param name="data">The data to add.</param>
        /// <param name="reportLogger">Report callback.</param>
        /// <returns>Success or failure.</returns>
        /// <remarks> This cannot currently be used to add to securities due to different type of data.</remarks>
        public static bool TryAddData(this IPortfolio portfolio, AccountType elementType, NameData name, DayValue_ChangeLogged data, IReportLogger reportLogger = null)
        {
            switch (elementType)
            {
                case (AccountType.Security):
                {
                    return false;
                }
                case (AccountType.Currency):
                {
                    return SingleListAdd(portfolio.Currencies, name, data, reportLogger);
                }
                case (AccountType.BankAccount):
                {
                    return SingleListAdd(portfolio.BankAccounts, name, data, reportLogger);
                }
                case (AccountType.Sector):
                {
                    return SingleListAdd(portfolio.BenchMarks, name, data, reportLogger);
                }
                default:
                    _ = reportLogger?.LogUseful(ReportType.Error, ReportLocation.AddingData, $"Editing an Unknown type.");
                    return false;
            }
        }

        private static bool SingleListAdd<T>(List<T> listToEdit, NameData name, DayValue_ChangeLogged data, IReportLogger reportLogger = null) where T : ISingleValueDataList
        {
            for (int accountIndex = 0; accountIndex < listToEdit.Count; accountIndex++)
            {
                if (listToEdit[accountIndex].Names.IsEqualTo(name))
                {
                    // now edit data
                    return listToEdit[accountIndex].TryAddData(data.Day, data.Value, reportLogger);
                }
            }

            return false;
        }
    }
}