using System;
using System.Collections.Generic;
using FinancialStructures.FinanceInterfaces;
using FinancialStructures.NamingStructures;
using StructureCommon.DataStructures;
using StructureCommon.Reporting;

namespace FinancialStructures.PortfolioAPI
{
    public static partial class PortfolioData
    {
        /// <summary>
        /// Adds the desired data to the security if it can.
        /// </summary>
        public static bool TryAddOrEditDataToSecurity(this IPortfolio portfolio, TwoName names, DateTime date, double shares, double unitPrice, double Investment, IReportLogger reportLogger = null)
        {
            for (int fundIndex = 0; fundIndex < portfolio.NumberOf(AccountType.Security); fundIndex++)
            {
                if (names.IsEqualTo(portfolio.Funds[fundIndex].Names))
                {
                    _ = reportLogger?.Log(ReportSeverity.Critical, ReportType.Error, ReportLocation.AddingData, $"Security `{names.Company}'-`{names.Name}' has data on date .");
                    return portfolio.Funds[fundIndex].TryAddOrEditData(date, unitPrice, shares, Investment, reportLogger);
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
        public static bool TryAddOrEditData(this IPortfolio portfolio, AccountType elementType, NameData name, DailyValuation data, IReportLogger reportLogger = null)
        {
            switch (elementType)
            {
                case (AccountType.Security):
                {
                    return false;
                }
                case (AccountType.Currency):
                {
                    return SingleListAddOrEdit(portfolio.Currencies, name, data, reportLogger);
                }
                case (AccountType.BankAccount):
                {
                    return SingleListAddOrEdit(portfolio.BankAccounts, name, data, reportLogger);
                }
                case (AccountType.Sector):
                {
                    return SingleListAddOrEdit(portfolio.BenchMarks, name, data, reportLogger);
                }
                default:
                    _ = reportLogger?.LogUseful(ReportType.Error, ReportLocation.AddingData, $"Editing an Unknown type.");
                    return false;
            }
        }

        private static bool SingleListAddOrEdit<T>(List<T> listToEdit, NameData name, DailyValuation data, IReportLogger reportLogger = null) where T : ISingleValueDataList
        {
            for (int accountIndex = 0; accountIndex < listToEdit.Count; accountIndex++)
            {
                if (listToEdit[accountIndex].Names.IsEqualTo(name))
                {
                    // now edit data
                    return listToEdit[accountIndex].TryAddOrEditData(data.Day, data.Value, reportLogger);
                }
            }

            return false;
        }
    }
}