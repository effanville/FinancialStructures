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
        /// Attempts to remove data from the account.
        /// </summary>
        /// <param name="portfolio">The database to remove from.</param>
        /// <param name="elementType">The type of data to remove from.</param>
        /// <param name="name">The name to remove from.</param>
        /// <param name="data">The data to remove.</param>
        /// <param name="reportLogger">Report callback.</param>
        /// <returns>Success or failure.</returns>
        public static bool TryDeleteData(this IPortfolio portfolio, AccountType elementType, NameData name, DailyValuation data, IReportLogger reportLogger)
        {
            switch (elementType)
            {
                case (AccountType.Security):
                {
                    for (int fundIndex = 0; fundIndex < portfolio.NumberOf(AccountType.Security); fundIndex++)
                    {
                        if (name.IsEqualTo(portfolio.Funds[fundIndex].Names))
                        {
                            // now edit data
                            return portfolio.Funds[fundIndex].TryDeleteData(data.Day, reportLogger);
                        }
                    }

                    break;
                }
                case (AccountType.Currency):
                {
                    return TryDeleteSingleListData(portfolio.Currencies, elementType, name, data, reportLogger);
                }
                case (AccountType.BankAccount):
                {
                    return TryDeleteSingleListData(portfolio.BankAccounts, elementType, name, data, reportLogger);
                }
                case (AccountType.Sector):
                {
                    return TryDeleteSingleListData(portfolio.BenchMarks, elementType, name, data, reportLogger);
                }
                default:
                    reportLogger.LogUseful(ReportType.Error, ReportLocation.DeletingData, $"Editing an Unknown type.");
                    return false;
            }


            reportLogger.Log(ReportSeverity.Critical, ReportType.Error, ReportLocation.DeletingData, $"Could not find {elementType.ToString()} - {name.ToString()}.");
            return false;
        }

        private static bool TryDeleteSingleListData<T>(List<T> values, AccountType elementType, NameData name, DailyValuation data, IReportLogger reportLogger) where T : ISingleValueDataList
        {
            foreach (T account in values)
            {
                if (name.IsEqualTo(account.Names))
                {
                    return account.TryDeleteData(data.Day, reportLogger);
                }
            }

            reportLogger.Log(ReportSeverity.Critical, ReportType.Error, ReportLocation.DeletingData, $"Could not find {elementType.ToString()} - {name.ToString()}.");
            return false;
        }
    }
}
