using FinancialStructures.DataStructures;
using FinancialStructures.FinanceInterfaces;
using FinancialStructures.NamingStructures;
using StructureCommon.DataStructures;
using StructureCommon.Reporting;
using System.Collections.Generic;

namespace FinancialStructures.PortfolioAPI
{
    public static partial class PortfolioData
    {
        /// <summary>
        /// Queries for data for the security of name and company. 
        /// </summary>
        public static List<SecurityDayData> SecurityData(this IPortfolio portfolio, TwoName names)
        {
            foreach (ISecurity security in portfolio.Funds)
            {
                if (names.IsEqualTo(security.Names))
                {
                    return security.GetDataForDisplay();
                }
            }

            return new List<SecurityDayData>();
        }

        /// <summary>
        /// Returns the 
        /// </summary>
        /// <param name="portfolio"></param>
        /// <param name="elementType"></param>
        /// <param name="name"></param>
        /// <param name="reportLogger"></param>
        /// <returns></returns>
        public static List<DailyValuation> NumberData(this IPortfolio portfolio, AccountType elementType, NameData name, IReportLogger reportLogger = null)
        {
            switch (elementType)
            {

                case (AccountType.Currency):
                {
                    return SingleDataListDataObtainer(portfolio.Currencies, elementType, name, reportLogger);
                }
                case (AccountType.BankAccount):
                {
                    return SingleDataListDataObtainer(portfolio.BankAccounts, elementType, name, reportLogger);
                }
                case (AccountType.Sector):
                {
                    return SingleDataListDataObtainer(portfolio.BenchMarks, elementType, name, reportLogger);
                }
                default:
                case (AccountType.Security):
                {
                    return new List<DailyValuation>();
                }
            }
        }

        private static List<DailyValuation> SingleDataListDataObtainer<T>(List<T> objects, AccountType elementType, NameData name, IReportLogger reportLogger = null) where T : ISingleValueDataList
        {
            foreach (var account in objects)
            {
                if (name.IsEqualTo(account.Names))
                {
                    return account.GetDataForDisplay();
                }
            }

            reportLogger?.LogUseful(ReportType.Error, ReportLocation.DatabaseAccess, $"Could not find {elementType.ToString()} - {name.ToString()}");
            return new List<DailyValuation>();
        }
    }
}
